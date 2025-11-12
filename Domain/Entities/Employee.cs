using MongoDB.Bson;

namespace EmployeeManagementSolu.Domain.Entities
{
    public class Employee : Entity
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }
        public int Age { get; private set; }
        public decimal Salary { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime JoiningDate { get; private set; }
        public Guid DepartmentId { get; private set; }

        public Employee() : base()
        {
        }

        public Employee(
            ObjectId id,
            string name,
            string address,
            string email,
            string? phone,
            int age,
            decimal salary,
            bool isActive,
            DateTime joiningDate,
            Guid departmentId) : base(id.ToString())
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            Age = age;
            Salary = salary;
            IsActive = isActive;
            JoiningDate = joiningDate;
            DepartmentId = departmentId;
        }
    }
}