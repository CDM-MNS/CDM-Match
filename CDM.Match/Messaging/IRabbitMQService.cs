namespace CMD.Match.Messaging;

public interface IRabbitMQService
{
    Task PublishAsync(string queueName, object message, string eventPattern);
}