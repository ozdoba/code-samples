using FluentValidation;
using Payroll.Application.Paycycles.PayInstructions.Commands.Shared;

namespace Payroll.Application.Paycycles.PayInstructions.Commands.UpdatePayInstruction
{
    public class UpdatePayInstructionCommandValidator : AbstractValidator<UpdatePayInstructionCommand>
    {
        public UpdatePayInstructionCommandValidator()
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