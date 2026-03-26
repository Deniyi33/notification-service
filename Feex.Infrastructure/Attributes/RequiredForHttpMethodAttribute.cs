using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Feex.Infrastructure.Attributes
{
    public class RequiredForHttpMethodAttribute : ValidationAttribute
    {
        public string? Methods { get; set; } // Accepts comma-separated HTTP methods

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();
            var httpMethod = httpContextAccessor?.HttpContext?.Request.Method;

            string propertyName = validationContext.MemberName ?? "Value";

            // If no methods are specified, require it for all HTTP methods
            if (string.IsNullOrEmpty(Methods) || Methods.Split(',').Select(m => m.Trim().ToUpper())
                .Contains(httpMethod?.ToUpper()))
            {
                bool isEmpty = value == null || (value is string str && string.IsNullOrWhiteSpace(str));

                if (isEmpty)
                {
                    string errorMessage = ErrorMessage ?? $"{propertyName} is required for this requests.";
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
