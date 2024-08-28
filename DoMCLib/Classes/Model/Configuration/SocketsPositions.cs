using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Model.Configuration
{
    public class SocketsPositions
    {
        private int[] UIPosition2PhysicalSockets; //Индекс - гнездо в UI, значение - физическое гнездо
        private int[] PhysicalSockets2UIPosition; // Индекс - физическое гнездо, значение - отоборажаемое в UI гнездо

        public void SetMaxSockets(int maxSockets)
        {
            UIPosition2PhysicalSockets = new int[maxSockets];
            PhysicalSockets2UIPosition = new int[maxSockets];
        }

        public int GetPhysicalSocket(int UISocket)
        {
            return UIPosition2PhysicalSockets[UISocket];
        }
        public int GetUISocket(int PhysicalSocket)
        {
            return PhysicalSockets2UIPosition[PhysicalSocket];
        }

        public struct CardSocket
        {
            public int Card;
            public int Socket;
            public CardSocket(int Card, int Socket)
            {
                this.Card = Card;
                this.Socket = Socket;
            }
            public CardSocket(int PhysicalSocket)
            {
                this.Card = PhysicalSocket / 8;
                this.Socket = PhysicalSocket % 8;
            }
            public int ToPhysicalSocket()
            {
                return Card * 8 + Socket;
            }
        }
    }

}
