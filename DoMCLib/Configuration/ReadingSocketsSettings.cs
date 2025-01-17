using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DoMCLib.Configuration
{
    /// <summary>
    /// Базовые параметры преформ, параметры их чтения и обработки. Эти параметры, которые не меняются при работе программы для данной преформы
    /// </summary>
    public class ReadingSocketsSettings
    {
        public SocketParameters[] CCDSocketParameters;
        public LCBSettings LCBSettings;
        public ReadingSocketsSettings(int SocketQuantity)
        {
            CCDSocketParameters = new SocketParameters[SocketQuantity];
            for (int i = 0; i < SocketQuantity; i++)
            {
                CCDSocketParameters[i] = new SocketParameters();
            }
            LCBSettings = new LCBSettings();
        }

        public bool IsLCBSettingsSet()
        {
            return LCBSettings.LEDCurrent != 0 && LCBSettings.LCBKoefficient != 0 && LCBSettings.PreformLength != 0;
        }
        public bool IsReadingParametersSet()
        {
            bool result = true;
            for (int i = 0; i < CCDSocketParameters.Length; i++)
            {
                result &= CCDSocketParameters[i] != null &&
                CCDSocketParameters[i].ReadingParameters != null &&
                CCDSocketParameters[i].ReadingParameters.FrameDuration != 0 &&
                CCDSocketParameters[i].ReadingParameters.Exposition != 0 &&
                CCDSocketParameters[i].ImageProcess != null;
            }
            return result;
        }

    }
}
