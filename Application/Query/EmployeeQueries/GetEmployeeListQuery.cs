using EmployeeManagementSolu.Application.DTOs;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeListQuery : IRequest<List<EmployeeResponseDTO>>
    {
    }
}