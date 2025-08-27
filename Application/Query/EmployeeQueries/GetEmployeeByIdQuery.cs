using EmployeeManagementSolu.Application.DTOs;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDTO>
    {
        public string Id { get; set; }
    }
}