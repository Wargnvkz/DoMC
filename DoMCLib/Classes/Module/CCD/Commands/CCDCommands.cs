using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class SendReadSocketCommand : GenericCommandBase<DoMCApplicationContext, CCDCardDataCommandResponse>
        {
            public SendReadSocketCommand(IMainController controller, AbstractModuleBase module)
                : base(controller, module) { }

            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var context = InputData;
                var result = new CCDCardDataCommandResponse();

                var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
                var cardParameters = context.GetCardParametersByCardList(workingCards);
                var tasks = new List<Task>();

                foreach (var (cardNumber, _) in cardParameters)
                {
                    result.SetCardRequested(cardNumber);

                    var task = module.tcpClients[cardNumber]
                        .SendCommandReadSocketAsync(1, CancelationTokenSourceToCancelCommandExecution.Token)
                        .ContinueWith(t =>
                        {
                            if (t.Status == TaskStatus.RanToCompletion && t.Result.ReadingSocketsResult == 0)
                                result.SetCardAnswered(cardNumber);
                        });

                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray(), CancelationTokenSourceToCancelCommandExecution.Token); // блокируем здесь, потому что это sync Executing()
                SetOutput(result);
            }
        }

    }

}

