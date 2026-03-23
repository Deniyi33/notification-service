using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Application.Exceptions
{
    public class InvalidException: ApplicationException
    {
        public InvalidException(string message): base(message)
        {
                
        }
    }
}
