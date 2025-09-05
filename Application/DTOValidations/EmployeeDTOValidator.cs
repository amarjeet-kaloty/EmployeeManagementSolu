using EmployeeManagementSolu.Application.DTOs;
using FluentValidation;

namespace Application.Validations
{
    public class EmployeeDTOValidator : AbstractValidator<CreateEmployeeDTO>
    {
        public EmployeeDTOValidator()
        {
            RuleFor(emp => emp.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(emp => emp.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

            RuleFor(emp => emp.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");
        }
    }
}
