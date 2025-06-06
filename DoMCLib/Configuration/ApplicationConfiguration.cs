#pragma warning disable IDE0063
#pragma warning disable IDE0090
#pragma warning disable IDE0290
using DoMCLib.Tools;
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
        private static string HardwareSettingsFile = "HardwareSettings.json";
        private static string ReadingSocketsSettingsFile = "ReadingSocketsSettings.json";
        private static string ProcessingDataSettingsFile = "ProcessingDataSettings.json";
        private static string MetaDataFile = "metadata.json";
        public HardwareSettings HardwareSettings { get; set; } = new HardwareSettings();
        public ReadingSocketsSettings ReadingSocketsSettings { get; set; } = new ReadingSocketsSettings(96);
        public ProcessingDataSettings ProcessingDataSettings { get; set; } = new ProcessingDataSettings(96);

        private readonly string ConfigurationFilePath;
        private const int CurrentFileVersion = 2;

        public ApplicationConfiguration(string configurationFilePath)
        {
            // Инициализация с значениями по умолчанию или загрузка из файла
            ConfigurationFilePath = configurationFilePath;

        }

        public void LoadConfiguration(string filename = null)
        {
            if (String.IsNullOrEmpty(filename)) filename = ConfigurationFilePath;
            if (File.Exists(ConfigurationFilePath))
            {
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(filename))
                    {
                        var metadata = ApplicationConfiguration.ReadZipEntry<dynamic>(archive, MetaDataFile);
                        int fileVersion = metadata?.FileVersion != null ? int.Parse((string)metadata.FileVersion) : 1;

                        if (fileVersion != CurrentFileVersion)
                        {
                            MigrateData(fileVersion, CurrentFileVersion);
                        }

                        HardwareSettings = ReadZipEntry<HardwareSettings>(archive, HardwareSettingsFile) ?? new HardwareSettings();
                        ReadingSocketsSettings = ReadZipEntry<ReadingSocketsSettings>(archive, ReadingSocketsSettingsFile) ?? new ReadingSocketsSettings(96);
                        ProcessingDataSettings = ReadZipEntry<ProcessingDataSettings>(archive, ProcessingDataSettingsFile) ?? new ProcessingDataSettings(96);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    HardwareSettings = new HardwareSettings();
                    ReadingSocketsSettings = new ReadingSocketsSettings(96);
                    ProcessingDataSettings = new ProcessingDataSettings(96);
                    SaveAllConfiguration(filename);
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

        }

        private void UpdateZipEntry<T>(string filename, string entryName, T data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var json = JsonConvert.SerializeObject(data);
                using (StreamWriter writer = new StreamWriter(memoryStream))
                {
                    writer.Write(json);
                    writer.Flush();
                    memoryStream.Position = 0;


                    using (FileStream zipToOpen = new FileStream(filename, FileMode.OpenOrCreate))
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

        public void SaveReadingSocketsSettings(string filename = null)
        {
            if (String.IsNullOrEmpty(filename)) filename = ConfigurationFilePath;

            UpdateZipEntry(filename, ReadingSocketsSettingsFile, ReadingSocketsSettings);
        }

        public void SaveProcessingDataSettings(string filename = null)
        {
            if (String.IsNullOrEmpty(filename)) filename = ConfigurationFilePath;

            UpdateZipEntry(filename, ProcessingDataSettingsFile, ProcessingDataSettings);
        }
        public void SaveHardwareSettings(string filename = null)
        {
            if (String.IsNullOrEmpty(filename)) filename = ConfigurationFilePath;

            UpdateZipEntry(filename, HardwareSettingsFile, HardwareSettings);
        }

        public void SaveAllConfiguration(string filename = null)
        {
            if (String.IsNullOrEmpty(filename)) filename = ConfigurationFilePath;

            SaveHardwareSettings(filename);
            SaveReadingSocketsSettings(filename);
            SaveProcessingDataSettings(filename);
            UpdateZipEntry(filename, "metadata.json", new { FileVersion = CurrentFileVersion.ToString(), LastUpdate = DateTime.UtcNow });
        }

        public void SaveStandardSettings(string filename)
        {
            SaveReadingSocketsSettings(filename);
            SaveProcessingDataSettings(filename);
        }

        public void LoadStandardSettings(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(filename))
                    {
                        ReadingSocketsSettings = ReadZipEntry<ReadingSocketsSettings>(archive, ReadingSocketsSettingsFile) ?? new ReadingSocketsSettings(96);
                        ProcessingDataSettings = ReadZipEntry<ProcessingDataSettings>(archive, ProcessingDataSettingsFile) ?? new ProcessingDataSettings(96);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    ReadingSocketsSettings = new ReadingSocketsSettings(96);
                    ProcessingDataSettings = new ProcessingDataSettings(96);
                    SaveAllConfiguration();
                }
            }
        }


        public void SetCheckCard(int CardNumber, bool IsCardChecking)
        {
            for (var socket = 0; socket < 8; socket++)
            {
                var tcpcardsocket = new TCPCardSocket(CardNumber, socket);
                HardwareSettings.SocketsToCheck[HardwareSettings.CardSocket2EquipmentSocket[tcpcardsocket.InnerSocketNumber] - 1] = IsCardChecking;
            }
        }

        /*private void WriteCurrentConfigEntry<TContext>(string filename, TContext data)
        {
            var json = JsonConvert.SerializeObject(data);

            using(var sw=new StreamWriter(filename))
            {
                sw.Write(json);
            }
        }
        private TContext ReadCurrentConfigEntry<TContext>(string filename)
        {
            string json;
            using (var sr = new StreamReader(filename))
            {
                json = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<TContext>(json, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include
            });
        }

        public void WriteCurrentConfig()
        {
            WriteCurrentConfigEntry(HardwareSettingsFile,HardwareSettings);
            WriteCurrentConfigEntry(ReadingSocketsSettingsFile, ReadingSocketsSettings);
            WriteCurrentConfigEntry(ProcessingDataSettingsFile,ProcessingDataSettings);
        }

        public void LoadCurrentConfig()
        {

            try
            {
                HardwareSettings=ReadCurrentConfigEntry<HardwareSettings>(HardwareSettingsFile);
                ReadingSocketsSettings= ReadCurrentConfigEntry<ReadingSocketsSettings>(ReadingSocketsSettingsFile);
                ProcessingDataSettings= ReadCurrentConfigEntry<ProcessingDataSettings>(ProcessingDataSettingsFile);
                
            }
            catch (FileNotFoundException ex)
            {
                HardwareSettings = new HardwareSettings();
                ReadingSocketsSettings = new ReadingSocketsSettings(96);
                ProcessingDataSettings = new ProcessingDataSettings(96);
                WriteCurrentConfig();
            }
        }
        public void SaveCurrentStandard() 
        {
            WriteCurrentConfigEntry(ProcessingDataSettingsFile, ProcessingDataSettings);
        }*/
    }
}
