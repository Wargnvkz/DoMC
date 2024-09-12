using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Tools
{

    public class BinaryConverter
    {
        /// <summary>
        /// Converts object to byte array according to field attributes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="v">Object instance</param>
        /// <returns>Converted byte array</returns>
        public static byte[] ToBytes<T>(T v)
        {
            var tt = typeof(T);
            var list = MemberInfoProcessing.GetMemberInfoList(tt);
            List<byte> res = new List<byte>();
            for (int i = 0; i < list.Count; i++)
            {
                var val = MemberInfoProcessing.GetValue(list[i], v);

                var attrs = list[i].GetCustomAttributes(false);
                //IBinaryConverterAttribute bca=null;
                for (int j = 0; j < attrs.Length; j++)
                {
                    var attrType = attrs[j].GetType();
                    var ints = attrType.GetInterfaces();
                    var theInttype = typeof(IBinaryConverterAttribute);
                    var binconvattr = Array.Find(ints, I => I == theInttype);
                    if (binconvattr != null)
                    {
                        var converter = (IBinaryConverterAttribute)attrs[j];// (IBinaryConverterAttribute)MemberInfoProcessing.CreateObject(attrType);
                        var bytes = converter.ToBytes(val);
                        res.AddRange(bytes);
                    }
                }
            }
            var resa = res.ToArray();
            res.Clear();
            res.TrimExcess();
            return resa;
        }
        protected static byte[] ObjectToBytes(object v)
        {
            var tt = v.GetType();
            var list = MemberInfoProcessing.GetMemberInfoList(tt);
            List<byte> res = new List<byte>();
            for (int i = 0; i < list.Count; i++)
            {
                var val = MemberInfoProcessing.GetValue(list[i], v);

                var attrs = list[i].GetCustomAttributes(false);
                //IBinaryConverterAttribute bca=null;
                for (int j = 0; j < attrs.Length; j++)
                {
                    var attrType = attrs[j].GetType();
                    var ints = attrType.GetInterfaces();
                    var theInttype = typeof(IBinaryConverterAttribute);
                    var binconvattr = Array.Find(ints, I => I == theInttype);
                    if (binconvattr != null)
                    {
                        var converter = (IBinaryConverterAttribute)attrs[j];// (IBinaryConverterAttribute)MemberInfoProcessing.CreateObject(attrType);
                        var bytes = converter.ToBytes(val);
                        res.AddRange(bytes);
                    }
                }
            }
            var resa = res.ToArray();
            res.Clear();
            res.TrimExcess();
            return resa;
        }

        /// <summary>
        /// Десериализация объекта из массива байтов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">Массив байт с данными объекта</param>
        /// <param name="res">Возвращаемый объект</param>
        /// <returns>Длина использованного массива в байтах</returns>
        public static int FromBytes<T>(byte[] arr, out T res) where T : class
        {
            var tt = typeof(T);
            var list = MemberInfoProcessing.GetMemberInfoList(tt);
            res = (T)Activator.CreateInstance(typeof(T));
            int position = 0;
            for (int i = 0; i < list.Count; i++)
            {

                var attrs = list[i].GetCustomAttributes(false);
                //IBinaryConverterAttribute bca=null;
                for (int j = 0; j < attrs.Length; j++)
                {
                    var attrType = attrs[j].GetType();
                    var ints = attrType.GetInterfaces();
                    var theInttype = typeof(IBinaryConverterAttribute);
                    var binconvattr = Array.Find(ints, I => I == theInttype);
                    if (binconvattr != null)
                    {
                        var converter = (IBinaryConverterAttribute)attrs[j];// (IBinaryConverterAttribute)MemberInfoProcessing.CreateObject(attrType);
                        var size = converter.ObjectSize(arr, position);

                        byte[] ToConvert = new byte[size];
                        Array.Copy(arr, position, ToConvert, 0, size);

                        var val = converter.ToObject(ToConvert);
                        MemberInfoProcessing.SetValue(list[i], res, val);
                        position += size;
                    }
                }

            }
            return position;
        }

        protected static int ObjectFromBytes(Type T, byte[] arr, out object res)
        {
            var list = MemberInfoProcessing.GetMemberInfoList(T);
            res = Activator.CreateInstance(T);
            int position = 0;
            for (int i = 0; i < list.Count; i++)
            {

                var attrs = list[i].GetCustomAttributes(false);
                //IBinaryConverterAttribute bca=null;
                for (int j = 0; j < attrs.Length; j++)
                {
                    var attrType = attrs[j].GetType();
                    var ints = attrType.GetInterfaces();
                    var theInttype = typeof(IBinaryConverterAttribute);
                    var binconvattr = Array.Find(ints, I => I == theInttype);
                    if (binconvattr != null)
                    {
                        var converter = (IBinaryConverterAttribute)attrs[j];// (IBinaryConverterAttribute)MemberInfoProcessing.CreateObject(attrType);
                        var size = converter.ObjectSize(arr, position);

                        byte[] ToConvert = new byte[size];
                        Array.Copy(arr, position, ToConvert, 0, size);

                        var val = converter.ToObject(ToConvert);
                        MemberInfoProcessing.SetValue(list[i], res, val);
                        position += size;
                    }
                }

            }
            return position;
        }

        /// <summary>
        /// Интерфейс для реализации аттрибута-преобразователя Объект<->Массив байт. 
        /// </summary>
        public interface IBinaryConverterAttribute//: IBinaryConverterAttribute
        {
            /// <summary>
            /// Преобразование объекта в массив байт
            /// </summary>
            /// <param name="obj">Объект</param>
            /// <returns>Массив</returns>
            byte[] ToBytes(object obj);
            /// <summary>
            /// Преобразование массива в объект
            /// </summary>
            /// <param name="bytes">массив байт</param>
            /// <returns>Объект</returns>
            object ToObject(byte[] bytes);
            /// <summary>
            /// Определение длины массива необходимой для полноценного преобразования объекта
            /// </summary>
            /// <param name="arr">Исходный массив</param>
            /// <param name="position">Начальная позиция данных в массиве</param>
            /// <returns></returns>
            int ObjectSize(byte[] arr, int position);
        }


        /// <summary>
        /// Реализация аттрибута для преобразования Int32 в массив байт
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class Int32Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(int))
                    return BitConverter.GetBytes((int)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToInt32(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 4; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class ByteAttribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(byte))
                    return new byte[] { (byte)obj };
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return bytes[0];
            }
            public int ObjectSize(byte[] arr, int position) { return 1; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class SByteAttribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(sbyte))
                    return new byte[] { (byte)(sbyte)obj };
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return (sbyte)bytes[0];
            }
            public int ObjectSize(byte[] arr, int position) { return 1; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class Int16Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(short))
                    return BitConverter.GetBytes((short)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToInt16(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 2; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class UInt16Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(ushort))
                    return BitConverter.GetBytes((ushort)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToUInt16(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 2; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class UInt32Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(uint))
                    return BitConverter.GetBytes((uint)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToUInt32(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 4; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class Int64Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(long))
                    return BitConverter.GetBytes((long)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToInt64(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 8; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class UInt64Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(ulong))
                    return BitConverter.GetBytes((ulong)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToUInt64(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 8; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class CharAttribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(char))
                    return BitConverter.GetBytes((char)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToChar(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 2; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class SingleAttribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(float))
                    return BitConverter.GetBytes((float)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToSingle(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 4; }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class DoubleAttribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(double))
                    return BitConverter.GetBytes((double)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToDouble(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 8; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class DecimalAttribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(decimal))
                    return MemberInfoProcessing.GetBytes((decimal)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return MemberInfoProcessing.ToDecimal(bytes);
            }
            public int ObjectSize(byte[] arr, int position) { return 16; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class BooleanAttribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType() == typeof(bool))
                    return new byte[] { (bool)obj ? (byte)1 : (byte)0 };
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return bytes[0] == 0 ? false : true;
            }
            public int ObjectSize(byte[] arr, int position) { return 1; }

        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class Enum32Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType().IsEnum)
                    return BitConverter.GetBytes((int)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return BitConverter.ToInt32(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 4; }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class Enum16Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType().IsEnum)
                    return BitConverter.GetBytes((short)(int)obj);
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return (int)BitConverter.ToInt16(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 2; }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class Enum8Attribute : Attribute, IBinaryConverterAttribute
        {
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType().IsEnum)
                    return new byte[] { (byte)(int)obj };//BitConverter.GetBytes((byte)((Int32)obj));
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                return (int)bytes[0];//BitConverter.ToInt32(bytes, 0);
            }
            public int ObjectSize(byte[] arr, int position) { return 1; }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class StringAttribute : Attribute, IBinaryConverterAttribute
        {
            public int CodePage;
            public StringAttributeType StringType;
            public StringAttribute(StringAttributeType type)
            {
                StringType = type;
                CodePage = 1251;

            }
            public StringAttribute(StringAttributeType type, int codePage)
            {
                StringType = type;
                CodePage = codePage;

            }
            public byte[] ToBytes(object obj)
            {
                byte[] result = new byte[0];
                if (obj != null && obj.GetType() == typeof(string))
                {
                    byte[] bytes;
                    string str = (string)obj;
                    switch (StringType)
                    {
                        case StringAttributeType.Pascal8:
                            {
                                string str2;
                                if (str.Length > 255)
                                    str2 = str.Substring(0, 255);
                                else
                                    str2 = str;
                                bytes = Encoding.GetEncoding(CodePage).GetBytes(str2);
                                Array.Resize(ref result, bytes.Length + 1);
                                result[0] = (byte)bytes.Length;
                                Array.Copy(bytes, 0, result, 1, bytes.Length);
                            }
                            break;
                        case StringAttributeType.Pascal16:
                            {
                                string str2;
                                if (str.Length > 65535)
                                    str2 = str.Substring(0, 65535);
                                else
                                    str2 = str;
                                bytes = Encoding.GetEncoding(CodePage).GetBytes(str2);
                                Array.Resize(ref result, bytes.Length + 2);
                                var alength = BitConverter.GetBytes((ushort)bytes.Length);
                                Array.Copy(alength, 0, result, 0, 2);
                                Array.Copy(bytes, 0, result, 2, bytes.Length);
                            }
                            break;
                        case StringAttributeType.Pascal32:
                            {
                                bytes = Encoding.GetEncoding(CodePage).GetBytes(str);
                                Array.Resize(ref result, bytes.Length + 4);
                                var alength = BitConverter.GetBytes(bytes.Length);
                                Array.Copy(alength, 0, result, 0, 4);
                                Array.Copy(bytes, 0, result, 4, bytes.Length);
                            }
                            break;
                        case StringAttributeType.ASCIIZ:
                            {
                                bytes = Encoding.GetEncoding(CodePage).GetBytes(str);
                                Array.Resize(ref result, bytes.Length + 1);
                                Array.Copy(bytes, 0, result, 0, bytes.Length);
                                result[bytes.Length] = 0;
                            }
                            break;
                        case StringAttributeType.UnicodePascal32:
                            {
                                bytes = Encoding.Unicode.GetBytes(str);
                                Array.Resize(ref result, bytes.Length + 4);
                                var alength = BitConverter.GetBytes(bytes.Length);
                                Array.Copy(alength, 0, result, 0, 4);
                                Array.Copy(bytes, 0, result, 4, bytes.Length);
                            }
                            break;
                        case StringAttributeType.UnicodeZ:
                            {
                                bytes = Encoding.Unicode.GetBytes(str);
                                Array.Resize(ref result, bytes.Length + 2);
                                Array.Copy(bytes, 0, result, 0, bytes.Length);
                                result[bytes.Length] = 0;
                                result[bytes.Length + 1] = 0;
                            }
                            break;
                    }
                }
                return result;
            }
            public object ToObject(byte[] bytes)
            {
                string str;
                switch (StringType)
                {
                    case StringAttributeType.Pascal8:
                        {
                            var length = bytes[0];
                            str = Encoding.GetEncoding(CodePage).GetString(bytes, 1, length);
                        }
                        break;
                    case StringAttributeType.Pascal16:
                        {
                            var length = BitConverter.ToUInt16(bytes, 0);
                            str = Encoding.GetEncoding(CodePage).GetString(bytes, 2, length);
                        }
                        break;
                    case StringAttributeType.Pascal32:
                        {
                            var length = BitConverter.ToInt32(bytes, 0);
                            str = Encoding.GetEncoding(CodePage).GetString(bytes, 4, length);
                        }
                        break;
                    case StringAttributeType.ASCIIZ:
                        {
                            /*var length=Array.FindIndex(bytes, b => b == 0);
                            if (length >= 0)
                            {
                                str = Encoding.GetEncoding(CodePage).GetString(bytes, 1, length);
                            }
                            else { str = ""; }*/
                            str = Encoding.GetEncoding(CodePage).GetString(bytes, 0, bytes.Length);
                            var index = str.IndexOf('\u0000');
                            if (index >= 0)
                            {
                                str = str.Substring(0, index);
                            }
                        }
                        break;
                    case StringAttributeType.UnicodePascal32:
                        {
                            var length = BitConverter.ToInt32(bytes, 0);
                            str = Encoding.Unicode.GetString(bytes, 4, length);
                        }
                        break;
                    case StringAttributeType.UnicodeZ:
                        {
                            str = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
                            var index = str.IndexOf('\u0000');
                            if (index >= 0)
                            {
                                str = str.Substring(0, index);
                            }
                        }
                        break;
                    default:
                        str = "";
                        break;
                }

                return str;
            }
            public int ObjectSize(byte[] bytes, int position)
            {
                int length;
                switch (StringType)
                {
                    case StringAttributeType.Pascal8:
                        {
                            length = bytes[position] + 1;
                        }
                        break;
                    case StringAttributeType.Pascal16:
                        {
                            length = BitConverter.ToUInt16(bytes, position) + 2;
                        }
                        break;
                    case StringAttributeType.Pascal32:
                        {
                            length = BitConverter.ToInt32(bytes, position) + 4;
                        }
                        break;
                    case StringAttributeType.ASCIIZ:
                        {
                            length = Array.FindIndex(bytes, b => b == 0);
                            if (length >= 0) length += 1; else length = 0;
                        }
                        break;
                    case StringAttributeType.UnicodePascal32:
                        {
                            length = BitConverter.ToInt32(bytes, position) + 4;
                        }
                        break;
                    case StringAttributeType.UnicodeZ:
                        {
                            var str = Encoding.Unicode.GetString(bytes, position, bytes.Length);
                            var index = str.IndexOf('\u0000');
                            if (index >= 0)
                            {
                                str = str.Substring(0, index);
                            }
                            length = Encoding.Unicode.GetBytes(str).Length + 2;
                        }
                        break;
                    default:
                        length = 0;
                        break;
                }
                return length;
            }
        }

        /// <summary>
        /// PascalN - Размер в байтах впереди N бит на размер (1 байт на символ)
        /// UnicodePascalN - Размер в байтах впереди N бит на размер (2 байт на символ, юникод)
        /// ASCIIZ - Строка заканчивающаяся байтом 0 (1 байт на символ)
        /// UnicodeZ - Строка заканчивающаяся байтом 0 (2 байт на символ, юникод)
        /// </summary>
        public enum StringAttributeType
        {
            Pascal8,
            Pascal16,
            Pascal32,
            UnicodePascal32,
            ASCIIZ,
            UnicodeZ

        }

        /// <summary>
        /// Преобразует массив с несколькими измерениями в массив байт
        /// задается тип данных, и размер по каждому измерению
        /// если измерения не заданы, то в результирующем массиве пишется Ранг массива и размер каждого измерения
        /// если заданы, то описание массива не пишется
        /// 
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class MultidimensionalArrayAttribute : Attribute, IBinaryConverterAttribute
        {
            bool DimensionsKnown = false;
            int[] Dimensions;
            Type ElementType;
            public MultidimensionalArrayAttribute(Type elementType) { ElementType = elementType; }
            public MultidimensionalArrayAttribute(Type elementType, params int[] dimensions)
            {
                ElementType = elementType;
                Dimensions = dimensions;
                DimensionsKnown = true;
            }

            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType().IsArray)
                {
                    var arr = (Array)obj;
                    byte[] bytes;
                    int position = 0;
                    int byteLength = Buffer.ByteLength(arr);
                    if (!DimensionsKnown)
                    {
                        var rank = arr.Rank;
                        bytes = new byte[byteLength + 4 * (rank + 1)];
                        var arank = BitConverter.GetBytes(rank);
                        Array.Copy(arank, 0, bytes, position, arank.Length);
                        position += arank.Length;
                        for (int i = 0; i < rank; i++)
                        {
                            var dim = arr.GetLength(i);
                            var adim = BitConverter.GetBytes(dim);

                            Array.Copy(adim, 0, bytes, position, arank.Length);
                            position += arank.Length;
                        }
                    }
                    else
                    {
                        bytes = new byte[byteLength];
                        position = 0;
                    }
                    Buffer.BlockCopy(arr, 0, bytes, position, byteLength);
                    return bytes;
                }
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                var elementSize = MemberInfoProcessing.SizeOf(ElementType);
                int elements = 1;
                if (!DimensionsKnown)
                {
                    int pos = 0;
                    var rank = BitConverter.ToInt32(bytes, pos);
                    pos += 4;
                    int[] dimensions = new int[rank];
                    for (int i = 0; i < rank; i++)
                    {
                        var dimension = BitConverter.ToInt32(bytes, pos);
                        pos += 4;
                        dimensions[i] = dimension;
                        elements *= dimension;
                    }
                    var length = elements * elementSize;
                    var arr = Array.CreateInstance(ElementType, dimensions);
                    Buffer.BlockCopy(bytes, pos, arr, 0, length);
                    return arr;
                }
                else
                {
                    for (int i = 0; i < Dimensions.Length; i++)
                    {
                        elements *= Dimensions[i];
                    }
                    var length = elements * elementSize;
                    var arr = Array.CreateInstance(ElementType, Dimensions);
                    Buffer.BlockCopy(bytes, 0, arr, 0, length);
                    return arr;
                }
            }
            public int ObjectSize(byte[] bytes, int position)
            {
                var elementSize = MemberInfoProcessing.SizeOf(ElementType);
                int elements = 1;
                if (!DimensionsKnown)
                {
                    int pos = position;
                    var rank = BitConverter.ToInt32(bytes, pos);
                    pos += 4;
                    for (int i = 0; i < rank; i++)
                    {
                        var dimension = BitConverter.ToInt32(bytes, pos);
                        pos += 4;
                        elements *= dimension;
                    }
                    return elements * elementSize + (rank + 1) * 4;
                }
                else
                {
                    for (int i = 0; i < Dimensions.Length; i++)
                    {
                        elements *= Dimensions[i];
                    }
                    return elements * elementSize;
                }
            }
        }

        /// <summary>
        /// Одномерный массив. Если размеры не заданы, то размер записывается первым байтом
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class OneDimensionalArrayAttribute : Attribute, IBinaryConverterAttribute
        {
            bool LengthKnown = false;
            Type ElementType;
            int Elements;
            public OneDimensionalArrayAttribute(Type elementType) { ElementType = elementType; LengthKnown = false; }
            public OneDimensionalArrayAttribute(Type elementType, int elements) { ElementType = elementType; Elements = elements; LengthKnown = true; }

            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType().IsArray)
                {
                    var arr = (Array)obj;
                    byte[] bytes;
                    int position = 0;
                    int byteLength = Buffer.ByteLength(arr);
                    if (!LengthKnown)
                    {
                        bytes = new byte[byteLength + 4];
                        var alength = BitConverter.GetBytes(arr.Length);
                        Array.Copy(alength, 0, bytes, position, alength.Length);
                        position += alength.Length;
                    }
                    else
                    {
                        bytes = new byte[byteLength];
                        position = 0;
                    }
                    Buffer.BlockCopy(arr, 0, bytes, position, byteLength);
                    return bytes;
                }
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                var elementSize = MemberInfoProcessing.SizeOf(ElementType);
                if (!LengthKnown)
                {
                    int pos = 0;
                    var elements = BitConverter.ToInt32(bytes, pos);
                    pos += 4;
                    var length = elements * elementSize;
                    var arr = Array.CreateInstance(ElementType, length);
                    Buffer.BlockCopy(bytes, pos, arr, 0, elements);
                    return arr;
                }
                else
                {
                    var length = Elements * elementSize;
                    var arr = Array.CreateInstance(ElementType, Elements);
                    Buffer.BlockCopy(bytes, 0, arr, 0, length);
                    return arr;
                }
            }
            public int ObjectSize(byte[] bytes, int position)
            {
                var elementSize = MemberInfoProcessing.SizeOf(ElementType);
                if (!LengthKnown)
                {
                    int pos = position;
                    var elements = BitConverter.ToInt32(bytes, pos);
                    pos += 4;
                    var length = elements * elementSize;
                    return length + 4;
                }
                else
                {
                    var length = Elements * elementSize;
                    return length;
                }
            }
        }

        /// <summary>
        /// Одномерный массив. Если размеры не заданы, то размер записывается первым байтом
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class OneDimensionalSizeArrayAttribute : Attribute, IBinaryConverterAttribute
        {
            bool LengthKnown = false;
            Type ElementType;
            int Elements;
            public OneDimensionalSizeArrayAttribute(Type elementType) { ElementType = elementType; LengthKnown = false; }
            public OneDimensionalSizeArrayAttribute(Type elementType, int elements) { ElementType = elementType; Elements = elements; LengthKnown = true; }

            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType().IsArray)
                {
                    var arr = (Array)obj;
                    byte[] bytes;
                    int position = 0;
                    int byteLength = Buffer.ByteLength(arr);
                    if (!LengthKnown)
                    {
                        bytes = new byte[byteLength + 4];
                        var alength = BitConverter.GetBytes(byteLength);
                        Array.Copy(alength, 0, bytes, position, alength.Length);
                        position += alength.Length;
                    }
                    else
                    {
                        bytes = new byte[byteLength];
                        position = 0;
                    }
                    Buffer.BlockCopy(arr, 0, bytes, position, byteLength);
                    return bytes;
                }
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                var elementSize = MemberInfoProcessing.SizeOf(ElementType);
                if (!LengthKnown)
                {
                    int pos = 0;
                    var length = BitConverter.ToInt32(bytes, pos);
                    pos += 4;
                    var elements = length / elementSize;
                    var arr = Array.CreateInstance(ElementType, elements);// length);
                    Buffer.BlockCopy(bytes, pos, arr, 0, elements);
                    return arr;
                }
                else
                {
                    var length = Elements * elementSize;
                    var arr = Array.CreateInstance(ElementType, Elements);
                    Buffer.BlockCopy(bytes, 0, arr, 0, length);
                    return arr;
                }
            }
            public int ObjectSize(byte[] bytes, int position)
            {
                var elementSize = MemberInfoProcessing.SizeOf(ElementType);
                if (!LengthKnown)
                {
                    var length = BitConverter.ToInt32(bytes, position);
                    return length + 4;
                }
                else
                {
                    var length = Elements * elementSize;
                    return length;
                }
            }
        }

        /// <summary>
        /// Класс
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class ClassAttribute : Attribute, IBinaryConverterAttribute
        {
            private Type T;
            public ClassAttribute(Type t) { T = t; }
            public byte[] ToBytes(object obj)
            {
                if (obj != null && obj.GetType().IsClass && obj.GetType() != typeof(string))
                {
                    var bytes = ObjectToBytes(obj);
                    return bytes;
                }
                return new byte[0];
            }
            public object ToObject(byte[] bytes)
            {
                var len = ObjectFromBytes(T, bytes, out object v);
                return v;
            }
            public int ObjectSize(byte[] bytes, int position)
            {
                var arr = new byte[bytes.Length - position];
                Array.Copy(bytes, position, arr, 0, arr.Length);
                var len = ObjectFromBytes(T, arr, out object v);
                return len;

            }
        }

    }


    /// <summary>
    /// В классе региструруются типы объектов и функции проверяющие соответствие структуры предполагаемой структуре и обрабатывающие результат
    /// </summary>
    /// 
    public class BinaryConverterList : BinaryConverter
    {
        public delegate bool BinaryConverterListProc(object obj);

        List<BinaryConverterListElement> Elements;

        public BinaryConverterList()
        {
            Elements = new List<BinaryConverterListElement>();
        }


        public void AddConvertType(Type objectType, BinaryConverterListProc proc)
        {
            var e = new BinaryConverterListElement();
            e.ObjectType = objectType;
            e.Proc = proc;
            Elements.Add(e);
        }

        public int BytesToObject(byte[] arr, out Type type, out object result)
        {
            lock (Elements)
            {
                foreach (var e in Elements)
                {
                    object o = null;
                    int len = 0;
                    try
                    {
                        len = ObjectFromBytes(e.ObjectType, arr, out o);
                    }
                    catch (Exception)
                    {
                        o = null;
                    }
                    if (o != null)
                    {
                        if (e.Proc != null)
                        {
                            var res = e.Proc(o);
                            if (res)
                            {
                                e.TrueCounter++;
                                var tsk = new Task(SortElements);
                                tsk.Start();
                                type = e.ObjectType;
                                result = o;
                                return len;
                            }
                        }
                        else
                        {
                        }
                    }

                }
            }
            type = null;
            result = null;
            return 0;
        }
        public int BytesToObject(byte[] arr)
        {
            lock (Elements)
            {
                foreach (var e in Elements)
                {
                    object o = null;
                    int len = 0;
                    try
                    {
                        len = ObjectFromBytes(e.ObjectType, arr, out o);
                    }
                    catch (Exception)
                    {
                        o = null;
                    }
                    if (o != null)
                    {
                        if (e.Proc != null)
                        {
                            var res = e.Proc(o);
                            if (res)
                            {
                                e.TrueCounter++;
                                var tsk = new Task(SortElements);
                                tsk.Start();
                                return len;
                            }
                        }
                        else
                        {
                        }
                    }

                }
            }
            return 0;
        }
        private void SortElements()
        {
            lock (Elements)
            {
                Elements = Elements.OrderByDescending(e => e.TrueCounter).ToList();
            }
        }


        private class BinaryConverterListElement
        {
            public Type ObjectType;
            public BinaryConverterListProc Proc;
            public int TrueCounter;
        }
    }

    /*
 	public class Test1
	{
		[BinaryConverter.Enum8]
		public TestEnum TE;
		[BinaryConverter.Double]
		public double A;
	}
	public class Test2 
	{
		[BinaryConverter.Enum8]
		public TestEnum TE;
		[BinaryConverter.Int32]
		public int A;
		[BinaryConverter.Int32]
		public int B;
		[BinaryConverter.Int32]
		public int C;
	}
	public enum TestEnum
    {
		A=1,
		B=2
    }

            BinaryConverterList bcl = new BinaryConverterList();
			bcl.AddConvertType(typeof(Test1), (o) =>
			{
				var test1 = o as Test1;
				if (test1 != null)
                {
					if (test1.TE==TestEnum.A)
                    {
						Console.WriteLine("Test1");
						return true;
                    }
                }
				return false;
			});

			bcl.AddConvertType(typeof(Test2), (o) =>
			{
				var test2 = o as Test2;
				if (test2 != null)
				{
					if (test2.TE == TestEnum.B)
					{
						Console.WriteLine("Test2");
						return true;
					}
				}
				return false;
			});

			var t1 = new Test1();
			t1.TE = TestEnum.A;
			t1.A = 1.01;
			var bytes1 = BinaryConverter.ToBytes(t1);

			var t2 = new Test2();
			t2.TE = TestEnum.B;
			t2.A = 1;
			t2.B = 2;
			var bytes2 = BinaryConverter.ToBytes(t2);

			var len1=bcl.BytesToObject(bytes1);
			var len2 = bcl.BytesToObject(bytes2);

			Console.WriteLine(len1);
			Console.WriteLine(len2);

			//var l = BinaryConverter.FromBytes(bytes, out Test t1);
			Console.ReadKey();

    Результат:
Test1
Test2
9
13*/

}


