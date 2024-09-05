using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoMCLib.Classes;

namespace DoMCLib.DB
{

    public class CycleData
    {
        public long CycleID;
        public DateTime CycleDateTime;
        public bool[] IsSocketsGood;
        public bool[] IsSocketActive;
        public List<CycleDataSocket> SocketImages;
        public bool[] SocketsToSave;
        public string TransporterSide;

        public static CycleData ConvertFromCycleImageCCD(CycleImagesCCD ci)
        {
            if (ci.LEDStatusesAdded)
            {
                ci.CycleCCDDateTime = ci.TimeLCBSyncSignalGot;
                for (int ledLineN = 0; ledLineN < ci.LEDStatuses.Length; ledLineN++)
                {
                    var LEDOn = ci.LEDStatuses[ledLineN];
                    if (!LEDOn)
                    {
                        for (int ledNSocket = 0; ledNSocket < 6; ledNSocket++)
                        {
                            var socket = ledNSocket + ledLineN * 8 + 1;
                            ci.IsSocketGood[socket] = true;
                        }
                    }
                }
            }
            var cd = new DoMCLib.DB.CycleData();
            cd.CycleDateTime = ci.CycleCCDDateTime;
            var n = ci.WorkModeImages.Length;
            cd.SocketImages = new List<DoMCLib.DB.CycleDataSocket>();
            for (int i = 0; i < n; i++)
            {
                //cd.SocketImages.Add(new DoMCClasses.DB.CycleDataSocket() { SocketNumber = i + 1, SocketImage = ci.Differences[i] });
                var cds = new DoMCLib.DB.CycleDataSocket()
                {
                    SocketNumber = i + 1,
                    SocketImage = ci.WorkModeImages[i],
                    SocketStandardImage = ci.StandardImage[i],
                    IsSocketActive = ci.SocketsToCheck[i],

                    /*DeviationWindow = ci.ImageProcessParameters != null ? ci.ImageProcessParameters[i].DeviationWindow : 10,
                    MaxDeviation = ci.ImageProcessParameters != null ? ci.ImageProcessParameters[i].MaxDeviation : (short)1000,
                    MaxAverage = ci.ImageProcessParameters != null ? ci.ImageProcessParameters[i].MaxAverage : (short)1000,
                    TopBorder = ci.ImageProcessParameters != null ? ci.ImageProcessParameters[i].TopBorder : 0,
                    BottomBorder = ci.ImageProcessParameters != null ? ci.ImageProcessParameters[i].BottomBorder : 511,
                    LeftBorder = ci.ImageProcessParameters != null ? ci.ImageProcessParameters[i].LeftBorder : 0,
                    RightBorder = ci.ImageProcessParameters != null ? ci.ImageProcessParameters[i].RightBorder : 511*/
                };
                cds.ImageProcessParameters = ci.ImageProcessParameters[i].Clone();
                cd.SocketImages.Add(cds);
            }
            cd.IsSocketsGood = new bool[n];
            cd.IsSocketActive = cd.SocketImages.Select(si => si.IsSocketActive).ToArray();
            Array.Copy(ci.IsSocketGood, 0, cd.IsSocketsGood, 0, n);
            cd.SocketsToSave = ci.SocketsToSave.ToArray();
            switch (ci.TransporterSide)
            {
                case RDPBTransporterSide.Left:
                    cd.TransporterSide = "L";
                    break;
                case RDPBTransporterSide.Right:
                    cd.TransporterSide = "R";
                    break;
                case RDPBTransporterSide.Stoped:
                    cd.TransporterSide = "S";
                    break;
                case RDPBTransporterSide.SensorError:
                    cd.TransporterSide = "E";
                    break;
            }

            return cd;
        }
    }

    public class CycleDataSocket
    {
        public int CycleID;
        public bool IsSocketActive;
        public int SocketNumber;
        public short[,] SocketImage;
        public short[,] SocketStandardImage;
        public byte[] SocketImageCompressed;
        public byte[] SocketStandardImageCompressed;

        /*public int DeviationWindow;
        public short MaxDeviation;
        public short MaxAverage;
        public int TopBorder;
        public int BottomBorder;
        public int LeftBorder;
        public int RightBorder;*/

        public ImageProcessParameters ImageProcessParameters;
    }

    public class Box
    {
        public int BoxID;
        public DateTime CompletedTime;
        public int BadCyclesCount;
        public string TransporterSide = "";
        public Box() { }

        public Box(DoMCLib.Classes.Box box)
        {
            this.CompletedTime = box.CompletedTime;
            this.BadCyclesCount = box.BadCyclesCount;
            switch (box.TransporterSide)
            {
                case RDPBTransporterSide.Left:
                    this.TransporterSide = "L";
                    break;
                case RDPBTransporterSide.Right:
                    this.TransporterSide = "R";
                    break;
                case RDPBTransporterSide.Stoped:
                    this.TransporterSide = "S";
                    break;
                case RDPBTransporterSide.SensorError:
                    this.TransporterSide = "E";
                    break;
                default:
                    this.TransporterSide = "";
                    break;
            }
        }

        public DoMCLib.Classes.Box Convert()
        {
            var box = new DoMCLib.Classes.Box();
            box.BadCyclesCount = this.BadCyclesCount;
            box.CompletedTime = this.CompletedTime;
            switch (this.TransporterSide)
            {
                case "L":
                    box.TransporterSide = RDPBTransporterSide.Left;
                    break;
                case "R":
                    box.TransporterSide = RDPBTransporterSide.Right;
                    break;
                case "S":
                    box.TransporterSide = RDPBTransporterSide.Stoped;
                    break;
                default:
                    box.TransporterSide = RDPBTransporterSide.SensorError;
                    break;
            }
            return box;
        }

        public string TransporterSideToString()
        {
            switch (this.TransporterSide)
            {
                case "L":
                    return "Левый";
                case "R":
                    return "Правый";
                case "S":
                    return "Стоит";
                case "E":
                    return "Ошибка";
                default:
                    return "Неизвестно";
            }

        }
    }

}
