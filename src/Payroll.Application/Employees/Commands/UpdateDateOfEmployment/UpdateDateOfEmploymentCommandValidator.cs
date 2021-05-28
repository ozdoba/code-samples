using FluentValidation;

namespace Payroll.Application.Employees.Commands.UpdateDateOfEmployment
{
    public class UpdateDateOfEmploymentCommandValidator : AbstractValidator<UpdateDateOfEmploymentCommand>
    {
        public UpdateDateOfEmploymentCommandValidator()
        {
            RuleFor(p => p.DateOfEmployment)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
        }
    }
}