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

            CreateMap<Employee, ReadEmployeeDTO>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone ?? string.Empty));

            CreateMap<Employee, EmployeeSearchDTO>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.Email));
        }
    }
}