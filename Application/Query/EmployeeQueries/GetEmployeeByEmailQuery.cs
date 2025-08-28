using Application.DTOs;
using MediatR;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailQuery : IRequest<EmployeeSearchDTO>
    {
        public required string Email { get; set; }
    }
}
