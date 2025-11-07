using EmployeeManagementSolu.Domain.Entities;

namespace EmployeeManagementSolu.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task AddEmployeeAsync(Employee employee);
        public void UpdateEmployee(Employee employee);
        public Task<int> DeleteEmployeeAsync(string id);
        public Task<List<Employee>> GetEmployeeListAsync();
        public Task<Employee> GetEmployeeByIdAsync(string id);
        public Task<Employee> GetEmployeeByEmailAsync(string email);
        public Task<IEnumerable<Employee>> GetByDepartmentIdAsync(Guid departmentId);
    }
}