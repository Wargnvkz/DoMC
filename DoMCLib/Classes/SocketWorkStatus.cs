using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DoMCLib
{
    public class SocketWorkStatus
    {
        //private static Mutex MutexImageDataRead = new Mutex();
        //private static Mutex MutexImageReadTime = new Mutex();


        //public int SocketNumber;
        public SocketReadParameters SocketConfig;
        public CardAndSocketStatus Status;
        public int LastCommandForSocket;
        public int LastCommandReceivedFromSocket;
        public DateTime? LastCommandSendingTime;
        public DateTime? LastCommandReceivingFromSocketTime;
        public bool ActiveInLastCommand;
        public bool SocketIsInUseForCheck;


        public bool IsSocketGoodReady;
        public bool IsImageReadComplete;
        public bool IsReadComplete;
        public bool IsSocketGood;
        public short[,] Image;


        public volatile bool IsSocketReadImageStart;
        public DateTime LastStatusChanged;
        public int CCDTimeSinceLastSynchrosignal;
        public byte[] ImageData;
        public bool ImageIsNotReady = false;
        public int ImageDataRead { get; internal set; }
        public double ImageReadTimeInMs { get; internal set; }

        public static int ProperImageSizeInBytes = 512 * 512 * 2;

        internal void SetStatus(CardAndSocketStatus newStatus)
        {
            Status = newStatus;
            LastStatusChanged = DateTime.Now;
        }

        public short[,] GetImage()
        {
            return Tools.ImageTools.ArrayToImage(ImageData);
        }
        public bool IsImageReady
        {
            get
            {
                return ImageDataRead == ProperImageSizeInBytes;
            }
        }

        public void ResetImage()
        {
            ImageDataRead = 0;
        }

        public void ResetSocketStatistics()
        {
            LastCommandForSocket = 0;
            LastCommandReceivedFromSocket = 0;
            LastCommandSendingTime = DateTime.MinValue;
            LastCommandReceivingFromSocketTime = DateTime.MinValue;
            LastStatusChanged = DateTime.MinValue;
            ActiveInLastCommand = false;
            ImageIsNotReady = false;
            SetStatus(CardAndSocketStatus.Idle);
        }
    }
    public enum CardAndSocketStatus
    {
        Idle,
        WaitForAnswer,
        Processing,
        AnswerComplete,
        AnswerCompleteError
    }
}
