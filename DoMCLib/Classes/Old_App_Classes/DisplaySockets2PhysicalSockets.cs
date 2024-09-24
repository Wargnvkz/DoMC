using System.Runtime.Serialization;

namespace DoMCLib.Classes
{
    [DataContract]
    public class DisplaySockets2PhysicalSockets
    {
        [DataMember]
        private int[] PhysicalSockets; //Индекс - гнездо в UI, значение - физическое гнездо
        private int[] DisplaySockets; // Индекс - физическое гнездо, значение - отоборажаемое в UI гнездо
        public void SetMatrixSize(int matrix)
        {
            PhysicalSockets = new int[matrix + 1];
        }
        public void SetDefaultMatrix(int matrix)
        {
            PhysicalSockets = Enumerable.Range(0, matrix + 1).ToArray();
        }
        public int PhysicalSocketToDisplaySocket(int PhysicalSocket)
        {
            if (DisplaySockets == null)
                FillDisplaySockets();
            return DisplaySockets[PhysicalSocket];
        }

        public int DisplaySocketToPhysicalSocket(int DisplaySocket)
        {
            return PhysicalSockets[DisplaySocket];
        }

        public void SetSocketNumbers(int PhysicalSocket, int DisplaySocket)
        {
            PhysicalSockets[DisplaySocket] = PhysicalSocket;
        }
        public bool FillDisplaySockets()
        {
            if (PhysicalSockets == null || PhysicalSockets.Length == 0) return false;
            DisplaySockets = new int[PhysicalSockets.Length];
            for (int i = 0; i < PhysicalSockets.Length; i++)
            {
                var ph = PhysicalSockets[i];
                if (DisplaySockets[ph] != 0) return false;
                DisplaySockets[ph] = i;
            }
            return true;
        }

        public int[] DisplayToPhysical(int[] displaySockets)
        {
            var result = new int[displaySockets.Length];
            for (int i = 0; i < displaySockets.Length; i++)
            {
                result[i] = PhysicalSockets[displaySockets[i]];
            }
            return result;
        }
        public int[] PhysicalToDisplay(int[] physicalSockets)
        {
            if (DisplaySockets == null)
                FillDisplaySockets();
            var result = new int[physicalSockets.Length];
            for (int i = 0; i < physicalSockets.Length; i++)
            {
                result[i] = DisplaySockets[physicalSockets[i]];
            }
            return result;
        }

        public List<Cards> GetSocketsByCards()
        {
            var result = new List<Cards>();
            for (int cardsocket = 1; cardsocket <= 8; cardsocket++)
            {
                var values = new int[12];
                for (int card = 1; card <= 12; card++)
                {
                    var physicalSocket = (card - 1) * 8 + cardsocket;
                    var displaySocket = PhysicalSocketToDisplaySocket(physicalSocket);
                    values[card - 1] = displaySocket;
                }
                Cards c = new Cards();
                c.Array = values;
                c.Socket = cardsocket;
                result.Add(c);
            }
            return result;
        }

        public void SetSocketsByCards(List<Cards> displaySocketsForPhysicalSockets)
        {
            if (displaySocketsForPhysicalSockets.Count != 8) return;
            SetMatrixSize(96);
            for (int cardsocket = 1; cardsocket <= 8; cardsocket++)
            {
                var dsocket = displaySocketsForPhysicalSockets.Find(ds => ds.Socket == cardsocket);
                if (dsocket == null) continue;
                var values = dsocket.Array;
                for (int card = 1; card <= 12; card++)
                {
                    var physicalSocket = (card - 1) * 8 + cardsocket;
                    SetSocketNumbers(physicalSocket, values[card - 1]);
                }
            }
        }

        public int GetSocketQuantity()
        {
            return PhysicalSockets != null ? PhysicalSockets.Length - 1 : 0;
        }

        public class Cards
        {
            public int Socket { get; set; }
            public int Card1 { get; set; }
            public int Card2 { get; set; }
            public int Card3 { get; set; }
            public int Card4 { get; set; }
            public int Card5 { get; set; }
            public int Card6 { get; set; }
            public int Card7 { get; set; }
            public int Card8 { get; set; }
            public int Card9 { get; set; }
            public int Card10 { get; set; }
            public int Card11 { get; set; }
            public int Card12 { get; set; }

            public int[] Array
            {
                get
                {
                    return new int[] { Card1, Card2, Card3, Card4, Card5, Card6, Card7, Card8, Card9, Card10, Card11, Card12 };
                }
                set
                {
                    Card1 = value[0];
                    Card2 = value[1];
                    Card3 = value[2];
                    Card4 = value[3];
                    Card5 = value[4];
                    Card6 = value[5];
                    Card7 = value[6];
                    Card8 = value[7];
                    Card9 = value[8];
                    Card10 = value[9];
                    Card11 = value[10];
                    Card12 = value[11];
                }
            }
        }
    }
}
