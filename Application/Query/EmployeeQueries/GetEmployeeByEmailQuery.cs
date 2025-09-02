using EmployeeManagementSolu.Application.DTOs;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailQuery : IRequest<EmployeeSearchDTO>
    {
        public string Email { get; set; }
    }
}