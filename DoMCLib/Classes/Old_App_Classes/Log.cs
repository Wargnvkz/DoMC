using DoMCModuleControl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Old_App_Classes
{

    public class Log : IDisposable, ILogger
    {
        private static DateTime CurrentDate = DateTime.MinValue;
        public DateTime LastLog = DateTime.MinValue;
        private string ModuleName;
        public static string ErrorMessage = string.Empty;
        public static bool WasError = false;
        private static ConcurrentDictionary<Exception, DateTime> LogExceptions = new ConcurrentDictionary<Exception, DateTime>();
        private static ConcurrentDictionary<string, ConcurrentQueue<string>> MessagesOfModule = new ConcurrentDictionary<string, ConcurrentQueue<string>>();
        private static int ExceptionClearTimeInSeconds = 5;
        Mutex FileMutex = new Mutex();
        private static Timer timer = new Timer(WriteLogByTimer, null, 0, 1000);

        public Log(LogModules module)
        {
            ModuleName = GetModuleName(module);
            ChangeDate();
            CreateMessagesForLogModules();
        }

        private static string GetPrefix(LogModules module)
        {
            return module.ToString();
        }
        private static string GetModuleName(LogModules module)
        {
            return module.ToString();
        }

        private void CreateMessagesForLogModules()
        {
            lock (MessagesOfModule)
            {
                if (!MessagesOfModule.ContainsKey(ModuleName))
                {
                    MessagesOfModule.TryAdd(ModuleName, new ConcurrentQueue<string>());
                }
            }
        }

        public static string GetLogFileName(string ModuleName, string LogPrefix, DateTime ShiftDate)
        {
            var path = GetPath(ModuleName);

            var filename = Path.Combine(path, $"{LogPrefix}_{ShiftDate.ToString("yyyyMMdd")}.log");
            return filename;
        }
        public static string GetLogFileName(LogModules module, DateTime ShiftDate)
        {
            return GetLogFileName(GetModuleName(module), GetPrefix(module), ShiftDate);
        }
        public static string GetPath(string ModuleName)
        {
            var path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Logs", ModuleName);

            if (!path.EndsWith("\\")) path = path + "\\";
            Directory.CreateDirectory(path);
            return path;
        }
        public static string GetPath(LogModules module)
        {
            return GetPath(GetModuleName(module));
        }

        public static DateTime GetCurrentShiftDate()
        {
            var date = DateTime.Now.Date;
            if (DateTime.Now.TimeOfDay < new TimeSpan(8, 0, 0))
            {
                date = date.AddDays(-1);
            }
            return date;
        }
        private void ChangeDate()
        {
            try
            {
                var date = GetCurrentShiftDate();
                if (date != CurrentDate)
                {
                    CurrentDate = date;
                    /*var filename = GetLogFileName(ModuleName, Prefix, CurrentDate);
                    var File = new StreamWriter(GetLogFileName(ModuleName, Prefix, CurrentDate), true);
                    File.AutoFlush = true;*/
                }
                WasError = false;
            }
            catch (Exception ex) { WasError = true; ErrorMessage = ex.Message; }
        }

        public void Add(string Message, bool ShowDateTime = true)
        {
            try
            {
                ChangeDate();
                StringBuilder sb = new StringBuilder();
                LastLog = DateTime.Now;
                if (ShowDateTime)
                {
                    sb.Append(LastLog.ToString("dd-MM-yyyy HH\\:mm\\:ss    "));
                }
                sb.Append(Message);
                var msg = sb.ToString();
                WriteTextAsync(msg);
                WasError = false;
            }
            catch (Exception ex) { WasError = true; ErrorMessage = ex.Message; }

        }

        public void Add(Exception ex, int loglevel = 0)
        {
            try
            {
                ChangeDate();
                StringBuilder sb = new StringBuilder();
                LastLog = DateTime.Now;
                sb.Append(LastLog.ToString("dd-MM-yyyy HH\\:mm\\:ss    "));
                sb.Append(ex.Message);
                sb.Append(ex.StackTrace);
                var msg = sb.ToString();
                WriteTextAsync(msg);
                WasError = false;
            }
            catch (Exception ex1) { WasError = true; ErrorMessage = ex1.Message; }

        }
        public void Add(string Message, Exception ex, int loglevel = 0)
        {
            try
            {
                ChangeDate();
                StringBuilder sb = new StringBuilder();
                LastLog = DateTime.Now;
                sb.Append(LastLog.ToString("dd-MM-yyyy HH\\:mm\\:ss    "));
                sb.Append(Message);
                sb.Append(ex.Message);
                sb.Append(ex.StackTrace);
                var msg = sb.ToString();
                WriteTextAsync(msg);
                WasError = false;
            }
            catch (Exception ex1) { WasError = true; ErrorMessage = ex1.Message; }

        }
        public void Add(string Message, byte[] packet, bool ShowDateTime = true)
        {
            try
            {
                ChangeDate();
                StringBuilder sb = new StringBuilder();
                LastLog = DateTime.Now;
                if (ShowDateTime)
                {
                    sb.Append(LastLog.ToString("dd-MM-yyyy HH\\:mm\\:ss    "));
                }
                sb.Append(Message);
                sb.Append("<");
                sb.Append(string.Join(", ", packet.Select(b => "0x" + b.ToString("X2"))));
                sb.Append(">");
                var msg = sb.ToString();
                WriteTextAsync(msg);
                WasError = false;
            }
            catch (Exception ex) { WasError = true; ErrorMessage = ex.Message; }

        }

        private void WriteTextAsync(string text)
        {
            MessagesOfModule[ModuleName].Enqueue(text);
            //WriteTextSync(text);
            //var task = new Task(new Action(() => WriteMessagesSync()));
            //task.Start();
        }

        private static void WriteLogByTimer(object state)
        {
            var task = new Task(new Action(() => WriteMessagesSync()));
            task.Start();
        }

        private static void WriteMessagesSync()
        {
            //FileMutex.WaitOne();
            try
            {
                foreach (var key in MessagesOfModule.Keys)
                {
                    using (var File = new StreamWriter(GetLogFileName(key, key, CurrentDate), true))
                    {
                        while (MessagesOfModule[key].TryDequeue(out string text))
                        {
                            File.WriteLine(text);
                        }
                        File.Flush();
                    }
                }
            }
            catch { }
            //FileMutex.ReleaseMutex();

        }
        public static string ByteArrayToHexString(byte[] data)
        {
            return string.Join(", ", data.Select(d => "0x" + d.ToString("X2")));
        }

        public void StopLog()
        {
            StopLog(null, null);
        }
        public void StopLog(object sender, EventArgs e)
        {
        }

        public void Dispose()
        {
            StopLog();
        }

        public enum LogModules
        {
            MainSystem,
            LCB,
            RDPB,
            DB,
            ArchiveDB,
            Settings,
            CCDPackets,
            SocketStatus,
            DataStorage,
            Others

        }

        public static void ClearStoredExceptions()
        {
            lock (LogExceptions)
            {
                var ToRemove = new List<Exception>();
                foreach (var ex in LogExceptions)
                {
                    if (ex.Value.AddSeconds(ExceptionClearTimeInSeconds) < DateTime.Now)
                    {
                        ToRemove.Add(ex.Key);
                    }
                }
                ToRemove.ForEach(r => LogExceptions.TryRemove(r, out DateTime _));
            }
        }

        public void Add(LoggerLevel level, string Message, bool ShowDateTime = true)
        {
            try
            {
                ChangeDate();
                StringBuilder sb = new StringBuilder();
                LastLog = DateTime.Now;
                sb.Append(LastLog.ToString("dd-MM-yyyy HH\\:mm\\:ss    "));
                sb.Append(Message);
                var msg = sb.ToString();
                WriteTextAsync(msg);
                WasError = false;
            }
            catch (Exception ex1) { WasError = true; ErrorMessage = ex1.Message; }
        }

        public void Add(LoggerLevel level, string Message, Exception ex)
        {
            try
            {
                ChangeDate();
                StringBuilder sb = new StringBuilder();
                LastLog = DateTime.Now;
                sb.Append(LastLog.ToString("dd-MM-yyyy HH\\:mm\\:ss    "));
                sb.Append(Message);
                sb.Append(ex.Message);
                sb.Append(ex.StackTrace);
                var msg = sb.ToString();
                WriteTextAsync(msg);
                WasError = false;
            }
            catch (Exception ex1) { WasError = true; ErrorMessage = ex1.Message; }
        }

        public void SetMaxLogginLevel(LoggerLevel level)
        {
            throw new NotImplementedException();
        }
    }

}
