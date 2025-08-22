using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Events;
using EmployeeManagementSolu.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class CreateEmployeeHandlers : IRequestHandler<CreateEmployeeCommand, Employee>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IValidator<Employee> _employeeValidator;

        public CreateEmployeeHandlers(IUnitOfWork unitOfWork, IMediator mediator, IValidator<Employee> employeeValidator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _employeeValidator = employeeValidator;
        }

        public async Task<Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
           
            Employee employee = Employee.Create(request.Name, request.Address, request.Email, request.Phone);
            
            var validationResult = await _employeeValidator.ValidateAsync(employee);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.EmployeeRepository.AddEmployeeAsync(employee);

            await _mediator.Publish(new EmployeeCreatedEvent(employee), cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return employee;
        }
    }
}