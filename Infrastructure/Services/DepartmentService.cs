using Dapr.Client;
using Domain.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<DepartmentService> _logger;
        private const string DEPARTMENT_APP_ID = "departmentservice";

        public DepartmentService(DaprClient daprClient, ILogger<DepartmentService> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        /// <summary>
        /// Validates if a department exists in the Department Solution.
        /// </summary>
        public async Task ValidateDepartmentExistsAsync(Guid departmentId, CancellationToken cancellationToken)
        {
            var methodPath = $"api/department/exists/{departmentId}";
            var invokableClient = _daprClient.CreateInvokableHttpClient(DEPARTMENT_APP_ID);
            var request = new HttpRequestMessage(HttpMethod.Get, methodPath);
            _logger.LogInformation($"Delegating employee detail query to App ID '{DEPARTMENT_APP_ID}' at path '{methodPath}' for ID: {departmentId}");

            var response = await invokableClient.SendAsync(request, cancellationToken);
            _logger.LogInformation($"Response: {response}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Department validation succeeded for ID: {DepartmentId}", departmentId);
                return;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogInformation("Department validation failed: Department ID '{DepartmentId}' not found (404).", departmentId);
                throw new ArgumentException($"Department ID '{departmentId}' is not valid. The department does not exist.");
            }
        }
    }
}
