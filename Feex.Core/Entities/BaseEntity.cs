using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Core.Entities
{
    public class BaseEntity
    {
        public virtual long Id { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public virtual DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
