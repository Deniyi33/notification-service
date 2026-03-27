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

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
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
                //Email provider logic to send Email

                email.Status = EmailStatus.Sent;
                email.ModifiedBy = "System";
                await _emailRepository.UpdateAsync(email);

                return new SendEmailResponseDto
                {
                    Success = true,
                    Message = "Email sent successfully."
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
