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

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    /// <summary>
    /// Управление получением данных из платы и передача данных в плату
    /// </summary>
    public partial class CCDCardDataModule : ModuleBase
    {
        CCDCardTCPClient[] tcpClients;
        public CCDCardDataModule(IMainController MainController) : base(MainController)
        {
            tcpClients = new CCDCardTCPClient[12];
        }

        /// <summary>
        /// послыка команды плате на передачу данных об изображении и подключение к плате для получения данных изображения гнезд
        /// </summary>
        public class GetSocketsImagesDataCommand : WaitCommandBase
        {
            GetImageDataCommandResponse result = new GetImageDataCommandResponse();
            CancellationTokenSource[] cancellationTokenSources = new CancellationTokenSource[12];
            public GetSocketsImagesDataCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(ApplicationContext), typeof(GetImageDataCommandResponse)) { }
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
                        result.SetCardRequested(i);
                        module.tcpClients[cardParameters[i].Item1].SendCommandGetAllSocketImages();
                        cancellationTokenSources[i] = new CancellationTokenSource();
                        var task = new Task((i) =>
                        {
                            var cardnumber = (int)i;
                            for (int socket = 0; socket < 8 && (!cancellationTokenSources[cardnumber].IsCancellationRequested); socket++)
                            {
                                var ReadResult = module.tcpClients[cardParameters[cardnumber].Item1].GetImageDataFromSocketSync(socket, context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout, cancellationTokenSources[cardnumber], out SocketReadData data);
                                if (ReadResult)
                                {
                                    TCPCardSocket cardSocket = new TCPCardSocket() { CCDCardNumber = cardnumber, InnerSocketNumber = socket };
                                    var equipmentSocket = context.Configuration.HardwareSettings.CardSocket2EquipmentSocket[cardSocket.CardSocketNumber()];
                                    result.SetSocketReadData(equipmentSocket - 1, data);
                                }
                                else
                                {
                                    result.SetCardError(cardnumber);
                                }
                            }
                            result.SetCardCompleteSuccessfully(cardnumber);
                        }, i, cancellationTokenSources[i].Token);
                        task.Start();
                    }
                }
                else
                {

                }

            }
            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseGetSocketsImages"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber);
                    result.SetCardError(CardAnswerResults.CardNumber);
                    cancellationTokenSources[CardAnswerResults.CardNumber].Cancel();
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return result.GetCardsNotStopped().Count() == 0;
            }

            protected override void PrepareOutputData()
            {
                OutputData = result;

            }
        }



    }

}
