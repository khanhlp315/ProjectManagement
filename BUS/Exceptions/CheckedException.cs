using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Exceptions
{
    public class CheckedException: SystemException
    {
        public CheckedException(string message) : base(message)
        {
        }
    }
}
