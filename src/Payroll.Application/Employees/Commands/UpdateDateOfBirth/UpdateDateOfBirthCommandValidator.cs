using System;
using FluentValidation;

namespace Payroll.Application.Employees.Commands.UpdateDateOfBirth
{
    public class UpdateDateOfBirthCommandValidator : AbstractValidator<UpdateDateOfBirthCommand>
    {
        public UpdateDateOfBirthCommandValidator()
        {
            RuleFor(p => p.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Must(DateIsInThePast).WithMessage("{PropertyName} must be in the past")
                .Must(AgeIsNotGreaterThan(70));
        }
        
        private bool DateIsInThePast(DateTime date)
        {
            return date < DateTime.Now;
        }

        private Func<DateTime, bool> AgeIsNotGreaterThan(int years)
            => date => DifferenceInYears(date, DateTime.Now) <= years;
        
        public static int? DifferenceInYears(DateTime birthday, DateTime now)  
        {  
            // return null if the birthday is not in the past
            if (birthday > now)  
                return null;  
            // get the basic number of years  
            int years = now.Year - birthday.Year;  
            // adjust the years against this year's birthday  
            if (now.Month < birthday.Month ||  
                (now.Month == birthday.Month &&  
                 now.Day < birthday.Day))  
            {  
                years--;  
            }  
            // Don't return a negative number for years alive  
            return (years >= 0) ? years : 0;
        }  
    }
}