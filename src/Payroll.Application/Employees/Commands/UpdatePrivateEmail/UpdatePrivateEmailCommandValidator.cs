using FluentValidation;

namespace Payroll.Application.Employees.Commands.UpdatePrivateEmail
{
    public class UpdatePrivateEmailCommandValidator : AbstractValidator<UpdatePrivateEmailCommand>
    {
        public UpdatePrivateEmailCommandValidator()
        {
            RuleFor(p => p.PrivateEmail)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .EmailAddress();

        }
    }
}