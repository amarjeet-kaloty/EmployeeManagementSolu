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
            IQueryable query = _unitOfWork.EmployeeRepository.GetAllAsQueryable()
                .Where(e => e.Email == request.Email);
            ReadEmployeeDTO? employeeDto = await query.ProjectTo<ReadEmployeeDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return employeeDto!;
        }
    }
}