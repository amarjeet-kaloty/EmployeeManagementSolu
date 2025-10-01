using Dapr;
using EmployeeManagementSolu.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("/api/v1/employees/events/EmployeeCreated")]
    public class EmployeeSubscriptionController : Controller
    {
        private readonly ILogger<EmployeeSubscriptionController> _logger;

        public EmployeeSubscriptionController(ILogger<EmployeeSubscriptionController> logger)
        {
            _logger = logger;
        }

        [Topic("rabbitmq-pubsub", "employee_events")]
        [HttpPost]
        public ActionResult HandleEmployeeCreatedEvent([FromBody] CreateEmployeeDTO employeeData)
        {
            _logger.LogInformation($" [=>] Dapr Received Employee Created Event for Employee:" +
                $" {employeeData.Name}, {employeeData.Address}, {employeeData.Email}, {employeeData.Phone}");

            return Ok($"Event successfully handled for employee creation.");
        }
    }
}
