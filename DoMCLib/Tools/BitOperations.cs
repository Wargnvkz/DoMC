using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoMCLib.Tools
{
    public class BitOperations
    {

        public static UInt64 Set(UInt64 v, int bitToSet, bool ValueToSet)
        {
            if (ValueToSet)
                return Set(v, bitToSet);
            else return Reset(v, bitToSet);
        }
        public static bool Get(UInt64 v, int bitToGet)
        {
            var r = ((v >> bitToGet) & 1) == 1;
            return r;
        }
        public static UInt64 Set(UInt64 v, int bitToSet)
        {
            var b = 1UL << bitToSet;
            v = v | b;
            return v;
        }
        public static UInt64 Reset(UInt64 v, int bitToReset)
        {
            var b = 1UL << bitToReset;
            b = ~b;
            v = v & b;
            return v;
        }

        public static void Set(byte[] arr, int bitToSet, bool ValueToSet)
        {
            if (ValueToSet)
                Set(arr, bitToSet);
            else Reset(arr, bitToSet);
        }
        public static bool Get(byte[] arr, int bitToGet)
        {
            var index = bitToGet / 8;
            var bit = bitToGet % 8;
            var v = Get(arr[index], bit);
            return v;
        }
        public static void Set(byte[] arr, int bitToSet)
        {
            var index = bitToSet / 8;
            var bit = bitToSet % 8;
            byte v = (byte)Set(arr[index], bit);
            arr[index] = v;
        }
        public static void Reset(byte[] arr, int bitToSet)
        {
            var index = bitToSet / 8;
            var bit = bitToSet % 8;
            byte v = (byte)Reset(arr[index], bit);
            arr[index] = v;
        }
    }
}
