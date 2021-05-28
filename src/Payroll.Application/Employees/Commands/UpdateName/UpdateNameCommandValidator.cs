using FluentValidation;

namespace Payroll.Application.Employees.Commands.UpdateName
{
    public class UpdateNameCommandValidator : AbstractValidator<UpdateNameCommand>
    {
        public UpdateNameCommandValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(p => p.MiddleName)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");
        }
    }
}