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
        private readonly EmployeeValidationService _validationService;

        public CreateEmployeeHandlers(
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IMapper mapper,
            EmployeeValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<ReadEmployeeDTO> Handle(CreateEmployeeDTO request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Employee>(request);
            await _validationService.ValidateAsync(employee);
            await _unitOfWork.EmployeeRepository.AddEmployeeAsync(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var readEmployeeDTO = _mapper.Map<ReadEmployeeDTO>(employee);

            return readEmployeeDTO;
        }
    }
}