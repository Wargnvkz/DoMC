#pragma warning disable IDE0063
#pragma warning disable IDE0090
using DoMCModuleControl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoMCModuleControl.Logging
{

    public class BaseFilesLogger : IDisposable, IBaseLogger
    {
        private static DateTime CurrentDate = DateTime.MinValue;
        private readonly static ConcurrentDictionary<string, ConcurrentQueue<string>> MessagesOfModule = new ConcurrentDictionary<string, ConcurrentQueue<string>>();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Task _logTask;
        private ILogger? ExternalLogger;

        public BaseFilesLogger()
        {
            _logTask = Task.Run(WriteMessagesSync);
        }


        private static string GetLogFileName(string ModuleName, DateTime ShiftDate)
        {
            var path = GetPath(ModuleName);

            var filename = Path.Combine(path, $"{ModuleName}_{ShiftDate:yyyyMMdd)}.log");
            return filename;
        }

        private static string GetPath(string ModuleName)
        {
            var path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location ?? ".") ?? ".", "Logs", ModuleName);

            if (!path.EndsWith('\\')) path += '\\';
            Directory.CreateDirectory(path);
            return path;
        }

        private static DateTime GetCurrentShiftDate()
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
                }
            }
            catch (Exception ex) { AddMessage("Logger", $"Ошибка изменения даты:{ex.Message} {ex.StackTrace}"); }
        }

        public void AddMessage(string Module, string Message)
        {
            try
            {
                ChangeDate();
                StringBuilder sb = new StringBuilder();

                sb.Append(DateTime.Now.ToString("dd-MM-yyyy HH\\:mm\\:ss    "));

                sb.Append(Message);
                var msg = sb.ToString();
                lock (MessagesOfModule)
                {
                    if (!MessagesOfModule.ContainsKey(Module))
                    {
                        MessagesOfModule.TryAdd(Module, new ConcurrentQueue<string>());
                    }
                    MessagesOfModule[Module].Enqueue(Message);
                }
            }
            catch (Exception ex) { AddMessage("Logger", $"Ошибка изменения даты:{ex.Message} {ex.StackTrace}"); }

        }

        private void WriteMessagesSync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    foreach (var key in MessagesOfModule.Keys)
                    {
                        using (var File = new StreamWriter(GetLogFileName(key, CurrentDate), true))
                        {
                            while (MessagesOfModule[key].TryDequeue(out string? text))
                            {
                                File.WriteLine(text ?? "");
                            }
                            File.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExternalLogger?.Add(LoggerLevel.Critical, $"Ошибка изменения даты:{ex.Message} {ex.StackTrace}");
                }
                Task.Delay(100).Wait();
            }

        }

        private void StopLog()
        {
            _cancellationTokenSource.Cancel();
            _logTask.Wait();
        }

        public void Dispose()
        {
            StopLog();
            GC.SuppressFinalize(this);
        }

        public void RegisterExternalLogger(ILogger logger)
        {
            ExternalLogger = logger;
        }

    }

}
