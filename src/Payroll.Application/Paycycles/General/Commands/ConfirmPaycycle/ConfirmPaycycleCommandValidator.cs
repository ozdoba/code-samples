using FluentValidation;

namespace Payroll.Application.Paycycles.General.Commands.ConfirmPaycycle
{
    public class ConfirmPaycycleCommandValidator : AbstractValidator<ConfirmPaycycleCommand>
    {
        public ConfirmPaycycleCommandValidator()
        {
            RuleFor(x => x.PaycycleId)
                .NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}