using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Application.DTO.ResponseDTO
{
    public class SendEmailResponseDto
    {
        public bool Success { get; set; }
        public required string Message { get; set; }
    }
}
