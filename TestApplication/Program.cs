using System;
using System.Net;
using System.Runtime.InteropServices;

namespace TestApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var class2 = new Class2() { Data = [1, 2, 3, 4, 5, 6, 7, 8], Volume = 0.01d };
            var class1 = new Class1() { ID = -1, Name = "Test", Data = class2 };

            var dto1 = DoMCLib.Tools.Mapper.Map<DTO1>(class1);
            var dto2 = DoMCLib.Tools.Mapper.Map<DTO2>(class1);
            var dto3 = DoMCLib.Tools.Mapper.Map<DTO3>(class1);
            Console.WriteLine($"dto1.ID = {dto1.ID}");
            Console.WriteLine($"dto1.Name = {dto1.Name}");
            Console.WriteLine($"dto1.Volume = {dto1.Volume}");
            Console.WriteLine($"dto1.VolumeData = {String.Join(", ", dto1.VolumeData)}");
            Console.WriteLine($"dto2.NewName = {dto2.NewName}");
            Console.WriteLine($"dto3.Data = {String.Join(", ", dto3.Data)}");
            Console.ReadKey();
        }


    }

    public class Class1
    {
        [DoMCLib.Tools.MapTo(typeof(DTO1), "ID")]
        public int ID { get; set; }
        [DoMCLib.Tools.MapTo(typeof(DTO1), "Name")]
        [DoMCLib.Tools.MapTo(typeof(DTO2), "NewName")]
        public string Name { get; set; }
        public Class2 Data { get; set; }
    }
    public class Class2
    {
        [DoMCLib.Tools.MapTo(typeof(DTO1), "Volume")]
        public double Volume { get; set; }
        [DoMCLib.Tools.MapTo(typeof(DTO1), "VolumeData")]
        [DoMCLib.Tools.MapTo(typeof(DTO3), "Data")]
        public byte[] Data { get; set; }
    }
    public class DTO1
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Volume { get; set; }
        public byte[] VolumeData { get; set; }

    }

    public class DTO2
    {
        public string NewName { get; set; }

    }
    public class DTO3
    {
        public DTO3(string NewName)
        {

        }
        public byte[] Data { get; set; }

    }

}
