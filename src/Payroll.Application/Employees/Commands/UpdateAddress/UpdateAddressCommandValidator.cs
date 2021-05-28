using FluentValidation;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Employees.Commands.Shared;

namespace Payroll.Application.Employees.Commands.UpdateAddress
{
    public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressCommandValidator(ICountryLookup countryLookup)
        {
            RuleFor(p => p.Address)
                .NotNull()
                .SetValidator(new AddressValidator(countryLookup));
        }
    }
}