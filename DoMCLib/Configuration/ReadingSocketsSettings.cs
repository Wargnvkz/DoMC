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
        public ReadingSocketsSettings()
        {
            CCDSocketParameters = new SocketParameters[96];
            LCBSettings = new LCBSettings();
        }

    }
}
