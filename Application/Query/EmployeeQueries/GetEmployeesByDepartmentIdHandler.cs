using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeesByDepartmentIdHandler : IRequestHandler<GetEmployeeByDepartmentIdQuery, IEnumerable<ReadEmployeeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeesByDepartmentIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadEmployeeDTO>> Handle(GetEmployeeByDepartmentIdQuery request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.EmployeeRepository.GetByDepartmentIdAsync(request.DepartmentId);

            if (employees == null)
            {
                return Enumerable.Empty<ReadEmployeeDTO>();
            }
            IEnumerable<ReadEmployeeDTO> readEmployeeDTO = _mapper.Map<IEnumerable<ReadEmployeeDTO>>(employees);
            return readEmployeeDTO;
        }
    }
}
