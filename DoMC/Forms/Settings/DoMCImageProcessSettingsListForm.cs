using DoMCLib.Configuration;
using DoMC.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DoMCLib.Classes.Configuration.CCD;

namespace DoMCLib.Forms
{
    public partial class DoMCImageProcessSettingsListForm : Form
    {
        private int SocketQuantity;
        //public bool[] SocketIsOn;

        private Panel[] SocketPanels;

        public SocketParameters[] SocketParameters;
        public DoMCImageProcessSettingsListForm()
        {
            InitializeComponent();
        }

        public new DialogResult ShowDialog()
        {

            SocketQuantity = SocketParameters.Length;
            lblSocketQuantity.Text = SocketQuantity.ToString();
            SocketPanels = UserInterfaceControls.CreateSocketStatusPanels(SocketQuantity, ref pnlSockets, SocketChange_Click);

            //ShowStatuses();
            return base.ShowDialog();
        }

        /*private void ShowStatuses()
        {
            UserInterfaceControls.SetSocketStatuses(SocketPanels, SocketIsOn, SystemColors.ButtonFace, SystemColors.ButtonFace);

        }*/

        private void SocketChange_Click(object sender, EventArgs e)
        {

            var ctrl = sender as Control;
            if (ctrl != null)
            {
                var n = (int)ctrl.Tag;
                var form = new DoMCImageProcessSettingsForm();
                form.ImageCheckingParameters = SocketParameters[n].ImageCheckingParameters.Clone();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SocketParameters[n].ImageCheckingParameters = form.ImageCheckingParameters.Clone();
                }
                //ShowStatuses();
            }

        }

        private void btnExpandToAll_Click(object sender, EventArgs e)
        {
            var ip1 = SocketParameters[0].ImageCheckingParameters;
            for (int i = 1; i < SocketParameters.Length; i++)
            {
                SocketParameters[i].ImageCheckingParameters = ip1.Clone();
            }

        }
    }
}
