using FluentValidation;
using Payroll.Application.Common.Validation;

namespace Payroll.Application.Employees.Commands.UpdateMobileNumber
{
    public class UpdateMobileNumberCommandValidator : AbstractValidator<UpdateMobileNumberCommand>
    {
        public UpdateMobileNumberCommandValidator()
        {
            RuleFor(p => p.MobileNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .PhoneNumber();
        }
    }
}