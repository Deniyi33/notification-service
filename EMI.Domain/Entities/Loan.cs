
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Domain.Entities
{
    public class Loan : BaseEntity
    {
     
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer  { get; set; }
    }
}
