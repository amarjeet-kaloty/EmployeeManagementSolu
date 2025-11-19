using AutoMapper;
using EmployeeManagementSolu.Application.DTOs;
using EmployeeManagementSolu.Domain.Entities;
using MongoDB.Bson;

namespace Application.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, ReadEmployeeDTO>().ReverseMap();

            CreateMap<UpdateEmployeeDTO, Employee>().ReverseMap();

            CreateMap<CreateEmployeeDTO, Employee>()
                .ConstructUsing(dto => new Employee(
                    ObjectId.GenerateNewId(),
                    dto.Name,
                    dto.Address,
                    dto.Email,
                    dto.Phone,
                    dto.Age,                 
                    dto.Salary,              
                    dto.IsActive,           
                    dto.JoiningDate,
                    dto.DepartmentId
                ))
                .ReverseMap();

            CreateMap<ReadEmployeeDTO, EmployeeBasicDTO>();
        }
    }
}