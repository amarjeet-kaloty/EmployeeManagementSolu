using EmployeeManagementSolu.Application.DTOs;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailQuery : IRequest<ReadEmployeeDTO>
    {
        public string Email { get; set; }
    }
}