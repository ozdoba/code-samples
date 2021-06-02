using FluentValidation;
using Payroll.Application.Paycycles.PayInstructions.Commands.Shared;

namespace Payroll.Application.Paycycles.PayInstructions.Commands.AddPayInstruction
{
    public class AddPayInstructionCommandValidator : AbstractValidator<AddPayInstructionCommand>
    {
        public AddPayInstructionCommandValidator()
        {
            RuleFor(x => x.PayCode)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");
            RuleFor(x => x.TotalAmount)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .SetValidator(new MoneyValidator());
        }
    }
}