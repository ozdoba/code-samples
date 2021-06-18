using FluentValidation;

namespace Payroll.Application.Paycycles.PayCodes.Commands.AddPayCode
{
    public class AddPayCodeCommandValidator : AbstractValidator<AddPayCodeCommand>
    {
        public AddPayCodeCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(35).WithMessage("{PropertyName} length must not exceed 35 characters");
            RuleFor(x=>x.Description)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(250).WithMessage("{PropertyName} length must not exceed 250 characters");
            RuleFor(x => x.Type)
                .NotNull().WithMessage("{PropertyName} is required");
        }
    }
}