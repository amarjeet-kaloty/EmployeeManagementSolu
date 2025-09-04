using EmployeeManagementSolu.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace EmployeeManagementSolu.Domain.Validation
{
    public class EmployeeValidationService
    {
        private readonly IValidator<Employee> _validator;

        public EmployeeValidationService(IValidator<Employee> validator)
        {
            _validator = validator;
        }

        public async Task ValidateAsync(Employee employee)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(employee);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }

}
