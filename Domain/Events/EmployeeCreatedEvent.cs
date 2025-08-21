
using EmployeeManagementSolu.Domain.Entities;
using MediatR;

namespace EmployeeManagementSolu.Domain.Events
{
    public class EmployeeCreatedEvent : INotification
    {
        public Employee _employee { get; set; }

        public EmployeeCreatedEvent(Employee employee)
        {
            _employee = employee;
        }
    }
}