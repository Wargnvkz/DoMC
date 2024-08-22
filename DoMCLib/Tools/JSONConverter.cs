using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DoMCLib.Tools
{
    public class JSONConverter
    {
        public static string ToJSON<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var mem = new MemoryStream())
            {
                serializer.WriteObject(mem, obj);
                mem.Seek(0, 0);
                using (var sr = new StreamReader(mem))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        public static T FromJSON<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }
    }

}
