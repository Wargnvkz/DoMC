using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoMCLib.Tools
{
    public class BoolArrayTools
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
        
    }
}
