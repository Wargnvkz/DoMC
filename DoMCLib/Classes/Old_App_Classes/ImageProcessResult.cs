using System.Drawing;

namespace DoMCLib.Classes
{
    public class ImageProcessResult
    {
        public ImageErrorType SocketErrorType;
        public short Average;
        public short[,] ResultImage;
        public Point MaxDeviationPoint;
        public short MaxDeviation;
        public bool IsSocketGood;
        public string ErrorToString()
        {
            List<string> defects = new List<string>();
            if (SocketErrorType.HasFlag(ImageErrorType.Average)) defects.Add("Цвет");
            if (SocketErrorType.HasFlag(ImageErrorType.Defect)) defects.Add("Дефект");
            return String.Join(", ", defects);
        }
    }
}
