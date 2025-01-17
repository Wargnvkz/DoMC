using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using static DoMCLib.Classes.Module.CCD.CCDCardDataModule;

namespace DoMCLib.Classes
{
    public class DoMCApplicationContext
    {
        public static string ConfigurationUpdateEventName = "ConfigurationUpdate";
        public const string FileName = "DoMC.cfg";
        public ApplicationConfiguration Configuration { get; set; }
        /// <summary>
        /// Индекс 0 - реальное гнездо 1, то есть при получении физического номера для платы нужно использовать логический номер гнезда(нумерацию с 1 сверху вниз слева направо минус 1)
        /// </summary>
        public int[] EquipmentSocket2CardSocket;
        public bool IsInWorkingMode;
        public DoMCApplicationContext()
        {
            Configuration = new ApplicationConfiguration(FileName);
            Configuration.Load();
            FillEquipmentSocket2CardSocket();
        }

        public List<TCPCardSocket> GetWorkingPhysicalSocket()
        {
            var result = new List<TCPCardSocket>();
            if (Configuration == null) return result;
            FillEquipmentSocket2CardSocket();
            for (int i = 0; i < EquipmentSocket2CardSocket.Length; i++)
            {
                if (Configuration.HardwareSettings.SocketsToCheck[i])
                {
                    var physicalSocket = EquipmentSocket2CardSocket[i];
                    var socket = new TCPCardSocket(physicalSocket);
                    result.Add(socket);
                }
            }
            return result;
        }
        public TCPCardSocket GetWorkingPhysicalSocket(int EquipmentSocket)
        {
            if (Configuration == null) throw new NullReferenceException(nameof(Configuration));
            FillEquipmentSocket2CardSocket();

            var physicalSocket = EquipmentSocket2CardSocket[EquipmentSocket];
            var socket = new TCPCardSocket(physicalSocket);
            return socket;
        }
        public List<int> GetWorkingCards(List<TCPCardSocket> WorkingPhysicalSocket)
        {
            var cards = new HashSet<int>();
            foreach (var socket in WorkingPhysicalSocket)
            {
                cards.Add(socket.CCDCardNumber);
            }
            return cards.ToList();
        }

        public void FillEquipmentSocket2CardSocket()
        {
            var maxSockets = Configuration.HardwareSettings.CardSocket2EquipmentSocket.Max();
            if (maxSockets == 0)
            {
                foreach (int i in Enumerable.Range(1, 96))
                {
                    Configuration.HardwareSettings.CardSocket2EquipmentSocket[i - 1] = i;
                }
            }
            maxSockets = Configuration.HardwareSettings.CardSocket2EquipmentSocket.Max();
            EquipmentSocket2CardSocket = new int[maxSockets];
            for (int i = 0; i < Configuration.HardwareSettings.CardSocket2EquipmentSocket.Length; i++)
            {
                EquipmentSocket2CardSocket[Configuration.HardwareSettings.CardSocket2EquipmentSocket[i] - 1] = i;
            }
        }

