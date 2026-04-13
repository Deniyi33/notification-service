
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
