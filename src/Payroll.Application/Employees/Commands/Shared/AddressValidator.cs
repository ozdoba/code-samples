using FluentValidation;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Common.Validation;

namespace Payroll.Application.Employees.Commands.Shared
{
    public class AddressValidator : AbstractValidator<Address> 
    {
        public AddressValidator(ICountryLookup countryLookup) 
        {
            RuleFor(p => p.BuildingNumber);

            RuleFor(p => p.Street)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.City)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.PostalCode)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Length(2)
                .CountryIsoCode(countryLookup);
        }
    }
}