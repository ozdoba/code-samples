using FluentValidation;

namespace Payroll.Application.Employees.Commands.UpdateTaxNumber
{
    public class UpdateTaxNumberValidator : AbstractValidator<UpdateTaxNumberCommand>
    {
        public UpdateTaxNumberValidator()
        {
            RuleFor(p=>p.LocalTaxNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");
        }
    }
}