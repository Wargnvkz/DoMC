using DoMCLib.Configuration;
using System.Drawing;
using DoMCLib.Classes.Module.RDPB.Classes;

namespace DoMCLib.Classes.Configuration.CCD
{
    public class CycleImagesCCD
    {
        public DateTime CycleCCDDateTime;
        public DateTime TimeLCBSyncSignalGot;
        public short[][,] WorkModeImages;
        public short[][,] Differences;
        public short[][,] StandardImage;
        public bool[] IsSocketGood; //Хорошая ли преформа
        public bool[] IsSocketHasImage; //Есть ли изображение
        public bool[] SocketsToCheck; //Нужно ли проверять гнездо
        public bool[] LEDStatuses; // горит ли светодиод
        public bool LEDStatusesAdded = false; // прочитаны ли статусы светодиодов
        public bool[] SocketsToSave; // Гнезда, которые надо сохранить. Берется из конфигурации
        private static int DefaultLEDQnt = 12;
        public short MaxDeviation;
        public Point MaxDeviationPoint;
        public short Average;
        public ImageProcessParameters[] ImageProcessParameters;
        public ImageErrorType SocketErrorType;
        public RDPBTransporterSide TransporterSide;

        public void SetLEDStatuses(bool[] LEDStatuses, DateTime TimeLCBSincSignal)
        {
            if (LEDStatuses == null)
            {
                this.LEDStatuses = new bool[DefaultLEDQnt];
                return;
            }
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
            ImageProcessParameters = parameters;
        }
    }
}
