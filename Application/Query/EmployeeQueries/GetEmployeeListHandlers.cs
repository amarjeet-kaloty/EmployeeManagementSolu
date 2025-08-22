using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeListHandlers : IRequestHandler<GetEmployeeListQuery, List<Employee>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeListHandlers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Employee>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.EmployeeRepository.GetEmployeeListAsync();
        }
    }
}