using DoMCLib.Classes.Model.RDPB;
using DoMCLib.DB;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Model.ArchiveDB
{
    public partial class ArchiveDBModule : DoMCModuleControl.Modules.ModuleBase
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
            ObserverForDataStorage.NotificationReceived += ObserverForDataStorage_NotificationReceived;
        }

        private void ObserverForDataStorage_NotificationReceived(string Name, object? data)
        {
            switch (Name)
            {
                //TODO: найти сообщениня совместимые с текущей работой переноса, а несовместимые и передать их наверх в приложение
                case "":
                    break;
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
                            Storage.MoveFromLocalToRemoteWithDutyCycle(Configuration.ArchiveRecordAgeSeconds, 300, 60);
                            WorkingLog.Add(LoggerLevel.Information, "Перенос данных в архив завершен");
                        }
                        else
                        {
                            WorkingLog.Add(LoggerLevel.Information, "Удаление данных превышающих время хранения из-за отсутствия архива");
                            Storage.LocalDeleteCyclesByTime(Configuration.ArchiveRecordAgeSeconds);
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
       
    }
}
