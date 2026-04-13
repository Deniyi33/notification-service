using EMI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Application.DTO.RequestDTO
{
    public class SendEmailRequestDto
    {
        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public int CustomerId { get; set; }
        public EmailType EmailType { get; set; }
       
    }
}
