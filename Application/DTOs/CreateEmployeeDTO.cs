
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSolu.Application.DTOs
{
    public class CreateEmployeeDTO : IRequest<ReadEmployeeDTO>
    {
        [Required]
        public string Name { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public long? Age { get; set; }
        public decimal? Salary { get; set; }
        public bool? IsActive { get; set; }
        public required DateTime JoiningDate { get; set; }

    }
}
