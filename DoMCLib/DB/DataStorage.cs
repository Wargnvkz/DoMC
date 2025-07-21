using DoMCLib.Classes;
using DoMCLib.Tools;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.DB
{
    public class DataStorage
    {
        private string ConnectionStringLocal;
        private string ConnectionStringRemote;

        /*private MSSQLDBStorage LocalStorage = null;
        private MSSQLDBStorage RemoteStorage = null;*/
        private FileDB LocalStorage = null;
        private FileDB RemoteStorage = null;

        public bool LocalIsActive;
        public bool RemoteIsActive;

        private bool IsTerminatingMovingToArchive = false;
        private bool IsMovingToArchive = false;
        private ILogger WorkingLog;
        Observer Observer;

        public DataStorage(string connectionStringLocal, string connectionStringRemote, ILogger logger, Observer observer)
        {
            Observer = observer;
            WorkingLog = logger;
            if (String.IsNullOrWhiteSpace(connectionStringLocal))
            {
                LocalIsActive = false;
            }
            else
            {
                try
                {
                    ConnectionStringLocal = connectionStringLocal;
                    //if (String.IsNullOrWhiteSpace(ConnectionStringLocal)) throw new Exception("Строка подключения не может быть пустой");
                    LocalStorage = new FileDB(ConnectionStringLocal);
                    LocalStorage.CheckDB(false);
                    LocalIsActive = true;
                }
                catch
                {
                    LocalIsActive = false;
                }
            }
            if (String.IsNullOrWhiteSpace(connectionStringRemote))
            {
                RemoteIsActive = false;
            }
            else
            {
                try
                {
                    ConnectionStringRemote = connectionStringRemote;
                    //if (String.IsNullOrWhiteSpace(ConnectionStringRemote)) return;
                    RemoteStorage = new FileDB(ConnectionStringRemote);//new MSSQLDBStorage(ConnectionStringRemote);
                    RemoteStorage.CheckDB(false);
                    RemoteIsActive = true;
                }
                catch
                {
                    RemoteIsActive = false;
                }
            }
            if (LocalStorage != null) LocalStorage.SetLog(WorkingLog);
            if (RemoteStorage != null) RemoteStorage.SetLog(WorkingLog);
        }



        #region Local
        public virtual void LocalSaveCycleAndImagesOfActiveSockets(CycleData cd)
        {
            if (LocalStorage == null) return;
            LocalStorage.SaveCycleAndImagesOfActiveSockets(cd);
        }

        public virtual CycleData LocalGetCycleById(long id)
        {
            if (LocalStorage == null) return null;

            var cd = LocalStorage.GetCycleById(id);
            return cd;
        }

        public virtual CycleData LocalGetCycleByDateTime(DateTime dt)
        {
            if (LocalStorage == null) return null;

            var cd = LocalStorage.GetTheLastCycleHeaderBeforeDateTime(dt);

            return cd;
        }

        public virtual List<CycleData> LocalGetCycles(DateTime From, DateTime To)
        {
            if (LocalStorage == null) return null;

            var cd = LocalStorage.GetCyclesHeaders(From, To);

            return cd;
        }
        public virtual List<CycleData> LocalGetCycles(DateTime To)
        {
            if (LocalStorage == null) return null;

            var cd = LocalStorage.GetCyclesHeadersBefore(To);

            return cd;
        }

        public virtual void LocalDeleteCycle(long id)
        {
            if (LocalStorage == null) return;
            LocalStorage.DeleteCycleByID(id);
        }

        public virtual void LocalDeleteCyclesByTime(int DelayOfTransferInSeconds)
        {
            if (LocalStorage == null) return;
            var now = DateTime.Now;
            var moveFrom = now.AddSeconds(-DelayOfTransferInSeconds);
            var cycles = LocalStorage.GetCyclesHeadersBefore(moveFrom);
            foreach (var cycle in cycles)
            {
                LocalDeleteCycle(cycle.CycleID);
            }
        }

        public virtual void LocalSaveBox(BoxDB box)
        {
            if (LocalStorage == null) return;
            LocalStorage.SaveBox(box);
        }

        public virtual List<BoxDB> LocalGetBox(DateTime start, DateTime end)
        {
            if (LocalStorage == null) return null;
            var res = LocalStorage.GetBox(start, end);
            return res;
        }

        public virtual void LocalDeleteBox(int BoxId)
        {
            if (LocalStorage == null) return;
            LocalStorage.DeleteBox(BoxId);

        }

        #endregion
        #region Remote

        public virtual CycleData RemoteGetCycleById(long id)
        {
            if (RemoteStorage == null) return null;

            var cd = RemoteStorage.GetCycleById(id);

            return cd;
        }

        public virtual CycleData RemoteGetCycleByDateTime(DateTime dt)
        {
            if (RemoteStorage == null) return null;

            var cd = RemoteStorage.GetTheLastCycleHeaderBeforeDateTime(dt);

            return cd;
        }

        /*public virtual int RemoteGetLastCycleID()
        {
            if (RemoteStorage == null) return 0;

            var cd = RemoteStorage.GetLastCycleID();

            return cd;
        }*/

        public virtual List<CycleData> RemoteGetCycles(DateTime From, DateTime To)
        {
            if (RemoteStorage == null) return null;

            var cd = RemoteStorage.GetCyclesHeaders(From, To);

            return cd;
        }
        public virtual List<CycleData> RemoteGetCycles(DateTime To)
        {
            if (RemoteStorage == null) return null;

            var cd = RemoteStorage.GetCyclesHeadersBefore(To);

            return cd;
        }

        public virtual void RemoteDeleteCycle(int id)
        {
            if (RemoteStorage == null) return;
            RemoteStorage.DeleteCycleByID(id);
        }


        public virtual void RemoteSaveBox(BoxDB box)
        {
            if (RemoteStorage == null) return;
            RemoteStorage.SaveBox(box);
        }

        public virtual List<BoxDB> RemoteGetBox(DateTime start, DateTime end)
        {
            if (RemoteStorage == null) return null;
            var res = RemoteStorage.GetBox(start, end);
            return res;
        }

        public virtual void RemoteDeleteBox(int BoxId)
        {
            if (RemoteStorage == null) return;
            RemoteStorage.DeleteBox(BoxId);

        }

        #endregion

        public void TerminateMovingToArchive()
        {
            IsTerminatingMovingToArchive = true;
        }
        public bool IsMovingReportWorking()
        {
            return IsMovingToArchive;
        }

        /*public virtual void MoveFromLocalToRemote(int DelayOfTransferInSeconds)
        {
            IsTerminatingMovingToArchive = false;
            if (LocalStorage == null) return;
            var now = DateTime.Now;
            var moveFrom = now.AddSeconds(-DelayOfTransferInSeconds);
            var cycles = LocalStorage.GetCyclesHeadersBefore(moveFrom);
            WorkingLog?.Add(LoggerLevel.Information, cycles.Count + " съемов для переноса.");
            IsMovingToArchive = true;
            foreach (var cycleHeader in cycles)
            {
                if (IsTerminatingMovingToArchive) break;

                try
                {
                    WorkingLog?.Add(LoggerLevel.Information, "Перенос съема от " + cycleHeader.CycleDateTime.ToString("dd-MM-yyyy HH\\:mm\\:ss"));
                }
                catch { }
                try
                {
                    var cycle = LocalStorage.GetCycleCompressedById(cycleHeader.CycleID);
                    if (cycle != null)
                    {
                        try
                        {
                            WorkingLog?.Add(LoggerLevel.FullDetailedInformation, "Хорошие гнезда:" + ArrayTools.BoolArrayToHex(cycle.IsSocketsGood));
                            WorkingLog?.Add(LoggerLevel.FullDetailedInformation, "Изображение гнезд:" + ((cycle.SocketImages != null && cycle.SocketImages.Count > 0) ? ArrayTools.BoolArrayToHex(Enumerable.Range(1, 96).Select(sn => cycle.SocketImages.Find(si => si.SocketNumber == sn) != null).ToArray()) : ""));
                        }
                        catch { }
                        var localCycleID = cycle.CycleID;
                        cycle.SocketImages.ForEach(si => si.IsSocketActive = true);

                        if (RemoteStorage != null)
                        {
                            try
                            {
                                RemoteStorage.SaveCycleAndCompressedImagesOfActiveSockets(cycle);
                            }
                            catch (Exception ex)
                            {
                                WorkingLog?.Add(LoggerLevel.Critical, "Ошибка при записи в архив: " + ex.Message);
                                Observer.Notify($"{this.GetType().Name}.DBRemoteWrite.Error", ex);
                            }
                        }

                        try
                        {
                            LocalStorage.DeleteCycleByIDAndIgnoreErrors(localCycleID);
                        }
                        catch (Exception ex)
                        {
                            WorkingLog?.Add(LoggerLevel.Critical, "Ошибка при удалении из локальной базы: " + ex.Message);
                            Observer.Notify($"{this.GetType().Name}.DBLocalDelete.Error", ex);
                        }
                    }
                }
                catch (Exception ex)
                {

                    WorkingLog?.Add(LoggerLevel.Critical, "Ошибка при получении съема в локальной БД: " + ex.Message);
                    Observer.Notify($"{this.GetType().Name}.DBLocalRead.Error", ex);

                }
            }
            try
            {
                var boxes = LocalStorage.GetBoxesBefore(moveFrom);
                foreach (var box in boxes)
                {
                    try
                    {
                        WorkingLog?.Add(LoggerLevel.Information, "Перенос данных о коробе: " + box.CompletedTime.ToString("G"));
                        RemoteStorage.SaveBox(box);
                        LocalStorage.DeleteBox(box.BoxID);
                    }
                    catch (Exception ex)
                    {
                        WorkingLog?.Add(LoggerLevel.Critical, "Ошибка при попытке переноса: " + ex.Message);
                        Observer.Notify($"{this.GetType().Name}.BoxDBMove.Error", ex);

                    }
                }
            }
            catch (Exception ex)
            {
                WorkingLog?.Add(LoggerLevel.Critical, "Ошибка при чтении списка коробов: " + ex.Message);
                Observer.Notify($"{this.GetType().Name}.BoxDBRead.Error", ex);
            }
            IsMovingToArchive = false;
            IsTerminatingMovingToArchive = false;
        }*/

        public virtual void MoveFromLocalToRemoteWithDutyCycle(int DelayOfTransferInSeconds, int DutyCycleInSeconds, int BeforeAndAfterErrorInSeconds, CancellationToken token)
        {
            IsTerminatingMovingToArchive = false;
            if (LocalStorage == null || RemoteStorage == null) return;
            if (token.IsCancellationRequested) return;
            LocalStorage.ResetCache();
            if (token.IsCancellationRequested) return;
            RemoteStorage.ResetCache();
            if (token.IsCancellationRequested) return;
            try
            {
                var now = DateTime.Now;
                var moveFrom = now.AddSeconds(-DelayOfTransferInSeconds);
                var cycles = LocalStorage.GetCyclesHeadersBefore(moveFrom);
                var ArchiveCycles = RemoteStorage.GetCyclesHeadersBefore(moveFrom);
                WorkingLog?.Add(LoggerLevel.Information, cycles.Count + " съемов для переноса.");

                IsMovingToArchive = true;
                List<CycleData> AllCycles = new List<CycleData>();
                AllCycles.AddRange(ArchiveCycles);
                AllCycles.AddRange(cycles);
                //cycle
                AllCycles = AllCycles.OrderBy(c => c.CycleDateTime).ToList();
                cycles = cycles.OrderBy(c => c.CycleDateTime).ToList();
                List<CycleData> toRemove = new List<CycleData>();
                List<CycleData> toMove = new List<CycleData>();
                foreach (var cycleHeader in cycles)
                {
                    if (token.IsCancellationRequested) return;

                    var StartPeriodID = cycleHeader.CycleDateTime.AddSeconds(-DutyCycleInSeconds).Ticks;
                    var id = cycleHeader.CycleID;
                    var PeriodCycles = AllCycles.FindAll(c => c.CycleID >= StartPeriodID && c.CycleID < id);
                    if (PeriodCycles.Count == 0)
                    {
                        toMove.Add(cycleHeader);
                        continue;
                    }
                    var ErrPeriodStartID = cycleHeader.CycleDateTime.AddSeconds(-BeforeAndAfterErrorInSeconds).Ticks;
                    var ErrPeriodEndID = cycleHeader.CycleDateTime.AddSeconds(BeforeAndAfterErrorInSeconds).Ticks;
                    var ErrCycles = AllCycles.FindAll(c => c.CycleID >= ErrPeriodStartID && c.CycleID <= ErrPeriodEndID && c.IsSocketsGood.Any(s => !s));
                    if (ErrCycles.Count > 0)
                    {
                        toMove.Add(cycleHeader);
                        continue;
                    }
                    toRemove.Add(cycleHeader);
                    AllCycles.Remove(cycleHeader);
                }

                foreach (var cycleMove in toMove)
                {
                    if (token.IsCancellationRequested) return;

                    WorkingLog?.Add(LoggerLevel.Information, "Перенос съема от " + cycleMove.CycleDateTime.ToString("dd-MM-yyyy HH\\:mm\\:ss"));
                    try
                    {
                        var localfilename = LocalStorage.GetFileName(cycleMove.CycleID);
                        var remotePath = RemoteStorage.GetPathForDate(cycleMove.CycleDateTime);
                        if (!Directory.Exists(remotePath))
                            Directory.CreateDirectory(remotePath);
                        File.Move(localfilename, Path.Combine(remotePath, Path.GetFileName(localfilename)));
                    }
                    catch (Exception ex)
                    {
                        WorkingLog?.Add(LoggerLevel.Information, "Ошибка при переносе файлов: " + ex.Message);
                        Observer.Notify($"{this.GetType().Name}.DB.Move.File.Error", ex);
                    }

                }
                foreach (var cycleRemove in toRemove)
                {
                    if (token.IsCancellationRequested) return;
                    LocalStorage.DeleteCycleByIDAndIgnoreErrors(cycleRemove.CycleID);
                    WorkingLog?.Add(LoggerLevel.Information, "Удаление съема от " + cycleRemove.CycleDateTime.ToString("dd-MM-yyyy HH\\:mm\\:ss"));
                }
                HashSet<string> pathDatesToCheck = new HashSet<string>();
                HashSet<string> pathMonthesToCheck = new HashSet<string>();
                foreach (var cycle in toMove)
                {
                    var pathElemets = LocalStorage.GetPathElemetsForDateTime(cycle.CycleDateTime);
                    var dir1 = Path.Combine(LocalStorage.DBDirectory, pathElemets[0], pathElemets[1]);
                    var dir2 = Path.Combine(LocalStorage.DBDirectory, pathElemets[0]);
                    if (!pathDatesToCheck.Contains(dir1)) { pathDatesToCheck.Add(dir1); }
                    if (!pathMonthesToCheck.Contains(dir2)) { pathMonthesToCheck.Add(dir2); }
                }
                var shift = new Shift();
                foreach (var cycle in toRemove)
                {
                    if (new Shift(cycle.CycleDateTime).ShiftDate == shift.ShiftDate) continue;
                    var pathElemets = LocalStorage.GetPathElemetsForDateTime(cycle.CycleDateTime);
                    var dir1 = Path.Combine(LocalStorage.DBDirectory, pathElemets[0], pathElemets[1]);
                    var dir2 = Path.Combine(LocalStorage.DBDirectory, pathElemets[0]);
                    if (!pathDatesToCheck.Contains(dir1)) { pathDatesToCheck.Add(dir1); }
                    if (!pathMonthesToCheck.Contains(dir2)) { pathMonthesToCheck.Add(dir2); }
                }
                foreach (var path in pathDatesToCheck)
                {
                    if (token.IsCancellationRequested) return;
                    if (!Directory.EnumerateFileSystemEntries(path).Any())
                    {
                        Directory.Delete(path);
                    }
                }
                foreach (var path in pathMonthesToCheck)
                {
                    if (token.IsCancellationRequested) return;
                    if (!Directory.EnumerateFileSystemEntries(path).Any())
                    {
                        Directory.Delete(path);

                    }
                }
                try
                {
                    var boxes = LocalStorage.GetBoxesBefore(moveFrom);
                    foreach (var box in boxes)
                    {
                        if (token.IsCancellationRequested) return;
                        try
                        {
                            WorkingLog?.Add(LoggerLevel.Information, "Перенос данных о коробе: " + box.CompletedTime.ToString("G"));
                            RemoteStorage.SaveBox(box);
                            LocalStorage.DeleteBox(box.BoxID);
                        }
                        catch (Exception ex)
                        {
                            WorkingLog?.Add(LoggerLevel.Critical, "Ошибка при попытке переноса: " + ex.Message);
                            Observer.Notify($"{this.GetType().Name}.BoxDB.Move.Error", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add(LoggerLevel.Critical, "Ошибка при чтении списка коробов: " + ex.Message);
                    Observer.Notify($"{this.GetType().Name}.BoxDB.Read.Error", ex);
                }
            }
            finally
            {
                LocalStorage.ResetCache();
                RemoteStorage.ResetCache();
                IsMovingToArchive = false;
                IsTerminatingMovingToArchive = false;
            }
        }
    }
}
