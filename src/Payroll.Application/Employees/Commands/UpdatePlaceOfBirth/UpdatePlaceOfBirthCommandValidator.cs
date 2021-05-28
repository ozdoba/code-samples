using FluentValidation;

namespace Payroll.Application.Employees.Commands.UpdatePlaceOfBirth
{
    public class UpdatePlaceOfBirthCommandValidator : AbstractValidator<UpdatePlaceOfBirthCommand>
    {
        public UpdatePlaceOfBirthCommandValidator()
        {
            RuleFor(p=>p.PlaceOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");
        }
    }
}