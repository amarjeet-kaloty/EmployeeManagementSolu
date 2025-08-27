using EmployeeManagementSolu.Application.DTOs;
using MediatR;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailQuery : IRequest<EmployeeDTO>
    {
        public required string Email { get; set; }
    }
}
