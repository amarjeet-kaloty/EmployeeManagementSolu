using EmployeeManagementSolu.Domain.Entities;
using FluentValidation;

namespace EmployeeManagementSolu.Application.Validation
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator() 
        {
            RuleFor(emp => emp.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(emp => emp.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(emp => emp.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");
        }
    }
}
