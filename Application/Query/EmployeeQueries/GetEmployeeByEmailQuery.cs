using EmployeeManagementSolu.Domain.Entities;
using MediatR;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailQuery : IRequest<Employee>
    {
        public required string Email { get; set; }
    }
}
