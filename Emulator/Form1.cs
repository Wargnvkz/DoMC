namespace Emulator
{
    public partial class Form1 : Form
    {
        private TCPCCDCardServer[] servers = new TCPCCDCardServer[12];
        private CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();
            Init();
            btnCCDStop.Enabled = false;
        }
        public void Init()
        {
            clbEquipment.Items.Clear();
            for (int i = 0; i < 12; i++)
                clbEquipment.Items.Add($"Машина {i + 1}", true);
        }

        private async void StartCCD(bool[] cardsActive)
        {
            listBoxLog.Items.Clear();
            cts = new CancellationTokenSource();
            for (int i = 0; i < 12; i++)
            {
                if (cardsActive == null || cardsActive.Length <= i || cardsActive[i])
                {
                    int cardNum = i + 1;
                    servers[i] = new TCPCCDCardServer(cardNum, LogMessage);
                    await Task.Run(() =>
                    {
                        try
                        {
                            servers[i].Start();
                        }
                        catch (Exception ex)
                        {
                            LogMessage($"Плата {cardNum}: Ошибка {ex.Message}");
                            servers[i] = null;
                        }
                    });
                }
            }

            btnCCDStart.Enabled = false;
            btnCCDStop.Enabled = true;

            LogMessage("Эмуляторы запущены.");

        }
        private async void StopCCD()
        {
            cts.Cancel();
            foreach (var server in servers)
            {
                if (server != null && server.isStarted)
                    await Task.Run(() =>
                    {
                        try
                        {
                            server.Stop();
                        }
                        catch (Exception ex)
                        {
                            LogMessage($"Плата {server.CardNumber}: Ошибка {ex.Message}");
                        }
                    });
            }

            btnCCDStart.Enabled = true;
            btnCCDStop.Enabled = false;
            LogMessage("Эмуляторы остановлены.");
        }
        private void LogMessage(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(() => LogMessage(msg));
                return;
            }

            var line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            listBoxLog.Items.Add(line);
            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
        }

        private void btnCCDStart_Click(object sender, EventArgs e)
        {
            StartCCD(Enumerable.Range(0, clbEquipment.Items.Count).Select(clbEquipment.GetItemChecked).ToArray());
        }

        private void btnCCDStop_Click(object sender, EventArgs e)
        {
            StopCCD();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var server in servers)
            {
                server?.RaiseSynchrosignal();
            }
        }
    }
}
