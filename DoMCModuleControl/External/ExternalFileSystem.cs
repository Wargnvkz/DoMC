using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.External
{
    public class ExternalFileSystem : IFileSystem
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            return new DirectoryInfo(path);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
        }

        public void DeleteFile(string path)
        {
        }

        public string? GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string[] GetFiles(string path)
        {
            return new string[0];
        }

        public StreamReader GetStreamReader(string path)
        {
            return new StreamReader(new MemoryStream());
        }

        public StreamWriter GetStreamWriter(string path, bool append)
        {
            return new StreamWriter(new MemoryStream());
        }

        public bool IsDirectoryExists(string path)
        {
            return false;
        }

        public bool IsFileExists(string path)
        {
            return false;
        }

        public string PathCombine(params string[] parts)
        {
            return Path.Combine(parts);
        }
    }
}
