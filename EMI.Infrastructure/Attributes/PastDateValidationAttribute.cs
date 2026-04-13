using System.ComponentModel.DataAnnotations;

namespace EMI.Infrastructure.Attributes
{
    public class PastDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Allow null values (not required)
            }

            string propertyName = validationContext.MemberName ?? "Date";

            if (value is DateTime date && date.Date > DateTime.Today)
            {
                return new ValidationResult($"{propertyName} must not be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
