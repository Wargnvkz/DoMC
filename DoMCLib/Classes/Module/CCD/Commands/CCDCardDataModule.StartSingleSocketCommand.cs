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
        public class StartSingleSocketCommand : WaitingCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public StartSingleSocketCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof((DoMCApplicationContext, int socket)), typeof(CCDCardDataCommandResponse)) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var data = ((DoMCApplicationContext context, int EquipmentSocketNumber)?)InputData;
                if (data != null && data.Value.context != null)
                {
                    var cardParameter = data.Value.context.GetWorkingPhysicalSocket(data.Value.EquipmentSocketNumber);
                    result.SetCardRequested(cardParameter.CCDCardNumber);
                    module.tcpClients[cardParameter.CCDCardNumber].Start();
                }
                else
                {
                    throw new ArgumentNullException(nameof(InputData));
                }

            }
            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains(".Module.Start"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber - 1);

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
