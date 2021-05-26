using FluentValidation;
using FluentValidation.Validators;
using PhoneNumbers;

namespace Payroll.Application.Common.Validation
{
    public class PhoneNumberValidator : PropertyValidator 
    {
        protected override string GetDefaultMessageTemplate()
        {
            return "'{PropertyValue}' is not a valid phone number.";
        }

        protected override bool IsValid(PropertyValidatorContext context) 
        {
            var phoneNumber = (string) context.PropertyValue;
            if (string.IsNullOrEmpty(phoneNumber)) 
            {
                return true;
            }

            return PhoneNumberUtil.IsViablePhoneNumber(PhoneNumberUtil.ExtractPossibleNumber(phoneNumber));
        }
    }

    public static class PhoneNumberValidatorExtension 
    {
        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(
            this IRuleBuilder<T, string> rule
        ) {
            return rule.SetValidator(new PhoneNumberValidator());
        }
    }
}