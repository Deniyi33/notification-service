using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Application.Interface
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
