using MediatR;

namespace EmployeeManagementSolu.Application.DTOs
{
    public class CreateEmployeeDTO : IRequest<ReadEmployeeDTO>
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public required int Age { get; set; }
        public required decimal Salary { get; set; }
        public required bool IsActive { get; set; }
        public required DateTime JoiningDate { get; set; }
        public required Guid DepartmentId { get; set; }
    }
}
