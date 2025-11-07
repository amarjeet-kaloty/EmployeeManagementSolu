using EmployeeManagementSolu.Application.DTOs;
using MediatR;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeeByDepartmentIdQuery : IRequest<IEnumerable<ReadEmployeeDTO>>
    {
        public required Guid DepartmentId { get; set; }
    }
}
