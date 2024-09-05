using DoMCLib.Classes;
using DoMCLib.Tools;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DoMCLib.DB
{
    public partial class FileDB : IDatabase
    {
        public string DBDirectory { get; private set; }
        public string BoxDirectory { get; private set; }

        public string DBExtension = ".cd";
        public string BoxExtension = ".box";

        DateTime LastCached = DateTime.MinValue;
        double CacheTimeoutInSeconds = 60;
        private List<CycleDBFileHeader> _CycleDataFiles;
        private List<BoxFileHeader> _BoxFiles;
        private ILogger? WorkingLog;
        private List<CycleDBFileHeader> CycleDataFiles
        {
            get
            {
                RenewCache();
                return _CycleDataFiles;
            }
        }
        private List<BoxFileHeader> BoxFiles
        {
            get
            {
                RenewCache();
                return _BoxFiles;
            }
        }

        public FileDB(string CycleDBDirectory)
        {
            DBDirectory = CycleDBDirectory;
            BoxDirectory = System.IO.Path.Combine(CycleDBDirectory, "boxes");
        }
        public bool CheckDB(bool RecreateDB)
        {
            try
            {
                RenewCache();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log">null значит логирования не будет</param>
        public void SetLog(ILogger log = null)
        {
            WorkingLog = log;
        }

        private string GetPathForDate(DateTime time)
        {
            var shift = new Shift(time);
            var day = shift.ShiftDate.Day;
            var month = shift.ShiftDate.Month;
            var year = shift.ShiftDate.Year;
            var monthStr = $"{month:D2}.{year:D4}";
            return Path.Combine(DBDirectory, monthStr, $"{day:D2}.{monthStr}");
        }
        protected List<CycleDBFileHeader> GetDBFileHeaderList(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
            List<CycleDBFileHeader> fileList = new List<CycleDBFileHeader>();
            var queuePath = new Queue<string>();
            //foreach (var InnerDirectory in directories)
            while (queuePath.Count > 0)
            {
                var InnerDirectory = queuePath.Dequeue();
                var files = System.IO.Directory.GetFiles(InnerDirectory);
                foreach (var file in files)
                {
                    fileList.Add(new CycleDBFileHeader() { FileName = file, CycleHeader = FileStorage<CycleData>.DecomposeFileName(file) });
                }
                var directories = Directory.EnumerateDirectories(directory);
                foreach (var d in directories)
                    queuePath.Enqueue(d);
            }
            return fileList;
        }
        protected List<BoxFileHeader> GetBoxFileHeaderList(string directory)
        {
            List<BoxFileHeader> fileList = new List<BoxFileHeader>();
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
            var files = System.IO.Directory.GetFiles(directory);
            foreach (var file in files)
            {
                fileList.Add(new BoxFileHeader() { FileName = file, BoxHeader = FileStorage<Box>.DecomposeFileName(file) });
            }
            return fileList;
        }


        private void RenewCache()
        {
            if ((DateTime.Now - LastCached).TotalSeconds > CacheTimeoutInSeconds)
            {
                _CycleDataFiles = GetDBFileHeaderList(DBDirectory);
                _BoxFiles = GetBoxFileHeaderList(BoxDirectory);
                GC.Collect();
            }
        }

        private int GetLastBoxID()
        {
            return BoxFiles == null && BoxFiles.Count > 0 ? BoxFiles.Max(b => b.BoxHeader.BoxID) : 0;
        }

        #region Insert Commands
        public void SaveCycleAndImagesOfActiveSockets(DB.CycleData cd)
        {
            //var newid = GetLastDBID() + 1;
            //cd.CycleID = cd.CycleID;
            WorkingLog?.Add(LoggerLevel.FullDetailedInformation, $"Съем {cd.CycleDateTime}. Начало сохранения");
            WorkingLog?.Add(LoggerLevel.FullDetailedInformation, $"Съем {cd.CycleDateTime}. Упаковка изображений");
            var localCD = CycleData.FromDBCycleData(cd);
            WorkingLog?.Add(LoggerLevel.FullDetailedInformation, $"Съем {cd.CycleDateTime}. Сохранение файла");
            //var fn = FileStorage<CycleData>.SaveFile(localCD, DBDirectory, DBExtension);
            var path = GetPathForDate(localCD.CycleDateTime);
            var fn = FileStorage<CycleData>.SaveFile(localCD, path, DBExtension);
            WorkingLog?.Add(LoggerLevel.FullDetailedInformation, $"Съем {cd.CycleDateTime}. Файл сохранен: {fn}");
            localCD.SocketImages = null;
            CycleDataFiles.Add(new CycleDBFileHeader() { FileName = fn, CycleHeader = localCD });
        }

        public void SaveCycleAndCompressedImagesOfActiveSockets(DB.CycleData cd)
        {
            //var newid = GetLastDBID() + 1;
            //cd.CycleID = newid;
            var localCD = CycleData.FromDBCycleDataCompressed(cd);
            //var fn = FileStorage<CycleData>.SaveFile(localCD, DBDirectory, DBExtension);
            var path = GetPathForDate(localCD.CycleDateTime);
            var fn = FileStorage<CycleData>.SaveFile(localCD, path, DBExtension);
            localCD.SocketImages = null;
            CycleDataFiles.Add(new CycleDBFileHeader() { FileName = fn, CycleHeader = localCD });
        }

        public void SaveBox(DB.BoxDB box)
        {
            var newid = GetLastBoxID() + 1;
            box.BoxID = newid;
            var localBox = new Box(box);
            var fn = FileStorage<Box>.SaveFile(localBox, BoxDirectory, BoxExtension);
            BoxFiles.Add(new BoxFileHeader() { FileName = fn, BoxHeader = localBox });
        }
        #endregion

        #region select
        public DB.CycleData GetCycleById(long id)
        {
            var ch = CycleDataFiles.Find(f => f.CycleHeader.CycleID == id);
            if (ch != null)
            {
                var data = FileStorage<CycleData>.OpenFile(ch.FileName);
                return CycleData.ToDBCycleData(data);
            }
            else
            {
                return null;
            }
        }

        public DB.CycleData GetCycleCompressedById(long id)
        {
            var ch = CycleDataFiles.Find(f => f.CycleHeader.CycleID == id);
            if (ch != null)
            {
                WorkingLog?.Add(LoggerLevel.FullDetailedInformation, $"Чтение файла {ch.FileName}");
                var data = FileStorage<CycleData>.OpenFile(ch.FileName);
                WorkingLog?.Add(LoggerLevel.FullDetailedInformation, $"Распаковка файла {ch.FileName}");
                return CycleData.ToDBCycleDataCompressed(data);
            }
            else
            {
                return null;
            }
        }
        public string GetFileName(long id)
        {
            var ch = CycleDataFiles.Find(f => f.CycleHeader.CycleID == id);
            return System.IO.Path.GetFileName(ch.FileName);
        }
        public DB.CycleData GetCycleHeaderById(long id)
        {
            var ch = CycleDataFiles.Find(f => f.CycleHeader.CycleID == id);
            if (ch != null)
            {
                return CycleData.ToDBCycleData(ch.CycleHeader);
            }
            else
            {
                return null;
            }
        }

        public DB.CycleData GetTheLastCycleHeaderBeforeDateTime(DateTime dt)
        {
            var ticks = dt.Ticks;
            var maxIndex = CycleDataFiles.Select((v, ind) => new { Value = v, Index = ind })
                .Where(a => a.Value.CycleHeader.CycleID < ticks)
                .Aggregate((max, b) => (max.Value.CycleHeader.CycleID > b.Value.CycleHeader.CycleID) ? max : b).Index;

            var ch = CycleDataFiles[maxIndex];
            if (ch != null)
            {
                return CycleData.ToDBCycleData(ch.CycleHeader);
            }
            else
                return null;
        }
        public DB.CycleData GetTheFirstCycleHeaderAfterOrEqualDateTime(DateTime dt)
        {
            var ticks = dt.Ticks;
            var maxIndex = CycleDataFiles.Select((v, ind) => new { Value = v, Index = ind })
                .Where(a => a.Value.CycleHeader.CycleID >= ticks)
                .Aggregate((min, b) => (min.Value.CycleHeader.CycleID > b.Value.CycleHeader.CycleID) ? b : min).Index;

            var ch = CycleDataFiles[maxIndex];
            if (ch != null)
            {
                return CycleData.ToDBCycleData(ch.CycleHeader);
            }
            else
                return null;
        }


        public List<DB.CycleData> GetCyclesHeaders(DateTime From, DateTime To)
        {
            var fromTicks = From.Ticks;
            var toTicks = To.Ticks;

            var fileList = CycleDataFiles.Where(cd => cd.CycleHeader.CycleID >= fromTicks && cd.CycleHeader.CycleID <= toTicks).ToList();
            var list = fileList.Select(c => CycleData.ToDBCycleData(c.CycleHeader)).ToList();
            return list;
        }

        public List<DB.CycleData> GetCyclesHeadersBefore(DateTime To)
        {
            var toTicks = To.Ticks;

            var list = CycleDataFiles.Where(cd => cd.CycleHeader.CycleID <= toTicks).Select(c => CycleData.ToDBCycleData(c.CycleHeader)).ToList();
            return list;
        }

        public List<DB.BoxDB> GetBox(DateTime start, DateTime end)
        {

            var fromTicks = start.Ticks;
            var toTicks = end.Ticks;

            var list = BoxFiles.Where(cd => cd.BoxHeader.BoxTicks >= fromTicks && cd.BoxHeader.BoxTicks <= toTicks).Select(c => Box.ToDBBox(c.BoxHeader)).ToList();
            return list;
        }

        public List<DB.BoxDB> GetBoxesBefore(DateTime end)
        {
            var toTicks = end.Ticks;

            var list = BoxFiles.Where(cd => cd.BoxHeader.BoxTicks <= toTicks).Select(c => Box.ToDBBox(c.BoxHeader)).ToList();
            return list;

        }

        #endregion

        #region delete commands

        public void DeleteCycleByID(long id)
        {
            var ch = CycleDataFiles.Find(f => f.CycleHeader.CycleID == id);
            if (ch != null)
            {
                FileStorage<CycleData>.DeleteFile(ch.FileName);
                CycleDataFiles.Remove(ch);
            }
        }
        public void DeleteCycleByIDAndIgnoreErrors(long id)
        {
            var ch = CycleDataFiles.Find(f => f.CycleHeader.CycleID == id);
            if (ch != null)
            {
                FileStorage<CycleData>.DeleteFile(ch.FileName);
                CycleDataFiles.Remove(ch);
            }
        }
        public void DeleteBox(int BoxID)
        {
            var bh = BoxFiles.Find(f => f.BoxHeader.BoxID == BoxID);
            if (bh != null)
            {
                FileStorage<Box>.DeleteFile(bh.FileName);
                BoxFiles.Remove(bh);
            }
        }

        public byte[] GetCycleBinary(DB.CycleData cd)
        {
            var fileH = CycleDataFiles.Find(c => c.CycleHeader.CycleID == cd.CycleID);

            return FileStorage<CycleData>.OpenFileBinary(fileH.FileName);
        }

        public void SetCycleBinary(DB.CycleData cd, byte[] data)
        {
            var cycleHeader = CycleData.FromDBCycleData(cd);
            var filename = FileStorage<CycleData>.CreateFileName(cycleHeader);
            var path = System.IO.Path.Combine(DBDirectory, System.IO.Path.ChangeExtension(filename, DBExtension));
            FileStorage<CycleData>.SaveFileBinary(path, data);
        }

        #endregion
        [Serializable, DataContract]
        protected internal class CycleDataSocket
        {
            [DataMember]
            public bool IsSocketActive;
            [DataMember]
            public int SocketNumber;
            public short[,] SocketImage
            {
                get
                {
                    var arr = ImageTools.ArrayToImage((ImageTools.FromBase64(SocketImageCompressed)));
                    return arr;
                    /*var bSocketImageCompressed = Convert.FromBase64String(SocketImageCompressed);
                    var uncompressed = ImageTools.Decompress(bSocketImageCompressed);
                    var img = ImageTools.ArrayToImage(uncompressed);
                    return img;*/
                }
                set
                {
                    SocketImageCompressed = ImageTools.ToBase64((ImageTools.ImageToArray(value)));

                    /*var imgarr = ImageTools.ImageToArray(value);
                    var bSocketImageCompressed = ImageTools.Compress(imgarr);
                    SocketImageCompressed = Convert.ToBase64String(bSocketImageCompressed);*/
                }
            }
            public short[,] SocketStandardImage
            {
                get
                {
                    var arr = ImageTools.ArrayToImage((ImageTools.FromBase64(SocketStandardImageCompressed)));
                    return arr;

                    /*var bSocketStandardImageCompressed = Convert.FromBase64String(SocketStandardImageCompressed);
                    var uncompressed = ImageTools.Decompress(bSocketStandardImageCompressed);
                    var img = ImageTools.ArrayToImage(uncompressed);
                    return img;*/
                }
                set
                {
                    SocketStandardImageCompressed = ImageTools.ToBase64((ImageTools.ImageToArray(value)));
                    /*var imgarr = ImageTools.ImageToArray(value);
                    var bSocketStandardImageCompressed = ImageTools.Compress(imgarr);
                    SocketStandardImageCompressed = Convert.ToBase64String(bSocketStandardImageCompressed);*/
                }
            }
            //public byte[] SocketImageCompressed;
            //public byte[] SocketStandardImageCompressed;

            [DataMember]
            public string SocketImageCompressed;
            [DataMember]
            public string SocketStandardImageCompressed;

            /*[DataMember]
            public int DeviationWindow;
            [DataMember]
            public short MaxDeviation;
            [DataMember]
            public short MaxAverage;
            [DataMember]
            public int TopBorder;
            [DataMember]
            public int BottomBorder;
            [DataMember]
            public int LeftBorder;
            [DataMember]
            public int RightBorder;*/
            [DataMember]
            public ImageProcessParameters ImageProcessParameters;

            public static CycleDataSocket From(DB.CycleDataSocket si)
            {
                var cdsi = new CycleDataSocket();
                cdsi.IsSocketActive = si.IsSocketActive;
                cdsi.SocketNumber = si.SocketNumber;
                cdsi.ImageProcessParameters = si.ImageProcessParameters.Clone();

                /*cdsi.DeviationWindow = si.DeviationWindow;
                cdsi.MaxDeviation = si.MaxDeviation;
                cdsi.MaxAverage = si.MaxAverage;*/
                /*cdsi.TopBorder = si.TopBorder;
                cdsi.BottomBorder = si.BottomBorder;
                cdsi.LeftBorder = si.LeftBorder;
                cdsi.RightBorder = si.RightBorder;*/
                cdsi.SocketImage = si.SocketImage;
                cdsi.SocketStandardImage = si.SocketStandardImage;
                return cdsi;
            }
            public static DB.CycleDataSocket ToUncompressed(CycleDataSocket si)
            {
                var cdsi = new DB.CycleDataSocket();
                cdsi.IsSocketActive = si.IsSocketActive;
                cdsi.SocketNumber = si.SocketNumber;
                cdsi.ImageProcessParameters = si.ImageProcessParameters?.Clone() ?? new ImageProcessParameters();

                /*cdsi.DeviationWindow = si.DeviationWindow;
                cdsi.MaxDeviation = si.MaxDeviation;
                cdsi.MaxAverage = si.MaxAverage;*/
                /*cdsi.TopBorder = si.TopBorder;
                cdsi.BottomBorder = si.BottomBorder;
                cdsi.LeftBorder = si.LeftBorder;
                cdsi.RightBorder = si.RightBorder;*/
                cdsi.SocketImage = si.SocketImage;
                cdsi.SocketStandardImage = si.SocketStandardImage;
                return cdsi;
            }
            /*public static DB.CycleDataSocket ToCompressed(CycleDataSocket si)
            {
                var cdsi = new DB.CycleDataSocket();
                cdsi.IsSocketActive = si.IsSocketActive;
                cdsi.SocketNumber = si.SocketNumber;

                cdsi.DeviationWindow = si.DeviationWindow;
                cdsi.MaxDeviation = si.MaxDeviation;
                cdsi.MaxAverage = si.MaxAverage;
                cdsi.TopBorder = si.TopBorder;
                cdsi.BottomBorder = si.BottomBorder;
                cdsi.LeftBorder = si.LeftBorder;
                cdsi.RightBorder = si.RightBorder;
                cdsi.SocketImage = si.SocketImage;
                cdsi.SocketStandardImage = si.SocketStandardImage;
                return cdsi;
            }*/

        }

        [Serializable]
        protected internal class Box
        {
            [FileStorageHeader]
            public int BoxID;
            [FileStorageHeaderID]
            public long BoxTicks;
            [FileStorageHeader]
            public DateTime CompletedTime;
            [FileStorageHeader]
            public int BadCyclesCount;
            [FileStorageHeader]
            public string TransporterSide = "";
            public Box() { }
            public Box(DB.BoxDB box)
            {
                BoxID = box.BoxID;
                CompletedTime = box.CompletedTime;
                BadCyclesCount = box.BadCyclesCount;
                TransporterSide = box.TransporterSide;
            }
            public static DB.BoxDB ToDBBox(Box box)
            {
                var res = new DB.BoxDB();
                res.BoxID = box.BoxID;
                res.CompletedTime = box.CompletedTime;
                res.BadCyclesCount = box.BadCyclesCount;
                res.TransporterSide = box.TransporterSide;
                return res;
            }

        }
        protected internal class CycleDBFileHeader
        {
            public string FileName;
            public CycleData CycleHeader;
        }
        protected internal class BoxFileHeader
        {
            public string FileName;
            public Box BoxHeader;
        }
    }
}
