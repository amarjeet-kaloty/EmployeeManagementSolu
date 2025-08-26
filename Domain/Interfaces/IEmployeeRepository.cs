using EmployeeManagementSolu.Domain.Entities;

namespace EmployeeManagementSolu.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task AddEmployeeAsync(Employee employee);
        public Task<int> DeleteEmployeeAsync(string id);
        public Task<Employee> GetEmployeeByIdAsync(string id);
        public Task<List<Employee>> GetEmployeeListAsync();
        public void UpdateEmployee(Employee employee);
        public Task<Employee> GetEmployeeByEmailAsync(string email);
    }
}