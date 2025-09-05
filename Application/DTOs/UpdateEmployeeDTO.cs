using MediatR;

namespace EmployeeManagementSolu.Application.DTOs
{
    public class UpdateEmployeeDTO : IRequest<ReadEmployeeDTO>
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
