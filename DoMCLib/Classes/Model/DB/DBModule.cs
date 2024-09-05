using DoMCLib.Classes.Model.ArchiveDB;
using DoMCLib.DB;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Model.DB
{
    public class DBModule : ModuleBase
    {
        public string DBPath;
        ILogger WorkingLog;
        DataStorage Storage;
        bool IsStarted;
        Task task;
        CancellationTokenSource cancelationTockenSource;
        ThrottledErrorNotifier errorNotifier;
        Observer ObserverForDataStorage;
        ConcurrentQueue<CycleData> cycleDatas = new ConcurrentQueue<CycleData>();
        public DBModule(IMainController MainController) : base(MainController)
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
            Storage = new DataStorage(DBPath, null, WorkingLog, ObserverForDataStorage);
            WorkingLog.Add(LoggerLevel.Critical, "Модуль переноса данных в архив запущен");
            cancelationTockenSource = new CancellationTokenSource();
            task = new Task(Process);
        }

        public void Stop()
        {
            cancelationTockenSource.Cancel();
            task.Wait();
        }

        public void EnqueueDate(CycleData cd)
        {
            cycleDatas.Enqueue(cd);

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
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Начало сохранения съема");
                        var cycleData = CycleData.ConvertFromCycleImageCCD(cycle);

                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Изображения: " + InterfaceDataExchange.BoolArrayToHex(cycle.WorkModeImages.Select(wi => wi != null).ToArray()));
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Разница: " + InterfaceDataExchange.BoolArrayToHex(cycle.Differences.Select(wi => wi != null).ToArray()));
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Эталон: " + InterfaceDataExchange.BoolArrayToHex(cycle.StandardImage.Select(wi => wi != null).ToArray()));


                        if (InterfaceDataExchange.DataStorage == null) return;
                        InterfaceDataExchange.DataStorage.LocalSaveCycleAndImagesOfActiveSockets(cycleData);
                        InterfaceDataExchange.Errors.NoLocalSQL = false;
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Сохранен");
                    }
                    catch (Exception ex)
                    {
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Ошибка при сохранении данных цикла", ex);
                        InterfaceDataExchange.Errors.LocalSQLCycleSaveError = true;
                        return;
                    }
                });
                Task.Delay(100).Wait();
            }
            IsStarted = false;
        }

        public bool OnCalculate(ref DataExchangeKernel.ACS_Core.Memory data, double dt)
        {
            if (InterfaceDataExchange == null) return true;
            if (InterfaceDataExchange.CyclesCCD == null) return true;
            if (InterfaceDataExchange.DataStorage == null) return true;
            try
            {
                var CycleDataList = new List<CycleImagesCCD>();
                while (InterfaceDataExchange.CyclesCCD.TryDequeue(out CycleImagesCCD cycle))
                {
                    CycleDataList.Add(cycle);
                }

                Parallel.ForEach(CycleDataList, cycle =>
                {
                    try
                    {
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Начало сохранения съема");
                        var cycleData = CycleData.ConvertFromCycleImageCCD(cycle);

                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Изображения: " + InterfaceDataExchange.BoolArrayToHex(cycle.WorkModeImages.Select(wi => wi != null).ToArray()));
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Разница: " + InterfaceDataExchange.BoolArrayToHex(cycle.Differences.Select(wi => wi != null).ToArray()));
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Эталон: " + InterfaceDataExchange.BoolArrayToHex(cycle.StandardImage.Select(wi => wi != null).ToArray()));


                        if (InterfaceDataExchange.DataStorage == null) return;
                        InterfaceDataExchange.DataStorage.LocalSaveCycleAndImagesOfActiveSockets(cycleData);
                        InterfaceDataExchange.Errors.NoLocalSQL = false;
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Сохранен");
                    }
                    catch (Exception ex)
                    {
                        WorkingLog.Add($"Съем {cycle.CycleCCDDateTime}. Ошибка при сохранении данных цикла", ex);
                        InterfaceDataExchange.Errors.LocalSQLCycleSaveError = true;
                        return;
                    }
                });
                if (CycleDataList.Count > 0)
                    WorkingLog.Add($"Несохраненных съемов: {InterfaceDataExchange.CyclesCCD.Count}");
            }
            catch { }
            try
            {
                while (InterfaceDataExchange.Boxes.TryPeek(out Classes.Box box))
                {
                    InterfaceDataExchange.Boxes.TryDequeue(out Classes.Box box1);
                    try
                    {
                        WorkingLog.Add($"Короб: {box.CompletedTime.ToString("G")} {box.BadCyclesCount} {box.TransporterSide.ToString()}");
                        if (InterfaceDataExchange.DataStorage == null) return true;
                        InterfaceDataExchange.DataStorage.LocalSaveBox(new DB.Box(box));
                        InterfaceDataExchange.Errors.NoLocalSQL = false;
                    }
                    catch (Exception ex)
                    {
                        WorkingLog.Add("Ошибка при сохранении данных короба");
                        WorkingLog.Add(ex);
                        InterfaceDataExchange.Errors.LocalSQLCycleSaveError = true;
                        return false;
                    }
                    WorkingLog.Add($"Несохраненных коробов: {InterfaceDataExchange.Boxes.Count}");

                }
            }
            catch { }

            return true;
        }

    }
}
