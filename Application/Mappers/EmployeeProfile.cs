using AutoMapper;
using EmployeeManagementSolu.Application.Command.EmployeeCommands;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Entities;

namespace Application.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDTO, CreateEmployeeCommand>();

            CreateMap<UpdateEmployeeDTO, UpdateEmployeeCommand>();

            CreateMap<UpdateEmployeeCommand, Employee>();

            CreateMap<Employee, ReadEmployeeDTO>();
        }
    }
}