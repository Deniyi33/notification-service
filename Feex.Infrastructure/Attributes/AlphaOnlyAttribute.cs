using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Feex.Infrastructure.Attributes
{
    public class AlphaOnlyAttribute : ValidationAttribute
    {
        private const string Pattern = @"^(?!.*\s{2,})[a-zA-Z]([a-zA-Z\s'-]*[a-zA-Z])?$"; // Allows letters, spaces, hyphens, and apostrophes

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Allow null values (not required)
            }

            string input = value.ToString() ?? string.Empty;
            string propertyName = validationContext.MemberName ?? "Value";


            if (!Regex.IsMatch(input, Pattern))
            {
                return new ValidationResult(ErrorMessage ?? $"{propertyName} can only contain letters, spaces, hyphens, and apostrophes.");
            }

            return ValidationResult.Success;
        }
    }
}
