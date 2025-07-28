using DoMCLib.DB;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DoMC.Forms
{
    public partial class DoMCArchiveForm : Form
    {
        DateTime dtArchiveFrom, dtArchiveTo;
        List<DisplayCycleData> ArchiveCycles;
        List<CycleData> LocalArchiveCycles;
        List<CycleData> RemoteArchiveCycles;

        string LocalPathString;
        string RemotePathString;
        //List<Tuple<DateTime, int>> lstArchiveByTime;

        Button[] selectedButtons;
        IMainController Controller;
        ILogger logger;
        Observer observer;
        DataStorage DS;

        public DoMCArchiveForm(IMainController mainController, string localConnectionString, string remoteConnectionString)
        {
            InitializeComponent();
            Controller = mainController;
            logger = Controller.GetLogger("Просмотр архива");
            observer = Controller.GetObserver();
            selectedButtons = new Button[] { btnArchiveSelect, btnArchiveSelectBad };

            LocalPathString = localConnectionString;
            RemotePathString = remoteConnectionString;
            StartDataStorage();
            this.Load += DoMCArchiveForm_Load;
            this.FormClosed += DoMCArchiveForm_FormClosed;

        }

        private void DoMCArchiveForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            StopDataStorage();
        }

        private void DoMCArchiveForm_Load(object? sender, EventArgs e)
        {
            SetNumericUpDownArrowsWidth(nudArchiveFrom, nudArchiveFrom.Width / 2);
            SetNumericUpDownArrowsWidth(nudArchiveSocketNumber, nudArchiveSocketNumber.Width / 2);
            SetNumericUpDownArrowsWidth(nudArchiveTo, nudArchiveTo.Width / 2);
            dtpArchiveFrom.CalendarFont = new Font(dtpArchiveFrom.CalendarFont.FontFamily, 8);
            ResetButtons();
        }

        public void SetParameters(string localConnectionString, string remoteConnectionString)
        {
            StopDataStorage();
            LocalPathString = localConnectionString;
            RemotePathString = remoteConnectionString;
            StartDataStorage();
        }
        private void StartDataStorage()
        {
            try
            {
                DS = new DoMCLib.DB.DataStorage(LocalPathString, RemotePathString, logger, observer);
            }
            catch { }
        }
        private void StopDataStorage()
        {
            if (DS.IsStarted)
                DS.Dispose();
        }
        private void SetNumericUpDownArrowsWidth(NumericUpDown nud, int newWidth)
        {
            try
            {
                nud?.GetType()?.GetField("defaultButtonsWidth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(nud, newWidth);
            }
            catch { }
        }

        private void ResetButtons()
        {
            if (selectedButtons != null)
            {
                Array.ForEach(selectedButtons, b => b.BackColor = SystemColors.Control);
            }
        }
        private void PressedButton(Button selectedButton)
        {
            if (selectedButtons != null && selectedButtons != null)
            {
                ResetButtons();
                selectedButton.BackColor = Color.LimeGreen;
            }
        }
        private void btnArchiveSelect_Click(object sender, EventArgs e)
        {
            if (!DS.IsStarted) { MessageBox.Show("Путь к базе данных не существует"); return; }
            PressedButton(sender as Button);
            LocalArchiveCycles = new List<CycleData>();
            RemoteArchiveCycles = new List<CycleData>();
            ArchiveCycles = new List<DisplayCycleData>();

            dtArchiveFrom = dtpArchiveFrom.Value.Date.AddHours((double)nudArchiveFrom.Value);
            dtArchiveTo = dtpArchiveTo.Value.Date.AddHours((double)nudArchiveTo.Value + 1);
            if (dtArchiveFrom > dtArchiveTo)
            {
                nudArchiveFrom.BackColor = Color.Red;
                nudArchiveTo.BackColor = Color.Red;
                MessageBox.Show("Время выбрано неправильно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudArchiveFrom.BackColor = SystemColors.Window;
                nudArchiveTo.BackColor = SystemColors.Window;
                return;
            }
            nudArchiveFrom.BackColor = SystemColors.Window;
            nudArchiveTo.BackColor = SystemColors.Window;

            if (!DS.LocalIsActive && !DS.RemoteIsActive)
            {
                MessageBox.Show("Не получилось подключиться к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LocalArchiveCycles = DS.LocalGetCycles(dtArchiveFrom, dtArchiveTo);
            RemoteArchiveCycles = DS.RemoteGetCycles(dtArchiveFrom, dtArchiveTo);

            if (LocalArchiveCycles != null)
                ArchiveCycles.AddRange(LocalArchiveCycles.Select(cd => new DisplayCycleData() { IsRemote = false, CycleData = cd }));
            if (RemoteArchiveCycles != null)
                ArchiveCycles.AddRange(RemoteArchiveCycles.Select(cd => new DisplayCycleData() { IsRemote = true, CycleData = cd }));

            ClearAllOutputs();
            if (ArchiveCycles.Count == 0) return;
            int[] TotalShow = new int[96];
            int[] Errors = new int[96];
            foreach (var cycle in ArchiveCycles)
            {
                for (int i = 0; i < (cycle.CycleData.IsSocketActive?.Length ?? (cycle.CycleData.IsSocketsGood?.Length ?? 0)); i++)
                {
                    if (IsSocketHasImage(cycle, i))
                    {
                        if (IsSocketGood(cycle, i))
                        {
                            TotalShow[i]++;

                        }
                        else
                        {
                            Errors[i]++;
                        }
                    }



                    /*if (cycle.CycleData.IsSocketsGood[i] || (!cycle.IsRemote && (cycle.CycleData.IsSocketActive?[i] ?? false)))
                        TotalShow[i]++;
                    if (!cycle.CycleData.IsSocketsGood[i])
                        Errors[i]++;*/
                }
            }
            var TotalMax = TotalShow.Max();
            var maxErrorSum = TotalShow.Max();
            var calulatedInterval = (maxErrorSum / 5 / 5) * 5;
            if (calulatedInterval == 0) calulatedInterval = maxErrorSum >= 5 ? maxErrorSum / 5 : 1;

            chArchiveSumBad.ChartAreas[0].AxisY.Interval = calulatedInterval;
            chArchiveSumBad.Series[0].Points.Clear();
            for (int i = 0; i < TotalShow.Length; i++)
            {
                var dpTotalMax = new System.Windows.Forms.DataVisualization.Charting.DataPoint(i + 1, TotalMax);
                dpTotalMax.Color = Color.Transparent;
                chArchiveSumBad.Series[0].Points.Add(dpTotalMax);

                var dpTotal = new System.Windows.Forms.DataVisualization.Charting.DataPoint(i + 1, TotalShow[i]);
                dpTotal.Color = Color.Green;
                chArchiveSumBad.Series[0].Points.Add(dpTotal);

                var dpError = new System.Windows.Forms.DataVisualization.Charting.DataPoint(i + 1, Errors[i]);
                dpError.Color = Color.OrangeRed;
                chArchiveSumBad.Series[0].Points.Add(dpError);

                /*chArchiveSumBad.Series[0].Points.AddXY(i + 1, TotalMax);
                chArchiveSumBad.Series[0].Points.AddXY(i + 1, TotalShow[i] );
                chArchiveSumBad.Series[0].Points.AddXY(i + 1, Errors[i]);*/
            }
            nudArchiveSocketNumber_ValueChanged(nudArchiveSocketNumber, new EventArgs());

            FillBoxes();
            FillDefectsList();

        }
        private bool IsSocketGood(DisplayCycleData cycle, int Socket)
        {
            return cycle?.CycleData.IsSocketsGood[Socket] ?? false;
        }
        private bool IsSocketHasImage(DisplayCycleData cycle, int Socket)
        {
            return !cycle.IsRemote || (cycle?.CycleData.IsSocketActive?[Socket] ?? false);
        }

        private void FillBoxes()
        {
            List<BoxDB> boxes = new List<BoxDB>();


            var localboxes = DS.LocalGetBox(dtArchiveFrom, dtArchiveTo).OrderBy(b => b.CompletedTime).ToList();
            if (localboxes != null)
                boxes.AddRange(localboxes);
            var remoteboxes = DS.RemoteGetBox(dtArchiveFrom, dtArchiveTo).OrderBy(b => b.CompletedTime).ToList();
            if (remoteboxes != null)
                boxes.AddRange(remoteboxes);

            boxes = boxes.OrderBy(b => b.CompletedTime).ToList();

            lvBoxes.Items.Clear();
            for (int i = 0; i < boxes.Count; i++)
            {
                var box = boxes[i];
                var lvi = new ListViewItem(new string[] { box.CompletedTime.ToString("G"), box.BadCyclesCount.ToString() });
                lvBoxes.Items.Add(lvi);
            }

            List<Tuple<DateTime, bool>> BoxDefects = new List<Tuple<DateTime, bool>>();
            boxes.ForEach(b => BoxDefects.Add(new Tuple<DateTime, bool>(b.CompletedTime, true)));
            ArchiveCycles.ForEach(c =>
            {
                if (c.CycleData.IsSocketsGood.Any(s => !s))
                {
                    BoxDefects.Add(new Tuple<DateTime, bool>(c.CycleData.CycleDateTime, false));
                }
            }
            );
            BoxDefects = BoxDefects.OrderBy(bd => bd.Item1).ThenBy(bd1 => !bd1.Item2).ToList();

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



            /* var calulatedInterval = (MaxTotal / 5 / 5) * 5;
             if (calulatedInterval == 0) calulatedInterval = MaxTotal >= 5 ? MaxTotal / 5 : 1;
             chArchiveByTime.ChartAreas[0].AxisY.Interval = calulatedInterval;
             chArchiveByTime.ChartAreas[0].AxisY.RoundAxisValues();*/

        }

        private void FillDefectsList()
        {
            List<Tuple<DateTime, List<int>>> defects = new List<Tuple<DateTime, List<int>>>();
            for (int i = 0; i < ArchiveCycles.Count; i++)
            {
                var rec = ArchiveCycles[i];
                var badsockets = rec.CycleData.IsSocketsGood.Select((b, k) => new { b, k }).Where(x => !x.b).Select(y => y.k + 1).ToList();
                if (badsockets.Count != 0)
                {
                    defects.Add(new Tuple<DateTime, List<int>>(rec.CycleData.CycleDateTime, badsockets));
                }
            }
            defects = defects.OrderBy(d => d.Item1).ToList();

            lvDefects.Items.Clear();
            for (int i = 0; i < defects.Count; i++)
            {
                var def = defects[i];
                var lvi = new ListViewItem(new string[] { def.Item1.ToString("G"), String.Join(",", def.Item2) });
                lvDefects.Items.Add(lvi);
            }
        }


        private void chArchiveSumBad_MouseDown(object sender, MouseEventArgs e)
        {
            var result = chArchiveSumBad.HitTest(e.X, e.Y);
            if (result.PointIndex >= 0)
            {
                var DataPoint = (System.Windows.Forms.DataVisualization.Charting.DataPoint)result.Object;
                if (DataPoint != null && !DataPoint.IsEmpty && DataPoint.XValue > 0 && DataPoint.XValue <= 96)
                {
                    nudArchiveSocketNumber.Value = (int)DataPoint.XValue;
                }
            }
        }
        private void ShowSelectedItem()
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            if (lvArchiveSavedSockets.SelectedItems == null || lvArchiveSavedSockets.SelectedItems.Count == 0) return;
            var cycleTag = lvArchiveSavedSockets.SelectedItems[0].Tag as CycleTag;
            CycleData cycle = null;

            if (!cycleTag.IsRemote)
            {
                cycle = DS.LocalGetCycleById(cycleTag.CycleID);
            }
            if (cycleTag.IsRemote || cycle == null)
            {
                cycle = DS.RemoteGetCycleById(cycleTag.CycleID);

            }

            //var cycle = DS.GetCycleById(cycleid);
            if (cycle == null)
            {
                MessageBox.Show("Запись не найдена. Попробуйте обновить результаты поиска.");
                return;
            }
            var nsocket = (int)nudArchiveSocketNumber.Value;
            var showPreformImages = new DoMCLib.Forms.ShowPreformImages();

            var simage = cycle.SocketImages.Find(si => si.SocketNumber == nsocket);
            if (simage == null)
            {
                MessageBox.Show("Нет изображения для этого гнезда в архиве");
                return;
            }
            showPreformImages.SetStandardImage(simage.SocketStandardImage);
            showPreformImages.SetImage(simage.SocketImage);
            showPreformImages.SetImageProcessParameters(simage.ImageProcessParameters.Clone());
            /*new ImageCheckingParameters()
            {
                DeviationWindow = simage.DeviationWindow,
                MaxDeviation = simage.MaxDeviation,
                MaxAverage = simage.MaxAverage,
                TopBorder = simage.TopBorder,
                RightBorder = simage.RightBorder,
                BottomBorder = simage.BottomBorder,
                LeftBorder = simage.LeftBorder
            });*/

            showPreformImages.WindowState = FormWindowState.Maximized;
            showPreformImages.Text = $"Просмотр съема: {cycle.CycleDateTime} Гнездо: {nsocket}";
            showPreformImages.Show();
            System.Windows.Forms.Cursor.Current = Cursors.Default;

        }
        private void lvArchiveSavedSockets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowSelectedItem();
        }

        private void lvArchiveSavedSockets_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x0D')
            {
                ShowSelectedItem();
            }
        }
        private void ClearAllOutputs()
        {
            chArchiveSumBad.Series[0].Points.Clear();
            lvArchiveSavedSockets.Items.Clear();
            chArchiveByTime.Series[0].Points.Clear();
            chEvents.Series[0].Points.Clear();
            lvBoxes.Items.Clear();
            lvDefects.Items.Clear();

        }

        private void ArchiveFillCycleList(DateTime HourPeriod)
        {
            /*if (ArchiveCycles == null) return;
            var nsocket = (int)nudArchiveSocketNumber.Value;
            lvArchiveSavedSockets.Items.Clear();
            var cyclesToShow = ArchiveCycles.Where(c => (!c.CycleData.IsSocketsGood[nsocket - 1] || (c.CycleData.SocketsToSave?[nsocket - 1] ?? false) || (!c.IsRemote && c.CycleData.IsSocketActive[nsocket - 1])) && c.CycleData.CycleDateTime >= HourPeriod && c.CycleData.CycleDateTime < HourPeriod.AddHours(1)).OrderBy(c => c.CycleData.CycleDateTime);
            var badColor = Color.Red;
            var goodColor = Color.Green;
            foreach (var cycle in cyclesToShow)
            {
                var item = new ListViewItem();
                item.BackColor = cycle.CycleData.IsSocketsGood[nsocket - 1] ? goodColor : badColor;
                item.Text = cycle.CycleData.CycleDateTime.ToString("dd-MM-yyyy HH\\:mm\\:ss");
                item.Tag = new CycleTag(cycle.CycleData.CycleID, cycle.IsRemote);
                lvArchiveSavedSockets.Items.Add(item);
            }*/
            ArchiveFillCycleList(HourPeriod, HourPeriod);
        }
        private void ArchiveFillCycleList(DateTime StartPeriod, DateTime EndPeriod)
        {
            if (ArchiveCycles == null) return;
            var nsocket = (int)nudArchiveSocketNumber.Value;
            lvArchiveSavedSockets.Items.Clear();
            var cyclesToShow = ArchiveCycles.Where(c => IsSocketHasImage(c, nsocket - 1) && c.CycleData.CycleDateTime >= StartPeriod && c.CycleData.CycleDateTime < EndPeriod.AddHours(1)).OrderBy(c => c.CycleData.CycleDateTime);
            var badColor = Color.Red;
            var goodColor = Color.Green;
            foreach (var cycle in cyclesToShow)
            {
                var item = new ListViewItem();
                item.BackColor = cycle.CycleData.IsSocketsGood[nsocket - 1] ? goodColor : badColor;
                item.Text = cycle.CycleData.CycleDateTime.ToString("dd-MM-yyyy HH\\:mm\\:ss");
                item.Tag = new CycleTag(cycle.CycleData.CycleID, cycle.IsRemote);
                lvArchiveSavedSockets.Items.Add(item);
            }

        }

        private void nudArchiveSocketNumber_ValueChanged(object sender, EventArgs e)
        {
            lvArchiveSavedSockets.Items.Clear();
            if (ArchiveCycles == null) return;
            var nsocket = (int)nudArchiveSocketNumber.Value;


            chArchiveByTime.Series[0].Points.Clear();
            /*if (lstArchiveByTime == null)
                lstArchiveByTime = new List<Tuple<DateTime, int>>();
            lstArchiveByTime.Clear();*/


            TimeSpan dt = new TimeSpan(1, 0, 0);
            int n = 1;
            var rnd = new Random();
            //string format = "HH\\:mm\\:ss";
            //if (t.Date != dtArchiveTo.Date) format = "dd.MM.yyyy HH\\:mm\\:ss";
            var timeTo = dtArchiveTo.AddHours(1);
            chArchiveByTime.Series[0].XValueType = ChartValueType.DateTime;
            int MaxTotal = 0;
            var Records = new Dictionary<double, List<DisplayCycleData>>();
            var t = dtArchiveFrom.AddHours(-1);
            var tStart = t.Date.AddHours(t.TimeOfDay.Hours);
            t = tStart;
            while (t < timeTo)
            {
                var dtime = t.ToOADate();
                int errsum = 0;
                int Total = 0;
                try
                {
                    var records = ArchiveCycles.FindAll(c => c.CycleData.CycleDateTime >= t && c.CycleData.CycleDateTime < t.Add(dt));
                    Records.Add(dtime, records);
                    Total = records.Select(s => (s.CycleData.IsSocketActive?[nsocket - 1] ?? false) ? 1 : 0).Sum();
                    errsum = records.Select(s => (s.CycleData.IsSocketsGood[nsocket - 1]) ? 0 : 1).Sum();
                }
                catch { }
                if (MaxTotal < Total) MaxTotal = Total;
                t += dt;
            }

            t = tStart;
            while (t < timeTo)
            {
                var dtime = t.ToOADate();
                int errsum = 0;
                int Total = 0;
                try
                {
                    var records = Records[dtime];// ArchiveCycles.FindAll(c => c.CycleData.CycleDateTime >= t && c.CycleData.CycleDateTime < t.Add(dt));
                                                 //Records.Add(tDouble, records);
                    Total = records.Select(s => (s.CycleData.IsSocketActive?[nsocket - 1] ?? false) ? 1 : 0).Sum();
                    errsum = records.Select(s => (s.CycleData.IsSocketsGood[nsocket - 1]) ? 0 : 1).Sum();
                }
                catch { }
                //if (MaxTotal < Total) MaxTotal = Total;
                //lstArchiveByTime.Add(new Tuple<DateTime, int>(t, Total));

                //var timeString = t.ToString(format);

                var dpTotalMax = new DataPoint();
                dpTotalMax.SetValueXY(dtime, MaxTotal);
                dpTotalMax.Color = Color.Transparent;
                chArchiveByTime.Series[0].Points.Add(dpTotalMax);

                var dpTotal = new DataPoint();
                dpTotal.SetValueXY(dtime, Total);
                dpTotal.Color = Color.Green;
                chArchiveByTime.Series[0].Points.Add(dpTotal);

                var dpError = new DataPoint();
                dpError.SetValueXY(dtime, errsum);
                dpError.Color = Color.OrangeRed;
                chArchiveByTime.Series[0].Points.Add(dpError);
                //chArchiveByTime.Series[0].Points.AddXY(t.ToString(format), Total);


                t += dt;
            }



            var calulatedInterval = (MaxTotal / 5 / 5) * 5;
            if (calulatedInterval == 0) calulatedInterval = MaxTotal >= 5 ? MaxTotal / 5 : 1;
            chArchiveByTime.ChartAreas[0].AxisY.Interval = calulatedInterval;
            chArchiveByTime.ChartAreas[0].AxisY.RoundAxisValues();
            ArchiveFillCycleList(dtArchiveFrom, dtArchiveTo);
        }
        private void nudArchiveSocketNumber_DoubleClick(object sender, EventArgs e)
        {
            var newValue = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog("Введите номер гнезда", false, (int)nudArchiveSocketNumber.Value);
            if (newValue >= 0)
                nudArchiveSocketNumber.Value = newValue;
        }

        private void nudArchiveFrom_DoubleClick(object sender, EventArgs e)
        {
            var newValue = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog("Введите час начала периода", false, (int)nudArchiveFrom.Value);
            if (newValue >= 0)
                nudArchiveFrom.Value = newValue;
        }

        private void nudArchiveTo_DoubleClick(object sender, EventArgs e)
        {
            var newValue = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog("Введите час конца периода", false, (int)nudArchiveTo.Value);
            if (newValue >= 0)
                nudArchiveTo.Value = newValue;
        }
        private void btnArchiveShow_Click(object sender, EventArgs e)
        {
            if (!DS.IsStarted) { MessageBox.Show("Путь к базе данных не существует"); return; }
            ShowSelectedItem();
        }

        public class DisplayCycleData
        {
            public bool IsRemote;
            public CycleData CycleData;
        }

        private void chArchiveByTime_MouseDown(object sender, MouseEventArgs e)
        {
            var result = chArchiveByTime.HitTest(e.X, e.Y);
            if (result.PointIndex >= 0)
            {
                var res = (DataPoint)result.Object;
                if (res != null && !res.IsEmpty)
                {
                    ArchiveFillCycleList(DateTime.FromOADate(res.XValue));
                }
            }
            /*if (result.PointIndex/3 > -1)
            {
                var dte = lstArchiveByTime[result.PointIndex];
                ArchiveFillCycleList(dte.Item1);
            }*/
        }

        private void btnArchiveSelectBad_Click(object sender, EventArgs e)
        {
            if (!DS.IsStarted) { MessageBox.Show("Путь к базе данных не существует"); return; }
            PressedButton(sender as Button);
            LocalArchiveCycles = new List<CycleData>();
            RemoteArchiveCycles = new List<CycleData>();
            ArchiveCycles = new List<DisplayCycleData>();

            dtArchiveFrom = dtpArchiveFrom.Value.Date.AddHours((double)nudArchiveFrom.Value);
            dtArchiveTo = dtpArchiveTo.Value.Date.AddHours((double)nudArchiveTo.Value + 1);
            if (dtArchiveFrom > dtArchiveTo)
            {
                nudArchiveFrom.BackColor = Color.Red;
                nudArchiveTo.BackColor = Color.Red;
                MessageBox.Show("Время выбрано неправильно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudArchiveFrom.BackColor = SystemColors.Window;
                nudArchiveTo.BackColor = SystemColors.Window;
                return;
            }
            nudArchiveFrom.BackColor = SystemColors.Window;
            nudArchiveTo.BackColor = SystemColors.Window;



            LocalArchiveCycles = DS.LocalGetCycles(dtArchiveFrom, dtArchiveTo);
            if (LocalArchiveCycles.Count > 0)
                LocalArchiveCycles = LocalArchiveCycles.FindAll(lc => lc.IsSocketsGood.Any(s => !s));
            RemoteArchiveCycles = DS.RemoteGetCycles(dtArchiveFrom, dtArchiveTo);
            if (RemoteArchiveCycles.Count > 0)
                RemoteArchiveCycles.FindAll(lc => lc.IsSocketsGood.Any(s => !s));


            if (LocalArchiveCycles != null)
                ArchiveCycles.AddRange(LocalArchiveCycles.Select(cd => new DisplayCycleData() { IsRemote = false, CycleData = cd }));
            if (RemoteArchiveCycles != null)
                ArchiveCycles.AddRange(RemoteArchiveCycles.Select(cd => new DisplayCycleData() { IsRemote = true, CycleData = cd }));


            ClearAllOutputs();
            if (ArchiveCycles.Count == 0) return;
            //int[] TotalShow = new int[96];
            int[] Errors = new int[96];
            foreach (var cycle in ArchiveCycles)
            {
                for (int i = 0; i < cycle.CycleData.IsSocketsGood.Length; i++)
                {
                    //if (!cycle.CycleData.IsSocketsGood[i] || cycle.CycleData.SocketsToSave[i])
                    //    TotalShow[i]++;
                    if (!cycle.CycleData.IsSocketsGood[i])
                        Errors[i]++;
                }
            }
            //var TotalMax = TotalShow.Max();
            var maxErrorSum = Errors.Max();
            var calulatedInterval = (maxErrorSum / 5 / 5) * 5;
            if (calulatedInterval == 0) calulatedInterval = maxErrorSum >= 5 ? maxErrorSum / 5 : 1;

            chArchiveSumBad.ChartAreas[0].AxisY.Interval = calulatedInterval;
            chArchiveSumBad.Series[0].Points.Clear();
            for (int i = 0; i < Errors.Length; i++)
            {
                /*var dpTotalMax = new System.Windows.Forms.DataVisualization.Charting.DataPoint(i + 1, TotalMax);
                dpTotalMax.Color = Color.Transparent;
                chArchiveSumBad.Series[0].Points.Add(dpTotalMax);

                var dpTotal = new System.Windows.Forms.DataVisualization.Charting.DataPoint(i + 1, TotalShow[i]);
                dpTotal.Color = Color.Green;
                chArchiveSumBad.Series[0].Points.Add(dpTotal);*/

                var dpError = new System.Windows.Forms.DataVisualization.Charting.DataPoint(i + 1, Errors[i]);
                dpError.Color = Color.OrangeRed;
                chArchiveSumBad.Series[0].Points.Add(dpError);

                /*chArchiveSumBad.Series[0].Points.AddXY(i + 1, TotalMax);
                chArchiveSumBad.Series[0].Points.AddXY(i + 1, TotalShow[i] );
                chArchiveSumBad.Series[0].Points.AddXY(i + 1, Errors[i]);*/
            }
            nudArchiveSocketNumber_ValueChanged(nudArchiveSocketNumber, new EventArgs());
            FillBoxes();
            FillDefectsList();
        }

        public class CycleTag
        {
            public long CycleID;
            public bool IsRemote;
            public CycleTag(long cycleID, bool isRemote)
            {
                CycleID = cycleID;
                IsRemote = isRemote;
            }
        }
    }
}
