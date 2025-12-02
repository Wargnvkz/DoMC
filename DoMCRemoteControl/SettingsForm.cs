using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoMCRemoteControl
{
    public partial class SettingsForm : Form
    {
        public string IP { get => txbIP.Text; set => txbIP.Text = value; }
        public int Port { get => (int)nudPort.Value; set => nudPort.Value = value; }
        public string LocalDBPath { get => txbLocalConnectionString.Text; set => txbLocalConnectionString.Text = value; }
        public string RemoteDBPath { get => txbRemoteConnectionString.Text; set => txbRemoteConnectionString.Text = value; }
        public SettingsForm()
        {
            InitializeComponent();
        }
        private void btnLocalDBBrowse_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = txbLocalConnectionString.Text;
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Выбор папки хранения данных";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txbLocalConnectionString.Text = fbd.SelectedPath;
            }
        }

        private void btnRemoteDBBrowse_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = txbRemoteConnectionString.Text;
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Выбор папки архива";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txbRemoteConnectionString.Text = fbd.SelectedPath;
            }

        }
    }
}
