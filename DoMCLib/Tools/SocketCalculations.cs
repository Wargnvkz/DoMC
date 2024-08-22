using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoMCLib.Tools
{
    public class CCDSocketNumber
    {
        public int CCDCardNumber;
        public int InnerSocketNumber;
        public CCDSocketNumber(int IMMSocketNumber)
        {
            if (IMMSocketNumber < 1 || IMMSocketNumber > 96) throw new ArgumentOutOfRangeException("Значение должно быть в пределах от 1 до 96");
            var zero_IMMSocketNumber = IMMSocketNumber - 1;
            var cn = zero_IMMSocketNumber / 8;
            var sn = zero_IMMSocketNumber % 8;
            CCDCardNumber = cn;
            InnerSocketNumber = sn;
        }

        public static int ToIMMSocketNumber(int CCDCardNumber, int InnerSocketNumber)
        {
            return CCDCardNumber * 8 + InnerSocketNumber + 1;
        }
        public int ToIMMSocketNumber()
        {
            return ToIMMSocketNumber(CCDCardNumber, InnerSocketNumber);
        }
    }

    public class CCDCardMAC
    {
        public readonly byte[] MAC;
        public CCDCardMAC(byte CCDCardNumber)
        {
            MAC = new byte[6];
            for (int i = 0; i < 5; i++)
            {
                MAC[i] = 0x55;
            }
            MAC[5] = (byte)CCDCardNumber;
        }
        public static byte[] ToMAC(byte CCDCardNumber)
        {
            var MAC = new byte[6];
            for (int i = 0; i < 5; i++)
            {
                MAC[i] = 0x55;
            }
            MAC[5] = (byte)CCDCardNumber;
            return MAC;

        }

        public static List<byte[]> GetAllMACs(int maxsockets)
        {
            var sn = new CCDSocketNumber(maxsockets);
            var maxccdcards = sn.CCDCardNumber;
            var macs = new List<byte[]>();
            for (byte c = 0; c <= maxccdcards; c++)
            {
                macs.Add(ToMAC((byte)(c+1)));
            }
            return macs;
        }
        public static List<byte[]> GetAllMACs(int maxsockets,bool[] SocketsToCheck, Dictionary<int, int> DisplaySocketToPhysicalSocket)
        {
            int[] sockets = new int[maxsockets];
            for (int socket = 0; socket < maxsockets; socket++) 
            {
                sockets[socket] = socket+1;
            }
            var macs = GetAllMACs(sockets,SocketsToCheck, DisplaySocketToPhysicalSocket);
            return macs;
            
        }
        public static List<byte[]> GetAllMACs(int maxsockets, bool[] SocketsToCheck )
        {
            int[] sockets = new int[maxsockets];
            for (int socket = 0; socket < maxsockets; socket++)
            {
                sockets[socket] = socket + 1;
            }
            var macs = GetAllMACs(sockets, SocketsToCheck);
            return macs;

        }
        public static List<byte[]> GetAllMACs(int[] sockets)
        {
            var ccdcardsnums = new List<byte>();
            for (var i = 0; i < sockets.Length; i++)
            {
                var sn = new CCDSocketNumber(sockets[i]);
                var ccdcardn = sn.CCDCardNumber;
                ccdcardsnums.Add((byte)ccdcardn);
            }
            ccdcardsnums = ccdcardsnums.Distinct().ToList();
            var macs = new List<byte[]>();
            for (var i = 0; i < ccdcardsnums.Count; i++)
            {
                macs.Add(ToMAC((byte)(ccdcardsnums[i]+1)));
            }
            return macs;
        }
        public static List<byte[]> GetAllMACs(int[] sockets,bool[] SocketsToCheck, Dictionary<int, int> DisplaySocketToPhysicalSocket)
        {
            var ccdcardsnums = new List<byte>();
            for (var i = 0; i < sockets.Length; i++)
            {
                if (!SocketsToCheck[sockets[i]-1]) continue;
                var physicalSocket = DisplaySocketToPhysicalSocket.ContainsKey(sockets[i]) ? DisplaySocketToPhysicalSocket[sockets[i]] : sockets[i];
                //var sn = new CCDSocketNumber(sockets[i]);
                var sn = new CCDSocketNumber(physicalSocket);
                var ccdcardn = sn.CCDCardNumber;
                ccdcardsnums.Add((byte)ccdcardn);
            }
            ccdcardsnums = ccdcardsnums.Distinct().ToList();
            var macs = new List<byte[]>();
            for (var i = 0; i < ccdcardsnums.Count; i++)
            {
                macs.Add(ToMAC((byte)(ccdcardsnums[i] + 1)));
            }
            return macs;
        }
        public static List<byte[]> GetAllMACs(int[] sockets, bool[] SocketsToCheck)
        {
            var ccdcardsnums = new List<byte>();
            for (var i = 0; i < sockets.Length; i++)
            {
                if (!SocketsToCheck[sockets[i] - 1]) continue;
                var sn = new CCDSocketNumber(sockets[i]);
                var ccdcardn = sn.CCDCardNumber;
                ccdcardsnums.Add((byte)ccdcardn);
            }
            ccdcardsnums = ccdcardsnums.Distinct().ToList();
            var macs = new List<byte[]>();
            for (var i = 0; i < ccdcardsnums.Count; i++)
            {
                macs.Add(ToMAC((byte)(ccdcardsnums[i] + 1)));
            }
            return macs;
        }
        public static Dictionary<byte[],int[]> GetAllSocketsForMACs(int[] sockets)
        {
            var tmp = new Dictionary<int, List<int>>();
            var ccdcardsnums = new List<byte>();
            for (var i = 0; i < sockets.Length; i++)
            {
                var sn = new CCDSocketNumber(sockets[i]);
                if (!tmp.ContainsKey(sn.CCDCardNumber))
                {
                    tmp.Add(sn.CCDCardNumber, new List<int>());
                }
                tmp[sn.CCDCardNumber].Add(sockets[i]);
            }
            var res = new Dictionary<byte[], int[]>();
            foreach(KeyValuePair<int,List<int>> kv in tmp)
            {
                var mac = ToMAC((byte)(kv.Key + 1));
                res.Add(mac, kv.Value.ToArray());
            }
            return res;
        }
        public static int SocketsAtLastCards(int MaxSocket)
        {
            var sn = MaxSocket - 1;
            if (sn < 0) return 0;
            var sr = sn % 8+1;
            return sr;
        }
        /// <summary>
        /// from 1 to N
        /// </summary>
        /// <param name="MaxSocket"></param>
        /// <returns></returns>
        public static int LastCardNumber(int MaxSocket)
        {
            var sn = MaxSocket - 1;
            if (sn < 0) return 0;
            var cn = sn / 8 + 1;
            return cn;
        }

        public static int SocketQuantity(int MaxSocket,int CardNum)
        {
            var start = CardNum * 8;
            var end = (CardNum + 1) * 8;
            end = Math.Min(MaxSocket, end);
            return end-start;
        }
        public static int FirstSocket(int CardNum)
        {
            var start = CardNum * 8;
            return start+1;
        }
    }
}
