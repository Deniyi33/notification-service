using EMI.Application.Interface;
using System.Text.Json;

namespace EMI.Application.Services
{
    public class TemplateRendererService : ITemplateRendererService
    {
        public string Render(string template, object model)
        {
            if (string.IsNullOrEmpty(template) || model == null)
                return template;

            var result = template;

            // CASE 1: Dictionary (BEST / RECOMMENDED)
            if (model is IDictionary<string, string> dict)
            {
                foreach (var item in dict)
                {
                    var placeholder = $"{{{{{item.Key}}}}}";
                    result = result.Replace(placeholder, item.Value ?? "");
                }

                return result;
            }

            // CASE 2: JSON element (ASP.NET sometimes sends this)
            if (model is JsonElement json)
            {
                foreach (var prop in json.EnumerateObject())
                {
                    var placeholder = $"{{{{{prop.Name}}}}}";
                    result = result.Replace(placeholder, prop.Value.ToString());
                }

                return result;
            }

            // CASE 3: fallback reflection (last resort)
            var properties = model.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var placeholder = $"{{{{{prop.Name}}}}}";
                var value = prop.GetValue(model)?.ToString() ?? "";

                result = result.Replace(placeholder, value);
            }

            return result;
        }
    }
}