using DoMCLib.Configuration;
using DoMCLib.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using DoMCLib.Classes.Module.LCB;

namespace DoMCLib.Classes
{
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

        public LEDDataExchangeStatus Clone()
        {
            var CopyStatus = new LEDDataExchangeStatus();
            CopyStatus.LСBInitialized = LСBInitialized;
            CopyStatus.LCBConfigurationLoaded = LCBConfigurationLoaded;
            CopyStatus.TimePreviousSyncSignalGot = TimePreviousSyncSignalGot;
            CopyStatus.TimeSyncSignalGot = TimeSyncSignalGot;
            CopyStatus.TimeLEDStatusGot = TimeLEDStatusGot;
            CopyStatus.InOutStatusGot = InOutStatusGot;
            CopyStatus.LEDStatuses = LEDStatuses;
            CopyStatus.LEDCurrent = LEDCurrent;
            CopyStatus.PreformLength = PreformLength;
            CopyStatus.DelayLength = DelayLength;
            CopyStatus.MaximumHorizontalStroke = MaximumHorizontalStroke;
            CopyStatus.CurrentHorizontalStroke = CurrentHorizontalStroke;
            CopyStatus.Inputs = new bool[8];
            Array.Copy(CopyStatus.Inputs, Inputs, Inputs.Length);
            CopyStatus.Outputs = new bool[6];
            Array.Copy(CopyStatus.Outputs, Outputs, Outputs.Length);
            CopyStatus.Magnets = Magnets;
            CopyStatus.Valve = Valve;
            CopyStatus.LastCommandSent = LastCommandSent;
            CopyStatus.NumberOfLastCommandSent = NumberOfLastCommandSent;
            CopyStatus.LastCommandResponseReceived = LastCommandResponseReceived;
            CopyStatus.NumberOfLastCommandReceived = NumberOfLastCommandReceived;
            CopyStatus.LastCommandReceivedStatusIsOK = LastCommandReceivedStatusIsOK;
            CopyStatus.LastMovementParametersReceived = LastMovementParametersReceived;
            CopyStatus.UDPReceived = UDPReceived;
            return CopyStatus;
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
