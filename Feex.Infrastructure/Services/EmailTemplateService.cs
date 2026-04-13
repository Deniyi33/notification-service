using EMI.Application.Interface;
using Feex.Core.Enums;
using System.Reflection;

namespace EMI.Application.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public async Task<string> GetTemplateAsync(EmailType type)
        {
            var templateName = GetTemplateName(type);

            var basePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "Email Templates"
             
            );

            var filePath = Path.Combine(basePath, $"{templateName}.html");

            if (!File.Exists(filePath))
                throw new Exception($"Email template not found: {templateName}");

            return await File.ReadAllTextAsync(filePath);
        }

        private string GetTemplateName(EmailType type)
        {
            return type switch
            {
                EmailType.LoanAccountCreated => "LoanAccountCreatedEmail",
                EmailType.DisbursementPaid => "DisbursementPaidEmail",
                EmailType.RepaymentReminder => "RepaymentReminderEmail",
                EmailType.LoanPaidOff => "LoanPaidOffEmail",
                _ => throw new Exception("Unknown email type")
            };
        }
    }
}
