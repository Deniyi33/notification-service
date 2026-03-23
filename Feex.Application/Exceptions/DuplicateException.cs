using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Application.Exceptions
{
    public class DuplicateException : ApplicationException
    {
        public DuplicateException(string message) : base(message)
        {

        }
    }
}
