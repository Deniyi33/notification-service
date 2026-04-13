using EMI.Application.DTO.ResponseDTO;
using EMI.Application.DTO.RequestDTO;

using EMI.Application.Interface;
using EMI.Domain.Entities;
using EMI.Domain.Interfaces;
using Feex.Core.Enums;
using Newtonsoft.Json;

namespace EMI.AInfrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailTemplateService _templateService;
        private readonly ITemplateRendererService _templateRendererService;

        public EmailService(
            IEmailRepository emailRepository,
            IEmailSenderService emailSenderService,
            IEmailTemplateService templateService,
            ITemplateRendererService templateRendererService)
        {
            _emailRepository = emailRepository;
            _emailSenderService = emailSenderService;
            _templateService = templateService;
            _templateRendererService = templateRendererService;
        }

        public async Task<SendEmailResponseDto> SendEmailAsync(SendEmailRequestDto request)
        {
            var email = new Email
            {
                To = request.To,
                Subject = request.Subject,
                EmailType = request.EmailType,
                TemplateData = JsonConvert.SerializeObject(request.TemplateData),
                Status = EmailStatus.Pending
            };

            try
            {
                // 1. Save email first
                await _emailRepository.AddAsync(email);

                // 2. Load template
                var template = await _templateService.GetTemplateAsync(email.EmailType);

                // 3. Render template with data
                var body = _templateRendererService.Render(template, request.TemplateData);

                // 4. Send email
                var sent = await _emailSenderService.SendEmailAsync(
                    email.To,
                    email.Subject,
                    body
                );

                // 5. Update status
                if (sent)
                {
                    email.Status = EmailStatus.Sent;
                }
                else
                {
                    email.Status = EmailStatus.Failed;
                    email.FailureReason = "Email provider failed to send email";
                }

                email.ModifiedBy = "System";
                await _emailRepository.UpdateAsync(email);

                return new SendEmailResponseDto
                {
                    Success = sent,
                    Message = sent ? "Email sent successfully." : "Failed to send email."
                };
            }
            catch (Exception ex)
            {
                if (email.Id > 0)
                {
                    email.Status = EmailStatus.Failed;
                    email.ModifiedBy = "System";
                    email.FailureReason = ex.Message;

                    await _emailRepository.UpdateAsync(email);
                }

                return new SendEmailResponseDto
                {
                    Success = false,
                    Message = $"Failed to send email. {ex.Message}"
                };
            }
        }
    }
}