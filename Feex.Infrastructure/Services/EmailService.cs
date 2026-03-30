using EMI.Application.DTO.RequestDTO;
using EMI.Application.DTO.ResponseDTO;
using EMI.Application.Interface;
using EMI.Domain.Entities;
using EMI.Domain.Interfaces;
using Feex.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.AInfrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailSenderService _emailSenderService;

        public EmailService(IEmailRepository emailRepository, IEmailSenderService emailSenderService)
        {
            _emailRepository = emailRepository;
            _emailSenderService = emailSenderService;
        }
        public async Task<SendEmailResponseDto> SendEmailAsync(SendEmailRequestDto request)
        {
            var email = new Email
            {
                To = request.To,
                Subject = request.Subject,
                Body = request.Body,
                EmailType = request.EmailType,
                Status = EmailStatus.Pending,
                
            };



            try
            {
                await _emailRepository.AddAsync(email);

                var sent = await _emailSenderService.SendEmailAsync(email.To, email.Subject, email.Body);

                if (sent)
                {
                    email.Status = EmailStatus.Sent;
                }
                else
                {
                    email.Status = EmailStatus.Failed;
                    email.FailureReason = "Zoho failed to send email";
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
                if(email.Id > 0)
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
