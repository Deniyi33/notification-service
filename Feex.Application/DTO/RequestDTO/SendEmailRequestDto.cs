using System.ComponentModel.DataAnnotations;
using Feex.Core.Enums;

namespace EMI.Application.DTO.RequestDTO
{
    public class SendEmailRequestDto
    {
        [Required]
        [EmailAddress]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public EmailType EmailType { get; set; }

        [Required]
        public Dictionary<string, string> TemplateData { get; set; }
    }
}