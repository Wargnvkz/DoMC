using DoMCLib.Classes;
using DoMCLib.Classes.Configuration;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DoMCLib.Configuration
{

    public class CurrentSettings
    {
        public Dictionary<int, CCDSocketReadParameters> CCDSocketReadParameters;
        public Dictionary<int, ImageProcessParameters> ImageProcessParameters;

    }
}
