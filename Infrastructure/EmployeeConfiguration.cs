using EmployeeManagementSolu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Infrastructure
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToCollection("employees");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasElementName("Employee_Name");

            builder.Property(e => e.Address)
                .IsRequired().HasMaxLength(200)
                .HasElementName("Employee_Address");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasElementName("Employee_Email");

            builder.Property(e => e.Phone)
                .IsRequired(false)
                .HasElementName("Employee_Phone");

            builder.Property(e => e.DepartmentId)
                .IsRequired()
                .HasElementName("Employee_DepartmentId");
        }
    }
}