using EmployeeManagementSolu.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Infrastructure
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToCollection("employees");

            builder.OwnsOne(e => e.Name, name =>
            {
                name.Property(n => n.FullName).HasElementName("fullName");
            });

            builder.Property(e => e.Address).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.Phone).IsRequired(false);
        }
    }
}
