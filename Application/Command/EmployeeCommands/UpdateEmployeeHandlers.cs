using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class UpdateEmployeeHandlers : IRequestHandler<UpdateEmployeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeHandlers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(request.Id);

            if (employee == null)
                return 0;

            employee.UpdateDetails(request.Name, request.Address, request.Email, request.Phone);

            _unitOfWork.EmployeeRepository.UpdateEmployee(employee);

            int affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return affectedRows;
        }
    }
}