using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Application.Query.EmployeeQueries;
using EmployeeManagementSolu.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Query.EmployeeQueries
{
    public class GetEmployeeByEmailHandlers : IRequestHandler<GetEmployeeByEmailQuery, EmployeeSearchDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeByEmailHandlers(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeSearchDTO> Handle(GetEmployeeByEmailQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.EmployeeRepository.GetAllAsQueryable()
                .Where(e => e.Email == request.Email);

            EmployeeSearchDTO? employeeDto = await query.ProjectTo<EmployeeSearchDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return employeeDto!;
        }
    }
}