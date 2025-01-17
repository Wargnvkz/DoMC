using DoMCLib.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DoMC.Forms.Settings
{
    public partial class DoMCDBSettingsForm : Form
    {
        [DllImport("ODBCCP32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SQLManageDataSources(IntPtr hwnd);

        [DllImport("ODBCCP32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SQLConfigDataSource(IntPtr hwndParent, uint fRequest, string lpszDriver, string lpszAttributes);

        public string LocalDBConnectionString
        {
            get
            {
                return txbLocalConnectionString.Text;
            }
            set
            {
                txbLocalConnectionString.Text = value;
            }
        }
        public string RemoteDBConnectionString
        {
            get
            {
                return txbRemoteConnectionString.Text;
            }
            set
            {
                txbRemoteConnectionString.Text = value;
            }
        }

        public int DelayBeforeMoveDataToArchive
        {
            get
            {
                int.TryParse(txbDelayBeforeMoveDataToArchive.Text, out int delay);
                return delay;
            }
            set
            {
                txbDelayBeforeMoveDataToArchive.Text = value.ToString();
            }
        }
        public DoMCDBSettingsForm()
        {
            InitializeComponent();
        }

        private void btnLocalDBBrowse_Click(object sender, EventArgs e)
        {
            //SQLConfigDataSource(Handle,1,null,null);
            //SQLManageDataSources(Handle);

            /*var sqlcsb = new System.Data.SqlClient.SqlConnectionStringBuilder(LocalDBConnectionString);
            using (var dialog = new DataConnectionDialog(sqlcsb))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LocalDBConnectionString = sqlcsb.ConnectionString;
                }

            }*/
            var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = LocalDBConnectionString;
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Выбор папки хранения данных";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                LocalDBConnectionString = fbd.SelectedPath;
            }
        }

        private void btnRemoteDBBrowse_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = RemoteDBConnectionString;
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Выбор папки архива";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                RemoteDBConnectionString = fbd.SelectedPath;
            }
            /*var sqlcsb = new System.Data.SqlClient.SqlConnectionStringBuilder(RemoteDBConnectionString);
            using (var dialog = new DataConnectionDialog(sqlcsb))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    RemoteDBConnectionString = sqlcsb.ConnectionString;
                }

            }*/
        }
    }
}
