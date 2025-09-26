using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Application.Messaging
{
    public class EmployeeCreatedConsumer : BackgroundService
    {
        private readonly ILogger<EmployeeCreatedConsumer> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;

        public EmployeeCreatedConsumer(ILogger<EmployeeCreatedConsumer> logger, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _connection = await _connectionFactory.CreateConnectionAsync();
                var channel = await _connection.CreateChannelAsync();

                await channel.ExchangeDeclareAsync(exchange: "employee_events", type: ExchangeType.Fanout);
                var queueName = await channel.QueueDeclareAsync().ConfigureAwait(false);
                await channel.QueueBindAsync(queue: queueName, exchange: "employee_events", routingKey: string.Empty);
                _logger.LogInformation(" [=>] Waiting for messages from the 'employee_events' exchange.");

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation($" [=>] Received {message}");
                    var employeeData = JsonSerializer.Deserialize<object>(message);
                    _logger.LogInformation(" [=>] Employee created event handled successfully.");
                };

                await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
            }catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start RabbitMQ consumer.");
                return;
            }

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
