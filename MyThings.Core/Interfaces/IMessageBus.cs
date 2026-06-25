namespace MyThings.Core.Interfaces;


public interface IMessageBus
{
    Task PublishAsync<T>(T message, string queueName);
}