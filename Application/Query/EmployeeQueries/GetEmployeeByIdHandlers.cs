using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeByIdHandlers : IRequestHandler<GetEmployeeByIdQuery, Employee>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeByIdHandlers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(request.Id);
        }
    }
}