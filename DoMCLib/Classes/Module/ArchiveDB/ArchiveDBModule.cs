using DoMCLib.Classes.Module.RDPB;
using DoMCLib.Configuration;
using DoMCLib.DB;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.ArchiveDB
{
    public partial class ArchiveDBModule : DoMCModuleControl.Modules.AbstractModuleBase
    {
        ArchiveDBConfiguration Configuration;
        DateTime TimeLastLocalToRemoteCheck;
        int DelayOfTransferInSeconds;
        ILogger WorkingLog;
        DataStorage Storage;
        public bool IsStarted { get; private set; } = false;
        Task task;
        CancellationTokenSource cancelationTockenSource;
        ThrottledErrorNotifier errorNotifier;
        Observer ObserverForDataStorage;

        public ArchiveDBModule(IMainController MainController) : base(MainController)
        {
            errorNotifier = new ThrottledErrorNotifier(MainController.GetObserver(), 300, 5);
            WorkingLog = MainController.GetLogger($"{this.GetType().Name}");
            ObserverForDataStorage = MainController.GetObserver();//new Observer(WorkingLog);
            ObserverForDataStorage.NotificationReceivers += ObserverForDataStorage_NotificationReceived;
        }

        private async Task ObserverForDataStorage_NotificationReceived(string Name, object? data)
        {
            if (Name == DoMCApplicationContext.ConfigurationUpdateEventName)
            {
                var cfg = data as ApplicationConfiguration;
                await SetConfigurationAsync(cfg.HardwareSettings.ArchiveDBConfig);
            }
        }

        public async Task SetConfigurationAsync(ArchiveDBConfiguration configuration)
        {
            var restart = IsStarted;
            if (restart)
            {

                await StopAsync();

            }
            this.Configuration = configuration;
            if (restart)
            {
                await StartAsync();
            }
        }
        public async Task StartAsync()
        {
            Storage = new DataStorage(Configuration.LocalDBPath, Configuration.ArchiveDBPath, WorkingLog, ObserverForDataStorage);
            WorkingLog.Add(LoggerLevel.Critical, "Модуль переноса данных в архив запущен");
            cancelationTockenSource = new CancellationTokenSource();
            task = Task.Run(() => Process());
        }

        public async Task StopAsync()
        {
            if (cancelationTockenSource == null || !IsStarted)
            {
                IsStarted = false;
                return;
            }
            cancelationTockenSource?.Cancel();
            try
            {
                await task;
                //await Task.WhenAny([task, Task.Delay(200)]);
                //task.;
            }
            catch (TaskCanceledException) { }
            IsStarted = false;
        }

        private async Task Process()
        {
            IsStarted = true;
            while (!cancelationTockenSource.Token.IsCancellationRequested)
            {
                try
                {
                    if ((DateTime.Now - TimeLastLocalToRemoteCheck).TotalSeconds > Configuration.TransferFrequency)
                    {
                        if (Storage.RemoteIsActive)
                        {
                            WorkingLog.Add(LoggerLevel.Information, "Начало переноса прошлых данных");
                            TimeLastLocalToRemoteCheck = DateTime.Now;
                            //TODO:обеспечить отмену операции при отмене CancelationTocken
                            Storage.MoveFromLocalToRemoteWithDutyCycle(Configuration.ArchiveRecordAgeSeconds, Configuration.DutyCycleInSeconds, Configuration.BeforeAndAfterErrorInSeconds, cancelationTockenSource.Token);
                            WorkingLog.Add(LoggerLevel.Information, "Перенос данных в архив завершен");
                            ObserverForDataStorage.Notify($"{this.GetType().Name}.Success", null);
                        }
                        else
                        {
                            WorkingLog.Add(LoggerLevel.Information, "Удаление данных превышающих время хранения из-за отсутствия архива");
                            Storage.LocalDeleteCyclesByTime(Configuration.ArchiveRecordAgeSeconds);
                            ObserverForDataStorage.Notify($"{this.GetType().Name}.Deleted", null);
                        }
                    }
                    await Task.Delay(100, cancelationTockenSource.Token);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    WorkingLog.Add(LoggerLevel.Critical, "Ошибка при переносе данных. ", ex);
                    errorNotifier.SendError($"{this.GetType().Name}.Error", ex);
                }
            }
            IsStarted = false;
        }

        public List<BoxDB> GetBoxes(DateTime From)
        {
            var localBoxes = Storage.LocalGetBox(From, DateTime.Now);
            var RemoteBoxes = Storage.RemoteGetBox(From, DateTime.Now);
            var result = localBoxes.Concat(RemoteBoxes).DistinctBy(b => b.CompletedTime).ToList();
            return result;
        }

    }
}
