using FluentValidation;

namespace Payroll.Application.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeByEmployeeNumberValidator 
        : AbstractValidator<DeleteEmployeeByEmployeeNumber>
    {
        public DeleteEmployeeByEmployeeNumberValidator()
        {
            RuleFor(p => p.EmployeeNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");
        }
    }
}