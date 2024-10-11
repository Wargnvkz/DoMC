using DoMCModuleControl.Modules;
using DoMCModuleControl;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        /// <summary>
        /// Получить изображение одного гнезда
        /// </summary>
        public class GetSocketImageDataCommand : WaitCommandBase
        {
            SocketReadData? result = null;
            CancellationTokenSource cancellationTokenSources = new CancellationTokenSource();
            public GetSocketImageDataCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(GetSocketImageDataCommandData), typeof(SocketReadData)) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var Data = (GetSocketImageDataCommandData)InputData;
                if (Data != null && Data.Context != null)
                {
                    var cardSocketParameters = Data.Context.GetSocketParametersByEquipmentSockets(new List<int>() { Data.EqipmentSocket });
                    var cardSocket = cardSocketParameters[0].Item2;
                    var SocketParameters = cardSocketParameters[0].Item3;

                    cancellationTokenSources = new CancellationTokenSource();
                    module.tcpClients[cardSocket.CCDCardNumber].SendCommandGetSocketImage(cardSocket.InnerSocketNumber, cancellationTokenSources.Token);
                    //var task = new Task(new Action<object?>((i) => { }), cardSocket.InnerSocketNumber,);
                    var task = new Task(new Action<object?>((i) =>
                    {
                        var cardnumber = (int)i;
                        var ReadResult = module.tcpClients[cardSocket.CCDCardNumber].GetImageDataFromSocketSync(cardSocket.InnerSocketNumber, Data.Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout, cancellationTokenSources, out SocketReadData data);
                        if (ReadResult)
                        {
                            var equipmentSocket = Data.Context.Configuration.HardwareSettings.CardSocket2EquipmentSocket[cardSocket.CardSocketNumber()];
                            result = data;
                        }
                        else
                        {
                            result = null;
                        }

                    }), cardSocket.InnerSocketNumber, cancellationTokenSources.Token);
                    task.Start();
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
                    cancellationTokenSources.Cancel();
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return result != null;
            }

            protected override void PrepareOutputData()
            {
                OutputData = result;

            }
        }

    }

}
