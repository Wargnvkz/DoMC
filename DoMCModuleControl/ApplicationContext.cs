using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DoMCModuleControl
{
    /// <summary>
    /// Данные всего приложения и всех модулей - конфигурация, состояния, временные данные
    /// </summary>
    public class ApplicationContext
    {
        public ApplicationConfiguration Configuration { get; private set; }
        public ProcessState State { get; private set; }
        private string filePath;
        private const int CurrentFileVersion = 2;

        public ApplicationContext(string filePath)
        {
            this.filePath = filePath;
            Configuration = new ApplicationConfiguration();
            State = new ProcessState();

            // Загрузка данных из файла
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            if (File.Exists(filePath))
            {
                using (ZipArchive archive = ZipFile.OpenRead(filePath))
                {
                    var metadata = ReadZipEntry<dynamic>(archive, "metadata.json");
                    int fileVersion = metadata?.FileVersion != null ? int.Parse((string)metadata.FileVersion) : 1;

                    if (fileVersion != CurrentFileVersion)
                    {
                        MigrateData(fileVersion, CurrentFileVersion);
                    }

                    Configuration.CurrentSettings = ReadZipEntry<CurrentSettings>(archive, "settings.json");
                    Configuration.ProcessingData = ReadZipEntry<ProcessingData>(archive, "data.bin");
                }
            }
            else
            {
                Configuration = new ApplicationConfiguration();
            }
        }

        private void MigrateData(int oldVersion, int newVersion)
        {
            if (oldVersion < 2)
            {/*
                if (Configuration.CurrentSettings.NewField == null)
                {
                    Configuration.CurrentSettings.NewField = "DefaultValue";
                }*/
            }

            UpdateZipEntry("metadata.json", new { FileVersion = newVersion.ToString(), LastUpdate = DateTime.UtcNow });
        }

        private void UpdateZipEntry<T>(string entryName, T data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var json = JsonConvert.SerializeObject(data);
                using (StreamWriter writer = new StreamWriter(memoryStream))
                {
                    writer.Write(json);
                    writer.Flush();
                    memoryStream.Position = 0;
                }

                using (FileStream zipToOpen = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        var entry = archive.GetEntry(entryName);
                        if (entry != null)
                        {
                            entry.Delete();
                        }
                        entry = archive.CreateEntry(entryName);
                        using (Stream entryStream = entry.Open())
                        {
                            memoryStream.CopyTo(entryStream);
                        }
                    }
                }
            }
        }

        private T ReadZipEntry<T>(ZipArchive archive, string entryName)
        {
            var entry = archive.GetEntry(entryName);
            if (entry != null)
            {
                using (StreamReader reader = new StreamReader(entry.Open()))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include
                    });
                }
            }
            return default(T);
        }

        public void SaveCurrentSettings()
        {
            UpdateZipEntry("settings.json", Configuration.CurrentSettings);
        }

        public void SaveProcessingData()
        {
            UpdateZipEntry("data.bin", Configuration.ProcessingData);
        }

        public void SaveAll()
        {
            SaveCurrentSettings();
            SaveProcessingData();
            UpdateZipEntry("metadata.json", new { FileVersion = CurrentFileVersion.ToString(), LastUpdate = DateTime.UtcNow });
        }
    }

}
