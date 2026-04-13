using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EMI.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredIfAnyAttribute : ValidationAttribute
    {
        public string OtherProperty { get; private set; }
        public string OtherPropertyDisplayName { get; private set; }
        public object[] OtherPropertyValues { get; private set; }
        public object[] OtherPropertyValuesToCheck { get; private set; }

        public RequiredIfAnyAttribute(string otherProperty, params object[] otherPropertyValues)
            : base("'{0}' is required because '{1}' has a value.")
        {
            OtherProperty = otherProperty;
            OtherPropertyValues = otherPropertyValues;
            OtherPropertyValuesToCheck = (object[])otherPropertyValues.Clone();
        }

        public override string FormatErrorMessage(string name)
        {
            if(OtherPropertyValues.Length > 1)
            {
                if (!OtherPropertyValues[^1].ToString().Contains("or"))
                {
                    OtherPropertyValues[OtherPropertyValues.Length - 1] = $"or {OtherPropertyValues[OtherPropertyValues.Length - 1]}";
                }
            }

            var otherValuesString = string.Join(OtherPropertyValues.Length > 2 ? ", " : " ", OtherPropertyValues);
            return string.Format(CultureInfo.CurrentCulture, 
                ErrorMessageString, 
                name, 
                OtherPropertyDisplayName ?? OtherProperty, 
                otherValuesString);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }

            var otherProperty = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherProperty == null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, $"Could not find a property named {OtherProperty}."));
            }

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            //check if the value is actually required and validate it
            if(OtherPropertyValuesToCheck.Any(a => Equals(otherValue, a)))
            {
                if(value == null)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                //additional check for string so that they aren't empty
                if (value is string val && val.Trim().Length == 0)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                //Additional check for lists so that they aren't empty
                switch (value)
                {
                    case Array array:
                        if(array.Length == 0)
                        {
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }
                        break;

                    case IList list:
                        if (list.Count == 0)
                        {
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }
                        break;

                    case ICollection collection:
                        if (collection.Count == 0)
                        {
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }
                        break;

                    case HashSet<Guid> set:
                        if (set.Count == 0)
                        {
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }
                        break;
                }
            }
            return ValidationResult.Success;
        }
    }
}
