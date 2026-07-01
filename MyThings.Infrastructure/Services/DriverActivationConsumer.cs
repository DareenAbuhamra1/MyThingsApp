using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyThings.Infrastructure.Helper;
using MyThings.Core.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class DriverActivationConsumer : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly ILogger<DriverActivationConsumer> _logger;

    public DriverActivationConsumer(IOptions<RabbitMqSettings> options,ILogger<DriverActivationConsumer> logger )
    {
        _rabbitMqSettings = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.Host,
            UserName = _rabbitMqSettings.Username,
            Password = _rabbitMqSettings.Password
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: "driver.account.status",
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var data = JsonSerializer.Deserialize<DriverActivationEventDto>(json);

                if (data is null)
                {
                    _logger.LogWarning("Received invalid message");
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    return;
                }

                if (data?.Active == true)
                    Console.WriteLine($"Admin {data.AdminId} activated driver {data.DriverId}");
                else
                    Console.WriteLine($"Admin {data?.AdminId} deactivated driver {data?.DriverId}");

                Console.WriteLine($"Sending SMS to {data?.DriverPhone}");

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");

                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: "driver.account.status",
            autoAck: false,
            consumer: consumer);

        // keep service alive
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
            await _channel.CloseAsync();

        if (_connection != null)
            await _connection.CloseAsync();

        await base.StopAsync(cancellationToken);
    }
}