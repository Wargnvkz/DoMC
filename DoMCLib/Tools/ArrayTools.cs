using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoMCLib.Tools
{
    public class ArrayTools
    {
        public static string BoolArrayToBase6xString(bool[] array)
        {
            var ba = BoolArray2ByteArray(array);
            var res=Convert.ToBase64String(ba);
            res = res.Replace('/', '#');
            return res;
        }

        public static bool[] Base6xStringToBoolArray(string code)
        {
            code = code.Replace('#', '/');
            var ba = Convert.FromBase64String(code);
            var res = ByteArray2BoolArray(ba);
            return res;
        }

        public static byte[] BoolArray2ByteArray(bool[] bools)
        {
            if (bools == null) return null;
            BitArray bitarray = new BitArray(bools);
            var bytea = new byte[(bools.Length + 7) / 8];
            bitarray.CopyTo(bytea, 0);
            return bytea;
        }
        public static bool[] ByteArray2BoolArray(byte[] bytes)
        {
            if (bytes == null) return null;
            BitArray bitarray = new BitArray(bytes);
            var bools = new bool[bytes.Length*8];
            bitarray.CopyTo(bools, 0);
            return bools;
        }
        public static string ByteArrayToHexString(byte[] data)
        {
            return string.Join(", ", data.Select(d => "0x" + d.ToString("X2")));
        }
        public static string BoolArrayToHex(bool[] array)
        {
            if (array == null || array.Length == 0) return "x";
            StringBuilder sb = new StringBuilder();
            var N = 8;
            var bytes = array.Length / N;
            if (array.Length % N != 0) bytes++;
            for (int b = 0; b < bytes; b++)
            {
                int value = 0;
                for (var i = 0; i < N; i++)
                {
                    var ind = b * N + i;
                    if (ind >= array.Length) break;
                    value |= (array[ind] ? 1 : 0) << i;
                }
                sb.Insert(0, value.ToString("X2"));
            }
            return "0x" + sb.ToString();
        }
    }
}
