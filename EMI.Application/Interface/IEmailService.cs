using EMI.Application.DTO.RequestDTO;
using EMI.Application.DTO.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Application.Interface
{
    public interface IEmailService
    {
        Task<SendEmailResponseDto> SendEmailAsync(SendEmailRequestDto request);
        Task SendLoanReminder(string to);
    }
}
