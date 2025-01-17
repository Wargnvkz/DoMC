using DoMCModuleControl.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DoMCTestingTools.ClassesForTests
{
    public class FileSystemForTests : IFileSystem
    {
        Dictionary<string, Stream> Files = new Dictionary<string, Stream>();
        HashSet<string> Directories = new HashSet<string>();
        public FileSystemForTests()
        {
            Directories.Add(Path.GetTempPath());
            Directories.Add(Directory.GetCurrentDirectory());
        }
        public DirectoryInfo CreateDirectory(string path)
        {
            Directories.Add(path);
            return new DirectoryInfo(path);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            Directories.Remove(path);
        }

        public void DeleteFile(string path)
        {
            Files.Remove(path);
        }

        public string? GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string[] GetFiles(string path)
        {
            if (path == null) return Files.Keys.ToArray();
            return Files.Where(kv => GetDirectoryName(kv.Key) == path).Select(kv => kv.Key).ToArray();
        }

        public StreamReader GetStreamReader(string path)
        {
            if (Files.ContainsKey(path))
            {
                var ts = Files[path];
                if (ts == null) Files[path] = ts = new TestStream();
                ts.Seek(0, SeekOrigin.Begin);
                return new StreamReader(ts);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public StreamWriter GetStreamWriter(string path, bool append)
        {
            Stream? ts = null;
            if (!Files.ContainsKey(path))
                Files.Add(path, ts = new TestStream());

            if (ts == null)
            {
                if (append)
                {
                    ts = Files[path];
                    ts.Seek(0, SeekOrigin.End);
                }
                else
                    Files[path] = ts = new TestStream();
            }
            return new StreamWriter(ts);
        }

        public bool IsDirectoryExists(string path)
        {
            if (Directories.Select(dir => dir.StartsWith(path)).Count() > 0) return true;
            if (Files.Select(kv => kv.Key.StartsWith(path)).Count() > 0) return true;
            return false;
        }

        public bool IsFileExists(string path)
        {
            return Files.ContainsKey(path);
        }

        public string PathCombine(params string[] parts)
        {
            return Path.Combine(parts);
        }
    }
}
