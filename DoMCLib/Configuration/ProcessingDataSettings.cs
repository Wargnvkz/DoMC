using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Classes.Module.CCD;
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
    /// Текущие значения, которые используются для обработки входящих данные - изображения эталонных преформ
    /// </summary>
    public class ProcessingDataSettings
    {
        public SocketStandardsImage[] CCDSocketStandardsImage;
        public ProcessingDataSettings(int socketQuantity)
        {
            CCDSocketStandardsImage = new SocketStandardsImage[socketQuantity];
            for (int i = 0; i < socketQuantity; i++)
            {
                CCDSocketStandardsImage[i] = new SocketStandardsImage();
            }
        }

    }

}
