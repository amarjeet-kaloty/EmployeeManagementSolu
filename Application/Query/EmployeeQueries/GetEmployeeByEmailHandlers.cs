using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var readEmployeeDTO = _mapper.Map<ReadEmployeeDTO>(employee);

            return readEmployeeDTO;
        }
    }
}