using FluentValidation;
using FluentValidation.Validators;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Application.Common.Validation
{
    internal class CountryCodeValidator : PropertyValidator
    {
        private readonly ICountryLookup _countryLookup;
        internal CountryCodeValidator(ICountryLookup countryLookup)
        {
            _countryLookup = countryLookup;
        }

        protected override string GetDefaultMessageTemplate()
        {
            return "'{PropertyValue}' is not a valid Country Code";
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var isoCode = (string) context.PropertyValue;

            if (string.IsNullOrEmpty(isoCode))
            {
                return true;
            }

            return _countryLookup.IsKnownIsoCode(isoCode);
        }
    }
    
    public static class CountryIsoCodeValidatorExtension 
    {
        public static IRuleBuilderOptions<T, string> CountryIsoCode<T>(
            this IRuleBuilder<T, string> rule, ICountryLookup countryLookup
        ) 
        {
            return rule.SetValidator(new CountryCodeValidator(countryLookup));
        }
    }
}