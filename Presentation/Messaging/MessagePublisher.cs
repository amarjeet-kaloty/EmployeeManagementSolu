using Dapr.Client;

namespace Presentation.Messaging
{
    public class MessagePublisher
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<MessagePublisher> _logger;
        private const string PUB_SUB_COMPONENT = "rabbitmq-pubsub";
        private const string TOPIC_NAME = "employee_events";

        public MessagePublisher(ILogger<MessagePublisher> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        public async Task PublishEmployeeCreatedEvent(object employeeData)
        {
            await _daprClient.PublishEventAsync(PUB_SUB_COMPONENT, TOPIC_NAME, employeeData);

            _logger.LogInformation($" [=>] Sent employee create event to Dapr pub-sub component: {PUB_SUB_COMPONENT}/{TOPIC_NAME}");
        }

        public async Task PublishEmployeeUpdatedEvent(object employeeData)
        {
            await _daprClient.PublishEventAsync(PUB_SUB_COMPONENT, "employee-updated-topic", employeeData);

            _logger.LogInformation($" [=>] Sent employee update event to Dapr pub-sub component...");
        }
    }
}
