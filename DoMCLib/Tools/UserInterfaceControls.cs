using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoMCLib.Tools
{
    public static class UserInterfaceControls
    {
        public static Dictionary<int, Tuple<int, int>> SocketRectSize = new Dictionary<int, Tuple<int, int>>() { {96,new Tuple<int,int>(6,16)},
                                                                                            { 72, new Tuple<int, int>(6, 12) },
                                                                                            { 48, new Tuple<int, int>(6, 8) },
                                                                                            { 32, new Tuple<int, int>(4, 8) },
            {8,new Tuple<int, int>(2,4) },
            {4,new Tuple<int, int>(2,2) },
            {2,new Tuple<int, int>(1,2) },
            {1,new Tuple<int, int>(1,1) }
        };

        public static Size GetPanelSocketSize(Panel pnl, int SocketQuantity)
        {
            var psz = pnl.Size;
            var wh = SocketRectSize[SocketQuantity];
            var sz = new Size(psz.Width / wh.Item1, psz.Height / wh.Item2);
            return sz;
        }
        public static Panel[] CreateSocketStatusPanels(int SocketQuantity, ref Panel MainPanel, EventHandler click_event = null)
        {
            if (!SocketRectSize.ContainsKey(SocketQuantity)) throw new Exception("Неверное количество гнезд - " + SocketQuantity);
            var wh = SocketRectSize[SocketQuantity];
            if (MainPanel == null)
            {
                MainPanel = new Panel();
                MainPanel.Top = 0;
                MainPanel.Left = 0;
                MainPanel.Width = wh.Item1 * 20;
                MainPanel.Height = wh.Item2 * 20;
            }
            foreach (Control ctl in MainPanel.Controls) { ctl.Hide(); }
            MainPanel.Controls.Clear();
            var SubPanels = new Panel[SocketQuantity];
            var socketsize = GetPanelSocketSize(MainPanel, SocketQuantity);
            int n = 0;
            for (int x = 0; x < wh.Item1; x++)
            {
                for (int y = 0; y < wh.Item2; y++)
                {
                    var pnl = new Panel();
                    pnl.Top = y * socketsize.Height;
                    pnl.Left = x * socketsize.Width;
                    pnl.Height = socketsize.Height - 1;
                    pnl.Width = socketsize.Width - 1;
                    pnl.Tag = n + 1;
                    if (click_event != null)
                        pnl.Click += click_event;
                    var lbl = new Label();
                    lbl.Text = (n + 1).ToString();
                    lbl.Top = -1;
                    lbl.Left = 0;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Height = pnl.Height;
                    lbl.Width = pnl.Width;
                    lbl.Tag = n + 1;
                    if (click_event != null)
                        lbl.Click += click_event;
                    pnl.Controls.Add(lbl);
                    pnl.BorderStyle = BorderStyle.FixedSingle;
                    SubPanels[n] = pnl;
                    MainPanel.Controls.Add(pnl);
                    n++;
                }
            }
            return SubPanels;
        }

        public static void RemoveSocketStatusPanels(Panel[] SocketPanels, Panel MainPanel)
        {
            if (SocketPanels != null)
            {
                Array.ForEach(SocketPanels, pnl => MainPanel.Controls.Remove(pnl));
            }
        }

        public static void SetSocketStatuses(Panel[] PanelsOfSockets, bool[] Statuses, Color ColorOK, Color ColorNotOK, bool TrueIsOK = true)
        {
            for (int i = 0; i < PanelsOfSockets.Length; i++)
            {
                if (i >= Statuses.Length) return;
                if (Statuses[i] ^ TrueIsOK)
                {
                    PanelsOfSockets[i].BackColor = ColorNotOK;

                }
                else
                {
                    PanelsOfSockets[i].BackColor = ColorOK;
                }
            }
        }
        public static void SetSocketStatuses(Panel[] PanelsOfSockets, int[] Statuses, params Color[] Colors)
        {
            if (Statuses.Length <= 0 || Colors.Length <= 0) return;
            for (int i = 0; i < PanelsOfSockets.Length; i++)
            {
                if (i >= Statuses.Length) return;
                if (Statuses[i] < 0 || Statuses[i] >= Colors.Length)
                    PanelsOfSockets[i].BackColor = Colors[0];
                else
                {
                    PanelsOfSockets[i].BackColor = Colors[Statuses[i]];
                }
            }
        }

        public static byte[] StringToMac(string mac)
        {
            var bytes = new byte[6];
            var cbytes = mac.Split(':');
            for (int i = 0; i < cbytes.Length; i++)
            {
                if (int.TryParse(cbytes[i], System.Globalization.NumberStyles.HexNumber, null, out int iv))
                    bytes[i] = (byte)iv;
                else
                    bytes[i] = 0;
            }
            return bytes;
        }
        public static string MacToString(byte[] mac)
        {
            return String.Join(":", mac.Select(b => b.ToString("X2")));
        }

        public static bool[] GetListOfSetSocketConfiguration(int SocketQuantity, Dictionary<int, DoMCLib.Configuration.CCDSocketConfiguration> SocketConfigurations)
        {
            bool[] setConfig = new bool[SocketQuantity];
            SocketConfigurations?.Keys.Select(k => k).ToList().ForEach(s => setConfig[s - 1] = true);
            return setConfig;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SocketQuantity"></param>
        /// <param name="SocketConfigurations"></param>
        /// <returns>0 - нет конфигурации или эталона, 1 - есть кофигурация и эталон</returns>
        public static int[] GetIntListOfSetSocketConfiguration(int SocketQuantity, Dictionary<int, DoMCLib.Configuration.CCDSocketConfiguration> SocketConfigurations)
        {
            int[] setConfig = new int[SocketQuantity];

            for (int i = 0; i < SocketQuantity; i++)
            {
                setConfig[i] = 0;
                if (SocketConfigurations.ContainsKey(i+1))
                {
                    if (SocketConfigurations[i + 1].StandardImage != null)
                    {
                        setConfig[i] = 1;
                    }
                }
            }

            return setConfig;
        }

        public static bool[] GetListOfSetStandardSocketConfiguration(int SocketQuantity, Dictionary<int, DoMCLib.Configuration.CCDSocketConfiguration> SocketConfigurations)
        {
            bool[] setConfig = new bool[SocketQuantity];
            SocketConfigurations?.Keys.Select(k => k).ToList().ForEach(s => setConfig[s - 1] = SocketConfigurations[s].StandardImage != null);
            return setConfig;
        }

        public static bool Wait(int msTimeout, Func<bool> ConditionFunc)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < msTimeout)
            {
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
                if (ConditionFunc != null)
                {
                    var res = ConditionFunc.Invoke();
                    if (res)
                        return true;
                }
            }
            return false;
        }
        public static bool Wait(int timeout, Func<bool> ConditionFunc, Func<bool> NegativeCondition)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < timeout)
            {
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
                if (ConditionFunc != null)
                {
                    var res = ConditionFunc.Invoke();
                    if (res)
                        return true;
                }
                if (NegativeCondition != null)
                {
                    var res = NegativeCondition.Invoke();
                    if (res)
                        return false;
                }
            }
            return false;
        }

        public static bool Wait(int timeout, Func<bool> ConditionFunc, Action OnTimeout)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < timeout)
            {
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
                if (ConditionFunc != null)
                {
                    var res = ConditionFunc.Invoke();
                    if (res)
                        return true;
                }
            }
            OnTimeout?.Invoke();
            return false;
        }
        public static void Wait(int timeout, Func<bool> ConditionFunc, Action OnTimeout, Action OnComplete)
        {
            if (OnTimeout == null && OnComplete == null) return;
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < timeout)
            {
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
                if (ConditionFunc != null)
                {
                    var res = ConditionFunc.Invoke();
                    if (res)
                    {
                        OnComplete?.Invoke();
                        return;
                    }

                }
            }
            OnTimeout?.Invoke();
        }
    }

}
