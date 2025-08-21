using EmployeeManagementSolu.Domain.Entities;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class UpdateEmployeeCommand : IRequest<int>
    {
        public string Id { get; set; }

        public EmployeeName? Name { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public UpdateEmployeeCommand(string id, EmployeeName name, string address, string email, string phone)
        {
            Id = id;
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
        }
    }
}