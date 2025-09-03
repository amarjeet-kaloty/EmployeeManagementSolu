using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Events;
using EmployeeManagementSolu.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class CreateEmployeeHandlers : IRequestHandler<CreateEmployeeCommand, ReadEmployeeDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IValidator<Employee> _employeeValidator;
        private readonly IMapper _mapper;

        public CreateEmployeeHandlers(IUnitOfWork unitOfWork, IMediator mediator, IValidator<Employee> employeeValidator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _employeeValidator = employeeValidator;
            _mapper = mapper;
        }

        public async Task<ReadEmployeeDTO> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee employee = Employee.Create(request.Name, request.Address, request.Email, request.Phone);
            ValidationResult validationResult = await _employeeValidator.ValidateAsync(employee);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _unitOfWork.EmployeeRepository.AddEmployeeAsync(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new EmployeeCreatedEvent(employee), cancellationToken);

            return _mapper.Map<ReadEmployeeDTO>(employee);
        }
    }
}