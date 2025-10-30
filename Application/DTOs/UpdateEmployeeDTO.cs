using MediatR;

namespace EmployeeManagementSolu.Application.DTOs
{
    public class UpdateEmployeeDTO : IRequest<ReadEmployeeDTO>
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
    }
}
