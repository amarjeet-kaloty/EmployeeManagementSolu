using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSolu.Infrastructure
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _dbContext;

        public EmployeeRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            _dbContext.Employees.Update(employee);
        }

        public async Task<int> DeleteEmployeeAsync(string id)
        {
            _dbContext.Employees.Remove(await _dbContext.Employees.FindAsync(id));
            return 1;
        }

        public async Task<List<Employee>> GetEmployeeListAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(string id)
        {
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
        }
    }
}