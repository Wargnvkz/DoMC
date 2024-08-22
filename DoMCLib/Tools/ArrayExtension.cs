using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoMCLib.Tools
{
    public static class ArrayExtension
    {
        public static T[] Fill<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
            return arr;
        }
    }
}
