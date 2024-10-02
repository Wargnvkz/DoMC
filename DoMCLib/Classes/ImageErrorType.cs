using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using DoMCLib.Classes.Module.LCB;

namespace DoMCLib.Classes
{
    public enum ImageErrorType
    {
        None = 0,
        Average = 1,
        Defect = 2
    }
}
