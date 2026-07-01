using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MyThings.Infrastructure.Helper;
using MyThings.Core.Interfaces;
using RabbitMQ.Client;

namespace MyThings.Infrastructure.Services;

public class RabbitMqMessageBus : IMessageBus, IAsyncDisposable
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private IConnection? _connection;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    public RabbitMqMessageBus(IOptions<RabbitMqSettings> options)
    {
        _rabbitMqSettings = options.Value;
    }
    private async Task<IConnection> GetConnectionAsync()
    {
        if (_connection is not null) return _connection;

        await _connectionLock.WaitAsync();

        try
        {
            if (_connection == null)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMqSettings.Host,
                    Port = _rabbitMqSettings.Port,
                    Password = _rabbitMqSettings.Password,
                    UserName = _rabbitMqSettings.Username,
                };

                _connection = await factory.CreateConnectionAsync();
            }
            return _connection;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async Task PublishAsync<T>(T message, string queueName)
    {
        var connection = await GetConnectionAsync();

        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue : queueName, durable: true, exclusive: false, autoDelete: false, arguments: null );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey : queueName,
            body: body
        );

    }
    public async ValueTask DisposeAsync()
    {
        if(_connection is not null) await _connection.DisposeAsync();

        _connectionLock.Dispose();
    }
}