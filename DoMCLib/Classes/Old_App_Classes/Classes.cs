using DoMCLib.Configuration;
using DoMCLib.Tools;
using DoMCLib.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes
{

    public class DoMCInterfaceDataExchange
    {
        public FullDoMCConfiguration Configuration;
        public DoMCCardsTCP CardsConnection;

        public CCDDataEchangeStatuses CCDDataEchangeStatuses = new CCDDataEchangeStatuses();

        public ModuleCommandStep[] ModuleCommandSteps;
        public ModuleCommandStep OperationOverallStatus;

        public bool IsWorkingModeStarted;
        public bool WasErrorWhileWorked;
        public int CCDReadsFailed = 0;
        public int CCDReadsFailedMax = 5;

        public int InitLCBStatus;
        public ConcurrentQueue<LEDBlockCommand> LEDInCommands = new ConcurrentQueue<LEDBlockCommand>();
        public ConcurrentQueue<LEDBlockCommand> LEDOutCommands = new ConcurrentQueue<LEDBlockCommand>();

        public LEDDataExchangeStatus LEDStatus = new LEDDataExchangeStatus();

        public DoMCTimings Timings = new DoMCTimings();
        //string NetworkInterfaceIPAddress;

        #region Timings
        public Stopwatch PreciseTimer = new Stopwatch();
        private DateTime PreciseTimer0;
        #endregion

        public CycleImagesCCD CurrentCycleCCD;
        /// <summary>
        /// Очередь из картинок циклов, для сохранения в БД
        /// </summary>
        public ConcurrentQueue<CycleImagesCCD> CyclesCCD = new ConcurrentQueue<CycleImagesCCD>();
        /// <summary>
        /// Очередь из насыпанных коробов
        /// </summary>
        public ConcurrentQueue<Box> Boxes = new ConcurrentQueue<Box>();
        //public ConcurrentQueue<CycleLCBDataExchange> CyclesLED = new ConcurrentQueue<CycleLCBDataExchange>();
        public DoMCInterfaceDataExchangeErrors Errors = new DoMCInterfaceDataExchangeErrors();

        public RDPBStatus RDPBCurrentStatus = new RDPBStatus();

        public int MaxDegreeOfParallelism = 16;
        public DataStorage DataStorage = null;
        private Log WorkingLog;

        public void SendCommand(ModuleCommand cmd)
        {
            OnCommand?.Invoke(this, cmd);
        }

        public event DoMCInterfaceDataExchangeCommandEventHandler OnCommand;

        public delegate void DoMCInterfaceDataExchangeCommandEventHandler(DoMCInterfaceDataExchange sender, ModuleCommand command);

        public DoMCInterfaceDataExchange()
        {
            PreciseTimer = new Stopwatch();
            PreciseTimer0 = DateTime.Now;
            PreciseTimer.Start();

            int coreCount = 0;
            try
            {
                var oc = new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get();
                foreach (var item in oc)
                {
                    coreCount += int.Parse(item["NumberOfCores"].ToString());
                }

                var threadCount = Environment.ProcessorCount;
                var threadsToCore = threadCount / coreCount;

                MaxDegreeOfParallelism = threadCount - threadsToCore;
            }
            catch
            {
                MaxDegreeOfParallelism = 16;
            }
            WorkingLog = new Log(Log.LogModules.MainSystem);
            var commands = (ModuleCommand[])Enum.GetValues(typeof(ModuleCommand));
            var maxc = commands.Max(c => (int)c);

            ModuleCommandSteps = new ModuleCommandStep[maxc + 1];
        }
        /*public long GetCurrentTick()
        {
            return PreciseTimer.ElapsedTicks;
        }*/

        public DateTime GetTimeByTick(long tick)
        {
            return PreciseTimer0.AddSeconds(tick * 1e-7);
        }

        public ModuleCommandStep GetModuleCommandStep(ModuleCommand cmd)
        {
            return ModuleCommandSteps[(int)cmd];
        }
        public void SetModuleCommandStep(ModuleCommand cmd, ModuleCommandStep step)
        {
            ModuleCommandSteps[(int)cmd] = step;
        }
        public void ResetAllModuleCommandsStep(ModuleCommandStep step = ModuleCommandStep.None)
        {
            for (int i = 0; i < ModuleCommandSteps.Length; i++)
            {
                ModuleCommandSteps[i] = step;
            }
        }

        public bool IsCCDConfigurationFull
        {
            get
            {
                if (Configuration == null) return false;
                var keys = Configuration.SocketToCardSocketConfigurations.Keys.ToList();
                if (keys.Count == 0) return false;
                var maxsocket = keys.Max();
                var socketq = keys.Count;
                if (maxsocket != socketq || socketq != Configuration.SocketQuantity) return false;
                foreach (var sc in Configuration.SocketToCardSocketConfigurations)
                {
                    if (sc.Value == null) return false;
                    if (sc.Value.FrameDuration < 800 || sc.Value.Exposition == 0) return false;
                }
                if (Configuration.DisplaySockets2PhysicalSockets == null || Configuration.DisplaySockets2PhysicalSockets.GetSocketQuantity() != Configuration.SocketQuantity) return false;
                return true;
            }
        }

        public bool IsLEDConfiguartionFull
        {
            get
            {
                if (Configuration.LCBSettings.DelayLength == 0 || Configuration.LCBSettings.LEDCurrent < 10 ||
                    Configuration.LCBSettings.LEDCurrent > 1000 || Configuration.LCBSettings.PreformLength == 0) return false;
                return true;
            }
        }

        public bool IsConfigurationLoaded
        {
            get
            {
                return this.CCDDataEchangeStatuses.IsConfigurationLoaded &&
                this.CCDDataEchangeStatuses.IsNetworkCardSet &&
                LEDStatus.LСBInitialized &&
                LEDStatus.LCBConfigurationLoaded;
            }
        }
        public bool IsAbleToTest
        {
            get
            {
                return this.CCDDataEchangeStatuses.IsConfigurationLoaded &&
                this.CCDDataEchangeStatuses.IsNetworkCardSet &&
                LEDStatus.LСBInitialized;
            }
        }

        public void CheckForMemory()
        {
            #region checkForMemory
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            ulong freemem = 0;
            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                freemem += Convert.ToUInt64(managementObject["FreePhysicalMemory"]) * 1024;
            }
            var sq = CardsConnection.SocketQuantity;
            UInt64 needForOneCycle = (ulong)sq * 512UL * 512UL * 2UL * 2UL + 16384; //sockets*imagesize*2 +16384(guess as size of class internal data)
            var cyclesleft = freemem / needForOneCycle;
            if (cyclesleft < 10 && CyclesCCD.Count > 0)
            {
                Errors.NotEnoughMemory = true;
                var n = CyclesCCD.Count / 10;
                for (int i = 0; i < n; i++)
                {
                    CyclesCCD.TryDequeue(out CycleImagesCCD ci);
                }
                GC.Collect();
            }
            else
            {
                Errors.NotEnoughMemory = false;
            }
            #endregion
        }

        public void CheckIfAllSocketsGood()
        {
            Parallel.For(0, this.CurrentCycleCCD.WorkModeImages.Length, new ParallelOptions() { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, i =>
            {
                try
                {
                    CheckIfSocketGood(i + 1);
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add($"Ошибка при проверке гнезда {i + 1}. ", ex);
                }
            });
        }
        public void RecalcAllStandards()
        {
            Parallel.For(0, this.CurrentCycleCCD.WorkModeImages.Length, new ParallelOptions() { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, i =>
            {
                try
                {
                    RecalcStandard(i + 1);
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add($"Ошибка при перерасчете эталона гнезда {i + 1}. ", ex);
                }
            });
        }
        public void SetStandardForAllCurrentCCDSockets()
        {
            Parallel.For(0, this.CurrentCycleCCD.WorkModeImages.Length, new ParallelOptions() { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, i =>
            {
                try
                {
                    SetStandardForCurrentCCDSocket(i + 1);
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add($"Ошибка при установке эталона гнезда {i + 1}. ", ex);
                }
            });
        }

        public void CheckIfSocketGood(int socketNumberStartedFrom1)
        {
            try
            {
                var socket0Started = (socketNumberStartedFrom1 - 1);
                var LEDLineNumber = socket0Started / (this.Configuration.SocketQuantity / 12);
                // нужно ли проверять это гнездо, есть ли в нем изображение и был ли включен светодиод в его линейке(или что-то сгорело)
                if (this.Configuration.SocketsToCheck[socket0Started] && this.CurrentCycleCCD.WorkModeImages[socket0Started] != null && (this.CurrentCycleCCD.LEDStatuses == null || this.CurrentCycleCCD.LEDStatuses[LEDLineNumber]))
                {
                    //есть ли конфигурация этого гнезда
                    if (this.Configuration.SocketToCardSocketConfigurations.ContainsKey(socketNumberStartedFrom1))
                    {
                        var socket = this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1];
                        if (socket == null) return;

                        var result = DoMCLib.Tools.ImageTools.CheckIfSocketGood(this.CurrentCycleCCD.WorkModeImages[socket0Started], this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].StandardImage, this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].ImageProcessParameters);

                        this.CurrentCycleCCD.Average = result.Average;
                        this.CurrentCycleCCD.IsSocketGood[socket0Started] = result.IsSocketGood;
                        this.CurrentCycleCCD.MaxDeviation = result.MaxDeviation;
                        this.CurrentCycleCCD.MaxDeviationPoint = result.MaxDeviationPoint;
                        this.CurrentCycleCCD.Differences[socket0Started] = result.ResultImage;
                        this.CurrentCycleCCD.SocketErrorType = result.SocketErrorType;
                        /*
                        //получаем разницу текущего изображения с эталоном
                        var diffImg = ImageTools.GetDifference(this.CurrentCycleCCD.WorkModeImages[socket0Started], this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].StandardImage);

                        //считаем среднее, на случай если отличается цвет, но дефектов нет
                        var average = ImageTools.Average(diffImg, this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].ImageProcessParameters.GetRectangle());
                        this.CurrentCycleCCD.Average = average;

                        //считаем отклонения по волокнам
                        var deviationImg = ImageTools.DeviationByLine(diffImg, this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].ImageProcessParameters.DeviationWindow);
                        this.CurrentCycleCCD.Differences[socket0Started] = deviationImg;

                        //определяем максимальное отклонение в изображении
                        var maxImageDeviation = ImageTools.MaxDeviation(deviationImg, out Point pMaxDev, this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].ImageProcessParameters.GetRectangle());
                        this.CurrentCycleCCD.MaxDeviationPoint = pMaxDev;

                        //если максимальное отклонение больше допустимого, то ошибка в отклонении
                        //если среднее отклоняется больше допустимого, то ошибка в среднем
                        // иначе в этом гнезде хорошая преформа
                        this.CurrentCycleCCD.SocketErrorType =
                            (average < this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].ImageProcessParameters.MaxAverage ? DoMCSocketErrorType.None : DoMCSocketErrorType.Average) |
                            (maxImageDeviation < this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].ImageProcessParameters.MaxDeviation ? DoMCSocketErrorType.None : DoMCSocketErrorType.Deviation);

                        //сохраняем данные о статусе преформы в этом гнезде
                        this.CurrentCycleCCD.IsSocketGood[socket0Started] = this.CurrentCycleCCD.SocketErrorType == DoMCSocketErrorType.None;
                        */

                    }
                }
                else
                {
                    // если нет, то гнездо автоматически считается хорошим
                    this.CurrentCycleCCD.IsSocketGood[socket0Started] = true;
                }
            }
            catch (Exception ex)
            {
                //Если что-то пошло не так, говорим, что ошибка и считаем гнездо хорошим
                this.Errors.ImageProcessError = true;
                this.CurrentCycleCCD.IsSocketGood[(socketNumberStartedFrom1 - 1)] = true;
            }
        }

        public void CheckIfSocketHasImage()
        {
            Parallel.For(0, this.CurrentCycleCCD.WorkModeImages.Length, new ParallelOptions() { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, i =>
            {
                CheckIfSocketHasImage(i + 1);
            });
        }
        public void CheckIfSocketHasImage(int socketNumberStartedFrom1)
        {
            try
            {
                var socket0Started = (socketNumberStartedFrom1 - 1);
                var LEDLineNumber = socket0Started / (this.Configuration.SocketQuantity / 12);
                // нужно ли проверять это гнездо, есть ли в нем изображение и был ли включен светодиод в его линейке(или что-то сгорело)
                if (this.Configuration.SocketsToCheck[socket0Started] && this.CurrentCycleCCD.WorkModeImages[socket0Started] != null && (this.CurrentCycleCCD.LEDStatuses == null || this.CurrentCycleCCD.LEDStatuses[LEDLineNumber]))
                {
                    //есть ли конфигурация этого гнезда
                    if (this.Configuration.SocketToCardSocketConfigurations.ContainsKey(socketNumberStartedFrom1))
                    {
                        var socket = this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1];
                        if (socket == null) return;

                        var result = DoMCLib.Tools.ImageTools.Average(this.CurrentCycleCCD.WorkModeImages[socket0Started], this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].ImageProcessParameters.GetRectangle());

                        this.CurrentCycleCCD.IsSocketHasImage[socket0Started] = result > this.Configuration.AverageToHaveImage;

                    }
                }
                else
                {
                    // если нет, то гнездо автоматически считается хорошим
                    this.CurrentCycleCCD.IsSocketHasImage[socket0Started] = true;
                }
            }
            catch (Exception ex)
            {
                //Если что-то пошло не так, говорим, что ошибка и считаем гнездо хорошим
                this.Errors.ImageProcessError = true;
                this.CurrentCycleCCD.IsSocketHasImage[(socketNumberStartedFrom1 - 1)] = true;
            }

        }

        public void RecalcStandard(int socketNumberStartedFrom1)
        {

            var LEDLineNumber = (socketNumberStartedFrom1 - 1) / (this.Configuration.SocketQuantity / 12);
            if (this.CurrentCycleCCD.IsSocketGood[socketNumberStartedFrom1 - 1] && CurrentCycleCCD.LEDStatuses[LEDLineNumber])
            {
                var newStandard = ImageTools.GetNewStandard(this.CurrentCycleCCD.StandardImage[socketNumberStartedFrom1 - 1], this.CurrentCycleCCD.WorkModeImages[socketNumberStartedFrom1 - 1], this.Configuration.WorkModeSettings.Koefficient);
                if (newStandard != null)
                {
                    this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].StandardImage = newStandard;
                }
            }
        }
        public void SetStandardForCurrentCCDSocket(int socketNumberStartedFrom1)
        {
            var newImg = ImageTools.ImageCopy(this.Configuration.SocketToCardSocketConfigurations[socketNumberStartedFrom1].StandardImage);
            this.CurrentCycleCCD.StandardImage[socketNumberStartedFrom1 - 1] = newImg;
        }


        // How to show and process errors?
        public DoMCInterfaceDataExchangeErrors GetErrors()
        {
            var res = new DoMCInterfaceDataExchangeErrors();

            //Какие ошибки могут появляться
            //настройки +
            //выбор сетевой платы +
            //наличие IP и доступ к порту +
            //ПЗС +
            //БУС +
            //SQL +
            //Память +
            //Ядра
            //Скорость работы (все ли успевает отрабатывать)
            //Бракер
            //внешний модуль

            res.ConfigurationNotSet = !IsCCDConfigurationFull;
            res.NetworkCardError = Errors.NetworkCardError;
            res.NetworkCardHasNoIP = Errors.NetworkCardHasNoIP;
            res.CCDNotRespond = Errors.CCDNotRespond;// (CardsConnection.LastTimeReceived - CardsConnection.LastTimeSent).TotalSeconds < Configuration.Timeouts.WaitForCCDCardAnswerTimeout;//Errors.CCDNotRespond;
            res.UDPError = Errors.UDPError;
            res.LCBDoesNotRespond = Errors.LCBDoesNotRespond;
            res.NoLocalSQL = Errors.NoLocalSQL;
            res.NoRemoteSQL = Errors.NoRemoteSQL;
            res.NotEnoughMemory = Errors.NotEnoughMemory;
            res.CCDMemoryError = Errors.CCDMemoryError;
            res.LCBMemoryError = Errors.LCBMemoryError;
            res.NotEnoughTimeToGetCCD = Errors.NotEnoughTimeToGetCCD;
            res.LEDStatusGettingError = Errors.LEDStatusGettingError;

            res.NotEnoughTimeToProcessData = Errors.NotEnoughTimeToProcessData; // Как сделать тут - непонятно... Если в момент, когда ПЗС прочитала данные, но они еще не начали поступать, а в буфере что-то осталось с предыдущего раза, значит процесс не успевает
            res.NotEnoughTimeToProcessSQL = Errors.NotEnoughTimeToProcessSQL;


            res.RemovingDefectedPreformsBlockError = Errors.RemovingDefectedPreformsBlockError;
            res.RemoteInterfaceModuleError = Errors.RemoteInterfaceModuleError;

            return res;
        }
        // How to show and process errors?

        public void SendResetToCCDCards()
        {
            SendCommand(ModuleCommand.CCDCardsReset);
        }

        public LoadError LoadCCDConfigurationAndStart()
        {
            bool res = false;
            CCDDataEchangeStatuses.IsConfigurationLoaded = false;
            CCDDataEchangeStatuses.IsNetworkCardSet = false;
            LEDStatus.LСBInitialized = false;
            LEDStatus.LCBConfigurationLoaded = false;
            CCDDataEchangeStatuses.FastRead = true;

            if (!IsCCDConfigurationFull) { return LoadError.ConfigurationNotComplete; }

            if (CardsConnection != null)
            {
                CardsConnection.PacketLogActive = Configuration.LogPackets;
            }
            if (Configuration.Timeouts == null) return LoadError.ConfigurationNotComplete;

            SendCommand(ModuleCommand.SetAllCardsAndSocketsConfiguration);
            res = UserInterfaceControls.Wait(Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.SetAllCardsAndSocketsConfiguration && CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete, () => CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
            if (!res)
            {
                CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                return LoadError.CardsAndSocketsConfiguration;

            }
            CCDDataEchangeStatuses.IsNetworkCardSet = true;

            SendCommand(ModuleCommand.CCDStart);
            System.Threading.Thread.Sleep(50);

            SendCommand(ModuleCommand.CCDCardsReset);
            res = UserInterfaceControls.Wait(Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.CCDCardsReset && CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete || CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error, () => CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
            if (!res || CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
            {
                CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                return LoadError.LoadConfiguration;

            }

            SendCommand(ModuleCommand.LoadConfiguration);
            res = UserInterfaceControls.Wait(Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.LoadConfiguration && CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete || CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error, () => CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
            if (!res || CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
            {
                CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                return LoadError.LoadConfiguration;

            }
            CCDDataEchangeStatuses.IsConfigurationLoaded = true;
            return LoadError.None;
        }
        public LoadError LoadLCBConfigurationAndStart(bool WorkingMode)
        {
            bool res = false;
            CCDDataEchangeStatuses.IsConfigurationLoaded = false;
            CCDDataEchangeStatuses.IsNetworkCardSet = false;
            LEDStatus.LСBInitialized = false;
            LEDStatus.LCBConfigurationLoaded = false;

            if (!IsCCDConfigurationFull) { return LoadError.ConfigurationNotComplete; }

            if (CardsConnection != null)
            {
                CardsConnection.PacketLogActive = Configuration.LogPackets;
            }
            if (Configuration.Timeouts == null) return LoadError.ConfigurationNotComplete;

            //InterfaceDataExchange.CardsConnection.PacketLogActive = false;
            SendCommand(ModuleCommand.InitLCB);
            res = UserInterfaceControls.Wait(Configuration.Timeouts.WaitForLCBCardAnswerTimeout, () => InitLCBStatus == 2);
            if (!res)
            {
                CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                return LoadError.InitLCB;
            }
            LEDStatus.LСBInitialized = true;

            if (WorkingMode)
            {
                if (!IsLEDConfiguartionFull) return LoadError.LCBConfigurationNotComplete;
                // Загрузить в БУС параметры работы и перевести в рабочий режим

                SendCommand(ModuleCommand.SetLCBCurrentRequest);
                res = UserInterfaceControls.Wait(Configuration.Timeouts.WaitForLCBCardAnswerTimeout, () => LEDStatus.NumberOfLastCommandSent == 1 && LEDStatus.LastCommandReceivedStatusIsOK);
                if (!res)
                {
                    CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                    return LoadError.LCBCurrentRequest;
                }

                SendCommand(ModuleCommand.SetLCBMovementParametersRequest);
                res = UserInterfaceControls.Wait(Configuration.Timeouts.WaitForLCBCardAnswerTimeout, () => InitLCBStatus == 2, () => CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                if (!res)
                {
                    CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                    return LoadError.LCBMovementParameters;
                }
                LEDStatus.LCBConfigurationLoaded = true;
            }

            return LoadError.None;
        }
        public void StopCCD()
        {
            SendCommand(ModuleCommand.CCDStop);
            System.Threading.Thread.Sleep(200);
        }

        public bool ReconnectToLCB()
        {
            SendCommand(ModuleCommand.LCBStop);
            System.Threading.Thread.Sleep(10);
            SendCommand(ModuleCommand.InitLCB);
            var res = UserInterfaceControls.Wait(Configuration.Timeouts.WaitForLCBCardAnswerTimeout, () => InitLCBStatus == 2);
            if (!res)
            {
                CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                return false;
            }
            return true;
        }

        public void StopLCB()
        {
            SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);
            System.Threading.Thread.Sleep(200);
            SendCommand(ModuleCommand.LCBStop);
            System.Threading.Thread.Sleep(200);
        }
        public enum LoadError
        {
            None,
            NetworkCard,
            ConfigurationNotComplete,
            CardsAndSocketsConfiguration,
            LoadConfiguration,
            LCBConfigurationNotComplete,
            InitLCB,
            LCBCurrentRequest,
            LCBMovementParameters
        }

        public CardStatus[] CardSocketWorkingStatus;
        public bool[] GetIsCardsWorking()
        {
            if (CardSocketWorkingStatus == null) return Enumerable.Repeat(true, 12).ToArray();
            return CardSocketWorkingStatus.Select(s => s == CardStatus.IsWorking).ToArray();
        }

        public enum CardStatus
        {
            IsWorking,
            NoAnswer
        }
        public enum SocketStatus
        {
            NotActive,
            ActiveAndWorking,
            NoAnswer
        }

        public string SocketStandardExist(short[][,] ImageArray)
        {
            if (ImageArray == null) return "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ImageArray.Length; i++)
            {
                if (i % 8 == 0) sb.Append(" ");
                sb.Append(ImageArray[i] != null ? "+" : "-");
            }
            return sb.ToString();
        }
        public string SocketStandardExist()
        {
            var stdImgArr = Enumerable.Range(1, 96).Select(sn => Configuration.SocketToCardSocketConfigurations.ContainsKey(sn) && Configuration.SocketToCardSocketConfigurations[sn].StandardImage != null).ToArray();
            return sBoolArrayToHex(stdImgArr);
            /*StringBuilder sb = new StringBuilder();
            var keys = Configuration.SocketToCardSocketConfigurations.Keys.OrderBy(k => k);
            int N = 0;
            foreach (var key in keys)
            {
                sb.Append(Configuration.SocketToCardSocketConfigurations[key].StandardImage != null ? "+" : "-");
                N++;
                if (N % 8 == 0) sb.Append(" ");
            }
            return sb.ToString();*/
        }
        public static string sBoolArrayToHex(bool[] array)
        {
            if (array == null || array.Length == 0) return "x";
            StringBuilder sb = new StringBuilder();
            var N = 8;
            var bytes = array.Length / N;
            if (array.Length % N != 0) bytes++;
            for (int b = 0; b < bytes; b++)
            {
                int value = 0;
                for (var i = 0; i < N; i++)
                {
                    var ind = b * N + i;
                    if (ind >= array.Length) break;
                    value |= (array[ind] ? 1 : 0) << i;
                }
                sb.Insert(0, value.ToString("X2"));
            }
            return "0x" + sb.ToString();
        }
        public string BoolArrayToHex(bool[] array)
        {
            return sBoolArrayToHex(array);
        }
        public string FalseBoolIndexPlus1(bool[] array)
        {
            List<int> falseIndex = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (!array[i]) falseIndex.Add(i + 1);
            }
            if (falseIndex.Count == 0)
            {
                return "(" + String.Join(", ", falseIndex) + ")";
            }
            return "";
        }
    }

    public enum ModuleCommandStep
    {
        None,
        Start,
        Processing,
        Complete,
        Error,
    }

    public class CycleImagesCCD
    {
        public DateTime CycleCCDDateTime;
        public DateTime TimeLCBSyncSignalGot;
        public short[][,] WorkModeImages;
        public short[][,] Differences;
        public short[][,] StandardImage;
        public bool[] IsSocketGood; //Хорошая ли преформа
        public bool[] IsSocketHasImage; //Есть ли изображение
        public bool[] SocketsToCheck; //Нужно ли проверять гнездо
        public bool[] LEDStatuses; // горит ли светодиод
        public bool LEDStatusesAdded = false; // прочитаны ли статусы светодиодов
        public bool[] SocketsToSave; // Гнезда, которые надо сохранить. Берется из конфигурации
        private static int DefaultLEDQnt = 12;
        public short MaxDeviation;
        public Point MaxDeviationPoint;
        public short Average;
        public ImageProcessParameters[] ImageProcessParameters;
        public ImageErrorType SocketErrorType;
        public RDPBTransporterSide TransporterSide;

        public void SetLEDStatuses(bool[] LEDStatuses, DateTime TimeLCBSincSignal)
        {
            if (LEDStatuses == null)
            {
                this.LEDStatuses = new bool[DefaultLEDQnt];
                return;
            }
            this.LEDStatuses = new bool[LEDStatuses.Length];
            for (int i = 0; i < LEDStatuses.Length; i++)
            {
                this.LEDStatuses[i] = (LEDStatuses[i]);
            }
            this.TimeLCBSyncSignalGot = TimeLCBSincSignal;
            this.LEDStatusesAdded = true;
        }

        public void SetImageProcessParameters(ImageProcessParameters[] parameters)
        {
            ImageProcessParameters = parameters;
        }
    }

    public class Box
    {
        public int BoxID;
        public DateTime CompletedTime;
        public int BadCyclesCount;
        public RDPBTransporterSide TransporterSide;

    }




    [DataContract]
    public class DoMCGeneralSettings
    {
        [DataMember]
        public int NCycle;
        [DataMember]
        public double StandardPercent;
        public double Koefficient
        {
            get
            {
                return Math.Exp(Math.Log(StandardPercent / 100) / NCycle);
            }
        }
    }

    public class LEDDataExchangeStatus
    {
        public bool LСBInitialized;
        public bool LCBConfigurationLoaded;
        public DateTime TimePreviousSyncSignalGot;
        public DateTime TimeSyncSignalGot;
        public DateTime TimeLEDStatusGot;
        public DateTime InOutStatusGot;
        public bool[] LEDStatuses;
        public int LEDCurrent;
        public int PreformLength;
        public int DelayLength;
        public int MaximumHorizontalStroke;
        public int CurrentHorizontalStroke;

        public bool[] Inputs = new bool[8];
        public bool[] Outputs = new bool[6];
        public bool Magnets;
        public bool Valve;

        public DateTime LastCommandSent;
        public int NumberOfLastCommandSent;
        public DateTime LastCommandResponseReceived;
        public int NumberOfLastCommandReceived;
        public bool LastCommandReceivedStatusIsOK;
        public DateTime LastMovementParametersReceived;

        public DateTime UDPReceived;

        public TimeSpan CycleDuration()
        {
            var _0 = new TimeSpan(0);
            if (TimeSyncSignalGot == DateTime.MinValue || TimePreviousSyncSignalGot == DateTime.MinValue) return _0;
            return TimeSyncSignalGot - TimePreviousSyncSignalGot;

        }
        public void ResetCycleDuration()
        {
            TimeSyncSignalGot = DateTime.MinValue;
            TimePreviousSyncSignalGot = DateTime.MinValue;

        }
    }

    public class DoMCInterfaceDataExchangeErrors
    {
        //Какие ошибки могут появляться
        //настройки
        //выбор сетевой платы
        //наличие IP и доступ к порту +
        //ПЗС
        //БУС +
        //SQL +
        //Память +
        //Скорость работы (все ли успевает отрабатывать)
        //Бракер
        //внешний модуль
        public bool ConfigurationNotSet;
        public bool NetworkCardError; // Общая ошибка сетевой плсаты или драйвера (не получилось найти ее с помощью драйвера N/Win PCap или получить о ней информацию)
        public bool NetworkCardHasNoIP; // не получилось определить IPv4 сетевой платы
        public bool CCDNotRespond; //плата ПЗС не отвечает
        public bool CCDFrameNotReceived; //изображения от платы ПЗС не получены
        public bool UDPError; // ошибка настройки и открытия UDP
        public bool LCBDoesNotRespond; // БУС не отвечает
        public bool LCBDoesNotSendSync; // БУС не послал синхроимульс
        public bool LEDStatusGettingError; // Не получены статусы светодиодов от БУС. Если после синхроимпульса уже есть данные от ПЗС, а статусов нет, то они не передавались.
        public bool UDPReceivesTrash; // По UDP приходит мусор (возможно проблема с БУС)
        public bool NoLocalSQL; // SQL недоступен или не работает
        public bool LocalSQLCycleSaveError;// ошибка при сохранении цикла
        public bool NoRemoteSQL; // SQL недоступен или не работает
        public bool ImageProcessError; //Ошибка при обработке изображений гнезд
        public bool NotEnoughMemory; // недостаточно памяти для хранения данных
        public bool LCBMemoryError; // недостаточно памяти для хранения данных БУС
        public bool CCDMemoryError; // недостаточно памяти при обработке ПЗС
        public bool NotEnoughTimeToGetCCD; // не хватает времени на получение данных от ПЗС
        public bool NotEnoughTimeToProcessData;
        public bool NotEnoughTimeToProcessSQL; // определять по накоплению данных в хранилищах. Среднее время каждого действия за некоторый период?
        public bool RemovingDefectedPreformsBlockError; // Ошибка бракёра
        public bool RemoteInterfaceModuleError;
        public int MissedSyncrosignalCounter = 0;


        public Dictionary<string, bool> ToDictionary()
        {
            var res = new Dictionary<string, bool>();
            res["ConfigurationNotSet"] = ConfigurationNotSet;
            res["NetworkCardError"] = NetworkCardError;
            res["NetworkCardHasNoIP"] = NetworkCardHasNoIP;
            res["CCDNotRespond"] = CCDNotRespond;
            res["CCDFrameNotReceived"] = CCDFrameNotReceived;
            res["UDPError"] = UDPError;
            res["LCBNotRespond"] = LCBDoesNotRespond;
            res["LCBDoesNotSendSync"] = LCBDoesNotSendSync;
            res["UDPReceivesTrash"] = UDPReceivesTrash;
            res["NoLocalSQL"] = NoLocalSQL;
            res["LocalSQLCycleSaveError"] = LocalSQLCycleSaveError;
            res["NoRemoteSQL"] = NoRemoteSQL;
            res["ImageProcessError"] = ImageProcessError;
            res["NotEnoughMemory"] = NotEnoughMemory;
            res["LCBMemoryError"] = LCBMemoryError;
            res["CCDMemoryError"] = CCDMemoryError;
            res["NotEnoughTimeToGetCCD"] = NotEnoughTimeToGetCCD;
            res["LEDStatusGettingError"] = LEDStatusGettingError;
            res["NotEnoughTimeToProcessData"] = NotEnoughTimeToProcessData;
            res["NotEnoughTimeToProcessSQL"] = NotEnoughTimeToProcessSQL;
            res["RemovingDefectedPreformsBlockError"] = RemovingDefectedPreformsBlockError;
            res["RemoteInterfaceModuleError"] = RemoteInterfaceModuleError;

            return res;
        }
        public string KeyToText(string key)
        {
            switch (key)
            {
                case "ConfigurationNotSet": return "Конфигурация не завершена";
                case "NetworkCardError": return "Ошибка сетевой карты";
                case "NetworkCardHasNoIP": return "У сетевой карты нет IP адреса";
                case "CCDNotRespond": return "Платы ПЗС не отвечают";
                case "CCDFrameNotReceived": return "Не получены изображения от плат ПЗС";
                case "UDPError": return "Ошибка UDP";
                case "LCBNotRespond": return "БУС не отвечает";
                case "LCBDoesNotSendSync": return "БУС не присылает синхросигнал";
                case "UDPReceivesTrash": return "По UDP приходит мусор";
                case "NoLocalSQL": return "Нет локальной БД";
                case "LocalSQLCycleSaveError": return "Ошибка при сохранении цикла";
                case "NoRemoteSQL": return "Нет удаленной БД";
                case "ImageProcessError": return "Ошибка при обработке изображений гнезд";
                case "NotEnoughMemory": return "Недостаточно памяти";
                case "LCBMemoryError": return "Недостаточно памяти для хранения данных БУС";
                case "CCDMemoryError": return "Недостаточно памяти при обработке ПЗС";
                case "NotEnoughTimeToGetCCD": return "Не хватает времени на получение данных от ПЗС";
                case "LEDStatusGettingError": return "Не получены статусы светодиодов от БУС.";
                case "NotEnoughTimeToProcessData": return "Не хватает времени на обработку данных от ПЗС";
                case "NotEnoughTimeToProcessSQL": return "Не хватает времени на обработку данных SQL";
                case "RemovingDefectedPreformsBlockError": return "Ошибка бракёра";
                case "RemoteInterfaceModuleError": return "Ошибка модуля удаленного интерфейса";

            }
            return "";
        }

    }

    [DataContract]
    public class RemoveDefectedPreformBlockConfig
    {
        [DataMember]
        public string IP;
        [DataMember]
        public int Port = 4001;
        [DataMember]
        public int CoolingBlocksQuantity;
        [DataMember]
        public int MachineNumber = 11;
        [DataMember]
        public bool SendBadCycleToRDPB = false;
        public bool IsConfigReady
        {
            get
            {
                return !String.IsNullOrWhiteSpace(IP) && (Port > 0 && Port < 65536) && (CoolingBlocksQuantity > 2 && CoolingBlocksQuantity < 5);
            }
        }
    }
    public class RemoveDefectedPreformBlock
    {

    }

    public class TimeoutOfActions
    {
        /// <summary>
        /// Время ожидания ответа от плат ПЗС в миллисекундах
        /// </summary>
        public int WaitForCCDCardAnswerTimeout = 30000;
        public int WaitForLCBCardAnswerTimeout = 30000;
        public int WaitForRDPBCardAnswerTimeout = 1000;
        public int DelayBeforeMoveDataToArchiveTimeInSeconds = 3600;
        public int WaitForSynchrosignalTimoutAfterCCDReadingFailed = 10000;
    }



    public class RDPBStatus
    {
        public int MachineNumber;
        public RDPBCommandType CommandType;
        public RDPBCommandType SentCommandType;
        public int CycleNumber;
        public int CoolingBlocksQuantity;
        public int CoolingBlocksQuantityToSet;
        //Ответ на хороший/плохой съем
        public RDPBTransporterSide TransporterSide = RDPBTransporterSide.Stoped;
        public RDPBErrors Errors = RDPBErrors.NoErrors;
        public BoxDirectionType BoxDirection = BoxDirectionType.Left;
        public bool BlockIsOn;
        public int BoxNumber;
        public int SetQuantityInBox;
        public int GoodSetQuantityInBox;
        public int BadSetQuantityInBox;

        public long TimeCurrentStatusGot;
        public long TimeCommandSent;
        public long TimeParametersGot;
        public long TimeLastSent;
        public long TimeLastReceived;
        private long RDPBTimeout;
        public string ManualCommand;
        public bool IsStarted;

        private Stopwatch Timer;

        #region Temporary RDPB data
        public int CurrentBoxDefectCycles;
        public int TotalDefectCycles;
        public RDPBTransporterSide CurrentTransporterSide;
        public RDPBTransporterSide PreviousDirection;
        public int ErrorCounter = 0;
        #endregion
        public RDPBStatus()
        {
            Timer = new Stopwatch();
            Timer.Start();
        }
        public bool ResponseGot
        {
            get
            {
                return TimeLastReceived > TimeLastSent;
            }
        }

        public bool IsTimeout
        {
            get
            {
                return TimeLastSent != 0 && !ResponseGot && (Timer.ElapsedTicks - TimeLastSent) < RDPBTimeout;

            }
        }

        public bool IsCurrentStatusActual()
        {
            return TimeCommandSent < TimeCurrentStatusGot;
        }
        public bool IsParametersActual()
        {
            return TimeCommandSent < TimeParametersGot;
        }

        public void SetTimeout(int Timeoutms)
        {
            RDPBTimeout = Timeoutms * 10000;
        }

        // Данные в сети в тексте по 4 байта в Hex с пробелами

        private string Data2Hex(ushort[] data)
        {
            string[] values = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                var v = data[i];
                var lo = (byte)v;
                var hi = (byte)(v >> 8);
                values[i] = String.Format("{0:X2}{1:X2} ", hi, lo);
            }
            var res = string.Join("", values);
            return res;
        }

        private string CreateHeader(byte machine, RDPBCommandType command)
        {
            var v = (int)command;
            var lo = (char)((byte)v);
            var hi = (char)(v >> 8);
            var res = String.Format("N{0:X1}{1}{2} ", machine, hi, lo);
            return res;
        }

        public static string CalcLRC(string BaseString)
        {
            var sum = 0;
            for (int i = 0; i < BaseString.Length; i++)
            {
                sum += BaseString[i];
            }
            //byte lrc=(byte)(0xff - (byte)sum + 1);
            byte lrc = (byte)(-(byte)sum);
            var res = String.Format("{0:X2}\r\n", lrc);
            return res;
        }

        public new string ToString()
        {
            ushort[] data = new ushort[0];
            switch (CommandType)
            {
                case RDPBCommandType.SetIsOK:
                    {

                    }
                    break;
                case RDPBCommandType.SetIsBad:
                    {

                    }
                    break;
                case RDPBCommandType.On:
                    {

                    }
                    break;
                case RDPBCommandType.Off:
                    {

                    }
                    break;
                case RDPBCommandType.GetParameters:
                    {

                    }
                    break;
                case RDPBCommandType.SetCoolingBlocks:
                    {
                        data = new ushort[1];
                        data[0] = (ushort)CoolingBlocksQuantity;
                    }
                    break;
            }
            var hexdata = Data2Hex(data);
            var header = CreateHeader((byte)MachineNumber, CommandType);
            StringBuilder sb = new StringBuilder();
            sb.Append(header);
            sb.Append(hexdata);
            sb.Append(CalcLRC(sb.ToString()));
            return sb.ToString();
        }

        public string ToResponseString()
        {
            ushort[] data = new ushort[0];
            switch (CommandType)
            {
                case RDPBCommandType.On:
                case RDPBCommandType.Off:
                case RDPBCommandType.SetIsOK:
                case RDPBCommandType.SetIsBad:
                    {
                        data = new ushort[2];
                        data[0] = 1;
                        data[1] = (ushort)((CoolingBlocksQuantity << 12) | ((BlockIsOn ? 1 : 0) << 8) | ((int)(TransporterSide - 0x30) << 4) | ((int)(Errors - 0x30)));
                    }
                    break;
                case RDPBCommandType.GetParameters:
                    {
                        data = new ushort[4];
                        data[0] = 0x2001;
                        data[1] = 1;
                        data[2] = 1;
                        data[3] = 0;
                    }
                    break;
                case RDPBCommandType.SetCoolingBlocks:
                    {
                        data = new ushort[1];
                        data[0] = (ushort)CoolingBlocksQuantity;
                    }
                    break;
            }
            var hexdata = Data2Hex(data);
            var header = CreateHeader((byte)MachineNumber, CommandType);
            StringBuilder sb = new StringBuilder();
            sb.Append(header);
            sb.Append(hexdata);
            sb.Append(CalcLRC(sb.ToString()));
            return sb.ToString();
        }

        public void ChangeFromString(string str)
        {
            if (str.StartsWith("N") && str.EndsWith("\r\n"))
            {
                var lrcindex = str.LastIndexOf(" ");
                if (lrcindex == -1 || lrcindex != str.Length - 5) return;
                var lrcstr = str.Substring(lrcindex + 1, 4).ToUpper();
                var basestring = str.Substring(0, lrcindex + 1);
                var lrcbase = CalcLRC(basestring);
                if (lrcbase != lrcstr) return; // несовпадение контрольной суммы
                var doublespace = basestring.IndexOf("  ");
                if (doublespace != -1) return; // неправильный формат
                var parts = basestring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Any(p => !(p.Length == 4 || p.Length == 0))) return; // неправильный формат
                int.TryParse(parts[0][1].ToString(), System.Globalization.NumberStyles.HexNumber, null, out this.MachineNumber);
                var cmd = (RDPBCommandType)BitConverter.ToUInt16(Encoding.ASCII.GetBytes(parts[0].Substring(2, 2)).Reverse().ToArray(), 0);
                this.CommandType = cmd;
                this.TimeCurrentStatusGot = Timer.ElapsedTicks;
                switch (cmd)
                {
                    case RDPBCommandType.SetIsOK:
                    case RDPBCommandType.SetIsBad:
                    case RDPBCommandType.On:
                    case RDPBCommandType.Off:
                        {
                            if (parts.Length != 3) return;
                            int.TryParse(parts[1], System.Globalization.NumberStyles.HexNumber, null, out this.CycleNumber);
                            int.TryParse(parts[2][0].ToString(), out this.CoolingBlocksQuantity);
                            int.TryParse(parts[2][1].ToString(), out int iBlockIsOn);
                            this.BlockIsOn = Convert.ToBoolean(iBlockIsOn);
                            this.TransporterSide = (RDPBTransporterSide)(parts[2][2]);
                            this.Errors = (RDPBErrors)parts[2][3];
                            // DateTime.Now;

                        }
                        break;
                    case RDPBCommandType.GetParameters:
                        {
                            if (parts.Length != 5) return;
                            switch (parts[1][0])
                            {
                                case '2':
                                    this.BoxDirection = BoxDirectionType.Left;
                                    break;
                                case '4':
                                    this.BoxDirection = BoxDirectionType.Right;
                                    break;
                                default:
                                    this.BoxDirection = BoxDirectionType.Unknown;
                                    break;
                            }
                            int.TryParse(parts[1].Substring(1, 3), System.Globalization.NumberStyles.HexNumber, null, out this.BoxNumber);
                            int.TryParse(parts[2], System.Globalization.NumberStyles.HexNumber, null, out this.SetQuantityInBox);
                            int.TryParse(parts[3], System.Globalization.NumberStyles.HexNumber, null, out this.GoodSetQuantityInBox);
                            int.TryParse(parts[4], System.Globalization.NumberStyles.HexNumber, null, out this.BadSetQuantityInBox);
                            SetTimeParametersGot();

                        }
                        break;
                    case RDPBCommandType.SetCoolingBlocks:
                        {
                            int.TryParse(parts[1].ToString(), out this.CoolingBlocksQuantity);

                        }
                        break;
                }
                return;
            }
            else
            {
                return;
            }
        }
        public void ChangeFromRequestString(string str)
        {
            if (str.StartsWith("N") && str.EndsWith("\r\n"))
            {
                var lrcindex = str.LastIndexOf(" ");
                if (lrcindex == -1 || lrcindex != str.Length - 5) return;
                var lrcstr = str.Substring(lrcindex + 1, 4).ToUpper();
                var basestring = str.Substring(0, lrcindex + 1);
                var lrcbase = CalcLRC(basestring);
                if (lrcbase != lrcstr) return; // несовпадение контрольной суммы
                var doublespace = basestring.IndexOf("  ");
                if (doublespace != -1) return; // неправильный формат
                var parts = basestring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Any(p => !(p.Length == 4 || p.Length == 0))) return; // неправильный формат
                int.TryParse(parts[0][1].ToString(), System.Globalization.NumberStyles.HexNumber, null, out this.MachineNumber);
                var cmd = (RDPBCommandType)BitConverter.ToUInt16(Encoding.ASCII.GetBytes(parts[0].Substring(2, 2)).Reverse().ToArray(), 0);
                this.CommandType = cmd;
                switch (cmd)
                {
                    case RDPBCommandType.SetIsOK:
                    case RDPBCommandType.SetIsBad:
                    case RDPBCommandType.On:
                    case RDPBCommandType.Off:
                        {

                        }
                        break;
                    case RDPBCommandType.GetParameters:
                        {

                        }
                        break;
                    case RDPBCommandType.SetCoolingBlocks:
                        int.TryParse(parts[1], System.Globalization.NumberStyles.HexNumber, null, out CoolingBlocksQuantity);
                        break;
                }
                return;
            }
            else
            {
                return;
            }
        }

        public void SetFromConfiguration(FullDoMCConfiguration cfg)
        {
            CoolingBlocksQuantity = cfg.RemoveDefectedPreformBlockConfig?.CoolingBlocksQuantity ?? 4;
            CoolingBlocksQuantityToSet = cfg.RemoveDefectedPreformBlockConfig?.CoolingBlocksQuantity ?? 4;
        }

        public void SetTimeCurrentStatusGot() { TimeCurrentStatusGot = Timer.ElapsedTicks; }
        public void SetTimeCommandSent() { TimeCommandSent = Timer.ElapsedTicks; }
        public void SetTimeParametersGot() { TimeParametersGot = Timer.ElapsedTicks; }
        public void SetTimeLastSent() { TimeLastSent = Timer.ElapsedTicks; }
        public void SetTimeLastReceived() { TimeLastReceived = Timer.ElapsedTicks; }

    }
    public enum RDPBCommandType
    {
        SetIsOK = 0x3830,
        SetIsBad = 0x3831,
        On = 0x3832,
        Off = 0x3833,
        GetParameters = 0x3930,
        SetCoolingBlocks = 0x4130,

    }
    public enum RDPBTransporterSide
    {
        NotSet = 0,
        Stoped = 0x30,
        Right = 0x31,
        Left = 0x32,
        SensorError = 0x33
    }
    public enum RDPBErrors
    {
        NoErrors = 0x30,
        TransporterDriveUnit = 0x31,
        SensorOfInitialState = 0x32
    }
    public enum BoxDirectionType
    {
        Right,
        Left,
        Unknown
    }

    public class RDPBConfiguration
    {
        public string Address;
        public int Port = 4001;
        public int CoolingBlockQuantity;
    }

    public enum ImageErrorType
    {
        None = 0,
        Average = 1,
        Defect = 2
    }
    public class ImageProcessResult
    {
        public ImageErrorType SocketErrorType;
        public short Average;
        public short[,] ResultImage;
        public Point MaxDeviationPoint;
        public short MaxDeviation;
        public bool IsSocketGood;
        public string ErrorToString()
        {
            List<string> defects = new List<string>();
            if (SocketErrorType.HasFlag(ImageErrorType.Average)) defects.Add("Цвет");
            if (SocketErrorType.HasFlag(ImageErrorType.Defect)) defects.Add("Дефект");
            return String.Join(", ", defects);
        }
    }
    [DataContract]
    public class ImageProcessParameters
    {
        /*[DataMember]
        public int DeviationWindow = 10;
        [DataMember]
        public short MaxDeviation = 1000;
        [DataMember]
        public short MaxAverage = 1000;*/
        [DataMember]
        public int TopBorder = 0;
        [DataMember]
        public int BottomBorder = 511;
        [DataMember]
        public int LeftBorder = 0;
        [DataMember]
        public int RightBorder = 511;
        [DataMember]
        public MakeDecision[] Decisions = new MakeDecision[2];
        public Rectangle GetRectangle()
        {
            return new Rectangle(LeftBorder, TopBorder, RightBorder - LeftBorder, BottomBorder - TopBorder);
        }

        public ImageProcessParameters Clone()
        {
            var ipp = new ImageProcessParameters()
            {
                /*DeviationWindow = this.DeviationWindow,
                MaxDeviation = this.MaxDeviation,
                MaxAverage = this.MaxAverage,*/
                TopBorder = this.TopBorder,
                BottomBorder = this.BottomBorder,
                LeftBorder = this.LeftBorder,
                RightBorder = this.RightBorder
            };
            ipp.Decisions = new MakeDecision[2];
            ipp.Decisions[0] = Decisions?[0]?.Clone() ?? new MakeDecision();
            ipp.Decisions[1] = Decisions?[1]?.Clone() ?? new MakeDecision();
            return ipp;
        }
    }

    [DataContract]
    public class DisplaySockets2PhysicalSockets
    {
        [DataMember]
        private int[] PhysicalSockets; //Индекс - гнездо в UI, значение - физическое гнездо
        private int[] DisplaySockets; // Индекс - физическое гнездо, значение - отоборажаемое в UI гнездо
        public void SetMatrixSize(int matrix)
        {
            PhysicalSockets = new int[matrix + 1];
        }
        public void SetDefaultMatrix(int matrix)
        {
            PhysicalSockets = Enumerable.Range(0, matrix + 1).ToArray();
        }
        public int PhysicalSocketToDisplaySocket(int PhysicalSocket)
        {
            if (DisplaySockets == null)
                FillDisplaySockets();
            return DisplaySockets[PhysicalSocket];
        }

        public int DisplaySocketToPhysicalSocket(int DisplaySocket)
        {
            return PhysicalSockets[DisplaySocket];
        }

        public void SetSocketNumbers(int PhysicalSocket, int DisplaySocket)
        {
            PhysicalSockets[DisplaySocket] = PhysicalSocket;
        }
        public bool FillDisplaySockets()
        {
            if (PhysicalSockets == null || PhysicalSockets.Length == 0) return false;
            DisplaySockets = new int[PhysicalSockets.Length];
            for (int i = 0; i < PhysicalSockets.Length; i++)
            {
                var ph = PhysicalSockets[i];
                if (DisplaySockets[ph] != 0) return false;
                DisplaySockets[ph] = i;
            }
            return true;
        }

        public int[] DisplayToPhysical(int[] displaySockets)
        {
            var result = new int[displaySockets.Length];
            for (int i = 0; i < displaySockets.Length; i++)
            {
                result[i] = PhysicalSockets[displaySockets[i]];
            }
            return result;
        }
        public int[] PhysicalToDisplay(int[] physicalSockets)
        {
            if (DisplaySockets == null)
                FillDisplaySockets();
            var result = new int[physicalSockets.Length];
            for (int i = 0; i < physicalSockets.Length; i++)
            {
                result[i] = DisplaySockets[physicalSockets[i]];
            }
            return result;
        }

        public List<Cards> GetSocketsByCards()
        {
            var result = new List<Cards>();
            for (int cardsocket = 1; cardsocket <= 8; cardsocket++)
            {
                var values = new int[12];
                for (int card = 1; card <= 12; card++)
                {
                    var physicalSocket = (card - 1) * 8 + cardsocket;
                    var displaySocket = PhysicalSocketToDisplaySocket(physicalSocket);
                    values[card - 1] = displaySocket;
                }
                Cards c = new Cards();
                c.Array = values;
                c.Socket = cardsocket;
                result.Add(c);
            }
            return result;
        }

        public void SetSocketsByCards(List<Cards> displaySocketsForPhysicalSockets)
        {
            if (displaySocketsForPhysicalSockets.Count != 8) return;
            SetMatrixSize(96);
            for (int cardsocket = 1; cardsocket <= 8; cardsocket++)
            {
                var dsocket = displaySocketsForPhysicalSockets.Find(ds => ds.Socket == cardsocket);
                if (dsocket == null) continue;
                var values = dsocket.Array;
                for (int card = 1; card <= 12; card++)
                {
                    var physicalSocket = (card - 1) * 8 + cardsocket;
                    SetSocketNumbers(physicalSocket, values[card - 1]);
                }
            }
        }

        public int GetSocketQuantity()
        {
            return PhysicalSockets != null ? PhysicalSockets.Length - 1 : 0;
        }

        public class Cards
        {
            public int Socket { get; set; }
            public int Card1 { get; set; }
            public int Card2 { get; set; }
            public int Card3 { get; set; }
            public int Card4 { get; set; }
            public int Card5 { get; set; }
            public int Card6 { get; set; }
            public int Card7 { get; set; }
            public int Card8 { get; set; }
            public int Card9 { get; set; }
            public int Card10 { get; set; }
            public int Card11 { get; set; }
            public int Card12 { get; set; }

            public int[] Array
            {
                get
                {
                    return new int[] { Card1, Card2, Card3, Card4, Card5, Card6, Card7, Card8, Card9, Card10, Card11, Card12 };
                }
                set
                {
                    Card1 = value[0];
                    Card2 = value[1];
                    Card3 = value[2];
                    Card4 = value[3];
                    Card5 = value[4];
                    Card6 = value[5];
                    Card7 = value[6];
                    Card8 = value[7];
                    Card9 = value[8];
                    Card10 = value[9];
                    Card11 = value[10];
                    Card12 = value[11];
                }
            }
        }
    }

    public class DoMCTimings
    {
        public DateTime CCDStart;
        public DateTime CCDEnd;
        public DateTime CCDGetImagesStarted;
        public DateTime CCDGetImagesEnded;
        public DateTime CCDImagesProcessStarted;
        public DateTime CCDImagesProcessEnded;
        public DateTime CCDEtalonsRecalculateStarted;
        public DateTime CCDEtalonsRecalculateEnded;

    }
    [DataContract]
    public class MakeDecision
    {
        [DataMember]
        public List<DecisionOperation> Operations = new List<DecisionOperation>();
        [DataMember]
        public MakeDecisionAction DecisionAction;
        [DataMember]
        public DecisionActionResult Result;
        [DataMember]
        public short ParameterCompareGoodIfLess;

        public void AddNextOperation(DecisionOperation operation)
        {
            Operations.Add(operation);
        }
        public void InsertOperation(int index, DecisionOperation operation)
        {
            Operations.Insert(index, operation);
        }
        public DecisionOperation GetOperation(int index)
        {
            return Operations[index];
        }
        public void RemoveOperation(int index)
        {
            Operations.RemoveAt(index);
        }
        public bool IsImageGood(short[,] std, short[,] img, ImageProcessParameters ipp, out short[,] ResultImg, out Point MaxCoord)
        {
            short[][,] res;
            if (Operations != null)
            {
                res = new short[][,] { std, img };
                foreach (var op in Operations)
                {
                    res = op.Operation(ipp, res);
                }
            }
            else
            {
                var op = new DecisionOperation() { OperationType = DecisionOperationType.Difference };
                res = new short[][,] { std, img };
                res = op.Operation(ipp, res);
            }
            ResultImg = res[0];
            MaxCoord = new Point(0, 0);
            switch (DecisionAction)
            {
                case MakeDecisionAction.Average:
                    var avg = ImageTools.Average(res[0]);
                    return avg < ParameterCompareGoodIfLess;
                case MakeDecisionAction.Max:
                    var imgarr = res[0].Cast<short>().ToArray();
                    var max = Math.Max(Math.Abs(imgarr.Max()), Math.Abs(imgarr.Min()));
                    var index = Array.FindIndex<short>(imgarr, e => Math.Abs(e) == max);
                    MaxCoord = new Point(index % 512, index / 512);
                    return max < ParameterCompareGoodIfLess;
                default: return true;
            }
        }

        public static List<Tuple<DecisionOperationType, string>> GetDecisionOperationTypeList()
        {
            return new List<Tuple<DecisionOperationType, string>>()
            {
                new Tuple<DecisionOperationType, string>(DecisionOperationType.None,"-"),
                new Tuple<DecisionOperationType, string>(DecisionOperationType.Normalize,"Нормализация"),
                new Tuple<DecisionOperationType, string>(DecisionOperationType.Difference,"Разница"),
                new Tuple<DecisionOperationType, string>(DecisionOperationType.Dispersion,"Дисперсия")
            };
        }
        public static List<Tuple<MakeDecisionAction, string>> GetMakeDecisionActionList()
        {
            return new List<Tuple<MakeDecisionAction, string>>()
            {
                new Tuple<MakeDecisionAction, string>(MakeDecisionAction.Average,"Среднее"),
                new Tuple<MakeDecisionAction, string>(MakeDecisionAction.Max,"Максимум")
            };
        }

        public MakeDecision Clone()
        {
            var md = new MakeDecision();
            md.DecisionAction = DecisionAction;
            md.Result = Result;
            md.ParameterCompareGoodIfLess = ParameterCompareGoodIfLess;
            md.Operations = new List<DecisionOperation>();
            md.Operations.AddRange(Operations.Select(o => o.Clone()));
            return md;
        }
    }
    [DataContract]
    public class DecisionOperation
    {
        [DataMember]
        public DecisionOperationType OperationType;
        [DataMember]
        public short Parameter;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipp"></param>
        /// <param name="img">index 0 - standard, if there are more than one image is passed</param>
        /// <returns></returns>
        public short[][,] Operation(ImageProcessParameters ipp, params short[][,] img)
        {
            switch (OperationType)
            {
                case DecisionOperationType.Dispersion:
                    return new short[][,] { ImageTools.DeviationByLine(img[0], Parameter) };
                case DecisionOperationType.Normalize:
                    var stdarr = img[0].Cast<short>();
                    var max = stdarr.Max();
                    var min = stdarr.Min();
                    var k = (double)Parameter / Math.Max(Math.Abs(max), Math.Abs(min));
                    var res = new short[img.Length][,];
                    for (int i = 0; i < res.Length; i++)
                    {
                        res[i] = ImageTools.Multiply(img[i], k);
                    }
                    return res;

                case DecisionOperationType.Difference:
                    return new short[][,] { ImageTools.GetDifference(img[0], img[1], ipp.GetRectangle()) };
                default: return new short[0][,];
            }
        }
        public DecisionOperation Clone()
        {
            var op = new DecisionOperation();
            op.OperationType = OperationType;
            op.Parameter = Parameter;
            return op;
        }
    }
    public enum DecisionOperationType
    {
        None,
        Normalize,
        Difference,
        Dispersion,
    }
    public enum MakeDecisionAction
    {
        Average,
        Max
    }
    public enum DecisionActionResult
    {
        Defect,
        Color
    }
}
