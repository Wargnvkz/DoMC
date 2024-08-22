using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoMCLib.Tools
{

    public struct TCPCardSocket
    {
        public int CCDCardNumber;
        public int InnerSocketNumber;

        public TCPCardSocket(int PhysicalSocketNumber)
        {
            if (PhysicalSocketNumber < 1 || PhysicalSocketNumber > 96) throw new ArgumentOutOfRangeException("Значение должно быть в пределах от 1 до 96");
            var zero_IMMSocketNumber = PhysicalSocketNumber - 1;
            var cn = zero_IMMSocketNumber / 8;
            var sn = zero_IMMSocketNumber % 8;
            CCDCardNumber = cn;
            InnerSocketNumber = sn;
        }

        public int PhysicalSocket()
        {
            return CCDCardNumber * 8 + InnerSocketNumber;
        }
    }
}
