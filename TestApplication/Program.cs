using System.Net;
using System.Runtime.InteropServices;

namespace TestApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = new A();
            var arr = a.ToByteArray();
            var b = A.FromByteArray(arr);
            Console.WriteLine("Hello, World!");
        }



        public struct A
        {
            public byte w = 1;
            public int x = -1;
            public byte y = 2;
            public short z = 3;
            public A() { }
            public byte[] ToByteArray()
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                    {
                        writer.Write(w);   // Записываем байт
                        writer.Write(x);   // Записываем байт
                        writer.Write(y);         // Записываем ushort (2 байта)
                        writer.Write(z);         // Записываем ushort (2 байта)
                    }
                    return ms.ToArray();
                }
            }

            // Метод для преобразования массива байт обратно в объект
            public static A FromByteArray(byte[] data)
            {
                A result = new A();

                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        result.w = reader.ReadByte();   // Читаем байт
                        result.x = reader.ReadInt32();   // Читаем байт
                        result.y = reader.ReadByte();       // Читаем ushort (2 байта)
                        result.z = reader.ReadInt16();       // Читаем ushort (2 байта)
                    }
                }

                return result;
            }
        }
    }
}
