using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailHandlers : IRequestHandler<GetEmployeeByEmailQuery, Employee>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeByEmailHandlers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Employee> Handle(GetEmployeeByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.EmployeeRepository.GetEmployeeByEmailAsync(request.Email);
        }
    }
}
