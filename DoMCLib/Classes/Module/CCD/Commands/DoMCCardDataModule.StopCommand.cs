using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class StopCommand : CommandBase
        {
            public StopCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var context = (ApplicationContext)InputData;
                if (context != null)
                {
                    var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
                    var cardParameters = context.GetCardParametersByCardList(workingCards);
                    for (int i = 0; i < cardParameters.Count; i++)
                    {
                        module.tcpClients[cardParameters[i].Item1].Stop();
                    }
                }
                else
                {

                }
            }
        }

    }


}
