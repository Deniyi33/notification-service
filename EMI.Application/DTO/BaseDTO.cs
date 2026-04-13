using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Application.DTO
{
    public abstract class BaseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
