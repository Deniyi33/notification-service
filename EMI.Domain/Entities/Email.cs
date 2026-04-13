
using EMI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Domain.Entities
{
    public class Email : BaseEntity
    {
        //Inherited the BaseEntity class
        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public int CustomerId { get; set; }
        public EmailType EmailType { get; set; }
        public EmailStatus Status { get; set; }
        public string? FailureReason { get; set; }
    }
}
