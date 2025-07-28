using DoMCLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Tools;
using DoMCModuleControl.Commands;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using System.Threading;
using DoMCLib.Classes.Module.LCB;
using System.ComponentModel;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    /// <summary>
    /// Управление получением данных из платы и передача данных в плату
    /// </summary>
    [Description("Команды плат ПЗС")]
    public partial class CCDCardDataModule : AbstractModuleBase
    {
        CCDCardTCPClient[] tcpClients;
        public CCDCardDataModule(IMainController MainController) : base(MainController)
        {
            tcpClients = new CCDCardTCPClient[12];
            for (int i = 0; i < tcpClients.Length; i++)
            {
                tcpClients[i] = new CCDCardTCPClient(i + 1, typeof(TcpSocketDevice), MainController);
            }
        }

        public CCDCardTCPClient this[int cardNumber]
        {
            get { return tcpClients[cardNumber]; }
        }
    }

}
