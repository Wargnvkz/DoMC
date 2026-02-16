using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;

namespace DoMCLib.Tools
{
    public class ArrayTools
    {
        public static string BoolArrayToBase6xString(bool[] array)
        {
            var ba = BoolArray2ByteArray(array);
            var res = Convert.ToBase64String(ba);
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
            var bools = new bool[bytes.Length * 8];
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

        public static bool[] BitwiseBoolArrayOperation(bool[] array1, bool[] array2, Func<bool, bool, bool> bitwiseOperation)
        {
            if (bitwiseOperation == null) throw new ArgumentNullException("Не задана bitwiseOperation");
            var result = new bool[Math.Min(array1.Length, array2.Length)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = bitwiseOperation(array1[i], array2[i]);
            }
            return result;
        }

        public static double[,] MultiplyByValue(double[,] arr, double value)
        {
            var result = new double[arr.GetLength(0), arr.GetLength(1)];
            for (int y = 0; y < arr.GetLength(0); y++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    result[y, x] = value * arr[y, x];
                }

            }
            return result;
        }
        public static double[,] AddValue(double[,] arr, double value)
        {
            var result = new double[arr.GetLength(0), arr.GetLength(1)];
            for (int y = 0; y < arr.GetLength(0); y++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    result[y, x] = value + arr[y, x];
                }

            }
            return result;
        }
        public static double[,] SumArrays(double[,] arr1, double[,] arr2)
        {
            if (arr1.GetLength(0) != arr2.GetLength(0) || arr1.GetLength(1) != arr2.GetLength(1)) throw new ArgumentOutOfRangeException();
            var result = new double[arr1.GetLength(0), arr1.GetLength(1)];
            for (int y = 0; y < arr1.GetLength(0); y++)
            {
                for (int x = 0; x < arr1.GetLength(1); x++)
                {
                    result[y, x] = arr1[y, x] + arr2[y, x];
                }

            }
            return result;
        }
        public static double[,] SubstactArrays(double[,] Minuend, double[,] Subtrahend)
        {
            if (Minuend.GetLength(0) != Subtrahend.GetLength(0) || Minuend.GetLength(1) != Subtrahend.GetLength(1)) throw new ArgumentOutOfRangeException();
            var result = new double[Minuend.GetLength(0), Minuend.GetLength(1)];
            for (int y = 0; y < Minuend.GetLength(0); y++)
            {
                for (int x = 0; x < Minuend.GetLength(1); x++)
                {
                    result[y, x] = Minuend[y, x] - Subtrahend[y, x];
                }

            }
            return result;
        }
        public static double[,] SubstactLine(double[,] Minuend, double[] SubtrahendLine)
        {
            if (Minuend.GetLength(0) != SubtrahendLine.GetLength(0)) throw new ArgumentOutOfRangeException();
            var result = new double[Minuend.GetLength(0), Minuend.GetLength(1)];
            for (int y = 0; y < Minuend.GetLength(0); y++)
            {
                for (int x = 0; x < Minuend.GetLength(1); x++)
                {
                    result[y, x] = Math.Max(0, Minuend[y, x] - SubtrahendLine[x]);
                }

            }
            return result;
        }
        public static double[,] OperationOnElement(double[,] arr, Func<double, double> operation)
        {
            if (operation == null) return arr;
            var result = new double[arr.GetLength(0), arr.GetLength(1)];
            for (int y = 0; y < arr.GetLength(0); y++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    result[y, x] = operation(arr[y, x]);
                }

            }
            return result;
        }
        public static void OperationOnElement(double[,] arr, Action<double> operation)
        {
            if (operation == null) return;
            for (int y = 0; y < arr.GetLength(0); y++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    operation(arr[y, x]);
                }

            }
        }
        public static short[,] ConvertToIamge(double[,] arr)
        {
            var result = new short[arr.GetLength(0), arr.GetLength(1)];
            for (int y = 0; y < arr.GetLength(0); y++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    result[y, x] = Math.Clamp((short)arr[y, x], short.MinValue, short.MaxValue);
                }

            }
            return result;
        }
    }
}
