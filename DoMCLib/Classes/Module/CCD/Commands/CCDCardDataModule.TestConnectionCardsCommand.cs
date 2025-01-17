using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Module.CCD.Commands.Classes;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class TestConnectionCardsCommand : WaitingCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            CCDCardDataModule module = null;
            public TestConnectionCardsCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(DoMCApplicationContext), null) { }
            protected override void Executing()
            {
                module = (CCDCardDataModule)Module;
                var context = (DoMCApplicationContext)InputData;
                if (context != null)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        result.SetCardRequested(i);
                        try
                        {
                            module.tcpClients[i].Start();
                        }
                        catch { }
                    }
                }
                else
                {

                }

            }
            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains(".Module.Start"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber - 1);
                    if (module != null) module.tcpClients[CardAnswerResults.CardNumber - 1].Stop();
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return result.CardsNotAnswered().Count() == 0;
            }

            protected override void PrepareOutputData()
            {
                OutputData = result;

            }
        }

    }


}
