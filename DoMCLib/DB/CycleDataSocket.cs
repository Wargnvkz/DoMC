using DoMCLib.Configuration;

namespace DoMCLib.DB
{
    public class CycleDataSocket
    {
        public int CycleID;
        public bool IsSocketActive;
        public int SocketNumber;
        public short[,] SocketImage;
        public short[,] SocketStandardImage;
        public byte[] SocketImageCompressed;
        public byte[] SocketStandardImageCompressed;

        /*public int DeviationWindow;
        public short MaxDeviation;
        public short MaxAverage;
        public int TopBorder;
        public int BottomBorder;
        public int LeftBorder;
        public int RightBorder;*/

        public ImageProcessParameters ImageProcessParameters;
    }

}
