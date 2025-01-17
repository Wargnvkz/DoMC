using DoMC.Tools;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMC.Forms
{
    public partial class DoMCSocketOnOffForm : Form
    {
        private int SocketQuantity;
        public bool[] SocketIsOn;

        private Panel[] SocketPanels;
        int[][] cards = new int[12][];
        CardNameDataGridViewClass[] CardsNames = new CardNameDataGridViewClass[12];
        public DoMCSocketOnOffForm(DoMCLib.Classes.DoMCApplicationContext context)
        {
            InitializeComponent();
            SocketQuantity = context.Configuration.HardwareSettings.SocketQuantity;
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = new int[8];
            }
            for (int i = 0; i < SocketQuantity; i++)
            {
                var cs = new TCPCardSocket(context.EquipmentSocket2CardSocket[i]);
                cards[cs.CCDCardNumber][cs.InnerSocketNumber] = i;
            }
            for (int i = 0; i < cards.Length; i++)
            {
                CardsNames[i] = new CardNameDataGridViewClass() { CardName = $"Плата {i + 1}" };
            }
            dgvCardNumbers.DataSource = CardsNames;
            foreach (DataGridViewColumn col in dgvCardNumbers.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Padding = new Padding(0);
            }
            SocketPanels = UserInterfaceControls.CreateSocketStatusPanels(SocketQuantity, ref pnlSockets, SocketChange_Click);
        }

        public new DialogResult ShowDialog()
        {
            if (SocketIsOn == null)
            {
                SocketQuantity = 96;
                SocketIsOn = new bool[SocketQuantity];
            }
            else
            {
                SocketQuantity = SocketIsOn.Length;
            }
            lblSocketQuantity.Text = SocketQuantity.ToString();
            //SocketPanels = UserInterfaceControls.CreateSocketStatusPanels(SocketQuantity, ref pnlSockets, SocketChange_Click);
            ShowStatuses();
            return base.ShowDialog();
        }

        private void ShowStatuses()
        {
            UserInterfaceControls.SetSocketStatuses(SocketPanels, SocketIsOn, Color.Green, Color.DarkGray);

        }

        private void SocketChange_Click(object? sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl != null)
            {
                var n = (int)ctrl.Tag;
                SocketIsOn[n] = !SocketIsOn[n];
                ShowStatuses();
            }
        }
        

        private void bntReset_Click(object sender, EventArgs e)
        {
            if (SocketIsOn == null) return;
            for (int i = 0; i < SocketIsOn.Length; i++)
            {
                SocketIsOn[i] = false;
            }
            ShowStatuses();
        }

        private void btnSetAll_Click(object sender, EventArgs e)
        {
            if (SocketIsOn == null) return;
            for (int i = 0; i < SocketIsOn.Length; i++)
            {
                SocketIsOn[i] = true;
            }
            ShowStatuses();

        }

        public class CardNameDataGridViewClass
        {
            public string? CardName { get; set; }
            public string On { get; set; } = "+";
            public string Off { get; set; } = "-";
        }

        private void dgvCardNumbers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        SocketIsOn[cards[e.RowIndex][i]] = true;
                    }
                    ShowStatuses();
                }
                if (e.ColumnIndex == 2)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        SocketIsOn[cards[e.RowIndex][i]] = false;
                    }
                    ShowStatuses();
                }

            }
        }
    }
}