        public List<(int, SocketParameters)> GetCardParametersByCardList(List<int> CCDCardList)
        {
            var result = new List<(int, SocketParameters)>();
            for (int card = 0; card < CCDCardList.Count; card++)
            {
                var CardSocket = new TCPCardSocket(CCDCardList[card], 0);
                var EquipmentSocket = Configuration.HardwareSettings.CardSocket2EquipmentSocket[CardSocket.CardSocketNumber()];
                result.Add((CCDCardList[card], Configuration.ReadingSocketsSettings.CCDSocketParameters[EquipmentSocket - 1]));
            }
            return result;
        }
        /// <summary>
        /// Получает список гнезщд на плате и их параметров из физических гнезд
        /// </summary>
        /// <param name="EquipmentSockets">Список номеров физических гнезд матрицы начиная с 0</param>
        /// <returns>(номер гнезда матрицы, плата и гнездо, параметры гнезда</returns>
        public List<(int, TCPCardSocket, SocketParameters)> GetSocketParametersByEquipmentSockets(List<int> EquipmentSockets)
        {
            FillEquipmentSocket2CardSocket();
            var result = new List<(int, TCPCardSocket, SocketParameters)>();
            for (int eqSocket = 0; eqSocket < EquipmentSockets.Count; eqSocket++)
            {
                var cSocket = EquipmentSocket2CardSocket[EquipmentSockets[eqSocket]];
                var CardSocket = new TCPCardSocket(cSocket);
                result.Add((EquipmentSockets[eqSocket], CardSocket, Configuration.ReadingSocketsSettings.CCDSocketParameters[EquipmentSockets[eqSocket]]));
            }
            return result;
        }
        /*
        /// <summary>
        /// Запустить команду для связанного с ней модуля с ожиданием
        /// </summary>
        /// <typeparam name="T1">Тип команды</typeparam>
        /// <typeparam name="T2">Тип возвращаемого значения</typeparam>
        /// <param name="InputData">Входные данные для команды</param>
        /// <param name="TimeoutInSeconds">Таймаут ожидания команды</param>
        /// <param name="LogMessage">Сообщение в лог о начале выполнения команды</param>
        /// <param name="LogMessageLevel">уровень сообщения в лог о начале выполнения команды</param>
        /// <param name="outputData">возвращаемые данные от команды</param>
        /// <returns></returns>
        public static bool WaitForCommand<T1>(IMainController Controller, ILogger WorkingLog, object? InputData, int TimeoutInSeconds, string LogMessage, LoggerLevel LogMessageLevel)
            where T1 : AbstractCommandBase
        {
            WorkingLog.Add(LogMessageLevel, LogMessage);

            var CommandInstance = Controller.CreateCommandInstance(typeof(T1));
            if (CommandInstance == null)
            {
                return false;
            };
            try
            {
                CommandInstance.Wait(InputData, TimeoutInSeconds);
                return CommandInstance.WasCompletedSuccessfully();

            }
            catch (NotImplementedException)
            {
                CommandInstance.ExecuteCommand(InputData);
                Task.Delay(100).Wait();
                return true;
            }
        }
        /// <summary>
        /// Запустить команду для связанного с ней модуля с ожиданием
        /// </summary>
        /// <typeparam name="T1">Тип команды</typeparam>
        /// <typeparam name="T2">Тип возвращаемого значения</typeparam>
        /// <param name="InputData">Входные данные для команды</param>
        /// <param name="TimeoutInSeconds">Таймаут ожидания команды</param>
        /// <param name="LogMessage">Сообщение в лог о начале выполнения команды</param>
        /// <param name="LogMessageLevel">уровень сообщения в лог о начале выполнения команды</param>
        /// <param name="outputData">возвращаемые данные от команды</param>
        /// <returns></returns>
        public static bool WaitForCommand<T1, T2>(IMainController Controller, ILogger WorkingLog, object? InputData, int TimeoutInSeconds, string LogMessage, LoggerLevel LogMessageLevel, out T2? outputData)
            where T1 : AbstractCommandBase
            where T2 : class
        {
            WorkingLog.Add(LogMessageLevel, LogMessage);

            var CommandInstance = Controller.CreateCommandInstance(typeof(T1));
            if (CommandInstance == null)
            {
                outputData = null;
                return false;
            }
            try
            {
                var resultExpositionCommand = CommandInstance.Wait(InputData, TimeoutInSeconds);
                if (resultExpositionCommand == null)
                {
                    outputData = null;
                }
                else
                {
                    outputData = resultExpositionCommand as T2;
                }
                return CommandInstance.WasCompletedSuccessfully();
                // return true;
            }
            catch (NotImplementedException)
            {
                CommandInstance.ExecuteCommand(InputData);
                Task.Delay(100).Wait();
                outputData = null;
                return true;
            }
        }
        /// <summary>
        /// Запустить команду для связанного с ней модуля с ожиданием
        /// </summary>
        /// <typeparam name="T1">Тип команды</typeparam>
        /// <typeparam name="T2">Тип модуля</typeparam>
        /// <typeparam name="T3">Тип возвращаемого значения</typeparam>
        /// <param name="InputData">Входные данные для команды</param>
        /// <param name="TimeoutInSeconds">Таймаут ожидания команды</param>
        /// <param name="LogMessage">Сообщение в лог о начале выполнения команды</param>
        /// <param name="LogMessageLevel">уровень сообщения в лог о начале выполнения команды</param>
        /// <param name="outputData">возвращаемые данные от команды</param>
        /// <returns></returns>
        public static bool WaitForCommand<T1, T2, T3>(IMainController Controller, ILogger WorkingLog, object? InputData, int TimeoutInSeconds, string LogMessage, LoggerLevel LogMessageLevel, out T3? outputData)
            where T1 : AbstractCommandBase
            where T2 : AbstractModuleBase
            where T3 : class
        {
            WorkingLog.Add(LogMessageLevel, LogMessage);

            var CommandInstance = Controller.CreateCommandInstance(typeof(T1), typeof(T2));
            if (CommandInstance == null)
            {
                outputData = null;
                return false;
            }
            try
            {
                var resultExpositionCommand = CommandInstance.Wait(InputData, TimeoutInSeconds);
                outputData = resultExpositionCommand as T3;
                return CommandInstance.WasCompletedSuccessfully();
            }
            catch (NotImplementedException)
            {
                CommandInstance.ExecuteCommand(InputData);
                Task.Delay(100).Wait();
                outputData = null;
                return true;
            }
        }*/
        public bool StartCCD(IMainController Controller, ILogger WorkingLog, int? SocketNumber = null)
        {
            if (SocketNumber == null)
            {
                if (!Controller.CreateCommandInstance(typeof(CCDCardDataModule.StartCommand))
                    .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result))
                {
                    if (result != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    else
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Неизвестная ошибка.");

                    }
                    return false;
                }
            }
            else
            {
                if (!Controller.CreateCommandInstance(typeof(CCDCardDataModule.StartSingleSocketCommand))
                    .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result))
                {
                    if (result != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    else
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Неизвестная ошибка.");

                    }
                    return false;
                }
            }
            return true;
        }
        public bool StopCCD(IMainController Controller, ILogger WorkingLog, int? SocketNumber = null)
        {
            if (SocketNumber == null)
            {
                if (!Controller.CreateCommandInstance(typeof(CCDCardDataModule.StopCommand))
                    .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result))
                {
                    if (result != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    return false;
                }
            }
            else
            {
                if (!Controller.CreateCommandInstance(typeof(CCDCardDataModule.StopSingleSocketCommand))
                    .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result))
                {
                    if (result != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    return false;
                }

            }
            return true;
        }
        public bool LoadCCDConfiguration(IMainController Controller, ILogger WorkingLog, bool WorkingMode, int? SocketNumber = null)
        {
            if (SocketNumber == null)
            {
                {
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек экспозиции гнезд в модуль плат ПЗС");
                    if (!Controller.CreateCommandInstance(typeof(CCDCardDataModule.SetExpositionCommand))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result))

                    //if (!WaitForCommand<CCDCardDataModule.SetExpositionCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Передача настроек экспозиции гнезд в модуль плат ПЗС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
                    {
                        if (result != null)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                        }
                        return false;
                    }
                }
                {
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек чтения гнезд в модуль плат ПЗС");
                    if (!Controller.CreateCommandInstance(typeof(CCDCardDataModule.SetReadingParametersCommand))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result))

                    //if (!WaitForCommand<CCDCardDataModule.SetReadingParametersCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Передача настроек чтения гнезд в модуль плат ПЗС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
                    {
                        if (result != null)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                        }
                        return false;
                    }
                }

            }
            else
            {

                {
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек экспозиции гнезд в модуль плат ПЗС");
                    if (!Controller.CreateCommandInstance(typeof(SetExpositionToSingleSocketCommand))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result))
                    {
                        if (result != null)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                        }
                        return false;
                    }
                }
                {
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек чтения гнезд в модуль плат ПЗС");
                    if (!Controller.CreateCommandInstance(typeof(SetReadingParametersToSingleSocketCommand))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result))
                    {
                        if (result != null)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                        }
                        return false;
                    }
                }

                /*
                if (!WaitForCommand<CCDCardDataModule.SetExpositionToSingleSocketCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, (this, SocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Передача настроек экспозиции гнезд в модуль плат ПЗС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
                {
                    if (result != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    return false;
                }


                if (!WaitForCommand<CCDCardDataModule.SetReadingParametersToSingleSocketCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, (this, SocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Передача настроек чтения гнезд в модуль плат ПЗС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? setReadingParametersResult))
                {
                    if (setReadingParametersResult != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", setReadingParametersResult.CardsNotAnswered())}) не отвечают");
                    }
                    return false;
                }*/
            }
            return true;
        }
        public bool StartLCB(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Подключение к БУС");
            if (!Controller.CreateCommandInstance(typeof(LCBModule.LCBStartCommand))
                .Wait<object>(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out _))
            //if (!WaitForCommand<LCBModule.LCBStartCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Подключение к БУС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
            {
                /*if (result != null)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }*/
            }
            return true;
        }
        public bool StopLCB(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Отключение от БУС");
            if (!Controller.CreateCommandInstance(typeof(LCBModule.LCBStopCommand))
                .Wait<object>(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out _))
            //if (!WaitForCommand<LCBModule.LCBStopCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Отключение от БУС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
            {
                /*if (result != null)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }*/
            }
            return true;
        }

        public bool SetLCBWorkingParameters(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек рабочего режима в БУС");
            if (!Controller.CreateCommandInstance(typeof(LCBModule.SetModuleWorkingParametersCommand))
                .Wait<object>(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out _))
            //if (!WaitForCommand<LCBModule.SetModuleWorkingParametersCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Передача настроек рабочего режима в БУС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
            {
                /*if (result != null)
                {

                }*/
            }

            return true;
        }

        public bool ReadSockets(IMainController Controller, ILogger WorkingLog, bool IsExternalRead, int? socketNumber = null)
        {
            var timeout = Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds;
            if (IsExternalRead)
            {
                if (!socketNumber.HasValue)
                {
                    if(Controller.CreateCommandInstance(typeof(SendReadSocketsWithExternalSignalCommand),typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse _))
                    {
                        return true;
                    }

                    /*if (WaitForCommand<CCDCardDataModule.SendReadSocketsWithExternalSignalCommand, CCDCardDataModule, SocketStandardsImage[]>(Controller, WorkingLog, this, timeout, "", LoggerLevel.Critical, out SocketStandardsImage[] result))
                    {
                        return true;
                    }*/
                }
                else
                {
                    if (Controller.CreateCommandInstance(typeof(SendReadSingleSocketWithExternalSignalCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse _))
                    {
                        return true;
                    }
                    /*if (WaitForCommand<CCDCardDataModule.SendReadSingleSocketWithExternalSignalCommand, CCDCardDataModule, SocketStandardsImage[]>(Controller, WorkingLog, (this, socketNumber.Value), timeout, "", LoggerLevel.Critical, out SocketStandardsImage[] result))
                    {
                        return true;
                    }*/

                }
            }
            else
            {
                if (!socketNumber.HasValue)
                {
                    if (Controller.CreateCommandInstance(typeof(SendReadSocketCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out SetReadingParametersCommandResult _))
                    {
                        return true;
                    }
                    /*if (WaitForCommand<CCDCardDataModule.SendReadSocketCommand, CCDCardDataModule, SocketStandardsImage[]>(Controller, WorkingLog, this, timeout, "", LoggerLevel.Critical, out SocketStandardsImage[] result))
                    {
                        return true;
                    }*/
                }
                else
                {
                    if (Controller.CreateCommandInstance(typeof(SendReadSingleSocketCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse _))
                    {
                        return true;
                    }
                    /*if (WaitForCommand<CCDCardDataModule.SendReadSingleSocketCommand, CCDCardDataModule, SocketStandardsImage[]>(Controller, WorkingLog, (this, socketNumber.Value), timeout, "", LoggerLevel.Critical, out SocketStandardsImage[] result))
                    {
                        return true;
                    }*/
                }
            }
            return false;
        }

        public GetImageDataCommandResponse ReadSocketsImages(IMainController Controller, ILogger WorkingLog, int? socketNumber = null)
        {
            var timeout = Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds;
            if (!socketNumber.HasValue)
            {
                if (Controller.CreateCommandInstance(typeof(GetSocketsImagesDataCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out GetImageDataCommandResponse result))
                {
                    if (result != null)
                    {
                        return result;
                    }
                }

                /*if (WaitForCommand<CCDCardDataModule.GetSocketsImagesDataCommand, CCDCardDataModule, GetImageDataCommandResponse>(Controller, WorkingLog, this, timeout, "", LoggerLevel.Critical, out GetImageDataCommandResponse result))
                {
                    if (result != null)
                    {
                        return result;
                    }
                }*/
            }
            else
            {
                if (Controller.CreateCommandInstance(typeof(GetSocketsImagesDataCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out GetImageDataCommandResponse result))
                {
                    if (result != null)
                    {
                        return result;
                    }
                }
                /*if (WaitForCommand<CCDCardDataModule.GetSingleSocketImageDataCommand, CCDCardDataModule, GetImageDataCommandResponse>(Controller, WorkingLog, (this, socketNumber.Value), timeout, "", LoggerLevel.Critical, out GetImageDataCommandResponse result))
                {
                    if (result != null)
                    {
                        return result;
                    }
                }*/
            }
            return null;
        }
        public bool TestCards(IMainController Controller, ILogger WorkingLog, out CCDCardDataCommandResponse result)
        {
            var timeout = 2;
            return Controller.CreateCommandInstance(typeof(TestConnectionCardsCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);
               // WaitForCommand<CCDCardDataModule.TestConnectionCardsCommand, CCDCardDataModule, CCDCardDataCommandResponse>(Controller, WorkingLog, this, timeout, "", LoggerLevel.Critical, out result);
        }
        public bool ResetCards(IMainController Controller, ILogger WorkingLog, int? EquipmentSocketNumber = null)
        {
            var timeout = Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds;
            if (EquipmentSocketNumber == null)
            {
                return Controller.CreateCommandInstance(typeof(GetSocketsImagesDataCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out GetImageDataCommandResponse result);
                
                //return WaitForCommand<CCDCardDataModule.ResetCardsCommand, CCDCardDataModule, CCDCardDataCommandResponse>(Controller, WorkingLog, this, timeout, "", LoggerLevel.Critical, out CCDCardDataCommandResponse result);
            }
            else
            {
                return Controller.CreateCommandInstance(typeof(ResetCardsSingleSocketCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out GetImageDataCommandResponse result);
                //return WaitForCommand<CCDCardDataModule.ResetCardsSingleSocketCommand, CCDCardDataModule, CCDCardDataCommandResponse>(Controller, WorkingLog, (this, EquipmentSocketNumber), timeout, "", LoggerLevel.Critical, out CCDCardDataCommandResponse result);

            }
        }
        public bool SetFastRead(IMainController Controller, ILogger WorkingLog, int? EquipmentSocketNumber = null)
        {
            var timeout = Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds;
            if (EquipmentSocketNumber == null)
            {
                return Controller.CreateCommandInstance(typeof(SetFastReadCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out GetImageDataCommandResponse result);
                //return WaitForCommand<CCDCardDataModule.SetFastReadCommand, CCDCardDataModule, GetImageDataCommandResponse>(Controller, WorkingLog, this, timeout, "", LoggerLevel.Critical, out GetImageDataCommandResponse result);
            }
            else
            {
                return Controller.CreateCommandInstance(typeof(SetFastReadSingleSocketCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out GetImageDataCommandResponse result);
                //return WaitForCommand<CCDCardDataModule.SetFastReadSingleSocketCommand, CCDCardDataModule, GetImageDataCommandResponse>(Controller, WorkingLog, (this, EquipmentSocketNumber.Value), timeout, "", LoggerLevel.Critical, out GetImageDataCommandResponse result);

            }
        }

        public class ErrorsReadingData
        {
            List<ErrorReadingData> errors = new List<ErrorReadingData>();
            public void Clear()
            {
                errors.Clear();
            }
            public void AddReadingError(ErrorReadingData data)
            {
                errors.Add(data);
            }

            public List<int> ErrorCards()
            {
                var errcards = errors.Select(e => e.CardNumber).Distinct().ToList();
                return errcards;
            }

            public class ErrorReadingData
            {
                public int CardNumber;
                public int SocketNumber;
                public int ReadBytes;
                public ErrorReadingData((int CardNumber, int SocketNumber, int ReadBytes) data)
                {
                    CardNumber = data.CardNumber;
                    SocketNumber = data.SocketNumber;
                    ReadBytes = data.ReadBytes;
                }
            }
        }
    }

}
