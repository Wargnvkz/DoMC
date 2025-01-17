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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CardSocketNumber">Номер гнезда связанный с платами</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public TCPCardSocket(int CardSocketNumber)
        {
            if (CardSocketNumber < 0 || CardSocketNumber > 95) throw new ArgumentOutOfRangeException("Значение должно быть в пределах от 0 до 95");
            var zero_IMMSocketNumber = CardSocketNumber;
            var cn = zero_IMMSocketNumber / 8;
            var sn = zero_IMMSocketNumber % 8;
            CCDCardNumber = cn;
            InnerSocketNumber = sn;
        }
        public TCPCardSocket(int cardNumber, int SocketNumber)
        {
            CCDCardNumber = cardNumber;
            InnerSocketNumber = SocketNumber;
        }

        public int CardSocketNumber()
        {
            return CCDCardNumber * 8 + InnerSocketNumber;
        }
    }
}
