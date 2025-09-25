using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Application.Messaging
{
    public class MessagePublisher
    {
        private readonly ConnectionFactory _connectionFactory;

        public MessagePublisher(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task PublishEmployeeCreatedEvent(object employeeData)
        {
            // Reference code
            //var factory = new ConnectionFactory { HostName = "localhost" };
            //using var connection = await factory.CreateConnectionAsync();
            //using var channel = await connection.CreateChannelAsync();

            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            // For Reference. Remove it later.
            //await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
            //arguments: null);

            await channel.ExchangeDeclareAsync(exchange: "employee_events", type: ExchangeType.Fanout);

            // For Reference. Remove it later.
            //const string message = "Hello World!";
            //var body = Encoding.UTF8.GetBytes(message);

            var message = JsonSerializer.Serialize(employeeData);
            var body = Encoding.UTF8.GetBytes(message);

            // Reference code
            // await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
            // Console.WriteLine($" [x] Sent {message}");

            await channel.BasicPublishAsync(
                exchange: "employee_events",
                routingKey: "",
                body: body);
            Console.WriteLine($" [x] Sent {message}");
        }
    }
}
