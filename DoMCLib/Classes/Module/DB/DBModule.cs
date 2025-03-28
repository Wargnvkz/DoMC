using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Classes.Module.ArchiveDB;
using DoMCLib.DB;
using DoMCLib.Tools;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.DB
{
    public class DBModule : AbstractModuleBase
    {
        public string DBPath;
        ILogger WorkingLog;
        DataStorage Storage;
        bool IsStarted;
        Task task;
        CancellationTokenSource cancelationTockenSource;
        ThrottledErrorNotifier errorNotifier;
        Observer ObserverForDataStorage;
        ConcurrentQueue<CycleImagesCCD> cycleDatas = new ConcurrentQueue<CycleImagesCCD>();
        ConcurrentQueue<Box> BoxDatas = new ConcurrentQueue<Box>();
        Observer ExternalObserver;
        public DBModule(IMainController MainController) : base(MainController)
        {
            errorNotifier = new ThrottledErrorNotifier(MainController.GetObserver(), 300, 5);
            WorkingLog = MainController.GetLogger($"{this.GetType().Name}");
            ObserverForDataStorage = new Observer(WorkingLog);
            ObserverForDataStorage.NotificationReceivers += ObserverForDataStorage_NotificationReceived;
            ExternalObserver = MainController.GetObserver();
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

        public void SetConfiguration(string dbPath)
        {
            var restart = IsStarted;
            if (restart)
            {
                Stop();
            }
            DBPath = dbPath;
            if (restart)
            {
                Start();
            }
        }
        public void Start()
        {
            if (IsStarted) return;
            Storage = new DataStorage(DBPath, null, WorkingLog, ObserverForDataStorage);
            WorkingLog.Add(LoggerLevel.Critical, "Модуль переноса данных в архив запущен");
            cancelationTockenSource = new CancellationTokenSource();
            task = new Task(Process);
            task.Start();
        }

        public void Stop()
        {
            if (!IsStarted) return;
            try
            {
                cancelationTockenSource?.Cancel();
                task?.Wait();
            }
            catch { }
        }

        public void EnqueueCycleDate(CycleImagesCCD cd)
        {
            cycleDatas.Enqueue(cd);

        }
        public void EnqueueBoxDate(Box box)
        {
            BoxDatas.Enqueue(box);
        }

        private void Process()
        {
            IsStarted = true;
            while (!cancelationTockenSource.Token.IsCancellationRequested)
            {
                var CycleDataList = new List<CycleImagesCCD>();
                while (cycleDatas.TryDequeue(out CycleImagesCCD cycle))
                {
                    CycleDataList.Add(cycle);
                }
                Parallel.ForEach(CycleDataList, cycle =>
                {
                    try
                    {
                        WorkingLog.Add(LoggerLevel.Information, $"Съем {cycle.CycleCCDDateTime}. Начало сохранения съема");
                        var cycleData = CycleData.ConvertFromCycleImageCCD(cycle);

                        WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Съем {cycle.CycleCCDDateTime}. Изображения: " + ArrayTools.BoolArrayToHex(cycle.CurrentImages.Select(wi => wi != null).ToArray()));
                        WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Съем {cycle.CycleCCDDateTime}. Разница: " + ArrayTools.BoolArrayToHex(cycle.Differences.Select(wi => wi != null).ToArray()));
                        WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Съем {cycle.CycleCCDDateTime}. Эталон: " + ArrayTools.BoolArrayToHex(cycle.StandardImages.Select(wi => wi != null).ToArray()));


                        Storage.LocalSaveCycleAndImagesOfActiveSockets(cycleData);
                        WorkingLog.Add(LoggerLevel.Information, $"Съем {cycle.CycleCCDDateTime}. Сохранен");
                    }
                    catch (Exception ex)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Съем {cycle.CycleCCDDateTime}. Ошибка при сохранении данных цикла", ex);
                        ExternalObserver.Notify(this, "CycleSave", "Error", ex);
                    }
                });
                ExternalObserver.Notify(this, "CycleSave", "NonSaved", cycleDatas.Count);

                while (BoxDatas.Count > 0)
                {
                    BoxDatas.TryDequeue(out Box box);
                    try
                    {
                        WorkingLog.Add(LoggerLevel.Information, $"Короб: {box.CompletedTime.ToString("G")} {box.BadCyclesCount} {box.TransporterSide.ToString()}");
                        Storage.LocalSaveBox(new BoxDB(box));
                    }
                    catch (Exception ex)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, "Ошибка при сохранении данных короба. ", ex);
                        ExternalObserver.Notify(this, "BoxSave", "Error", ex);
                    }
                    WorkingLog.Add(LoggerLevel.Information, $"Несохраненных коробов: {BoxDatas.Count}");

                }
                ExternalObserver.Notify(this, "BoxSave", "NonSaved", BoxDatas.Count);
                Task.Delay(100).Wait();
            }
            IsStarted = false;
        }

        public class SetConfigurationCommand : AbstractCommandBase
        {
            public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(string), null) { }
            protected override void Executing() => ((DBModule)Module).SetConfiguration((string)InputData);
        }
        public class StartCommand : AbstractCommandBase
        {
            public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((DBModule)Module).Start();
        }
        public class StopCommand : AbstractCommandBase
        {
            public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((DBModule)Module).Stop();
        }
        public class EnqueueCycleDateCommand : AbstractCommandBase
        {
            public EnqueueCycleDateCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(CycleImagesCCD), null) { }
            protected override void Executing() => ((DBModule)Module).EnqueueCycleDate((CycleImagesCCD)InputData);
        }
        public class EnqueueBoxDateCommand : AbstractCommandBase
        {
            public EnqueueBoxDateCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(Box), null) { }
            protected override void Executing() => ((DBModule)Module).EnqueueBoxDate((Box)InputData);
        }


    }


}
