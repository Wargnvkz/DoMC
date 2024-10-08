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
    public partial class DoMCSocketOnOffForm : Form
    {
        private int SocketQuantity;
        public bool[] SocketIsOn;

        private Panel[] SocketPanels;
        public DoMCSocketOnOffForm()
        {
            InitializeComponent();
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
            SocketPanels = UserInterfaceControls.CreateSocketStatusPanels(SocketQuantity, ref pnlSockets, SocketChange_Click);
            ShowStatuses();
            return base.ShowDialog();
        }

        private void ShowStatuses()
        {
            UserInterfaceControls.SetSocketStatuses(SocketPanels, SocketIsOn, Color.Green, Color.DarkGray);

        }

        private void SocketChange_Click(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl != null)
            {
                var n = (int)ctrl.Tag;
                SocketIsOn[n - 1] = !SocketIsOn[n - 1];
                ShowStatuses();
            }
        }

        /*private void ddlSocketQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterfaceControls.RemoveSocketStatusPanels(SocketPanels, pnlSockets);
            SocketPanels = UserInterfaceControls.CreateSocketStatusPanels(SocketQuantity, ref pnlSockets);
            var toremove = SocketConfigurations?.Keys.Select(s => s).Where(sn => sn >= SocketPanels.Length).ToList();
            if (toremove != null)
                foreach (var rk in toremove)
                {
                    SocketConfigurations.Remove(rk);
                }

            for (int i = 0; i < SocketPanels.Length; i++)
            {
                SocketPanels[i].Click += DoMCSocketSettingsPanel_Click;
                SocketPanels[i].Tag = i + 1;
                for (int j = 0; j < SocketPanels[i].Controls.Count; j++)
                {
                    SocketPanels[i].Controls[j].Click += DoMCSocketSettingsPanel_Click;
                    SocketPanels[i].Controls[j].Tag = i + 1;
                }
            }
            UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);
        }
        */
        /*private void DoMCSocketSettingsPanel_Click(object sender, EventArgs e)
        {
            var SocketN = (int)((Control)sender).Tag;
            var ss = new DoMCSocketSettingsForm();
            if (SocketConfigurations == null) SocketConfigurations = new Dictionary<int, DoMC.Configuration.CCDSocketConfiguration>();
            if (SocketConfigurations.ContainsKey(SocketN))
            {
                var cfg = SocketConfigurations[SocketN];
                ss.Configuration = cfg;
            }
            if (ss.ShowDialog() == DialogResult.OK)
            {
                var cfg = ss.Configuration;
                SocketConfigurations[SocketN] = cfg;
                UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);
            }
        }*/

        private void bntReset_Click(object sender, EventArgs e)
        {
            if (SocketIsOn == null) return;
            for (int i = 0; i < SocketIsOn.Length; i++) {
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
    }
}
