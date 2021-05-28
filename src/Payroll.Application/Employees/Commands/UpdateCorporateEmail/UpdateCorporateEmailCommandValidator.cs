using FluentValidation;

namespace Payroll.Application.Employees.Commands.UpdateCorporateEmail
{
    public class UpdateCorporateEmailCommandValidator : AbstractValidator<UpdateCorporateEmailCommand>
    {
        public UpdateCorporateEmailCommandValidator()
        {
            RuleFor(p => p.CorporateEmail)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .EmailAddress();

        }
    }
}