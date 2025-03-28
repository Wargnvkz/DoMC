using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DoMCLib.Classes.Configuration.CCD
{

    public class SocketStandardsImage
    {
        [JsonIgnore]
        public short[,] StandardImage;

        public string ImageText
        {
            get
            {
                if (StandardImage == null) return "";
                return Tools.ImageTools.ToBase64(/*Tools.ImageTools.Compress*/(Tools.ImageTools.ImageToArray(StandardImage)));
            }
            set
            {
                if (string.IsNullOrEmpty(value)) StandardImage = null;
                else
                    StandardImage = Tools.ImageTools.ArrayToImage(/*Tools.ImageTools.Decompress*/(Tools.ImageTools.FromBase64(value)));
            }
        }
    }
}
