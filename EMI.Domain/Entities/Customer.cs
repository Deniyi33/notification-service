using EMI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Domain.Entities
{
    public class Customer : BaseEntity
    {
    
        public required string Email { get; set; }
        public int RepaymentDay { get; set; }
    }
}
