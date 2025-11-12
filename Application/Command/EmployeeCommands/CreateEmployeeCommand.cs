using EmployeeManagementSolu.Application.DTOs;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class CreateEmployeeCommand : IRequest<ReadEmployeeDTO>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public required int Age { get; set; }
        public required decimal Salary { get; set; }
        public required bool IsActive { get; set; }
        public required DateTime JoiningDate { get; set; }

        public Guid DepartmentId { get; set; }

        public CreateEmployeeCommand(
            string name,
            string address,
            string email,
            string phone,
            int age,
            decimal salary,
            bool isActive,
            DateTime joiningDate,
            Guid departmentId)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            Age = age;
            Salary = salary;
            IsActive = isActive;
            JoiningDate = joiningDate;
            DepartmentId = departmentId;
        }
    }
}