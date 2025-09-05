using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Events;
using EmployeeManagementSolu.Domain.Interfaces;
using EmployeeManagementSolu.Domain.Validation;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class CreateEmployeeHandlers : IRequestHandler<CreateEmployeeDTO, ReadEmployeeDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateEmployeeHandlers(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ReadEmployeeDTO> Handle(CreateEmployeeDTO request, CancellationToken cancellationToken)
        {
            Employee employee = Employee.Create(request.Name, request.Address, request.Email, request.Phone);

            await _unitOfWork.EmployeeRepository.AddEmployeeAsync(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new EmployeeCreatedEvent(employee), cancellationToken);

            return _mapper.Map<ReadEmployeeDTO>(employee);
        }
    }
}