using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class DeleteEmployeeHandlers : IRequestHandler<DeleteEmployeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeHandlers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(request.Id);
            if (employee == null)
            {
                return 0;
            }
            await _unitOfWork.EmployeeRepository.DeleteEmployeeAsync(request.Id);
            int affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return affectedRows;
        }
    }
}