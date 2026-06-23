using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DoMC.Classes
{
    public class MemoryInfo
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MemoryStatusEx lpBuffer);

        public static ulong GetAvailableRamInBytes()
        {
            MemoryStatusEx status = new MemoryStatusEx();
            if (GlobalMemoryStatusEx(status))
            {
                // Возвращаем именно доступную физическую RAM
                return status.ullAvailPhys;
            }
            return 0;
        }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MemoryStatusEx
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;     // Всего физической памяти (RAM)
        public ulong ullAvailPhys;     // Свободно физической памяти (RAM)
        public ulong ullTotalPageFile; // Виртуальная память (с файлом подкачки)
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public MemoryStatusEx()
        {
            this.dwLength = (uint)Marshal.SizeOf(typeof(MemoryStatusEx));
        }
    }
}
