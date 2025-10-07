using Dapr.Client;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Presentation.Messaging
{
    public class MessagePublisher
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<MessagePublisher> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private const string PUB_SUB_COMPONENT = "rabbitmq-pubsub";
        private const string EXCHANGE_NAME = "employee-data-changes";
        private const string TOPIC_NAME = "employee_events";
        private const string UPDATED_TOPIC_NAME = "employee-updated-topic";

        public MessagePublisher(ILogger<MessagePublisher> logger, DaprClient daprClient, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _daprClient = daprClient;
            _connectionFactory = connectionFactory;
        }

        public async Task PublishEmployeeCreatedEvent(object employeeData)
        {
            await _daprClient.PublishEventAsync(PUB_SUB_COMPONENT, TOPIC_NAME, employeeData);

            _logger.LogInformation($" [=>] Sent employee create event to Dapr pub-sub component: {PUB_SUB_COMPONENT}/{TOPIC_NAME}");
        }

        public async Task PublishEmployeeUpdatedEvent(object employeeData)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: EXCHANGE_NAME, type: ExchangeType.Topic);

            var message = JsonSerializer.Serialize(employeeData);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: EXCHANGE_NAME,
                routingKey: UPDATED_TOPIC_NAME,
                body: body);
            _logger.LogInformation($" [=>] Sent employee update event to RabbitMQ component... {message}");
        }
    }
}
