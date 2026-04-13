using Feex.Core.Entities;
using Feex.Core.Enums;

namespace EMI.Domain.Entities
{
    public class Email : BaseEntity
    {
        public string To { get; set; }
        public string Subject { get; set; }

        public EmailType EmailType { get; set; }

        public EmailStatus Status { get; set; }

        public string? TemplateData { get; set; }

        public string? FailureReason { get; set; }
    }
}