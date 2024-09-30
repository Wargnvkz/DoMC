using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.CCD
{
    public class SocketReadData
    {
        public int ImageDataRead;
        public long ImageTicksRead;
        public byte[] ImageData;
        private short[,] _Image;
        public short[,]? Image
        {
            get
            {
                if (ImageData == null) return null;
                if (_Image == null)
                    _Image = ImageTools.ArrayToImage(ImageData);
                return _Image;
            }
        }
    }
}
