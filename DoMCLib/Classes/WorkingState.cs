using DoMCLib.Classes.Module.ArchiveDB;
using DoMCModuleControl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DoMCLib.Classes
{
    public class WorkingState : IDisposable
    {
        public SocketStatuses? SocketStatuses;

        public int[] ErrorsBySockets = new int[96]; //Счетчик дефективных съемов
        public int TotalDefectCycles = 0; //Счетчик сброшенных съемов
        public double CycleDuration; //длительность цикла
        public int CurrentBoxDefectCycles;//количество сбросов в текущем коробе
        public List<string>? EquipmentErrors;
        public List<Box>? Boxes;
        public bool IsRunning;
        public StopReason? LastStopReason;
        public List<int> CCDCardsFails;

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        public int PeriodOfAveragesInHours = 12;
        public List<AverageOfCycle> AveragesOfCycles = new List<AverageOfCycle>();
        public short?[] AveragesOfStandards;

        private Action? RefreshWorkingForm;
        private Func<Task>? StartWorkProc;
        private Func<Task>? StopWorkProc;

        public WorkingState()
        {
            LoadData();
        }

        public void ResetTotalDefectCyles()
        {
            TotalDefectCycles = 0;
            RefreshWorkingForm?.Invoke();
        }

        public void ResetStatistics()
        {
            ErrorsBySockets = new int[96];
            RefreshWorkingForm?.Invoke();
        }
        public void StartWork()
        {
            StartWorkProc?.Invoke();
        }
        public void StopWork()
        {
            StopWorkProc?.Invoke();
        }
        public void SetRefreshWorkingForm(Action refreshProc)
        {
            RefreshWorkingForm = refreshProc;
        }
        public void SetStartWorkProc(Func<Task> startProc)
        {
            StartWorkProc = startProc;
        }
        public void SetStopWorkProc(Func<Task> stopProc)
        {
            StopWorkProc = stopProc;
        }

        public async Task AddNewAverageOfCycle(DateTime cycleTime, short[] averages)
        {
            await _lock.WaitAsync(); // Асинхронное ожидание входа
            try
            {
                AveragesOfCycles.Add(new AverageOfCycle() { CycleTime = cycleTime, AveragesOfSockets = averages });
                var RemoveTime = DateTime.Now.AddHours(-PeriodOfAveragesInHours);
                AveragesOfCycles.RemoveAll(rec => rec.CycleTime < RemoveTime);
                if (AveragesOfCycles.Capacity > 50000)
                    AveragesOfCycles.TrimExcess();
            }
            finally
            {
                _lock.Release(); // Обязательно освобождаем
            }
        }

        public async Task<List<AverageOfSocket>> GetAverageOfSocket(int PhysicalSocketOnMatrix, int PeriodInHours)
        {
            await _lock.WaitAsync(); // Асинхронное ожидание входа
            try
            {
                var foundRecords = new List<AverageOfSocket>();
                var from = DateTime.Now.AddHours(-PeriodInHours);
                for (int i = 0; i < AveragesOfCycles.Count; i++)
                {
                    var cycle = AveragesOfCycles[i];
                    if (cycle.CycleTime >= from)
                    {
                        foundRecords.Add(new AverageOfSocket() { CycleTime = cycle.CycleTime, AveragesOfSocket = cycle.AveragesOfSockets[PhysicalSocketOnMatrix - 1] });
                    }
                }
                return foundRecords;
            }
            finally
            {
                _lock.Release(); // Обязательно освобождаем
            }
        }

        public async Task<List<DefectedCycleSockets>> GetDefectesCycles(IMainController controller, double PeriodInHours)
        {
            return await new DoMCLib.Classes.Module.ArchiveDB.Commands.GetCyclesWithDefectsFromCommand(controller, controller.GetModule(typeof(ArchiveDBModule))).ExecuteCommandAsync(PeriodInHours);
        }
        public async Task<List<Box>> GetBoxes(IMainController controller, double PeriodInHours)
        {
            return await new DoMCLib.Classes.Module.ArchiveDB.Commands.GetBoxFromCommand(controller, controller.GetModule(typeof(ArchiveDBModule))).ExecuteCommandAsync(DateTime.Now.AddHours(-PeriodInHours));
        }



        public void Dispose()
        {
            SaveData();
        }

        private void SaveData()
        {

        }
        private void LoadData()
        {

        }

        public string GetWorkingStatusString()
        {
            if (LastStopReason == null) return "";
            return LastStopReason.Value switch
            {
                StopReason.Manual => "Пользователь",
                StopReason.LCBFailure => "Авария БУС",
                StopReason.LCBBeaconFailure => "Авария БУС(Маячки выдвинуты)",
                StopReason.CCDFailure => $"Ошибка плат ПЗС(платы: {(CCDCardsFails != null ? string.Join(", ", CCDCardsFails.Select(c => c + 1)) : "")})",
                StopReason.Prepare => "Подготовка к работе",
                StopReason.UnhandledException or _ => "Неустановленная ошибка",

            };

        }

    }
    public class SocketStatus
    {
        public DateTime CycleDT;
        public bool[]? IsSocketGood;
    }

    public class SocketStatuses
    {
        private int SocketQuantity;
        private List<SocketStatus> Statuses;
        private double DeleteAfter = 3600;
        public SocketStatuses()
        {
            Statuses = new List<SocketStatus>();
            SocketQuantity = 96;
        }
        public SocketStatuses(int socketQuantity)
        {
            Statuses = new List<SocketStatus>();
            SocketQuantity = socketQuantity;
        }
        public void Add(SocketStatus ss)
        {
            if ((ss.IsSocketGood?.Length ?? 0) != SocketQuantity) return;
            lock (Statuses)
            {
                ClearByTime();
                Statuses.Add(ss);
            }
        }
        public void Add(DateTime cycleDT, bool[] isSocketsGood)
        {
            if (isSocketsGood.Length != SocketQuantity) return;
            lock (Statuses)
            {
                ClearByTime();
                Statuses.Add(new SocketStatus() { CycleDT = cycleDT, IsSocketGood = isSocketsGood });
            }
        }

        private void ClearByTime()
        {
            var now = DateTime.Now;
            Statuses.RemoveAll(s => (now - s.CycleDT).TotalSeconds > DeleteAfter);
            Statuses.RemoveAll(s => s == null);
            Statuses.TrimExcess();
        }

        public int[] GetBadPreformsForSockets()
        {
            int[] sum = new int[SocketQuantity];
            lock (Statuses)
            {
                foreach (var ss in Statuses)
                {
                    if (ss != null)
                    {
                        for (int n = 0; n < SocketQuantity; n++)
                        {
                            sum[n] += (ss.IsSocketGood == null) ? 0 : (ss.IsSocketGood[n] ? 0 : 1);
                        }
                    }
                }
            }
            return sum;
        }

        public bool[]? GetLast()
        {
            if (Statuses.Count == 0) return Enumerable.Repeat(true, 96).ToArray();
            var maxDT = Statuses.Max(s => s.CycleDT);
            var lastStatus = Statuses.Find(s => s.CycleDT == maxDT);
            if (lastStatus == null) return new bool[96];
            else return lastStatus.IsSocketGood;
        }

    }

    public class AverageOfCycle
    {
        public DateTime CycleTime;
        public short[] AveragesOfSockets;
    }
    public class AverageOfSocket
    {
        public DateTime CycleTime;
        public short AveragesOfSocket;
    }

    public enum StopReason
    {
        Manual,
        Prepare,
        LCBFailure,
        LCBBeaconFailure,
        CCDFailure,
        UnhandledException
    }
}
