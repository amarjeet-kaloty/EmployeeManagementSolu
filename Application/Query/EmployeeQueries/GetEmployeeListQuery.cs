using EmployeeManagementSolu.Domain.Entities;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeListQuery : IRequest<List<Employee>>
    {
    }
}