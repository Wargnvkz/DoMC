using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DoMCLib.Tools
{
    public class FileAndDirectoryTools
    {
        public static void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
        }
        public static void OpenNotepad(string filename)
        {
            if (File.Exists(filename))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = filename,
                    FileName = "notepad.exe"
                };

                Process.Start(startInfo);
            }
        }
    }
}
