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
                result &= IsReadingParametersSet(i);
            }
            return result;
        }
        public bool IsReadingParametersSet(int socket)
        {
            return CCDSocketParameters[socket] != null &&
            CCDSocketParameters[socket].ReadingParameters != null &&
            CCDSocketParameters[socket].ReadingParameters.IsSocketReadingParametersSet();
        }
        public bool IsImageProcessParametersMakeDecisionSet(int socket)
        {
            return CCDSocketParameters[socket].ImageCheckingParameters != null
                && CCDSocketParameters[socket].ImageCheckingParameters.IsImageProcessParametersMakeDecisionSet();
        }
        public bool IsImageProcessParametersWindowSet(int socket)
        {
            return CCDSocketParameters[socket].ImageCheckingParameters != null
                && CCDSocketParameters[socket].ImageCheckingParameters.IsImageProcessParametersWindowSet();
        }

    }
}
