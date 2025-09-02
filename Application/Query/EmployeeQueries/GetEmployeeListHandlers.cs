using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeListHandlers : IRequestHandler<GetEmployeeListQuery, List<EmployeeResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeListHandlers(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeResponseDTO>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            var employeeList = await _unitOfWork.EmployeeRepository.GetEmployeeListAsync();
            return _mapper.Map<List<EmployeeResponseDTO>>(employeeList);
        }
    }
}