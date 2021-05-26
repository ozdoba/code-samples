using FluentValidation;
using Payroll.Application.Employees.DeleteEmployee;

namespace Marcura.Payroll.Employees.Application.Employees.Commands
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