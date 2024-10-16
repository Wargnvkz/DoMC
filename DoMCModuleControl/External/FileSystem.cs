namespace DoMCModuleControl.External
{
    public class FileSystem : IFileSystem
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public string? GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public StreamReader GetStreamReader(string path)
        {
            return new StreamReader(path);
        }

        public StreamWriter GetStreamWriter(string path, bool append)
        {
            return new StreamWriter(path, append);
        }

        public bool IsDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public bool IsFileExists(string path)
        {
            return File.Exists(path);
        }

        public string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}
