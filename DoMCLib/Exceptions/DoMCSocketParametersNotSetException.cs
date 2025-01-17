using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Exceptions
{
    public class DoMCSocketParametersNotSetException : DoMCException
    {
        public readonly int CardNumber, SocketNumber;
        public DoMCSocketParametersNotSetException(int cardNumber, int socketNumber)
        {
            CardNumber = cardNumber;
            SocketNumber = socketNumber;
        }
    }
}
