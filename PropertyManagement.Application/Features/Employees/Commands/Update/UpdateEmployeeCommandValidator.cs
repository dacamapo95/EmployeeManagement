using FluentValidation;

namespace EmployeeManagement.Application.Features.Employees.Commands.Update;

public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.CompanyId)
            .GreaterThan(0).WithMessage("Company is required.");

        RuleFor(x => x.PortalId)
            .GreaterThan(0).WithMessage("Portal is required.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Role is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Telephone)
            .MaximumLength(50).WithMessage("Telephone must not exceed 50 characters.");

        RuleFor(x => x.Fax)
            .MaximumLength(50).WithMessage("Fax must not exceed 50 characters.");

        RuleFor(x => x.StatusId)
            .GreaterThan(0).WithMessage("Status is required.");
    }
}
