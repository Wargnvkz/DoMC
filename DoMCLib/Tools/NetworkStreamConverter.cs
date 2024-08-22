using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DoMCLib.Tools
{
    public class NetworkStreamConverter
    {
        public static byte HeadByte = 0;
        public static byte TailByte = 1;
        public static byte EscapeByte = 2;
        /// <summary>
        /// Пытается получить пакет из данных
        /// 0 - заголовок/начало пакета, 1 - конец пакета, 2 - escape символ, следующий за ним воспринимается, как символ данных
        /// </summary>
        /// <param name="input">массив полученных данных из которых нужно получить пакет</param>
        /// <returns>пакет или null, если пакет не сформировался</returns>
        public static byte[] GetDataFromInputPackets(ref byte[] input)
        {
            if (input == null) return null;
            List<byte> data = new List<byte>();
            bool isStarted = false;
            bool isEscape = false;
            int i;
            bool isError = false;
            for (i = 0; i < input.Length; i++)
            {
                var b = input[i];
                if (isEscape)
                {
                    data.Add(b);
                    isEscape = false;
                }
                else
                {
                    if (b == HeadByte)
                    {
                        if (!isStarted)
                        {
                            isStarted = true;
                        }
                        else
                        {
                            i--;
                            isError = true;
                            goto CreatePacketAndReturn;

                        }
                    }
                    else
                    {
                        if (b == TailByte)
                        {
                            if (isStarted)
                            {
                                isError = false;
                                goto CreatePacketAndReturn;
                            }
                            else
                            {
                                isError = true;
                                goto CreatePacketAndReturn;
                            }
                        }
                        else
                        {
                            if (b == EscapeByte)
                            {
                                if (isStarted)
                                {
                                    isEscape = true;
                                }
                                else
                                {
                                    isError = true;
                                    goto CreatePacketAndReturn;
                                }
                            }
                            else
                            {
                                data.Add(b);
                            }
                        }
                    }
                }
            }
            return null;
        CreatePacketAndReturn:
            var newArray = new byte[input.Length - (i + 1)];
            if (newArray.Length != 0)
                Array.Copy(input, i + 1, newArray, 0, newArray.Length);
            input = newArray;
            if (isError)
            {
                return null;
            }
            else
            {
                var result = data.ToArray();
                return result;
            }

        }
        /// <summary>
        /// Преобразует массив данных в пакет для передачи
        /// </summary>
        /// <param name="data">пакет данных</param>
        /// <returns>Преобразованный пакет, с начальными, конечными и escape символами</returns>
        public static byte[] Data2Packet(byte[] data)
        {
            List<byte> result = new List<byte>();
            result.Add(HeadByte);
            for(int i = 0; i < data.Length; i++)
            {
                var d = data[i];
                if (d == HeadByte || d == TailByte || d == EscapeByte)
                {
                    result.Add(EscapeByte);
                }
                result.Add(d);
            }
            result.Add(TailByte);
            return result.ToArray();
        }
    }
}
