using System.Diagnostics;

namespace SimpleTest
{
    internal class Program
    {
        static long[] times = new long[10];
        static void Main(string[] args)
        {
            string[] path = ["c:\\windows\\system32", "\\\\192.168.211.100\\d$\\DBArchiveISCSI\\01.2025\\", "\\\\192.168.211.100\\d$\\DBArchiveISCSI\\11.2024\\"];
            var sw = new Stopwatch();
            sw.Start();
            var list0 = Directory.GetFiles(path[0]);
            times[0] = sw.ElapsedMilliseconds;
            var list1 = new DirectoryInfo(path[1]).GetFileSystemInfos();
            times[1] = sw.ElapsedMilliseconds;
            var list2 = Directory.EnumerateFileSystemEntries(path[2]);
            times[2] = sw.ElapsedMilliseconds;
            sw.Stop();
            Console.WriteLine($"0: {0}");
            Console.WriteLine($"1: {times[0]}");
            Console.WriteLine($"2: {times[1]}");
            Console.WriteLine($"3: {times[2]}");
            Console.ReadKey();
        }
    }
}
