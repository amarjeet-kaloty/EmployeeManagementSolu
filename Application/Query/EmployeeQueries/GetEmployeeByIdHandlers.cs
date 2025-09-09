using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;

namespace EmployeeManagementSolu.Application.Query.EmployeeQueries
{
    public class GetEmployeeByIdHandlers : IRequestHandler<GetEmployeeByIdQuery, ReadEmployeeDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeByIdHandlers(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReadEmployeeDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(request.Id);
            var readEmployeeDTO = _mapper.Map<ReadEmployeeDTO>(employee);

            return readEmployeeDTO;
        }
    }
}