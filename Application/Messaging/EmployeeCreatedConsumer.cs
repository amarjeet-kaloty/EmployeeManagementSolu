using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Application.Messaging
{
    public class EmployeeCreatedConsumer : BackgroundService
    {
        private readonly IConnectionFactory _connectionFactory;

        public EmployeeCreatedConsumer(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            // Reference code
            // await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
            // arguments: null);
            await channel.ExchangeDeclareAsync(exchange: "employee_events", type: ExchangeType.Fanout);
            Console.WriteLine(" [*] Waiting for messages.");

            var queueDeclareResult = await channel.QueueDeclareAsync();
            var queueName = queueDeclareResult.QueueName;
            await channel.QueueBindAsync(queue: queueName, exchange: "employee_events", routingKey: "");

            // Reference Code
            //consumer.ReceivedAsync += (model, ea) =>
            //{
            //    var body = ea.Body.ToArray();
            //    var message = Encoding.UTF8.GetString(body);
            //    Console.WriteLine($" [x] Received {message}");
            //    return Task.CompletedTask;
            //};

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var employee = JsonSerializer.Deserialize<object>(message);
                Console.WriteLine($"Received employee created event: {message}");
                return Task.CompletedTask;
            };

            // await channel.BasicConsumeAsync("hello", autoAck: true, consumer: consumer);
            await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
