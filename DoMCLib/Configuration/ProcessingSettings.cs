using DoMCLib.Classes.Module.CCD;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DoMCLib.Configuration
{

    public class ProcessingSettings
    {
        public Dictionary<int, CCDSocketStandardsImage> CCDSocketStandardsImage;

    }

}
