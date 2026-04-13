using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EMI.Infrastructure.Attributes
{
    public class FixedLengthAttribute : ValidationAttribute
    {
        public int MinLength { get; set; } = 1;
        public int MaxLength { get; set; }
        public int ExactLength { get; set; }
        public bool IsDigit { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Allow null values (not required)
            }

            string valueString = value.ToString() ?? string.Empty;
            string propertyName = validationContext.MemberName ?? "Value";

            // Prioritize ExactLength if provided
            if (ExactLength > 0 && valueString.Length != ExactLength)
            {
                return new ValidationResult(ErrorMessage ?? $"{propertyName} must be exactly {ExactLength} characters.");
            }
            // Validate min and max length if ExactLength is not set
            else 
            {
                if(MaxLength > 0 && valueString.Length > MaxLength)
                {
                    return new ValidationResult(ErrorMessage ?? $"{propertyName} must be between {MinLength} and {MaxLength} characters.");
                }

                if (valueString.Length < MinLength)
                {
                    return new ValidationResult(ErrorMessage ?? $"{propertyName} cannot be empty"); 
                }
            }

            // Check if the value should only contain digits
            if (IsDigit && !Regex.IsMatch(valueString, @"^\d+$"))
            {
                return new ValidationResult(ErrorMessage ?? $"{propertyName} must contain only digits.");
            }

            return ValidationResult.Success;
        }
    }
}
