using System;
using FluentValidation;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Common.Validation;
using Payroll.Application.Employees.Shared;

namespace Payroll.Application.Employees.UpdateEmployee
{
    public class UpdateEmployeeDetailsCommandValidator : AbstractValidator<UpdateEmployeeDetailsCommand>
    {
        public UpdateEmployeeDetailsCommandValidator(ICountryLookup countryLookup) 
        {
            RuleFor(p => p.JobTitle)
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");
            
            RuleFor(p => p.FirstName)
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(p => p.MiddleName)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(p => p.LastName)
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

            RuleFor(p => p.Address)
                .NotNull()
                .SetValidator(new AddressValidator(countryLookup));
            
            RuleFor(p => p.CorporateEmail)
                .NotNull()
                .EmailAddress();
            
            RuleFor(p => p.PrivateEmail)
                .NotNull()
                .EmailAddress();
            
            RuleFor(p => p.MobileNumber)
                .NotNull()
                .PhoneNumber();

            RuleFor(p => p.Nationality)
                .NotNull()
                .MaximumLength(50);

            RuleFor(p => p.DateOfBirth)
                .NotNull()
                .Must(DateIsInThePast).WithMessage("{PropertyName} must be in the past");

            RuleFor(p => p.DateOfEmployment)
                .NotNull();
        }
        
        private bool DateIsInThePast(DateTime date)
        {
            return date < DateTime.Now;
        }
    }
}