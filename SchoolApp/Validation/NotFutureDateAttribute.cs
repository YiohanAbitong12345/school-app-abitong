
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Validation
{
    // Custom rule to prevent future birth dates.
    public class NotFutureDateAttribute : ValidationAttribute
    {
        // Checks the date entered by the user.
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            // A date later than today is invalid.
            if (value is DateTime date && date.Date > DateTime.Today)
            {
                return new ValidationResult(
                    ErrorMessage ?? "The date can't be in the future.");
            }

            // Other values, including an empty date, pass this rule.
            return ValidationResult.Success;
        }
    }
}
