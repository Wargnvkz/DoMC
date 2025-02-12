using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DoMCLib.DB
{
    /// <summary>
    /// Класс файловой базы данных, где каждая запись это отдельный файл. Предназначен для хранения данных больших объемов 
    /// с большим входящим потоком без возможности изменять содержимое.
    /// Имя файла, это поля и значения, по которым можно искать нужные записи они обозначаются атрибутами FileStorageHeaderAttribute. 
    /// FileStorageHeaderIDAttribute 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileStorage<T> where T : class, new()
    {
        static string DateTimeFormat = "yyyy-MM-dd HH_mm_ss_fffffff";
        static List<Tuple<char, char>> Replaces = new List<Tuple<char, char>>() {
            new Tuple<char, char>(':', '\u0080'),
            new Tuple<char, char>('\\', '\u0081'),
            new Tuple<char, char>('*', '\u0082'),
            new Tuple<char, char>('?', '\u0083'),
            new Tuple<char, char>('/', '\u0084'),
            new Tuple<char, char>('<', '\u0085'),
            new Tuple<char, char>('>', '\u0086'),
            new Tuple<char, char>('\"', '\u0087'),
            new Tuple<char, char>('|', '\u0088'),
            new Tuple<char, char>(';', '\u0089'),
            new Tuple<char, char>('=', '\u0090')
        };
        // Структура имени файла: 01.01.2024 15_00_12.001;Field1=xxxx;Field2=yyyyy;Field3=xxxxxxxxxxxxxxxx.fs 

        #region FileNames
        #region ComposeName
        public static string CreateFileName(T data)
        {
            var headerAttributeType = typeof(FileStorageHeaderAttribute);
            var headerIDAttributeType = typeof(FileStorageHeaderIDAttribute);
            var stringType = typeof(string);

            List<string> resultNameElements = new List<string>();

            var IDDateTime = DateTime.Now.ToString(DateTimeFormat);
            var dataTypeMembers = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var mi in dataTypeMembers)
            {
                var mt = GetMemberType(mi);
                if (mt == null) continue;
                if (mt.IsValueType || mt.IsArray || mt == stringType)
                {
                    var attributes = mi.GetCustomAttributes(headerAttributeType, false);
                    var attributesID = mi.GetCustomAttributes(headerIDAttributeType, false);
                    if (attributes.Length != 0)
                    {
                        resultNameElements.Add(String.Format("{0}={1}", mi.Name, ObjectToString(GetValue(mi, data))));
                    }
                    if (attributesID.Length != 0)
                    {
                        var oticks = GetValue(mi, data);
                        if (Convert.ToInt64(oticks) != 0)
                        {
                            var ticks = Convert.ToInt64(oticks);
                            var dt = new DateTime(ticks);
                            IDDateTime = dt.ToString(DateTimeFormat);
                        }
                    }
                }
            }
            resultNameElements.Insert(0, IDDateTime);
            var fileName = String.Join(";", resultNameElements);
            return fileName;
        }

        protected static string ObjectToString(object o)
        {
            if (o == null) return String.Empty;
            if (o is string) return String.Format("{0}", StringToFileString((string)o));
            var oType = o.GetType();
            if (oType.IsValueType)
            {
                if (oType == typeof(DateTime))
                {
                    o = ((DateTime)o).Ticks;
                }
                return Convert.ToString(o);
            }
            if (oType.IsArray)
            {
                if (oType.GetElementType().IsValueType)
                {
                    var oArray = (Array)o;
                    var arr = new byte[Buffer.ByteLength(oArray)];
                    Buffer.BlockCopy(oArray, 0, arr, 0, arr.Length);
                    var str = StringToFileString(Convert.ToBase64String(arr));
                    return str;
                }
            }
            return String.Empty;
        }

        protected static string StringToFileString(string str)
        {
            foreach (var r in Replaces)
            {
                str = str.Replace(r.Item1, r.Item2);
            }
            return str;
        }
        #endregion

        public static bool HasAnyFieldsWithoutAttributes()
        {

            var headerAttributeType = typeof(FileStorageHeaderAttribute);
            var headerIDAttributeType = typeof(FileStorageHeaderIDAttribute);
            var dataTypeMembers = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var mi in dataTypeMembers)
            {
                var mt = GetMemberType(mi);
                if (mt == null) continue;
                var attributes = mi.GetCustomAttributes(headerAttributeType, false);
                var attributesID = mi.GetCustomAttributes(headerIDAttributeType, false);
                if (attributes.Length == 0 && attributesID.Length == 0)
                    return true;
            }
            return false;
        }

        #region DecomposeName
        public static T DecomposeFileName(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            var data = new T();
            var parts = fileName.Split(';');
            Dictionary<string, string> FieldValue = new Dictionary<string, string>();
            foreach (var p in parts)
            {
                var eqPos = p.IndexOf('=');
                if (eqPos >= 0)
                {
                    var field = p.Substring(0, eqPos);
                    var value = p.Substring(eqPos + 1);
                    if (!FieldValue.ContainsKey(field))
                        FieldValue.Add(field, value);
                }
            }

            if (!DateTime.TryParseExact(parts[0], DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out DateTime dt))
            {
                throw new ArgumentException();
            }

            var headerAttributeType = typeof(FileStorageHeaderAttribute);
            var headerIDAttributeType = typeof(FileStorageHeaderIDAttribute);

            var stringType = typeof(string);

            var dataTypeMembers = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var mi in dataTypeMembers)
            {
                var mt = GetMemberType(mi);
                if (mt == null) continue;
                var attributesID = mi.GetCustomAttributes(headerIDAttributeType, false);
                if (attributesID.Length > 0)
                {
                    if (mt == typeof(long))
                    {
                        SetValue(mi, data, dt.Ticks);
                    }
                }
                else
                {
                    if (mt.IsValueType || mt.IsArray || mt == stringType)
                    {
                        var attributes = mi.GetCustomAttributes(headerAttributeType, false);
                        if (attributes.Length != 0 || attributes.Length != 0)
                        {
                            if (FieldValue.ContainsKey(mi.Name))
                            {
                                var strValue = FieldValue[mi.Name];
                                var resValue = StringToObject(strValue, mt);
                                SetValue(mi, data, resValue);
                            }
                        }
                    }
                }
            }

            return data;
        }

        protected static object StringToObject(string str, Type oType)
        {
            if (string.IsNullOrEmpty(str)) return null;
            var stringType = typeof(string);
            if (oType == stringType) return FileStringToString(str);
            if (oType.IsValueType)
            {
                if (oType == typeof(DateTime))
                {
                    if (Int64.TryParse(str, out long ticks))
                    {
                        return new DateTime(ticks);
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                }
                TypeConverter typeConverter = TypeDescriptor.GetConverter(oType);
                object propValue = typeConverter.ConvertFromString(str);
                return propValue;
            }
            if (oType.IsArray)
            {
                var eType = oType.GetElementType();
                if (eType.IsValueType)
                {
                    var eSize = System.Runtime.InteropServices.Marshal.SizeOf(eType);
                    var base64str = FileStringToString(str);
                    var byteArr = Convert.FromBase64String(base64str);

                    var resArr = Array.CreateInstance(eType, byteArr.Length / eSize);
                    Buffer.BlockCopy(byteArr, 0, resArr, 0, byteArr.Length);
                    return resArr;
                }
            }
            return String.Empty;
        }

        protected static string FileStringToString(string str)
        {
            foreach (var r in Replaces)
            {
                str = str.Replace(r.Item2, r.Item1);
            }
            return str;
        }
        #endregion
        #endregion

        #region Save data in files
        private static string AddExtension(string fileName, string newExtesion)
        {
            return Path.ChangeExtension(fileName, newExtesion);
        }
        #region No compression
        public static string SaveFile(T data, string path, string fileExtenstion)
        {
            return SaveFileUncompressed(data, path, fileExtenstion);
        }
        public static T OpenFile(string fileName)
        {
            try
            {
                return OpenFileCompressed(fileName);
            }
            catch
            {
                return OpenFileUncompressed(fileName);
            }
        }
        protected static string SaveFileUncompressed(T data, string path, string fileExtenstion)
        {
            var fileName = CreateFileName(data);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            fileName = Path.Combine(path, AddExtension(fileName, fileExtenstion));
            using (var file = File.OpenWrite(fileName))
            {
                if (HasAnyFieldsWithoutAttributes())
                {
                    var bf = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                    bf.WriteObject(file, data);
                    /*var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bf.Serialize(file, data);*/
                }
            }
            return fileName;
        }
        protected static T OpenFileUncompressed(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            {
                if (file.Length > 0)
                {
                    var bf = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                    return bf.ReadObject(file) as T;
                    /*var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return bf.Deserialize(file) as T;*/
                }
                return DecomposeFileName(fileName);
            }
        }
        #endregion
        #region GZip
        protected static string SaveFileGZip(T data, string path, string fileExtenstion)
        {
            var fileName = CreateFileName(data);
            fileName = Path.Combine(path, AddExtension(fileName, fileExtenstion));
            using (var file = File.OpenWrite(fileName))
            {
                if (HasAnyFieldsWithoutAttributes())
                {
                    var ms = new MemoryStream();
                    var bf = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                    bf.WriteObject(ms, data);
                    //var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //bf.Serialize(ms, data);
                    ms.Seek(0, 0);
                    CompressGZip(ms, file);
                }
            }
            return fileName;
        }
        protected static T OpenFileGZip(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            {
                if (file.Length > 0)
                {

                    var ms = new MemoryStream();
                    DecompressGZip(file, ms);
                    ms.Seek(0, 0);
                    var bf = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                    return bf.ReadObject(ms) as T;
                    //var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //return bf.Deserialize(ms) as T;
                }
                return DecomposeFileName(fileName);
            }
        }
        #endregion
        #region Compression
        protected static string SaveFileCompressed(T data, string path, string fileExtenstion)
        {
            var fileName = CreateFileName(data);
            fileName = Path.Combine(path, AddExtension(fileName, fileExtenstion));
            using (var file = File.OpenWrite(fileName))
            {
                if (HasAnyFieldsWithoutAttributes())
                {
                    var ms = new MemoryStream();
                    var bf = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                    bf.WriteObject(ms, data);
                    //var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //bf.Serialize(ms, data);
                    ms.Seek(0, 0);
                    Compress(ms, file);
                }
            }
            return fileName;
        }
        protected static T OpenFileCompressed(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            {
                if (file.Length > 0)
                {
                    var ms = new MemoryStream();
                    Decompress(file, ms);
                    ms.Seek(0, 0);
                    var bf = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                    return bf.ReadObject(ms) as T;
                    //var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //return bf.Deserialize(ms) as T;
                }
                return DecomposeFileName(fileName);
            }
        }
        #endregion
        #region Binary
        public static void SaveFileBinary(string filename, byte[] data)
        {
            using (var file = File.OpenWrite(filename))
            {
                file.Write(data, 0, data.Length);
            }
        }
        public static byte[] OpenFileBinary(string fileName)
        {
            byte[] data;
            using (var file = File.OpenRead(fileName))
            {
                data = new byte[file.Length];
                file.Read(data, 0, data.Length);
            }
            return data;
        }

        #endregion

        public static void DeleteFile(string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
        }
        #endregion

        #region Fields and properties tools
        protected internal static object GetValue(System.Reflection.MemberInfo mi, object o)
        {
            if (mi is System.Reflection.FieldInfo)
            {
                return (mi as System.Reflection.FieldInfo).GetValue(o);
            }
            if (mi is System.Reflection.PropertyInfo)
            {
                return (mi as System.Reflection.PropertyInfo).GetValue(o, null);
            }
            return null;
        }
        protected internal static Type GetMemberType(System.Reflection.MemberInfo mi)
        {
            if (mi is System.Reflection.FieldInfo)
            {
                return (mi as System.Reflection.FieldInfo).FieldType;
            }
            if (mi is System.Reflection.PropertyInfo)
            {
                return (mi as System.Reflection.PropertyInfo).PropertyType;
            }
            return null;
        }

        protected internal static void SetValue(System.Reflection.MemberInfo mi, object o, object value)
        {
            if (mi is System.Reflection.FieldInfo)
            {
                (mi as System.Reflection.FieldInfo).SetValue(o, value);
            }
            if (mi is System.Reflection.PropertyInfo)
            {
                try
                {
                    (mi as System.Reflection.PropertyInfo).SetValue(o, value, null);
                }
                catch (ArgumentException)
                {
                }
            }
        }
        protected internal static object GetValue(string name, object o)
        {
            var mis = GetFieldAndProperies(o.GetType());
            var mi = mis.Find(m => m.Name == name);
            if (mi == null) return null;
            if (mi is System.Reflection.FieldInfo)
            {
                return (mi as System.Reflection.FieldInfo).GetValue(o);
            }
            if (mi is System.Reflection.PropertyInfo)
            {
                return (mi as System.Reflection.PropertyInfo).GetValue(o, null);
            }
            return null;
        }
        protected internal static Type GetMemberType(string name, Type type)
        {
            var mis = GetFieldAndProperies(type);
            var mi = mis.Find(m => m.Name == name);
            if (mi == null) return null;
            if (mi is System.Reflection.FieldInfo)
            {
                return (mi as System.Reflection.FieldInfo).FieldType;
            }
            if (mi is System.Reflection.PropertyInfo)
            {
                return (mi as System.Reflection.PropertyInfo).PropertyType;
            }
            return null;
        }

        protected internal static void SetValue(string name, object o, object value)
        {
            var mis = GetFieldAndProperies(o.GetType());
            var mi = mis.Find(m => m.Name == name);
            if (mi == null) return;
            if (mi is System.Reflection.FieldInfo)
            {
                (mi as System.Reflection.FieldInfo).SetValue(o, value);
            }
            if (mi is System.Reflection.PropertyInfo)
            {
                try
                {
                    (mi as System.Reflection.PropertyInfo).SetValue(o, value, null);
                }
                catch (ArgumentException)
                {
                }
            }
        }
        protected internal static List<System.Reflection.MemberInfo> GetFieldAndProperies<T1>()
        {
            var type = typeof(T1);
            return GetFieldAndProperies(type);
        }
        protected internal static List<System.Reflection.MemberInfo> GetFieldAndProperies(Type type)
        {
            var Members = type.GetFields().Cast<System.Reflection.MemberInfo>().ToList();
            Members.AddRange(type.GetProperties().Cast<System.Reflection.MemberInfo>().ToList());
            return Members;
        }
        #endregion

        #region Compression
        public static byte[] Compress(byte[] data)
        {
            if (data == null) return new byte[0];
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionMode.Compress))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
        public static void Compress(Stream from, Stream To)
        {
            from.Seek(0, 0);
            using (var dstream = new DeflateStream(To, CompressionMode.Compress))
            {
                from.CopyTo(dstream);
            }
        }

        public static void Decompress(Stream from, Stream To)
        {
            from.Seek(0, 0);
            using (var dstream = new DeflateStream(from, CompressionMode.Decompress))
            {
                dstream.CopyTo(To);
            }
        }

        public static void CompressGZip(Stream from, Stream To)
        {
            from.Seek(0, 0);
            using (var dstream = new GZipStream(To, CompressionMode.Compress))
            {
                from.CopyTo(dstream);
            }
        }

        public static void DecompressGZip(Stream from, Stream To)
        {
            from.Seek(0, 0);
            using (var dstream = new GZipStream(from, CompressionMode.Decompress))
            {
                dstream.CopyTo(To);
            }
        }
        #endregion
    }

    /// <summary>
    /// Атрибут на поле указывающий, что значение этого поля будет использовано в имени файла
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FileStorageHeaderAttribute : Attribute { }
    /// <summary>
    /// Атрибут, где будет хранится ID записи. Тип long. Он не обязателен
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FileStorageHeaderIDAttribute : Attribute { }
}
