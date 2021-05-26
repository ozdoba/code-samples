using FluentValidation;

namespace Payroll.Application.Paycycles.Commands.CreatePaycycle
{
    public class CreatePaycycleCommandValidator : AbstractValidator<CreatePaycycleCommand>
    {
        public CreatePaycycleCommandValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate");
            
            RuleFor(x => x.Payday)
                .NotNull();
        }
    }
}