using System;
using FluentValidation;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Common.Validation;
using Payroll.Application.Employees.Commands.Shared;

namespace Payroll.Application.Employees.Commands.RegisterEmployee
{
    public class RegisterEmployeeCommandValidator : AbstractValidator<RegisterEmployeeCommand>
    {
        public RegisterEmployeeCommandValidator(ICountryLookup countryLookup)
        {
            RuleFor(p => p.EmployeeNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");
            
            RuleFor(p => p.JobTitle)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");
            
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(p => p.MiddleName)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(p => p.Address)
                .NotNull()
                .SetValidator(new AddressValidator(countryLookup));
            
            RuleFor(p => p.CorporateEmail)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .EmailAddress();
            
            RuleFor(p => p.PrivateEmail)
                .EmailAddress();
            
            RuleFor(p => p.MobileNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .PhoneNumber();

            RuleFor(p => p.Nationality)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50);

            RuleFor(p => p.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Must(DateIsInThePast).WithMessage("{PropertyName} must be in the past");
            
            RuleFor(p => p.DateOfEmployment)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
        }

        private bool DateIsInThePast(DateTime date)
        {
            return date < DateTime.Now;
        }
    }
}