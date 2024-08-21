using DoMCLib.Classes;
using DoMCLib.Classes.Configuration;
using DoMCLib.Classes.LCB;
using DoMCLib.ProcessState;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Configuration
{

    public class HardwareSettings
    {
        // 
        //public List<DoMCCardListSetting> CardListSettings;
         
        public int SocketQuantity = 96;

        public bool[] SocketsToCheck;
         
        public DoMCGeneralSettings WorkModeSettings;

         
        public LCBSettings LCBSettings = new LCBSettings();

         
        public int LCBPort;
         
        public int DoMCPort;

         
        public bool LogPackets;

         
        public RemoveDefectedPreformBlockConfig RemoveDefectedPreformBlockConfig;

         
        public string LocalDataStorageConnectionString;

         
        public string RemoteDataStorageConnectionString;

         
        public TimeoutOfActions Timeouts = new TimeoutOfActions();

         
        public bool[] SocketsToSave;

         
        public DisplaySockets2PhysicalSockets DisplaySockets2PhysicalSockets;

         
        public short AverageToHaveImage = 200;

         
        public bool RegisterEmptyImages = false;
         
        public bool LoggingDBRequests = false;

        public void Save()
        {
            Save(Path.Combine(Application.StartupPath, ApplicationCardParameters.DOMCConfigFilename));
        }
        public void Load()
        {
            Load(Path.Combine(Application.StartupPath, ApplicationCardParameters.DOMCConfigFilename));
        }
        public void Save(string Filename)
        {
            var backupfile = Path.ChangeExtension(Filename,".bak");
            try
            {
                try
                {
                    File.Delete(backupfile);
                    File.Move(Filename, backupfile);
                }
                catch { }
                var jsonCfg = JSONConverter.ToJSON(this);
                using (var sw = new StreamWriter(Filename))
                {
                    sw.Write(jsonCfg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении конфигурации");
            }
        }
        public void Load(string Filename)
        {
            string jsonCfg;
            try
            {
                using (var sr = new StreamReader(Filename))
                {
                    jsonCfg = sr.ReadToEnd();
                }
                var cfg = JSONConverter.FromJSON<HardwareSettings>(jsonCfg);
                //this.CardListSettings = cfg.CardListSettings;
                this.SocketQuantity = cfg.SocketQuantity;
                this.SocketToCardSocketConfigurations = cfg.SocketToCardSocketConfigurations;

                if (cfg.WorkModeSettings != null)
                    this.WorkModeSettings = cfg.WorkModeSettings;
                else
                    this.WorkModeSettings = new DoMCGeneralSettings();

                LogPackets = cfg.LogPackets;

                if (cfg.LCBSettings != null)
                    LCBSettings = cfg.LCBSettings;
                else
                    LCBSettings = new LCBSettings();

                if (cfg.RemoveDefectedPreformBlockConfig != null)
                    RemoveDefectedPreformBlockConfig = cfg.RemoveDefectedPreformBlockConfig;
                else
                    RemoveDefectedPreformBlockConfig = new RemoveDefectedPreformBlockConfig();

                if (cfg.SocketsToCheck == null)
                    SocketsToCheck = new bool[SocketQuantity];
                else
                    SocketsToCheck = cfg.SocketsToCheck;

                LocalDataStorageConnectionString = cfg.LocalDataStorageConnectionString;
                RemoteDataStorageConnectionString = cfg.RemoteDataStorageConnectionString;
                /*if (cfg.ImageProcessParameters == null)
                    ImageProcessParameters = new ImageProcessParameters();
                else
                    ImageProcessParameters = cfg.ImageProcessParameters;*/
                if (cfg.SocketsToSave == null)
                    SocketsToSave = new bool[SocketQuantity];
                else
                    SocketsToSave = cfg.SocketsToSave;

                if (cfg.Timeouts == null)
                    Timeouts = new TimeoutOfActions();
                else
                    Timeouts = cfg.Timeouts;

                if (cfg.DisplaySockets2PhysicalSockets == null)
                {
                    DisplaySockets2PhysicalSockets = new DisplaySockets2PhysicalSockets();
                    DisplaySockets2PhysicalSockets.SetDefaultMatrix(SocketQuantity);
                }
                else
                {
                    DisplaySockets2PhysicalSockets = cfg.DisplaySockets2PhysicalSockets;
                }

            }
            catch (Exception ex)
            {
                this.SocketQuantity = 1;
                this.SocketToCardSocketConfigurations = new Dictionary<int, CCDSocketReadParameters>();
                this.WorkModeSettings = new DoMCGeneralSettings();
                this.SocketsToCheck = new bool[SocketQuantity];
                RemoveDefectedPreformBlockConfig = new RemoveDefectedPreformBlockConfig();
                LogPackets = false;
                LCBSettings = new LCBSettings();
            }
        }

        public void SaveStandard(string filename)
        {
            try
            {
                var std = new StandardConfiguration();
                //std.CardListSettings = CardListSettings;
                std.SocketQuantity = SocketQuantity;
                std.SocketToCardSocketConfigurations = SocketToCardSocketConfigurations;
                std.LCBSettings = LCBSettings;
                //std.ImageProcessParameters = ImageProcessParameters;
                var jsonCfg = JSONConverter.ToJSON(std);
                using (var sw = new StreamWriter(filename))
                {
                    sw.Write(jsonCfg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении конфигурации");
            }
        }
        public void LoadStandard(string filename)
        {
            string jsonCfg;
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    jsonCfg = sr.ReadToEnd();
                }
                var cfg = JSONConverter.FromJSON<StandardConfiguration>(jsonCfg);
                //this.CardListSettings = cfg.CardListSettings;
                this.SocketQuantity = cfg.SocketQuantity;
                this.SocketToCardSocketConfigurations = cfg.SocketToCardSocketConfigurations;
                if (cfg.LCBSettings != null)
                    LCBSettings = cfg.LCBSettings;
                else
                    LCBSettings = new LCBSettings();
                /*if (cfg.ImageProcessParameters == null)
                    this.ImageProcessParameters = new ImageProcessParameters();
                else
                    this.ImageProcessParameters = cfg.ImageProcessParameters;*/
            }
            catch (Exception ex)
            {

            }
        }
    }

     
    public class StandardConfiguration
    {
        // 
        //public List<DoMC.Configuration.DoMCCardListSetting> CardListSettings;
         
        public int SocketQuantity = 96;
         
        public Dictionary<int, DoMCLib.Configuration.CCDSocketReadParameters> SocketToCardSocketConfigurations;
         
        public LCBSettings LCBSettings;
         
        public ImageProcessParameters ImageProcessParameters = new ImageProcessParameters();

        public void Save(string filename)
        {
            try
            {
                var jsonCfg = JSONConverter.ToJSON(this);
                using (var sw = new StreamWriter(filename))
                {
                    sw.Write(jsonCfg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении конфигурации");
            }
        }
        public void Load(string filename)
        {
            string jsonCfg;
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    jsonCfg = sr.ReadToEnd();
                }
                var cfg = JSONConverter.FromJSON<StandardConfiguration>(jsonCfg);
                //this.CardListSettings = cfg.CardListSettings;
                this.SocketQuantity = cfg.SocketQuantity;
                this.SocketToCardSocketConfigurations = cfg.SocketToCardSocketConfigurations;
                if (cfg.LCBSettings != null)
                    LCBSettings = cfg.LCBSettings;
                else
                    LCBSettings = new LCBSettings();

                ImageProcessParameters = cfg.ImageProcessParameters;

            }
            catch (Exception ex)
            {

            }
        }
    }

     
    public class SocketDoMCConfiguration
    {
         
        public int SocketQuantity = 96;

         
        public bool[] SocketStatus;

         
        public bool[] LEDOn;

         
        public int[] SocketToRead;

        public void Save()
        {
            try
            {
                var jsonCfg = JSONConverter.ToJSON(this);
                using (var sw = new StreamWriter(Path.Combine(Application.StartupPath, ApplicationCardParameters.SocketsConfigFilename)))
                {
                    sw.Write(jsonCfg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении конфигурации");
            }
        }
        public void Load()
        {
            string jsonCfg;
            try
            {
                using (var sr = new StreamReader(Path.Combine(Application.StartupPath, ApplicationCardParameters.SocketsConfigFilename)))
                {
                    jsonCfg = sr.ReadToEnd();
                }
                var cfg = JSONConverter.FromJSON<SocketDoMCConfiguration>(jsonCfg);
                this.SocketQuantity = cfg.SocketQuantity;
                this.SocketStatus = cfg.SocketStatus;
                this.LEDOn = cfg.LEDOn;
                this.SocketToRead = cfg.SocketToRead;
            }
            catch (Exception)
            {

            }
        }
    }

     

}
