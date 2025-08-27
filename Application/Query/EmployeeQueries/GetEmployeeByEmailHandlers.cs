using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailHandlers : IRequestHandler<GetEmployeeByEmailQuery, EmployeeDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeByEmailHandlers(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDTO> Handle(GetEmployeeByEmailQuery request, CancellationToken cancellationToken)
        {
            Employee employee = await _unitOfWork.EmployeeRepository.GetEmployeeByEmailAsync(request.Email);
            return _mapper.Map<EmployeeDTO>(employee);
        }
    }
}
