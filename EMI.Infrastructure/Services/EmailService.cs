using EMI.Application.DTO.RequestDTO;
using EMI.Application.DTO.ResponseDTO;
using EMI.Application.Interface;
using EMI.Domain.Entities;
using EMI.Domain.Interfaces;
using EMI.Infrastructure.Services;
using EMI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EMI.AInfrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailSenderService _emailSenderService;

        private readonly ZohoEmailSender _zohoEmailSender;

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
                CustomerId = request.CustomerId,
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
                    email.IsSent = true;
                }
                else
                {
                    email.Status = EmailStatus.Failed;
                    email.FailureReason = "Zoho rejected the request";
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
                Console.WriteLine($"DATABASE ERROR: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"INNER ERROR: {ex.InnerException.Message}");

                throw;
            }
        }

        public async Task SendLoanReminder(string to)
        {
            Console.WriteLine("Job started");

            var request = new SendEmailRequestDto
            {
             To = to,
             Subject = "Loan Repayment Reminder",
             Body = "Dear Customer, your loan repayment is due.",
            };
            await SendEmailAsync(request);

            Console.WriteLine("Job finished");

            //Save to Database
            var email = new Email
            {
                To = to,
                Subject = "" ,
                Body = "",
                Status = EmailStatus.Sent,
                CustomerId = 1 // change if needed
            };


        }
    }
}
