using DoMC.Forms;
using DoMC.Tools;
using DoMCLib.Classes;
using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
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
                if (int.TryParse((string)cmbSocketQuantity.SelectedItem, out int sockets))
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
                for (int i = 0; i < cmbSocketQuantity.Items.Count; i++)
                {
                    if ((string)cmbSocketQuantity.Items[i] == value.ToString())
                    {
                        cmbSocketQuantity.SelectedItem = cmbSocketQuantity.Items[i];
                        return;
                    }
                }
            }
        }
        public SocketParameters[] SocketConfigurations;

        private Panel[] SocketPanels;
        private bool FirstStart = true;
        private DoMCApplicationContext Context;
        public DoMCSocketSettingsListForm(DoMCApplicationContext context)
        {
            InitializeComponent();

            cmbSocketQuantity.Items.Clear();
            var keys = UserInterfaceControls.SocketRectSize.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
                cmbSocketQuantity.Items.Add(keys[i].ToString());
            Context = context;
        }

        private void FillSockets()
        {
            SocketPanels = UserInterfaceControls.CreateSocketStatusPanels(SocketQuantity, ref pnlSockets, DoMCSocketSettingsPanel_Click);
        }
        private void ShowSocketStatuses()
        {
            UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetStandardSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);
        }
        private void RemoveSockets()
        {
            UserInterfaceControls.RemoveSocketStatusPanels(SocketPanels, pnlSockets);
        }

        public new DialogResult ShowDialog()
        {
            FillSockets();
            ShowSocketStatuses();
            return base.ShowDialog();
        }


        private void cmbSocketQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!FirstStart &&
                cmbSocketQuantity.SelectedIndex > -1
                &&
                MessageBox.Show("Вы действительно хотите изменить количество гнезд?", "Изменение размера матрицы", MessageBoxButtons.YesNo) == DialogResult.Yes
                )
            {

                SocketQuantity = int.Parse(cmbSocketQuantity.SelectedItem as string);
                SocketConfigurations = new SocketParameters[SocketQuantity];
                RemoveSockets();
                for (int i = 0; i < SocketQuantity; i++)
                {
                    SocketConfigurations[i] = new SocketParameters();

                }
                FillSockets();
                ShowSocketStatuses();
            }
            if (FirstStart) FirstStart = false;

            /*UserInterfaceControls.RemoveSocketStatusPanels(SocketPanels, pnlSockets);

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
            FillSockets();*/
        }

        private void DoMCSocketSettingsPanel_Click(object? sender, EventArgs e)
        {

            var SocketN = (int)((Control)sender).Tag;
            var ss = new DoMCSocketSettingsForm();
            if (SocketConfigurations == null) throw new ArgumentNullException(nameof(SocketConfigurations));
            if (SocketConfigurations[SocketN] == null)
            {
                SocketConfigurations[SocketN] = new SocketParameters();
            }
            if (SocketConfigurations[SocketN].ImageCheckingParameters == null) SocketConfigurations[SocketN].ImageCheckingParameters = new Configuration.ImageProcessParameters();
            if (SocketConfigurations[SocketN].ReadingParameters == null) SocketConfigurations[SocketN].ReadingParameters = new SocketReadingParameters();

            if (SocketConfigurations[SocketN] != null)
            {
                var cfg = SocketConfigurations[SocketN];
                ss.Configuration = cfg;
            }
            ss.Text = "Параметры гнеда " + (SocketN + 1);
            if (ss.ShowDialog() == DialogResult.OK)
            {
                var cfg = ss.Configuration;
                SocketConfigurations[SocketN] = cfg;
                UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetStandardSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);
            }
        }

        private void bntCopy_Click(object sender, EventArgs e)
        {

            var form = new DoMCSocketCopyParametersForm(Context, SocketConfigurations, SocketQuantity, DoMCSocketCopyParametersOption.FullAccess);
            if (form.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Параметры успешно скопированы");
            }
            else
            {
                MessageBox.Show("Копирование отменено");
            }

            /*
            for (int i = 1; i < SocketQuantity; i++)
            {
                SocketConfigurations[i] = SocketConfigurations[0].Clone();
            }*/
            UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetStandardSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            UserInterfaceControls.SetSocketStatuses(SocketPanels, UserInterfaceControls.GetListOfSetStandardSocketConfiguration(SocketQuantity, SocketConfigurations), Color.Green, Color.DarkGray);

        }
    }
}
