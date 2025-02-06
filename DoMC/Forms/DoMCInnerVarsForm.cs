using DoMCLib.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DoMCApplicationContext = DoMCLib.Classes.DoMCApplicationContext;

namespace DoMC
{
    public partial class DoMCInnerVarsForm : Form
    {
        DoMCApplicationContext Context;
        public DoMCInnerVarsForm(DoMCApplicationContext context)
        {
            InitializeComponent();
            Context = context;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            /*listBox1.Items.Add("LEDStatus.TimeSyncSignalGotForShowInCycle: " + DoMC.LEDStatus.TimeSyncSignalGotForShowInCycle);
            listBox1.Items.Add("LEDStatus.TimePreviousSyncSignalGot: " + DoMC.LEDStatus.TimePreviousSyncSignalGot);
            listBox1.Items.Add("Timings.CCDStart: " + DoMC.Timings.CCDStart);
            listBox1.Items.Add("Timings.CCDEnd: " + DoMC.Timings.CCDEnd);
            listBox1.Items.Add("Timings.CCDGetImagesStarted: " + DoMC.Timings.CCDGetImagesStarted);
            listBox1.Items.Add("Timings.CCDGetImagesEnded: " + DoMC.Timings.CCDGetImagesEnded);
            listBox1.Items.Add("Timings.CCDImagesProcessStarted: " + DoMC.Timings.CCDImagesProcessStarted);
            listBox1.Items.Add("Timings.CCDImagesProcessEnded: " + DoMC.Timings.CCDImagesProcessEnded);
            listBox1.Items.Add("Timings.CCDEtalonsRecalculateStarted: " + DoMC.Timings.CCDEtalonsRecalculateStarted);
            listBox1.Items.Add("Timings.CCDEtalonsRecalculateEnded: " + DoMC.Timings.CCDEtalonsRecalculateEnded);
            listBox1.Items.Add("Errors.MissedSyncrosignalCounter: " + DoMC.Errors.MissedSyncrosignalCounter);
            listBox1.Items.Add("CyclesCCD.Count: " + DoMC.CyclesCCD.Count);
            listBox1.Items.Add("CCDReadsFailed: " + DoMC.CCDReadsFailed);*/
            /*listBox1.Items.Add("Timings." + DoMC.Timings.);
            listBox1.Items.Add("Timings." + DoMC.Timings.);
            listBox1.Items.Add("Timings." + DoMC.Timings.);
            listBox1.Items.Add("Timings." + DoMC.Timings.);
            listBox1.Items.Add("Timings." + DoMC.Timings.);
            listBox1.Items.Add("Timings." + DoMC.Timings.);*/
        }
    }
}
