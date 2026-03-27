using Feex.Core.Entities;
using Feex.Core.Enums;
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
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailType EmailType { get; set; }
        public EmailStatus Status { get; set; }
        public string? FailureReason { get; set; }
    }
}
