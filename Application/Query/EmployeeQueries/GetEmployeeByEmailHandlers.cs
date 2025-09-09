using Application.Exceptions;
using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailHandlers : IRequestHandler<GetEmployeeByEmailQuery, ReadEmployeeDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeByEmailHandlers(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReadEmployeeDTO> Handle(GetEmployeeByEmailQuery request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByEmailAsync(request.Email);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with email {request.Email} not found.");
            }
            ReadEmployeeDTO readEmployeeDTO = _mapper.Map<ReadEmployeeDTO>(employee);

            return readEmployeeDTO;
        }
    }
}