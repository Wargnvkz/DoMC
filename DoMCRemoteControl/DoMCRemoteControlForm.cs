using DoMCLib.Classes.Module.API.Controllers;
using DoMCLib.Tools;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Configuration;

namespace DoMCRemoteControl
{
    public partial class DoMCRemoteControlForm : Form
    {
        private string DoMCIP = "localhost";//"192.168.211.100";
        private int DoMCPort = 8080;
        private ApiClient _api;
        APIStatusResponse _LastStatus;
        private int ShowPeriodInHours = 2;
        public bool NoConnection = false;
        public DoMCRemoteControlForm()
        {
            InitializeComponent();
            ReadConfig();
            SetDoMCServerAddress();
        }

        private void ReadConfig()
        {
            DoMCIP = ConfigurationManager.AppSettings["IP"];
            int.TryParse(ConfigurationManager.AppSettings["Port"], out DoMCPort);
        }

        private void SaveConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["IP"].Value = DoMCIP;
            config.AppSettings.Settings["Port"].Value = DoMCPort.ToString();
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
            await RequestStatus();
            RefreshAll();
        }

        private void RefreshAll()
        {
            if (_LastStatus == null) return;
            var shift = new Shift();
            var now = DateTime.Now;
            var from = shift.ShiftStartsAt();//now.AddHours(-ShowPeriodInHours);
            var boxes = _LastStatus.Boxes.Where(b => b.CompletedTime >= from && b.CompletedTime <= now).OrderBy(box => box.CompletedTime).ToList();
            var defects = _LastStatus.LastDefects.Where(d => d.CycleDateTime >= from && d.CycleDateTime <= now).OrderBy(t => t.CycleDateTime).ToList();
            FillBoxesAndDefectsChart(boxes, defects.Select(d => d.CycleDateTime).ToList());


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
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                DoMCIP = settingsForm.IP;
                DoMCPort = settingsForm.Port;
                SaveConfig();
                SetDoMCServerAddress();
            }
        }

        private void cbCurrentShowStatistics_CheckedChanged(object sender, EventArgs e)
        {
            pnlCurrentSockets.Invalidate();
        }
    }
}
