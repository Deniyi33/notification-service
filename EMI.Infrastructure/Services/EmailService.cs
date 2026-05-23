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
using EMI.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EMI.AInfrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly AppDbContext _context;

        private readonly ZohoEmailSender _zohoEmailSender;

        public EmailService(IEmailRepository emailRepository, IEmailSenderService emailSenderService, AppDbContext context)
        {
            _emailRepository = emailRepository;
            _emailSenderService = emailSenderService;
            _context = context;
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
                IsSent = false
                
            };

            await _emailRepository.AddAsync(email);

            bool sent = false;

            try
            {
                sent = await _emailSenderService.SendEmailAsync(
                    email.To, 
                    email.Subject, 
                    email.Body
                );

                if (sent)
                {
                    email.Status = sent ? EmailStatus.Sent : EmailStatus.Failed;
                    email.IsSent = true;
                }
                else
                {
                    email.FailureReason = sent ? null : "Zoho rejected the request";
                }

                email.ModifiedBy = "System";
                await _emailRepository.UpdateAsync(email);

               
            }
            catch (Exception ex)
            {
                email.Status = EmailStatus.Failed;
                email.FailureReason = ex.Message;

                await _emailRepository.UpdateAsync(email);

                throw;
            }

            return new SendEmailResponseDto
            {
                Success = sent,
                Message = sent ? "Email sent successfully." : "Failed to send email."
            };
        }

        public async Task SendLoanReminder()
        {
            Console.WriteLine("Job started");

            var customers = await _context.Loans
                .Where(x => x.DueDate.Date == DateTime.UtcNow.Date)
                .Select(x => new
                {
                    x.CustomerId,
                    x.Customer.Email
                })
                .ToListAsync();

            foreach (var customer in customers)
            {
                var email = new Email
                {
                    To = customer.Email,
                    Subject = "Loan Repayment Reminder",
                    Body = "Dear Customer, your loan repayment is due.",
                    Status = EmailStatus.Pending,
                    CustomerId = customer.CustomerId,
                    IsSent = false
                };

                await _emailRepository.AddAsync(email);

                try
                {

                    var sent = await _emailSenderService.SendEmailAsync(
                        email.To,
                        email.Subject,
                        email.Body
                        );

                    email.Status = sent ? EmailStatus.Sent : EmailStatus.Failed;
                    email.IsSent = sent;
                    email.ModifiedBy = "System";

                    await _emailRepository.UpdateAsync(email);

                }
                catch (Exception ex)
                {
                    email.Status = EmailStatus.Failed;
                    email.FailureReason = ex.Message;

                    await _emailRepository.UpdateAsync(email);
                }
            }
            Console.WriteLine("Job finished");
        }
    }
}
