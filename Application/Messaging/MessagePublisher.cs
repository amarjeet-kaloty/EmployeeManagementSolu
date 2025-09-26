using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Application.Messaging
{
    public class MessagePublisher
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<EmployeeCreatedConsumer> _logger;

        public MessagePublisher(ILogger<EmployeeCreatedConsumer> logger, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task PublishEmployeeCreatedEvent(object employeeData)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "employee_events", type: ExchangeType.Fanout);

            var message = JsonSerializer.Serialize(employeeData);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: "employee_events",
                routingKey: "",
                body: body);
            _logger.LogInformation($" [=>] Sent {message}");
        }
    }
}
