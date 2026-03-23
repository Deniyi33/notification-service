using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Feex.Core.Enums
{
    /// <summary>
    /// STRICTLY, ALL ENUMS SHOULD FOLLOW SEQUENTIAL NUMBERING
    /// </summary>

    public static class EnumHelper
    {
        public static (bool isValid, TEnum enumValue) IsStringAnEnum<TEnum>(string enumString) where TEnum : struct
        {
            bool isValid = Enum.TryParse<TEnum>(enumString, true, out TEnum enumValue);
            return (isValid, enumValue);
        }


        private static readonly Dictionary<Enum, DescriptionAttribute> EnumDescriptions;

        static EnumHelper()
        {
            EnumDescriptions = GetEnumDescriptions();
        }

        [DebuggerStepThrough]
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            return value.GetMetaData().Description;
        }

        private static DescriptionAttribute GetMetaData<TEnum>(this TEnum value) where TEnum : Enum
        {
            if (!EnumDescriptions.TryGetValue(value, out var metadata))
            {
                var description = value.ToString();
                metadata = new DescriptionAttribute(description);

                if (description != "")
                {
                    EnumDescriptions[value] = metadata;
                }
            }
            return metadata;
        }

        internal static Dictionary<Enum, DescriptionAttribute> GetEnumDescriptions()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var enums = assemblies
                .Where(assembly => assembly.FullName.StartsWith("Feex"))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsEnum)
                .SelectMany(GetEnumValues)
                .ToList();

            var descriptions = enums.ToDictionary(tuple => tuple.Value, elementSelector: GetEnumDescription);

            return descriptions;
        }

        private static IEnumerable<EnumValueTuple> GetEnumValues(Type enumType)
        {
            return enumType.GetEnumValues()
                           .Cast<Enum>()
                           .Select(enumValue => new EnumValueTuple
                           {
                               Value = enumValue,
                               FieldInfo = enumType.GetField(enumValue.ToString())
                           });
        }

        private static DescriptionAttribute GetEnumDescription(EnumValueTuple tuple)
        {
            var defaultDescription = tuple.Value.ToString();

            foreach (var attribute in tuple.FieldInfo.GetCustomAttributes())
            {
                #region TryGetDescriptionFromAttribute

                bool hasDescriptionAttribute = default;
                string? description;

                var descriptionAttribute = attribute as DescriptionAttribute;
                if (descriptionAttribute != null)
                {
                    description = descriptionAttribute.Description;
                    hasDescriptionAttribute = true;
                }
                else description = null;
                #endregion

                if (hasDescriptionAttribute)
                    return new DescriptionAttribute(description);
            }

            return new DescriptionAttribute(defaultDescription);
        }

        private struct EnumValueTuple
        {
            public Enum Value { get; set; }
            public FieldInfo FieldInfo { get; set; }
        }

        public static IEnumerable<EnumValueModel> GetValues<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var enumType = typeof(TEnum);
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var values = Enum.GetValues(enumType).Cast<TEnum>();
            foreach (var value in values)
            {
                var enumValue = value as Enum;
                if (enumValue != null)
                {
                    if (EnumDescriptions.TryGetValue(enumValue, out var metadata))
                    {
                        yield return new EnumValueModel
                        {
                            Name = enumValue.ToString(),
                            Description = metadata.Description,
                            Value = EnumConverter<TEnum>.ToInt32(value)
                        };
                    }
                }
            }
        }

        public class EnumValueModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public object Value { get; set; }
        }

        public static class EnumConverter<TEnum> where TEnum : struct, IConvertible
        {
            public static readonly Func<int, TEnum> FromInt32 = GenerateMethod<int, TEnum>();
            public static readonly Func<long, TEnum> FromInt64 = GenerateMethod<long, TEnum>();
            public static readonly Func<TEnum, int> ToInt32 = GenerateMethod<TEnum, int>();
            public static readonly Func<TEnum, long> ToLong = GenerateMethod<TEnum, long>();

            private static Func<TFrom, TTo> GenerateMethod<TFrom, TTo>()
            {
                var parameter = Expression.Parameter(typeof(TFrom));
                var convert = Expression.Convert(parameter, typeof(TTo));
                return Expression.Lambda<Func<TFrom, TTo>>(convert, parameter).Compile();
            }
        }
    }
    public enum TenantStatus
    {
        ACTIVE = 1,
        INACTIVE
    }

    public enum DbConnectionSource
    {
        FEEX = 1,
        HOOKMASTER = 2,
        NX360 = 3
    }

    public enum ApplicationName
    {
        PdfConverter,
        FeexFileS3Upload,
        NotificationServiceEmail,
        FeexPassportApi
    }

    public enum TicketStatus { Draft, New, Open, Active, Closed }
    public enum RelatedUserType { Staff,Customer}

    public enum TicketSubStatus { Unassigned, Assigned, Resolved, Overdued, Accepted, Rejected }
    public enum EscalationLevel { Resolver, TL, HOD, Admin }
    public enum  StatusDto
    {
        Draft, New, Active, Closed, Assigned, Resolved, Overdued, Accepted, Rejected
    }
    public enum TATPeriod
    {
        Minute,Hour,Day,Month,Year,Percent
    }
    public enum EscalationTiming
    {
        Before,
        After
    }
    public enum RequestStatusEnums
    {

        Open = 1,
        Resolved = 2,
        Pendingfeedback = 3,
        Closed = 4,
        Feedbackprovided = 5,
        ReOpen = 6,
        Indraft = 7,
    }


}
