#pragma warning disable IDE0063
#pragma warning disable IDE0090
#pragma warning disable IDE0290
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Configuration
{
    /// <summary>
    /// Данные конфигурации
    /// </summary>
    public class ApplicationConfiguration
    {
        public HardwareSettings HardwareSettings { get; set; }
        public ReadingSocketsSettings CurrentSettings { get; set; }
        public ProcessingSettings ProcessingData { get; set; }

        private readonly string ConfigurationFilePath;
        private const int CurrentFileVersion = 2;

        public ApplicationConfiguration(string configurationFilePath)
        {
            // Инициализация с значениями по умолчанию или загрузка из файла
            ConfigurationFilePath = configurationFilePath;

        }

        public void Load(string configurationFilePath)
        {
            if (File.Exists(ConfigurationFilePath))
            {
                using (ZipArchive archive = ZipFile.OpenRead(ConfigurationFilePath))
                {
                    var metadata = ApplicationConfiguration.ReadZipEntry<dynamic>(archive, "metadata.json");
                    int fileVersion = metadata?.FileVersion != null ? int.Parse((string)metadata.FileVersion) : 1;

                    if (fileVersion != CurrentFileVersion)
                    {
                        MigrateData(fileVersion, CurrentFileVersion);
                    }

                    CurrentSettings = ReadZipEntry<ReadingSocketsSettings>(archive, "settings.json") ?? new ReadingSocketsSettings();
                    ProcessingData = ReadZipEntry<ProcessingSettings>(archive, "data.bin") ?? new ProcessingSettings();
                }
            }
        }

        private void MigrateData(int oldVersion, int newVersion)
        {
            if (oldVersion < 2)
            {/*
                if (Configuration.ReadingSocketsSettings.NewField == null)
                {
                    Configuration.ReadingSocketsSettings.NewField = "DefaultValue";
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

                using (FileStream zipToOpen = new FileStream(ConfigurationFilePath, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        var entry = archive.GetEntry(entryName);
                        entry?.Delete();
                        entry = archive.CreateEntry(entryName);
                        using (Stream entryStream = entry.Open())
                        {
                            memoryStream.CopyTo(entryStream);
                        }
                    }
                }
            }
        }

        private static T? ReadZipEntry<T>(ZipArchive archive, string entryName)
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
            return default;
        }

        public void SaveCurrentSettings()
        {
            UpdateZipEntry("settings.json", CurrentSettings);
        }

        public void SaveProcessingData()
        {
            UpdateZipEntry("data.bin", ProcessingData);
        }

        public void SaveAll()
        {
            SaveCurrentSettings();
            SaveProcessingData();
            UpdateZipEntry("metadata.json", new { FileVersion = CurrentFileVersion.ToString(), LastUpdate = DateTime.UtcNow });
        }
    }
}
