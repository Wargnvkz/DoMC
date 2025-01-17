using DoMCModuleControl.External;

namespace DoMCTestingTools.ClassesForTests
{
    public class ErrorFileSystem : IFileSystem
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public string? GetDirectoryName(string path)
        {
            throw new NotImplementedException();
        }

        public string[] GetFiles(string path)
        {
            throw new NotImplementedException();
        }

        public StreamReader GetStreamReader(string path)
        {
            throw new NotImplementedException();
        }

        public StreamWriter GetStreamWriter(string path, bool append)
        {
            throw new NotImplementedException();
        }

        public bool IsDirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public bool IsFileExists(string path)
        {
            throw new NotImplementedException();
        }

        public string PathCombine(params string[] parts)
        {
            throw new NotImplementedException();
        }
    }
}