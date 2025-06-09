using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DoMCLib.Tools
{
    public static class MemberInfoProcessing
    {
        public static object GetValue(MemberInfo mi, object o)
        {
            if (mi is FieldInfo)
            {
                return (mi as FieldInfo).GetValue(o);
            }
            if (mi is PropertyInfo)
            {
                return (mi as PropertyInfo).GetValue(o, null);
            }
            return null;
        }
        public static Type GetMemberType(MemberInfo mi)
        {
            if (mi is FieldInfo)
            {
                return (mi as FieldInfo).FieldType;
            }
            if (mi is PropertyInfo)
            {
                return (mi as PropertyInfo).PropertyType;
            }
            return null;
        }

        public static void SetValue(MemberInfo mi, object o, object value)
        {
            if (mi is FieldInfo)
            {
                (mi as FieldInfo).SetValue(o, value);
            }
            if (mi is PropertyInfo)
            {
                try
                {
                    (mi as PropertyInfo).SetValue(o, value, null);
                }
                catch (ArgumentException)
                {
                }
            }
        }


        public static List<MemberInfo> GetMemberInfoList(Type T)
        {
            var tfields = T.GetFields();
            var tproperties = T.GetProperties();
            List<MemberInfo> mi = new List<MemberInfo>();
            mi.AddRange(tfields);
            mi.AddRange(tproperties);
            return mi;
        }

        public static MemberInfo GetMemberInfo(Type T, string fieldname)
        {
            var mil = GetMemberInfoList(T);
            var mi = mil.Find(m => m.Name.Equals(fieldname));
            return mi;
        }
        public static MemberInfo GetMemberInfo(List<MemberInfo> MIList, string fieldname)
        {
            var mi = MIList.Find(m => m.Name.Equals(fieldname));
            return mi;
        }

        public static object CreateObject(MemberInfo mi)
        {
            if (mi is FieldInfo)
            {
                var x = Activator.CreateInstance((mi as FieldInfo).FieldType);
                return x;
            }
            if (mi is PropertyInfo)
            {
                var x = Activator.CreateInstance((mi as PropertyInfo).PropertyType);
                return x;
            }
            return null;
        }
        public static object CreateObject(Type t)
        {
            var x = Activator.CreateInstance(t);
            return x;
        }
        public static int SizeOf(MemberInfo mi)
        {
            try
            {
                var x = CreateObject(mi);
                var size = SizeOf(x);//System.Runtime.InteropServices.Marshal.SizeOf(x);
                return size;
            }
            catch 
            {
                return -1;
            }
        }
        public static int SizeOfBase(object o)
        {
            try
            {
                var size = System.Runtime.InteropServices.Marshal.SizeOf(o);
                return size;
            }
            catch 
            {
                return -1;
            }
        }
        public static int SizeOf(object obj)
        {
            int res = 0;
            var type = obj.GetType();
            if (type == typeof(sbyte)) res = sizeof(sbyte);
            else
            if (type == typeof(byte)) res = sizeof(byte);
            else
            if (type == typeof(short)) res = sizeof(short);
            else
            if (type == typeof(ushort)) res = sizeof(ushort);
            else
            if (type == typeof(int)) res = sizeof(int);
            else
            if (type == typeof(uint)) res = sizeof(uint);
            else
            if (type == typeof(long)) res = sizeof(long);
            else
            if (type == typeof(ulong)) res = sizeof(ulong);
            else
            if (type == typeof(char)) res = sizeof(char);
            else
            if (type == typeof(float)) res = sizeof(float);
            else
            if (type == typeof(double)) res = sizeof(double);
            else
            if (type == typeof(decimal)) res = sizeof(decimal);
            else
            if (type == typeof(bool)) res = sizeof(bool);
            else
            if (type.IsEnum) res = sizeof(int);
            else
                res = -1;// ObjectToByteArrayObject(obj);

            return res;
        }
        public static int SizeOf(Type type)
        {
            int res = 0;
            if (type == typeof(sbyte)) res = sizeof(sbyte);
            else
            if (type == typeof(byte)) res = sizeof(byte);
            else
            if (type == typeof(short)) res = sizeof(short);
            else
            if (type == typeof(ushort)) res = sizeof(ushort);
            else
            if (type == typeof(int)) res = sizeof(int);
            else
            if (type == typeof(uint)) res = sizeof(uint);
            else
            if (type == typeof(long)) res = sizeof(long);
            else
            if (type == typeof(ulong)) res = sizeof(ulong);
            else
            if (type == typeof(char)) res = sizeof(char);
            else
            if (type == typeof(float)) res = sizeof(float);
            else
            if (type == typeof(double)) res = sizeof(double);
            else
            if (type == typeof(decimal)) res = sizeof(decimal);
            else
            if (type == typeof(bool)) res = sizeof(bool);
            else
            if (type.IsEnum) res = sizeof(int);
            else
                res = -1;// ObjectToByteArrayObject(obj);

            return res;
        }
        public static byte[] ObjectToByteArray(object obj)
        {
            byte[] res;
            var type = obj.GetType();
            if (type == typeof(sbyte)) res = new[] { (byte)obj };
            else
            if (type == typeof(byte)) res = new[] { (byte)obj };
            else
            if (type == typeof(short)) res = BitConverter.GetBytes((short)obj);
            else
            if (type == typeof(ushort)) res = BitConverter.GetBytes((ushort)obj);
            else
            if (type == typeof(int)) res = BitConverter.GetBytes((int)obj);
            else
            if (type == typeof(uint)) res = BitConverter.GetBytes((uint)obj);
            else
            if (type == typeof(long)) res = BitConverter.GetBytes((long)obj);
            else
            if (type == typeof(ulong)) res = BitConverter.GetBytes((ulong)obj);
            else
            if (type == typeof(char)) res = BitConverter.GetBytes((char)obj);
            else
            if (type == typeof(float)) res = BitConverter.GetBytes((float)obj);
            else
            if (type == typeof(double)) res = BitConverter.GetBytes((double)obj);
            else
            if (type == typeof(decimal)) res = GetBytes((decimal)obj);
            else
            if (type == typeof(bool)) res = BitConverter.GetBytes((bool)obj);
            else
            if (type.IsEnum) res = BitConverter.GetBytes((int)obj);
            else
                res = [];

            return res;
        }
        public static object? ByteArrayToObject(byte[] arr, Type type)
        {
            object res;
            if (type == typeof(sbyte)) res = (sbyte)arr[0];
            else
            if (type == typeof(byte)) res = arr[0];
            else
            if (type == typeof(short)) res = BitConverter.ToInt16(arr, 0);
            else
            if (type == typeof(ushort)) res = BitConverter.ToUInt16(arr, 0);
            else
            if (type == typeof(int)) res = BitConverter.ToInt32(arr, 0);
            else
            if (type == typeof(uint)) res = BitConverter.ToUInt32(arr, 0);
            else
            if (type == typeof(long)) res = BitConverter.ToInt64(arr, 0);
            else
            if (type == typeof(ulong)) res = BitConverter.ToUInt64(arr, 0);
            else
            if (type == typeof(char)) res = BitConverter.ToChar(arr, 0);
            else
            if (type == typeof(float)) res = BitConverter.ToSingle(arr, 0);
            else
            if (type == typeof(double)) res = BitConverter.ToDouble(arr, 0);
            else
            if (type == typeof(decimal)) res = ToDecimal(arr);
            else
            if (type == typeof(bool)) res = BitConverter.ToBoolean(arr, 0);
            else
            if (type.IsEnum) res = BitConverter.ToInt32(arr, 0);
            else
                res = null;

            return res;
        }
  /*
        private static byte[] ObjectToByteArrayObject(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
        private static object ByteArrayToObjectObject(byte[] param)
        {
            object obj = null;
            using (MemoryStream ms = new MemoryStream(param))
            {
                BinaryFormatter br = new BinaryFormatter();
                obj = br.Deserialize(ms);
            }

            return obj;
        }
  */
        public static byte[] GetBytes(decimal dec)
        {
            //LoadConfiguration four 32 bit integers from the Decimal.GetBits function
            int[] bits = decimal.GetBits(dec);
            //Create a temporary list to hold the bytes
            List<byte> bytes = new List<byte>();
            //iterate each 32 bit integer
            foreach (int i in bits)
            {
                //add the bytes of the current 32bit integer
                //to the bytes list
                bytes.AddRange(BitConverter.GetBytes(i));
            }
            //return the bytes list as an array
            return bytes.ToArray();
        }
        public static decimal ToDecimal(byte[] bytes)
        {
            //check that it is even possible to convert the array
            if (bytes.Count() < 16)
                throw new Exception("A decimal must be created from exactly 16 bytes");
            //make an array to convert back to int32's
            int[] bits = new int[4];
            for (int i = 0; i <= 15; i += 4)
            {
                //convert every 4 bytes into an int32
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }
            //Use the decimal's new constructor to
            //create an instance of decimal
            return new decimal(bits);
        }
    }

}
