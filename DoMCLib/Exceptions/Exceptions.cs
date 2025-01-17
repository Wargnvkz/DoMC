using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Exceptions
{
    public class DoMCException : Exception
    {
        public DoMCException() : base() { }
        public DoMCException(string message) : base(message) { }

    }

}
