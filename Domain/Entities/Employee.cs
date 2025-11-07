using MongoDB.Bson;

namespace EmployeeManagementSolu.Domain.Entities
{
    public class Employee : Entity
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }
        public Guid DepartmentId { get; private set; }

        public Employee() : base()
        {
        }

        public Employee(ObjectId id, string name, string address, string email, string? phone, Guid departmentId) : base(id.ToString())
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            DepartmentId = departmentId;
        }
    }
}