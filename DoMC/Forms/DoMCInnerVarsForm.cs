using DoMCLib.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMCInterface
{
    public partial class DoMCInnerVarsForm : Form
    {
        DoMCInterfaceDataExchange DoMCInterface;
        public DoMCInnerVarsForm(DoMCInterfaceDataExchange dataExcange)
        {
            InitializeComponent();
            DoMCInterface = dataExcange;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("LEDStatus.TimeSyncSignalGot: " + DoMCInterface.LEDStatus.TimeSyncSignalGot);
            listBox1.Items.Add("LEDStatus.TimePreviousSyncSignalGot: " + DoMCInterface.LEDStatus.TimePreviousSyncSignalGot);
            listBox1.Items.Add("Timings.CCDStart: " + DoMCInterface.Timings.CCDStart);
            listBox1.Items.Add("Timings.CCDEnd: " + DoMCInterface.Timings.CCDEnd);
            listBox1.Items.Add("Timings.CCDGetImagesStarted: " + DoMCInterface.Timings.CCDGetImagesStarted);
            listBox1.Items.Add("Timings.CCDGetImagesEnded: " + DoMCInterface.Timings.CCDGetImagesEnded);
            listBox1.Items.Add("Timings.CCDImagesProcessStarted: " + DoMCInterface.Timings.CCDImagesProcessStarted);
            listBox1.Items.Add("Timings.CCDImagesProcessEnded: " + DoMCInterface.Timings.CCDImagesProcessEnded);
            listBox1.Items.Add("Timings.CCDEtalonsRecalculateStarted: " + DoMCInterface.Timings.CCDEtalonsRecalculateStarted);
            listBox1.Items.Add("Timings.CCDEtalonsRecalculateEnded: " + DoMCInterface.Timings.CCDEtalonsRecalculateEnded);
            listBox1.Items.Add("Errors.MissedSyncrosignalCounter: " + DoMCInterface.Errors.MissedSyncrosignalCounter);
            listBox1.Items.Add("CyclesCCD.Count: " + DoMCInterface.CyclesCCD.Count);
            listBox1.Items.Add("CCDReadsFailed: " + DoMCInterface.CCDReadsFailed);
            /*listBox1.Items.Add("Timings." + DoMCInterface.Timings.);
            listBox1.Items.Add("Timings." + DoMCInterface.Timings.);
            listBox1.Items.Add("Timings." + DoMCInterface.Timings.);
            listBox1.Items.Add("Timings." + DoMCInterface.Timings.);
            listBox1.Items.Add("Timings." + DoMCInterface.Timings.);
            listBox1.Items.Add("Timings." + DoMCInterface.Timings.);*/
        }
    }
}
