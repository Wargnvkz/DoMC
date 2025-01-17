using DoMC.Classes;
using DoMCLib.Classes.Configuration.CCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.SimpleCommands
{
    public class GetAllStandards : ISimpleCommand<DoMCLib.Classes.DoMCApplicationContext, SocketStandardsImage[]>
    {
        public SocketStandardsImage[] Execute(DoMCApplicationContext data)
        {
            return data.Configuration.ProcessingDataSettings.CCDSocketStandardsImage;
        }
    }
    public class GetEquipmentSocketStandard : ISimpleCommand<(DoMCLib.Classes.DoMCApplicationContext, int EquipmentSocketNumber), SocketStandardsImage>
    {
        public SocketStandardsImage Execute((DoMCApplicationContext, int) data)
        {
            var cardsocketNumber = data.Item1.EquipmentSocket2CardSocket[data.Item2 - 1];
            return data.Item1.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[cardsocketNumber];
        }
    }
    public class GetCardSocketStandard : ISimpleCommand<(DoMCLib.Classes.DoMCApplicationContext, int EquipmentSocketNumber), SocketStandardsImage>
    {
        public SocketStandardsImage Execute((DoMCApplicationContext, int) data)
        {
            return data.Item1.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[data.Item2];
        }
    }
}
