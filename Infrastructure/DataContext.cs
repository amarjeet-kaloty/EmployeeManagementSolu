
using EmployeeManagementSolu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSolu.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}