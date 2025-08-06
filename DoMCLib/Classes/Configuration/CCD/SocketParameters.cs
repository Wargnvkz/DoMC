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
            cfg.ImageCheckingParameters = ImageCheckingParameters?.Clone() ?? null;
            cfg.ReadingParameters = ReadingParameters?.Clone() ?? null;
            return cfg;
        }
        public void FillTarget(ref SocketParameters target, CopySocketParameters copySocketParameters)
        {
            if (target == null) target = new SocketParameters();
            if (copySocketParameters.CopySocketReadingParameters.HasAnySelection())
            {
                if (target.ReadingParameters == null) target.ReadingParameters = new SocketReadingParameters();
                ReadingParameters.FillTarget(ref target.ReadingParameters, copySocketParameters.CopySocketReadingParameters);
            }
            if (copySocketParameters.CopyImageProcessParameters.HasAnySelection())
            {
                if (target.ImageCheckingParameters == null) target.ImageCheckingParameters = new ImageProcessParameters();
                ImageCheckingParameters.FillTarget(ref target.ImageCheckingParameters, copySocketParameters.CopyImageProcessParameters);
            }
        }

        public class CopySocketParameters
        {
            public SocketReadingParameters.CopySocketReadingParameters CopySocketReadingParameters = new SocketReadingParameters.CopySocketReadingParameters();
            public ImageProcessParameters.CopyImageProcessParameters CopyImageProcessParameters = new ImageProcessParameters.CopyImageProcessParameters();
        }
    }
    public static class SocketParametersArrayExtension
    {
        public static SocketParameters[] CloneParameters(this SocketParameters[] array)
        {
            var res = new SocketParameters[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                res[i] = array[i]?.Clone() ?? null;
            }
            return res;
        }
    }
}
