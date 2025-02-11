using DoMCLib.Tools;
using System.Runtime.Serialization;

namespace DoMCLib.DB
{
    public partial class FileDB
    {

        [Serializable, DataContract]
        protected internal class CycleData
        {
            [DataMember]
            [FileStorageHeaderID]
            public long CycleID { get; set; }
            [DataMember]
            [FileStorageHeader]
            public DateTime CycleDateTime { get; set; }
            [DataMember]
            [FileStorageHeader]
            public byte[] IsSocketsGood { get; set; }
            [DataMember]
            [FileStorageHeader]
            public byte[] IsSocketActive { get; set; }
            [DataMember]
            public List<CycleDataSocket> SocketImages { get; set; }
            [DataMember]
            [FileStorageHeader]
            public string TransporterSide { get; set; }

            public static CycleData FromDBCycleData(DB.CycleData cd)
            {
                var res = new CycleData();
                res.IsSocketsGood = ArrayTools.BoolArray2ByteArray(cd.IsSocketsGood);
                res.IsSocketActive = ArrayTools.BoolArray2ByteArray(cd.IsSocketActive);

                res.TransporterSide = cd.TransporterSide;
                res.CycleDateTime = cd.CycleDateTime;
                res.CycleID = cd.CycleDateTime.Ticks;
                if (cd.SocketImages != null)
                {
                    res.SocketImages = cd.SocketImages.Where(si => si != null && si.IsSocketActive && si.SocketImage != null && si.SocketStandardImage != null).Select(si => CycleDataSocket.From(si)).ToList();

                }
                return res;
            }
            public static CycleData FromDBCycleDataCompressed(DB.CycleData cd)
            {
                var res = new CycleData();
                res.IsSocketsGood = ArrayTools.BoolArray2ByteArray(cd.IsSocketsGood);
                res.IsSocketActive = ArrayTools.BoolArray2ByteArray(cd.IsSocketActive);

                res.TransporterSide = cd.TransporterSide;
                res.CycleDateTime = cd.CycleDateTime;
                res.CycleID = cd.CycleID;

                if (cd.SocketImages != null)
                {
                    res.SocketImages = cd.SocketImages.Where(si => si != null && si.IsSocketActive && si.SocketImage != null && si.SocketStandardImage != null).Select(si => CycleDataSocket.From(si)).ToList();

                }
                return res;
            }
            public static DB.CycleData ToDBCycleData(CycleData cd)
            {
                var res = new DB.CycleData();
                res.IsSocketsGood = ArrayTools.ByteArray2BoolArray(cd.IsSocketsGood);
                res.IsSocketActive = ArrayTools.ByteArray2BoolArray(cd.IsSocketActive);
                res.TransporterSide = cd.TransporterSide;
                res.CycleDateTime = cd.CycleDateTime;
                res.CycleID = cd.CycleID;

                if (cd.SocketImages != null)
                {
                    res.SocketImages = cd.SocketImages.Where(si => si != null && si.IsSocketActive && si.SocketImageCompressed != null && si.SocketStandardImageCompressed != null).Select(si => CycleDataSocket.ToUncompressed(si)).ToList();
                }
                return res;
            }
            public static DB.CycleData ToDBCycleDataCompressed(CycleData cd)
            {
                var res = new DB.CycleData();
                res.IsSocketsGood = ArrayTools.ByteArray2BoolArray(cd.IsSocketsGood);
                res.IsSocketActive = ArrayTools.ByteArray2BoolArray(cd.IsSocketActive);

                res.TransporterSide = cd.TransporterSide;
                res.CycleDateTime = cd.CycleDateTime;
                res.CycleID = cd.CycleID;

                if (cd.SocketImages != null)
                {
                    res.SocketImages = cd.SocketImages.Where(si => si != null && si.IsSocketActive && si.SocketImageCompressed != null && si.SocketStandardImageCompressed != null).Select(si => CycleDataSocket.ToUncompressed(si)).ToList();

                }
                return res;
            }
        }
    }
}
