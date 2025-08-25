using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSolu.Domain.Entities
{
    public class Employee : Entity
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }

        public Employee() : base()
        {
        }

        public Employee(string id, string name, string address, string email, string? phone) : base(id)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
        }

        public static Employee Create(string name, string address, string email, string? phone)
        {
            Employee employee = new Employee(
                ObjectId.GenerateNewId().ToString(),
                name,
                address,
                email,
                phone);

            return employee;
        }

        public void UpdateDetails(string name, string address, string email, string? phone)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
        }
    }
}