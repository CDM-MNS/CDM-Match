using System.Text;
using System.Text.Json;
using CMD.Match.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UrbanFlow_trips.Infrastucture.Messaging;

public class RabbitMQService : IRabbitMQService
{
    private readonly ConnectionFactory _factory;
    private readonly JsonSerializerOptions _jsonOptions;

    public RabbitMQService()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            UserName = "user",
            Password = "password"
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
    
    public async Task PublishAsync(string queueName, object message, string eventPattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueName);
        ArgumentException.ThrowIfNullOrWhiteSpace(eventPattern);
        ArgumentNullException.ThrowIfNull(message);
        
        try
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queueName, 
                durable: true,  // Persistance
                exclusive: false, 
                autoDelete: false
            );
            
    
            var json = JsonSerializer.Serialize(message, _jsonOptions);
            var body = Encoding.UTF8.GetBytes(json);

    
            var properties = new BasicProperties
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ReplyTo = queueName,
                Persistent = true,  // Messages persistants
                ContentType = "application/json"
            };
    
            await channel.BasicPublishAsync(
                exchange: string.Empty, 
                routingKey: queueName, 
                mandatory: true,
                basicProperties: properties,
                body: body
            );
        }
        catch (Exception ex)
        {
            throw new Exception($"Error publishing message to queue {queueName}: {ex.Message}");
        }
    }
    
    public async Task ConsumeAsync(string queueName, object message, string eventPattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueName);
        ArgumentException.ThrowIfNullOrWhiteSpace(eventPattern);
        ArgumentNullException.ThrowIfNull(message);
        
        try
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queueName, 
                durable: true,  // Persistance
                exclusive: false, 
                autoDelete: false
            );
            

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] {message}");
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);

        }
        catch (Exception ex)
        {
            throw new Exception($"Error consuming message from queue {queueName}: {ex.Message}");
        }
    }
    
}