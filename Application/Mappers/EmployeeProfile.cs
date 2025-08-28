using Application.DTOs;
using AutoMapper;
using EmployeeManagementSolu.Domain.Entities;

namespace Application.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Mapper returning full Employee DTO
            CreateMap<Employee, EmployeeResponseDTO>();

            // Mapper returning partial Employee DTO using ForMember
            CreateMap<Employee, EmployeeSearchDTO>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.Email));

            CreateProjection<Employee, EmployeeResponseDTO>();
        }
    }
}
