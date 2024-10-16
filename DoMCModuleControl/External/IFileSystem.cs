using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.External
{
    public interface IFileSystem
    {
        StreamWriter GetStreamWriter(string path, bool append);
        StreamReader GetStreamReader(string path);
        DirectoryInfo CreateDirectory(string path);
        string[] GetFiles(string path);
        void DeleteFile(string path);
        void DeleteDirectory(string path, bool recursive);
        bool IsFileExists(string path);
        bool IsDirectoryExists(string path);
        string? GetDirectoryName(string path);
        string PathCombine(params string[] parts);
    }
}
