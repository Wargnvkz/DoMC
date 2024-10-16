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
            public GetSocketImageDataCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(GetSocketImageDataCommandData), typeof(SocketReadData)) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var Data = (GetSocketImageDataCommandData)InputData;
                if (Data != null && Data.Context != null)
                {
                    var cardSocketParameters = Data.Context.GetSocketParametersByEquipmentSockets(new List<int>() { Data.EqipmentSocket });
                    var cardSocket = cardSocketParameters[0].Item2;
                    var SocketParameters = cardSocketParameters[0].Item3;

                    module.tcpClients[cardSocket.CCDCardNumber].SendCommandGetSocketImage(cardSocket.InnerSocketNumber, CancellationTokenSourceBase.Token);
                    //var task = new Task(new Action<object?>((i) => { }), cardSocket.InnerSocketNumber,);
                    var task = new Task(new Action<object?>((i) =>
                    {
                        var cardnumber = (int)i;
                        var ReadResult = module.tcpClients[cardSocket.CCDCardNumber].GetImageDataFromSocketAsync(cardSocket.InnerSocketNumber, Data.Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout, CancellationTokenSourceBase.Token, out SocketReadData data);
                        if (ReadResult)
                        {
                            var equipmentSocket = Data.Context.Configuration.HardwareSettings.CardSocket2EquipmentSocket[cardSocket.CardSocketNumber()];
                            result = data;
                        }
                        else
                        {
                            result = null;
                        }

                    }), cardSocket.InnerSocketNumber, CancellationTokenSourceBase.Token);
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
                    CancellationTokenSourceBase.Cancel();
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
