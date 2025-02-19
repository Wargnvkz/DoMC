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
        bool IsStarted;
        Task task;
        CancellationTokenSource cancelationTockenSource;
        ThrottledErrorNotifier errorNotifier;
        Observer ObserverForDataStorage;

        public ArchiveDBModule(IMainController MainController) : base(MainController)
        {
            errorNotifier = new ThrottledErrorNotifier(MainController.GetObserver(), 300, 5);
            WorkingLog = MainController.GetLogger($"{this.GetType().Name}");
            ObserverForDataStorage = new Observer(WorkingLog);
            ObserverForDataStorage.NotificationReceivers += ObserverForDataStorage_NotificationReceived;
        }

        private void ObserverForDataStorage_NotificationReceived(string Name, object? data)
        {
            if (Name == DoMCApplicationContext.ConfigurationUpdateEventName)
            {
                var cfg = data as ApplicationConfiguration;
                SetConfiguration(cfg.HardwareSettings.ArchiveDBConfig);
            }
        }

        public void SetConfiguration(ArchiveDBConfiguration configuration)
        {
            var restart = IsStarted;
            if (restart)
            {
                Stop();
            }
            this.Configuration = configuration;
            if (restart)
            {
                Start();
            }
        }
        public void Start()
        {
            Storage = new DataStorage(Configuration.LocalDBPath, Configuration.ArchiveDBPath, WorkingLog, ObserverForDataStorage);
            WorkingLog.Add(LoggerLevel.Critical, "Модуль переноса данных в архив запущен");
            cancelationTockenSource = new CancellationTokenSource();
            task = new Task(Process);
            task.Start();
        }

        public void Stop()
        {
            cancelationTockenSource.Cancel();
            task.Wait();
            IsStarted = false;
        }

        private void Process()
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
                            Storage.MoveFromLocalToRemoteWithDutyCycle(Configuration.ArchiveRecordAgeSeconds, Configuration.DutyCycleInSeconds, Configuration.BeforeAndAfterErrorInSeconds);
                            WorkingLog.Add(LoggerLevel.Information, "Перенос данных в архив завершен");
                            ObserverForDataStorage.Notify($"{this.GetType().Name}.Success",null);
                        }
                        else
                        {
                            WorkingLog.Add(LoggerLevel.Information, "Удаление данных превышающих время хранения из-за отсутствия архива");
                            Storage.LocalDeleteCyclesByTime(Configuration.ArchiveRecordAgeSeconds);
                            ObserverForDataStorage.Notify($"{this.GetType().Name}.Deleted", null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WorkingLog.Add(LoggerLevel.Critical, "Ошибка при переносе данных. ", ex);
                    errorNotifier.SendError($"{this.GetType().Name}.Error", ex);
                }
                Task.Delay(100).Wait();
            }
            IsStarted = false;
        }

        public class SetConfigurationCommand : AbstractCommandBase
        {
            public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(ArchiveDBConfiguration), null) { }
            protected override void Executing() => ((ArchiveDBModule)Module).SetConfiguration((ArchiveDBConfiguration)InputData);
        }
        public class StartCommand : AbstractCommandBase
        {
            public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((ArchiveDBModule)Module).Start();
        }
        public class StopCommand : AbstractCommandBase
        {
            public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((ArchiveDBModule)Module).Stop();
        }
        public class GetWorkingStatusCommand : AbstractCommandBase
        {
            public GetWorkingStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(bool)) { }
            protected override void Executing() => OutputData = ((ArchiveDBModule)Module).IsStarted;
        }
    }
}
