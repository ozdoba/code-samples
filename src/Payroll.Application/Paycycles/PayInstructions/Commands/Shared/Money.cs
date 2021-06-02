using FluentValidation;

namespace Payroll.Application.Paycycles.PayInstructions.Commands.Shared
{
    public class Money
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }

    public class MoneyValidator : AbstractValidator<Money>
    {
        public MoneyValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Currency)
                .NotNull().WithMessage("{PropertyName} is required");
        }
    }
}