using DoMCLib.Configuration;
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
    public partial class DoMCImageProcessSettingsListForm : Form
    {
        private int SocketQuantity;
        //public bool[] SocketIsOn;

        private Panel[] SocketPanels;

        //public Dictionary<int, CCDSocketConfiguration> SocketParameters;
        public DoMCImageProcessSettingsListForm()
        {
            InitializeComponent();
        }

        public new DialogResult ShowDialog()
        {
            /*
            SocketQuantity = SocketParameters.Keys.Max();
            lblSocketQuantity.Text = SocketQuantity.ToString();
            SocketPanels = UserInterfaceControls.CreateSocketStatusPanels(SocketQuantity, ref pnlSockets, SocketChange_Click);
            */
            //ShowStatuses();
            return base.ShowDialog();
        }

        /*private void ShowStatuses()
        {
            UserInterfaceControls.SetSocketStatuses(SocketPanels, SocketIsOn, SystemColors.ButtonFace, SystemColors.ButtonFace);

        }*/

        private void SocketChange_Click(object sender, EventArgs e)
        {
            /*
            var ctrl = sender as Control;
            if (ctrl != null)
            {
                var n = (int)ctrl.Tag;
                var form = new DoMCImageProcessSettingsForm();
                form.ImageProcessParameters = SocketParameters[n].ImageProcessParameters.Clone();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SocketParameters[n].ImageProcessParameters = form.ImageProcessParameters.Clone();
                }
                //ShowStatuses();
            }
            */
        }

        private void btnExpandToAll_Click(object sender, EventArgs e)
        {
            /*
            var ip1 = SocketParameters[1].ImageProcessParameters;
            foreach (var kv in SocketParameters)
            {
                if (kv.Key == 1) continue;
                kv.Value.ImageProcessParameters = ip1.Clone();
            }
            */
        }
    }
}
