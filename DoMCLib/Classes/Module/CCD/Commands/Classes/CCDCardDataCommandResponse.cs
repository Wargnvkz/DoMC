using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.CCD.Commands.Classes
{
    public class CCDCardDataCommandResponse
    {
        /// <summary>
        /// Используется, чтобы до того, как будет сделан первый запрос выдавать, что все платы работают
        /// </summary>
        public bool FirstRequestSent = false;
        /// <summary>
        /// Платы получившие ответ на команду
        /// </summary>
        public bool[] answered = new bool[12];
        /// <summary>
        /// Платы которым была отправлена команда
        /// </summary>
        public bool[] requested = new bool[12];
        public void Clear()
        {
            Array.Fill(answered, false);
            Array.Fill(requested, false);
            FirstRequestSent = false;
        }
        /// <summary>
        /// Устанавливает номер платы, которой была отправлена команда
        /// </summary>
        /// <param name="i"></param>
        public void SetCardRequested(int i) { FirstRequestSent = true; requested[i] = true; }
        /// <summary>
        /// Устанавливает номер платы, от которой пришел ответ
        /// </summary>
        /// <param name="i"></param>
        public void SetCardAnswered(int i) => answered[i] = true;
        /// <summary>
        /// Список не ответивших плат
        /// </summary>
        /// <returns></returns>
        public List<int> CardsNotAnswered()
        {
            return Enumerable.Range(0, 12).Where(i => requested[i] && !answered[i] || !FirstRequestSent).ToList();
        }
        /// <summary>
        /// список ответивших плат
        /// </summary>
        /// <returns></returns>
        public List<int> CardsAnswered()
        {
            return Enumerable.Range(0, 12).Where(i => requested[i] && answered[i] && FirstRequestSent).ToList();
        }
    }
}
