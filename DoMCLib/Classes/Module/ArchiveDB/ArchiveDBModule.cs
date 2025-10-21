using DoMCLib.Classes.Module.RDPB;
using DoMCLib.Configuration;
using DoMCLib.DB;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.ArchiveDB
{
    [Description("Архив")]
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
            WorkingLog = MainController.GetLogger($"{this.GetType().GetDescriptionOrName()}");
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
            WorkingLog.Add(LoggerLevel.Critical, "Установка конфигкрации работы модуля переноса данных в архив");
            var restart = IsStarted;
            if (restart)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Остановка работы модуля переноса данных в архив");
                await StopAsync();

            }
            WorkingLog.Add(LoggerLevel.Critical, "Применение конфигурации");
            this.Configuration = configuration;
            if (restart)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Перезапуск работы модуля переноса данных в архив");
                await StartAsync();
            }
        }
        public async Task StartAsync()
        {
            if (IsStarted) return;
            IsStarted = true;
            cancelationTockenSource = new CancellationTokenSource();
            task = Task.Run(() => Process());
            WorkingLog.Add(LoggerLevel.Critical, "Модуль переноса данных в архив запущен");
        }

        public async Task StopAsync()
        {
            if (!IsStarted) return;
            WorkingLog.Add(LoggerLevel.Critical, "Остановка модуля переноса данных в архив");
            if (cancelationTockenSource == null)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Задача не запущена?");
                IsStarted = false;
                cancelationTockenSource?.Cancel();
                WorkingLog.Add(LoggerLevel.Critical, "Остановка задачи обработчика модуля переноса данных в архив");
                if (task != null && !task.IsCompleted) await task;
                return;
            }
            cancelationTockenSource?.Cancel();
            try
            {
                WorkingLog.Add(LoggerLevel.Critical, "Остановка задачи обработчика модуля переноса данных в архив");
                if (task != null && !task.IsCompleted) await task;
                //await Task.WhenAny([task, Task.Delay(200)]);
                //task.;
            }
            catch (TaskCanceledException) { }
            WorkingLog.Add(LoggerLevel.Critical, "Освобождение ресурсов хранилища");
            Storage.Dispose();
            Storage = null;
            IsStarted = false;
        }

        private async Task Process()
        {
            IsStarted = true;
            Storage = new DataStorage(Configuration.LocalDBPath, Configuration.ArchiveDBPath, WorkingLog, ObserverForDataStorage);
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
            if (Storage == null) return new List<BoxDB>();
            var localBoxes = Storage.LocalGetBox(From, DateTime.Now);
            var RemoteBoxes = Storage.RemoteGetBox(From, DateTime.Now);
            var result = localBoxes.Concat(RemoteBoxes).DistinctBy(b => b.CompletedTime).ToList();
            return result;
        }

        public List<DefectedCycleSockets> GetDefectsList(double PeriodInHours)
        {
            if (Storage == null) return new List<DefectedCycleSockets>();
            var now = DateTime.Now;
            var From = now.AddHours(-PeriodInHours);
            var To = now;

            List<CycleData> ArchiveCycles = new List<CycleData>();

            var LocalArchiveCycles = Storage.LocalGetCycles(From, To);
            if (LocalArchiveCycles != null && LocalArchiveCycles.Count > 0)
                LocalArchiveCycles = LocalArchiveCycles.FindAll(lc => lc.IsSocketsGood.Any(s => !s));
            var RemoteArchiveCycles = Storage.RemoteGetCycles(From, To);
            if (RemoteArchiveCycles != null && RemoteArchiveCycles.Count > 0)
                RemoteArchiveCycles = RemoteArchiveCycles.FindAll(lc => lc.IsSocketsGood.Any(s => !s));


            if (LocalArchiveCycles != null)
                ArchiveCycles.AddRange(LocalArchiveCycles);
            if (RemoteArchiveCycles != null)
                ArchiveCycles.AddRange(RemoteArchiveCycles);


            List<DefectedCycleSockets> defects = new List<DefectedCycleSockets>();
            for (int i = 0; i < ArchiveCycles.Count; i++)
            {
                var rec = ArchiveCycles[i];
                var badsockets = rec.IsSocketsGood.Select((b, k) => new { b, k }).Where(x => !x.b).Select(y => y.k + 1).ToList();
                if (badsockets.Count != 0)
                {
                    defects.Add(new DefectedCycleSockets() { CycleDateTime = rec.CycleDateTime, DefectedSockets = badsockets });
                }
            }
            defects = defects.OrderBy(d => d.CycleDateTime).ToList();
            return defects;
        }


    }
}
