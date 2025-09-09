using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeListHandlers : IRequestHandler<GetEmployeeListQuery, List<ReadEmployeeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeListHandlers(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ReadEmployeeDTO>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            var employeeList = await _unitOfWork.EmployeeRepository.GetEmployeeListAsync();
            var readEmployeeDTO = _mapper.Map<List<ReadEmployeeDTO>>(employeeList);

            return readEmployeeDTO;
        }
    }
}