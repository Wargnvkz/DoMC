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
    public partial class ImageReadBytesStatiscticsForm : Form
    {
        public ImageReadBytesStatiscticsForm()
        {
            InitializeComponent();
            PrepareList();
        }

        private void PrepareList()
        {
            for (int cards = 0; cards < 6; cards++)
            {
                lvImagesReadStatistics.Columns.Add("", 150);
            }
            for (int socket = 0; socket < 16; socket++)
            {
                lvImagesReadStatistics.Items.Add(new ListViewItem(new string[] { "", "", "", "", "", "" }));
            }
        }

        private void FillList()
        {
            /*
            if ((InterfaceDataExchange?.CardsConnection ?? null) == null)
            {
                for (int socket = 0; socket < 16; socket++)
                {
                    for (int cards = 0; cards < 6; cards++)
                    {
                        lvImagesReadStatistics.Items[socket].SubItems[cards].Text = "";
                    }
                }
                return;
            }

            var stat = InterfaceDataExchange.CardsConnection.GetStatisctisImagesBytesRead();
            for (int i = 0; i < stat.Count; i++)
            {
                //var socket = i + 1;
                var imageReadData = stat[i];
                var row = i % 16;
                var column = i / 16;
                lvImagesReadStatistics.Items[row].SubItems[column].Text = $"{imageReadData.BytesRead} байт ({imageReadData.TimeReadInMs:F0} мс)";

            }
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            FillList();
        }
    }
}
