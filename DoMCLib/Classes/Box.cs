using DoMCLib.Classes.Module.RDPB.Classes;

namespace DoMCLib.Classes
{
    public class Box
    {
        public int BoxID;
        public DateTime CompletedTime;
        public int BadCyclesCount;
        public RDPBTransporterSide TransporterSide;
        public string TransporterSideToString()
        {
            switch (this.TransporterSide)
            {
                case RDPBTransporterSide.Left: return "Левый";
                case RDPBTransporterSide.Right: return "Правый";
                case RDPBTransporterSide.Stoped: return "Стоит";
                case RDPBTransporterSide.SensorError: return "Ошибка";
                default: return "Неизвестно";
            }
        }
    }
}
