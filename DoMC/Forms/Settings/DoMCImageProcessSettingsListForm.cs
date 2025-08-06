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
using DoMC.Forms;
using DoMCLib.Classes;
using System.Runtime.CompilerServices;
using DoMC;

namespace DoMCLib.Forms
{
    public partial class DoMCImageProcessSettingsListForm : Form
    {
        private int SocketQuantity;
        //public bool[] SocketIsOn;

        private Panel[] SocketPanels;

        public SocketParameters[] SocketParameters;
        DoMCApplicationContext Context;
        public DoMCImageProcessSettingsListForm(DoMCApplicationContext context)
        {
            InitializeComponent();
            Context = context;
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
                if (SocketParameters[n] == null) SocketParameters = new SocketParameters[Context.Configuration.HardwareSettings.SocketQuantity];
                if (SocketParameters[n].ImageCheckingParameters == null) SocketParameters[n].ImageCheckingParameters = new ImageProcessParameters();
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
            var form = new DoMCSocketCopyParametersForm(Context, SocketParameters, SocketQuantity, DoMCSocketCopyParametersOption.DecisionMakingAccess);
            if (form.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Параметры успешно скопированы");
            }
            else
            {
                MessageBox.Show("Копирование отменено");
            }

        }
    }


}
