using EMI.Domain.Entities;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; } = "System";
        public string? ModifiedBy { get; set; }
        public  DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsSent { get; set; }
    }
}
