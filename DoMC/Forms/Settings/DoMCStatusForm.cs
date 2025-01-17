using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Forms
{
    public partial class DoMCStatusForm : Form
    {
        Dictionary<int, Tuple<int, int>> Sockets = new Dictionary<int, Tuple<int, int>>() { {96,new Tuple<int,int>(6,16)},
                                                                                            { 72, new Tuple<int, int>(6, 12) },
                                                                                            { 48, new Tuple<int, int>(6, 8) },
                                                                                            { 32, new Tuple<int, int>(4, 8) },
                                                                                            { 8, new Tuple<int, int>(2, 4) },
                                                                                            { 1, new Tuple<int, int>(1, 1) },
        };
        public int SocketQuantity = 96;

        Panel[] PanelsOfSockets; 
        int[] SocketErrorStatus;

        public DoMCStatusForm(int socketQuantity=96)
        {
            InitializeComponent();
            SocketQuantity = socketQuantity;
            PanelsOfSockets = new Panel[SocketQuantity];
            SocketErrorStatus = new int[SocketQuantity];
            //PanelsOfSockets= UserInterfaceControls.CreateSocketStatusPanels(socketQuantity, ref pnlSocketStatus);
            //CreateSocketStatusPanels();
        }

        /*private Size GetPanelSocketSize()
        {
            var psz = pnlSocketStatus.Size;
            var wh = Sockets[SocketQuantity];
            var sz = new Size(psz.Width / wh.Item1, psz.Height / wh.Item2);
            return sz;
        }
        private void CreateSocketStatusPanels()
        {
            var wh = Sockets[SocketQuantity];
            var socketsize = GetPanelSocketSize();
            int n = 0;
            for (int x = 0; x < wh.Item1; x++)
            {
                for (int y = 0; y < wh.Item2; y++)
                {
                    var pnl = new Panel();
                    pnl.Top = y * socketsize.Height;
                    pnl.Left = x * socketsize.Width;
                    pnl.Height =socketsize.Height-1;
                    pnl.Width = socketsize.Width-1;
                    var lbl = new Label();
                    lbl.Text = (n + 1).ToString();
                    lbl.Top = 0;
                    lbl.Left = 0;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Height = pnl.Height;
                    lbl.Width = pnl.Width;
                    pnl.Controls.Add(lbl);
                    PanelsOfSockets[n] = pnl;
                    pnlSocketStatus.Controls.Add(pnl);
                    n++;
                }
            }
        }*/

        
        public void SetStatus(bool[] Statuses,bool TrueIsOK=true)
        {
            //UserInterfaceControls.SetSocketStatuses(PanelsOfSockets, Statuses, Color.Green, Color.Red);
            chSocketErrors.Series[0].Points.Clear();
            chSocketErrors.Series[0].MarkerStep = 1;
            for (int i = 0; i < Statuses.Length; i++)
            {
                if (Statuses[i] ^ TrueIsOK)
                {
                    SocketErrorStatus[i]++;
                }
                chSocketErrors.Series[0].Points.AddXY(i+1, SocketErrorStatus[i]);
            }
        }
    }
}
