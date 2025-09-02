using EmployeeManagementSolu.Application.DTOs;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeResponseDTO>
    {
        public string Id { get; set; }
    }
}