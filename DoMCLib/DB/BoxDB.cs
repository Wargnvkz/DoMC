using DoMCLib.Classes.Module.RDPB.Classes;

namespace DoMCLib.DB
{
    public class BoxDB
    {
        public int BoxID;
        public DateTime CompletedTime;
        public int BadCyclesCount;
        public string TransporterSide = "";
        public BoxDB() { }

        public BoxDB(DoMCLib.Classes.Box box)
        {
            this.CompletedTime = box.CompletedTime;
            this.BadCyclesCount = box.BadCyclesCount;
            switch (box.TransporterSide)
            {
                case RDPBTransporterSide.Left:
                    this.TransporterSide = "L";
                    break;
                case RDPBTransporterSide.Right:
                    this.TransporterSide = "R";
                    break;
                case RDPBTransporterSide.Stoped:
                    this.TransporterSide = "S";
                    break;
                case RDPBTransporterSide.SensorError:
                    this.TransporterSide = "E";
                    break;
                default:
                    this.TransporterSide = "";
                    break;
            }
        }

        public DoMCLib.Classes.Box Convert()
        {
            var box = new DoMCLib.Classes.Box();
            box.BadCyclesCount = this.BadCyclesCount;
            box.CompletedTime = this.CompletedTime;
            switch (this.TransporterSide)
            {
                case "L":
                    box.TransporterSide = RDPBTransporterSide.Left;
                    break;
                case "R":
                    box.TransporterSide = RDPBTransporterSide.Right;
                    break;
                case "S":
                    box.TransporterSide = RDPBTransporterSide.Stoped;
                    break;
                default:
                    box.TransporterSide = RDPBTransporterSide.SensorError;
                    break;
            }
            return box;
        }

        public string TransporterSideToString()
        {
            switch (this.TransporterSide)
            {
                case "L":
                    return "Левый";
                case "R":
                    return "Правый";
                case "S":
                    return "Стоит";
                case "E":
                    return "Ошибка";
                default:
                    return "Неизвестно";
            }

        }
    }

}
