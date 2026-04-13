using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EMI.Application.Helpers
{
    public static class QueryStringBuiderHelper
    {
        public static string BuildQueryString(object filterObject)
        {
            var queryParams = new List<string>();
            var properties = filterObject.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(filterObject);

                // Handle different types: strings, nullable types, and normal types
                if (value != null && IsSerializableType(prop.PropertyType))
                {
                    if (value is string stringValue)
                    {
                        // Skip empty strings
                        if (!string.IsNullOrEmpty(stringValue))
                        {
                            queryParams.Add($"{HttpUtility.UrlEncode(prop.Name)}={HttpUtility.UrlEncode(stringValue)}");
                        }
                    }
                    else
                    {
                        // Non-string values (e.g., int, bool) are directly added
                        queryParams.Add($"{HttpUtility.UrlEncode(prop.Name)}={HttpUtility.UrlEncode(value.ToString())}");
                    }
                }
            }

            var queryString = string.Join("&", queryParams);
            return queryString;
        }

        private static bool IsSerializableType(Type type)
        {
            // Check for simple types or nullable types that can be serialized
            return type.IsPrimitive || type.IsValueType || type == typeof(string) || (Nullable.GetUnderlyingType(type) != null);
        }
    }
}
