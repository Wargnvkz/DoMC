using DoMCLib.Classes.Module.API.Controllers;
using DoMCLib.Tools;
using System.Windows.Forms.DataVisualization.Charting;

namespace DoMCRemoteControl
{
    public partial class DoMCRemoteControlForm : Form
    {
        private string DoMCIP = "192.168.211.100";
        private int DoMCPort = 8080;
        private ApiClient _api;
        APIStatusResponse _LastStatus;
        public DoMCRemoteControlForm()
        {
            InitializeComponent();
        }

        public void SetDoMCServerAddress()
        {
            _api = new($"http://{DoMCIP}:{DoMCPort}/");
        }
        private async Task Start()
        {
            await _api.PostAsync("api/working/start");
        }

        private async Task Stop()
        {
            await _api.PostAsync("api/working/reset");
        }

        private async Task ResetStatistics()
        {
            await _api.PostAsync("api/status/reset-statistics");
        }

        private async Task ResetTotalDefectCyles()
        {
            await _api.PostAsync("api/status/reset-cycles");
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            var status = await _api.GetStatusAsync();
            FillBoxesAndDefectsChart(
                status.LastDefects.Select(d =>
                (d.CycleDateTime, ((d.DefectedSockets?.Count ?? 0) != 0))
                ).ToList()
                );
            //labelDefects.Text = status?.TotalDefectCycles.ToString();
        }

        private void FillBoxesAndDefectsChart(List<(DateTime, bool)> BoxDefects)
        {
            chEvents.Series[0].Points.Clear();

            chEvents.Series[0].XValueType = ChartValueType.DateTime;
            foreach (var bd in BoxDefects)
            {

                if (!bd.Item2)
                {
                    var dpTotal = new DataPoint();
                    dpTotal.SetValueXY(bd.Item1.ToOADate(), 0.5);
                    dpTotal.Color = Color.OrangeRed;
                    dpTotal.BorderWidth = 1;
                    chEvents.Series[0].Points.Add(dpTotal);
                }
                else
                {
                    var dpTotal = new DataPoint();
                    dpTotal.SetValueXY(bd.Item1.ToOADate(), 1);
                    dpTotal.Color = Color.Green;
                    dpTotal.BorderWidth = 1;
                    chEvents.Series[0].Points.Add(dpTotal);
                }
            }
        }

        private void DrawMatrix(Graphics g, Size size, int[] ErrorsBySockets, bool ShowingStatistics)
        {
            var goodsockets = new bool[96];// InterfaceDataExchange?.CurrentCycleCCD?.IsSocketGood ?? new bool[96];
            var socketsToSave = new bool[96];
            var IsSocketsChecking = new bool[96];
            var bmp = VisualTools.DrawMatrix(size, goodsockets, socketsToSave, ErrorsBySockets, IsSocketsChecking, Color.LawnGreen, Color.Red, Color.LightGray, Color.LimeGreen, ShowingStatistics);
            g.DrawImage(bmp, 0, 0);

        }
    }
}
