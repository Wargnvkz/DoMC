using DoMCForms;
using DoMCLib.Classes;
using DoMCLib.Classes.Module.API.Controllers;
using DoMCLib.Tools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DoMCRemoteControl
{
    public partial class DoMCRemoteControlForm : Form
    {
        private string DoMCIP = "localhost";//"192.168.211.100";
        private int DoMCPort = 8080;
        private string LocalDBPath;
        private string RemoteDBPath;

        private ApiClient _api;
        APIStatusResponse _LastStatus;
        List<AverageOfSocket> _Averages;

        private int ShowPeriodInHours = 2;
        public bool NoConnection = false;
        DoMCArchiveForm archiveForm = null;
        DateTime? LastActionTime;

        private int Socket = 1, Period = 1;

        public DoMCRemoteControlForm()
        {
            InitializeComponent();
            ReadConfig();
            SetDoMCServerAddress();
            LoadArchiveTab();
            HookAllControls(archiveForm);
            HookAllControls(this);

        }

        private void ReadConfig()
        {
            DoMCIP = ConfigurationManager.AppSettings["IP"];
            int.TryParse(ConfigurationManager.AppSettings["Port"], out DoMCPort);
            LocalDBPath = ConfigurationManager.AppSettings["DB"];
            RemoteDBPath = ConfigurationManager.AppSettings["DBArchive"];
        }

        private void SaveConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["IP"] == null)
                config.AppSettings.Settings.Add("IP", DoMCIP);
            else
                config.AppSettings.Settings["IP"].Value = DoMCIP;


            if (config.AppSettings.Settings["Port"] == null)
                config.AppSettings.Settings.Add("Port", DoMCPort.ToString());
            else
                config.AppSettings.Settings["Port"].Value = DoMCPort.ToString();


            if (config.AppSettings.Settings["DB"] == null)
                config.AppSettings.Settings.Add("DB", LocalDBPath);
            else
                config.AppSettings.Settings["DB"].Value = LocalDBPath;

            if (config.AppSettings.Settings["DBArchive"] == null)
                config.AppSettings.Settings.Add("DBArchive", RemoteDBPath);
            else
                config.AppSettings.Settings["DBArchive"].Value = RemoteDBPath;


            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }

        public void SetDoMCServerAddress()
        {
            _api = new($"http://{DoMCIP}:{DoMCPort}/");
        }

        private async Task RequestStatus()
        {
            try
            {
                _LastStatus = await _api.GetStatusAsync();
                try
                {
                    _Averages = await _api.GetSocketAvergaeAsync(Socket, Period);
                }
                catch (Exception ex)
                { }
                var cycleDuration = (int)((_LastStatus?.WorkingState?.CycleDuration * 1000) ?? 0);
                if (cycleDuration < 10000) cycleDuration = 10000;
                timer1.Interval = cycleDuration;
                NoConnection = false;
            }
            catch (Exception ex)
            {
                NoConnection = true;
            }
        }
        private async Task Start()
        {
            try
            {
                await _api.PostAsync("api/working/start");
                NoConnection = false;
            }
            catch { NoConnection = true; }
        }

        private async Task Stop()
        {
            try
            {
                await _api.PostAsync("api/working/stop");
                NoConnection = false;
            }
            catch { NoConnection = true; }
        }

        private async Task ResetStatistics()
        {
            try
            {
                await _api.PostAsync("api/status/reset-statistics");
                NoConnection = false;
            }
            catch { NoConnection = true; }
        }

        private async Task ResetTotalDefectCyles()
        {
            try
            {
                await _api.PostAsync("api/status/reset-cycles");
                NoConnection = false;
            }
            catch { }
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            await RefreshDataAndFillForm();
        }
        private async Task RefreshDataAndFillForm()
        {
            await RequestStatus();
            RefreshAll();
            if (LastActionTime != null)
            {
                if ((DateTime.Now - LastActionTime.Value).TotalSeconds > 120 && tabPages.SelectedIndex != 0)
                {
                    tabPages.SelectedIndex = 0;
                }
            }
        }

        private void RefreshAll()
        {
            if (_LastStatus == null) return;
            //var shift = new Shift();
            var now = DateTime.Now;
            var from = now.AddHours(-Period);//shift.ShiftStartsAt();//now.AddHours(-ShowPeriodInHours);
            var boxes = _LastStatus.Boxes.Where(b => b.CompletedTime >= from && b.CompletedTime <= now).OrderBy(box => box.CompletedTime).ToList();
            var defects = _LastStatus.LastDefects.Where(d => d.CycleDateTime >= from && d.CycleDateTime <= now).OrderBy(t => t.CycleDateTime).ToList();
            FillBoxesAndDefectsChart(boxes, defects.Select(d => d.CycleDateTime).ToList());
            /*if (_Averages == null)
            {
                _Averages = new List<AverageOfSocket>();

            }*/
            if (_Averages != null)
            {
                /*if (_Averages.Count == 0)
                 {
                     _Averages.Add(new AverageOfSocket() { CycleTime = DateTime.Now, AveragesOfSocket = 100 });
                     _Averages.Add(new AverageOfSocket() { CycleTime = DateTime.Now.AddMinutes(-1), AveragesOfSocket = 110 });
                 }*/
                FillAverages(_Averages);

            }



            lvBoxes.Items.Clear();
            for (int i = 0; i < boxes.Count; i++)
            {
                var box = boxes[i];
                var lvi = new ListViewItem(new string[] { (i + 1).ToString(), box.CompletedTime.ToString("G"), box.BadCyclesCount.ToString() });
                lvBoxes.Items.Add(lvi);
            }

            lvDefects.Items.Clear();
            for (int i = 0; i < defects.Count; i++)
            {
                var def = defects[i];
                var lvi = new ListViewItem(new string[] { (i + 1).ToString(), def.CycleDateTime.ToString("G"), String.Join(",", def.DefectedSockets) });
                lvDefects.Items.Add(lvi);
            }

            lblCycleDuration.Text = _LastStatus.WorkingState.CycleDuration.ToString("F2") + " с";
            //lblWorkingStatus.Text = _LastStatus.WorkingState.IsRunning ? "Работает" : "Стоит";
            if (_LastStatus.WorkingState.IsRunning)
            {
                lblWorkingStatus.Text = "Работает";
                lblWorkingStatus.BackColor = Color.LimeGreen;

            }
            else
            {
                lblWorkingStatus.Text = "Стоит";
                lblWorkingStatus.BackColor = Color.OrangeRed;

            }

            pnlCurrentSockets.Invalidate();
            if (NoConnection)
            {
                lblNoConnection.Visible = true;
            }
            else
            {
                lblNoConnection.Visible = false;

            }
            lblDefectsInLastHour.Text = _LastStatus.WorkingState.TotalDefectCycles.ToString();
            lblCurrentBoxDefects.Text = _LastStatus.WorkingState.CurrentBoxDefectCycles.ToString();
        }

        private void FillBoxesAndDefectsChart(List<DoMCLib.Classes.Box> Boxes, List<DateTime> Defects)
        {
            chEvents.Series[0].Points.Clear();

            chEvents.Series[0].XValueType = ChartValueType.DateTime;
            foreach (var box in Boxes)
            {
                var dpTotal = new DataPoint();
                dpTotal.SetValueXY(box.CompletedTime.ToOADate(), 1);
                dpTotal.Color = Color.Green;
                dpTotal.BorderWidth = 1;
                chEvents.Series[0].Points.Add(dpTotal);

            }
            foreach (var def in Defects)
            {
                var dpTotal = new DataPoint();
                dpTotal.SetValueXY(def.ToOADate(), 0.5);
                dpTotal.Color = Color.OrangeRed;
                dpTotal.BorderWidth = 1;
                chEvents.Series[0].Points.Add(dpTotal);
            }
        }
        private void FillAverages(List<AverageOfSocket> averages)
        {
            chEvents.Series[1].Points.Clear();

            chEvents.Series[1].XValueType = ChartValueType.DateTime;
            foreach (var cycle in averages)
            {
                var dpTotal = new DataPoint();
                dpTotal.SetValueXY(cycle.CycleTime.ToOADate(), cycle.AveragesOfSocket);
                dpTotal.Color = Color.Blue;
                dpTotal.BorderWidth = 2;
                chEvents.Series[1].Points.Add(dpTotal);

            }

        }

        private void DrawMatrix(Graphics g, Size size, int[] ErrorsBySockets, bool ShowingStatistics)
        {
            var goodsockets = new bool[96];// InterfaceDataExchange?.CurrentCycleCCD?.IsSocketGood ?? new bool[96];
            var socketsToSave = new bool[96];
            var IsSocketsChecking = Enumerable.Range(0, 96).Select(i => true).ToArray();// new bool[96];
            var bmp = VisualTools.DrawMatrix(size, goodsockets, socketsToSave, ErrorsBySockets, IsSocketsChecking, Color.LawnGreen, Color.Red, Color.LightGray, Color.LimeGreen, ShowingStatistics);
            g.DrawImage(bmp, 0, 0);

        }
        private void pnlCurrentSockets_Paint(object sender, PaintEventArgs e)
        {
            if ((_LastStatus?.WorkingState?.ErrorsBySockets ?? null) == null) return;
            DrawMatrix(e.Graphics, pnlCurrentSockets.Size, _LastStatus.WorkingState.ErrorsBySockets, cbCurrentShowStatistics.Checked);
        }
        private async void DoMCRemoteControlForm_Load(object sender, EventArgs e)
        {
            await RequestStatus();
            RefreshAll();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            await Start();
        }
        private async void btnStop_Click(object sender, EventArgs e)
        {
            await Stop();
        }

        private void DoMCRemoteControlForm_Resize(object sender, EventArgs e)
        {
            RefreshAll();
        }

        private async void tsmiSocketCounter_Click(object sender, EventArgs e)
        {
            await ResetStatistics();
        }

        private async void tsmiResetCycles_Click(object sender, EventArgs e)
        {
            await ResetTotalDefectCyles();
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.IP = DoMCIP;
            settingsForm.Port = DoMCPort;
            settingsForm.LocalDBPath = LocalDBPath;
            settingsForm.RemoteDBPath = RemoteDBPath;
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                DoMCIP = settingsForm.IP;
                DoMCPort = settingsForm.Port;
                LocalDBPath = settingsForm.LocalDBPath;
                RemoteDBPath = settingsForm.RemoteDBPath;
                SaveConfig();
                SetDoMCServerAddress();

                if (archiveForm != null)
                    archiveForm.SetParameters(LocalDBPath, RemoteDBPath);
            }
        }

        private void cbCurrentShowStatistics_CheckedChanged(object sender, EventArgs e)
        {
            pnlCurrentSockets.Invalidate();
        }


        private void LoadArchiveTab()
        {
            archiveForm = new DoMCForms.DoMCArchiveForm(new EmptyController(), LocalDBPath, RemoteDBPath);
            archiveForm.TopLevel = false;
            archiveForm.Parent = tbpArchive;
            archiveForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            archiveForm.StartPosition = FormStartPosition.Manual;
            archiveForm.Location = new Point(0, 0);
            //tbpArchive.Scale(new SizeF(1f, 1f));
            archiveForm.Scale(new SizeF(1f, 1f));
            archiveForm.Size = new Size(tbpArchive.ClientSize.Width, tbpArchive.ClientSize.Height);
            archiveForm.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            archiveForm.Visible = true;

        }

        private void tbpState_Click(object sender, EventArgs e)
        {

        }

        void HookAllControls(Control root)
        {
            foreach (Control c in root.Controls)
            {
                c.MouseMove += (_, __) => ResetIdleTimer();
                c.KeyPress += (_, __) => ResetIdleTimer();
                HookAllControls(c);
            }
        }
        private void ResetIdleTimer()
        {
            LastActionTime = DateTime.Now;
        }

        private async void nudAverageSocket_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Socket = (int)nudAverageSocket.Value;
                Period = (int)nudAveragePeriod.Value;
                e.Handled = true;
                await RefreshDataAndFillForm();
            }
        }

        private async void nudAverage_ValueChanged(object sender, EventArgs e)
        {
            Socket = (int)nudAverageSocket.Value;
            Period = (int)nudAveragePeriod.Value;
            await RefreshDataAndFillForm();
        }
    }
}
