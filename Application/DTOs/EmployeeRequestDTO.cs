

namespace EmployeeManagementSolu.Presentation.DTOs
{
    public class EmployeeRequestDTO
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
    }
}
