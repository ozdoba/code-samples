using System;
using FluentValidation;

namespace Payroll.Application.Employees.Commands.AddIdDocument
{
    public class AddIdDocumentCommandValidator : AbstractValidator<AddIdDocumentCommand>
    {
        public AddIdDocumentCommandValidator()
        {
            RuleFor(p => p.EmployeeNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");
            
            RuleFor(p => p.IdType)
                .NotNull();

            RuleFor(p => p.DocumentNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.IssuedBy)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.IssuedAt)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.IssueDate)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Must(DateIsInThePast).WithMessage("{PropertyName} must be in the past");

            RuleFor(p => p.ExpiryDate)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Must(DateIsInTheFuture).WithMessage("{PropertyName} must be in the future");

            RuleFor(p => p.Content)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
        }
        
        private bool DateIsInThePast(DateTime date)
        {
            return date < DateTime.Now;
        }
        private bool DateIsInTheFuture(DateTime date)
        {
            return date > DateTime.Now;
        }
    }
}