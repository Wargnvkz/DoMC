using DoMCLib.Classes.Model;
using DoMCLib.Classes.Model.Configuration;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DoMCLib.Configuration
{
    public class ReadingSocketsSettings
    {
        public Dictionary<int, CCDSocketReadParameters> CCDSocketReadParameters;
        public Dictionary<int, ImageProcessParameters> ImageProcessParameters;

    }
}
