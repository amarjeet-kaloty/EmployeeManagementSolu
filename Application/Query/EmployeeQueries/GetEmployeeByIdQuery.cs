using EmployeeManagementSolu.Domain.Entities;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeByIdQuery : IRequest<Employee>
    {
        public string Id { get; set; }
    }
}