using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoMCLib.DB
{
    public interface IDatabase
    {
        #region Insert Commands
        void SaveCycleAndImagesOfActiveSockets(CycleData cd);
        void SaveCycleAndCompressedImagesOfActiveSockets(CycleData cd);
        void SaveBox(BoxDB box);
        #endregion

        #region select
        CycleData GetCycleById(long id);

        CycleData GetCycleCompressedById(long id);
        CycleData GetCycleHeaderById(long id);
        CycleData GetTheLastCycleHeaderBeforeDateTime(DateTime dt);
        CycleData GetTheFirstCycleHeaderAfterOrEqualDateTime(DateTime dt);
        List<CycleData> GetCyclesHeaders(DateTime From, DateTime To);

        List<CycleData> GetCyclesHeadersBefore(DateTime To);

        List<BoxDB> GetBox(DateTime start, DateTime end);
        List<BoxDB> GetBoxesBefore(DateTime end);

        byte[] GetCycleBinary(CycleData cd);
        void SetCycleBinary(CycleData cd, byte[] data);

        #endregion

        #region delete commands

        void DeleteCycleByID(long id);
        void DeleteCycleByIDAndIgnoreErrors(long id);
        /*void DeleteSocketImages(int CycleID, int SocketNumber);
        void DeleteSocketImages(int CycleID, int[] SocketNumber);*/
        void DeleteBox(int BoxID);

        #endregion
    }
}
