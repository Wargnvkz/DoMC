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
    public partial class DoMCSocketSettingsListForm : Form
    {
        public int SocketQuantity
        {
            get
            {
                if (int.TryParse((string)ddlSocketQuantity.SelectedItem, out int sockets))
                {
                    return sockets;
                }
                else
                {
                    return 96;
                }
            }
            set
            {
                for (int i = 0; i < ddlSocketQuantity.Items.Count; i++)
                {
                    if ((string)ddlSocketQuantity.Items[i] == value.ToString())
                    {
                        ddlSocketQuantity.SelectedItem = ddlSocketQuantity.Items[i];
                        return;
                    }
                }
            }
        }
        public Dictionary<int, DoMCLib.Configuration.CCDSocketConfiguration> SocketConfigurations;

        private Panel[] SocketPanels;
        public DoMCSocketSettingsListForm()
        {
            InitializeComponent();
            SocketConfigurations = new Dictionary<int, DoMCLib.Configuration.CCDSocketConfiguration>();
            ddlSocketQuantity.Items.Clear();
            var keys = UserInterfaceControls.SocketRectSize.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
                ddlSocketQuantity.Items.Add(keys[i].ToString());
        }

        public new DialogResult ShowDialog()
        {
            UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetSocketConfiguration(SocketQuantity,SocketConfigurations), Color.Green, Color.DarkGray);
            return base.ShowDialog();
        }


        private void ddlSocketQuantity_SelectedIndexChanged(object sender, EventArgs e)
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
                SocketPanels[i].Tag = i+1;
                for (int j = 0; j < SocketPanels[i].Controls.Count; j++)
                {
                    SocketPanels[i].Controls[j].Click += DoMCSocketSettingsPanel_Click;
                    SocketPanels[i].Controls[j].Tag = i+1;
                }
            }
            UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);
        }

        private void DoMCSocketSettingsPanel_Click(object sender, EventArgs e)
        {
            var SocketN = (int)((Control)sender).Tag;
            var ss = new DoMCSocketSettingsForm();
            if (SocketConfigurations == null) SocketConfigurations = new Dictionary<int, DoMCLib.Configuration.CCDSocketConfiguration>();
            if (SocketConfigurations.ContainsKey(SocketN))
            {
                var cfg = SocketConfigurations[SocketN];
                ss.Configuration = cfg;
            }
            else
            {
                ss.Configuration = new Configuration.CCDSocketConfiguration();
            }
            ss.Text = "Параметры гнеда "+SocketN;
            if (ss.ShowDialog() == DialogResult.OK)
            {
                var cfg = ss.Configuration;
                SocketConfigurations[SocketN] = cfg;
                UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);
            }
        }

        private void bntCopy_Click(object sender, EventArgs e)
        {
            if (!(SocketConfigurations?.ContainsKey(1)??false)) return;
            for (int i = 1; i < SocketQuantity; i++)
            {
                SocketConfigurations[i+1] = SocketConfigurations[1].Clone();
            }
            UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SocketConfigurations?.Clear();
            UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);
        }
    }
}
