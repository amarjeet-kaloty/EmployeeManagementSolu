using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EmployeeManagementSolu.Domain.Entities
{
    public class EmployeeName
    {
        [BsonElement("fullName")]
        public string FullName { get; set; }

        public EmployeeName() : base()
        {
        }

        public EmployeeName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Employee name cannot be null or empty.", nameof(fullName));
            }

            if (fullName.Length > 50)
            {
                throw new ArgumentException("Employee name cannot exceed 50 characters.", nameof(fullName));
            }

            FullName = fullName;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}