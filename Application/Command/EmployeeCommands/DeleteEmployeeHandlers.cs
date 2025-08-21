using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class DeleteEmployeeHandlers : IRequestHandler<DeleteEmployeeCommand, int>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeHandlers(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee employee = await _employeeRepository.GetEmployeeByIdAsync(request.Id);
            if (employee == null)
            {
                return 0;
            }

            await _employeeRepository.DeleteEmployeeAsync(request.Id);

            int affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return affectedRows;
        }
    }
}