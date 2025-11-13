using Domain.Services;
using EmployeeManagementSolu.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementSolu.Domain.Validation
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<EmployeeValidator> _logger;

        public EmployeeValidator(IDepartmentService departmentService, ILogger<EmployeeValidator> logger)
        {
            _departmentService = departmentService;
            _logger = logger;

            RuleFor(emp => emp.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(emp => emp.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

            RuleFor(emp => emp.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(emp => emp.DepartmentId)
           .NotEmpty().WithMessage("DepartmentId is required.")
            .CustomAsync(async (departmentId, validationContext, cancellationToken) =>
            {
                try
                {
                    await _departmentService.ValidateDepartmentExistsAsync(departmentId, cancellationToken);
                }
                catch (Exception ex)
                {
                    validationContext.AddFailure($"Department validation failed: {ex.Message}");
                }
            });
        }
    }
}