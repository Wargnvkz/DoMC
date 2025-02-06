using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCLib.Configuration;
using Microsoft.Extensions.Configuration;

namespace DoMCLib.Classes.Configuration.CCD
{
    public class SocketParameters
    {
        public SocketReadingParameters ReadingParameters;
        public ImageProcessParameters ImageCheckingParameters;
        public SocketParameters Clone()
        {
            var cfg = new SocketParameters();
            cfg.ImageCheckingParameters = ImageCheckingParameters.Clone();
            cfg.ReadingParameters = ReadingParameters.Clone();
            return cfg;
        }
    }
}
