using Application.Exceptions;
using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using EmployeeManagementSolu.Domain.Validation;
using MediatR;

namespace EmployeeManagementSolu.Application.Command.EmployeeCommands
{
    public class UpdateEmployeeHandlers : IRequestHandler<UpdateEmployeeDTO, ReadEmployeeDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly EmployeeValidationService _validationService;

        public UpdateEmployeeHandlers(IUnitOfWork unitOfWork, IMapper mapper, EmployeeValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<ReadEmployeeDTO> Handle(UpdateEmployeeDTO request, CancellationToken cancellationToken)
        {
            Employee employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(request.Id!);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {request.Id} not found.");
            }
            _mapper.Map(request, employee);
            await _validationService.ValidateAsync(employee);
            _unitOfWork.EmployeeRepository.UpdateEmployee(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var readEmployeeDTO = _mapper.Map<ReadEmployeeDTO>(employee);

            return readEmployeeDTO;
        }
    }
}