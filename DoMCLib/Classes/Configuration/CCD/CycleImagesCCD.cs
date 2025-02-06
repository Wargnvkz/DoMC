using DoMCLib.Configuration;
using System.Drawing;
using DoMCLib.Classes.Module.RDPB.Classes;

namespace DoMCLib.Classes.Configuration.CCD
{
    public class CycleImagesCCD
    {
        public DateTime CycleCCDDateTime;
        public DateTime TimeLCBSyncSignalGot;
        public short[][,] CurrentImages;
        public short[][,] Differences;
        public short[][,] StandardImages;
        public bool[] IsSocketGood; //Хорошая ли преформа
        public bool[] IsSocketHasImage; //Есть ли изображение
        public bool[] SocketsToCheck; //Нужно ли проверять гнездо
        public bool[] LEDStatuses; // горит ли светодиод
        public bool LEDStatusesAdded = false; // прочитаны ли статусы светодиодов
        public static readonly int DefaultLEDQnt = 12;
        public short MaxDeviation;
        public Point MaxDeviationPoint;
        public short Average;
        public ImageProcessParameters[] ImageProcessParameters;
        public ImageErrorType SocketErrorType;
        public RDPBTransporterSide TransporterSide;

        public void SetLEDStatuses(bool[] LEDStatuses, DateTime TimeLCBSincSignal)
        {
            if (LEDStatuses == null) throw new ArgumentNullException(nameof(LEDStatuses));
            if (TimeLCBSincSignal == DateTime.MinValue) throw new ArgumentOutOfRangeException(nameof(TimeLCBSincSignal));
            this.LEDStatuses = new bool[LEDStatuses.Length];
            for (int i = 0; i < LEDStatuses.Length; i++)
            {
                this.LEDStatuses[i] = LEDStatuses[i];
            }
            TimeLCBSyncSignalGot = TimeLCBSincSignal;
            LEDStatusesAdded = true;
        }

        public void SetImageProcessParameters(ImageProcessParameters[] parameters)
        {
            ImageProcessParameters = parameters.Select(p => p.Clone()).ToArray();
        }
    }
}
