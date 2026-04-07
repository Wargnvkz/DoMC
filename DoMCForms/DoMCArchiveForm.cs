using DoMCLib.Classes;
using DoMCLib.DB;
using DoMCLib.Tools;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DoMCForms
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
            var showPreformImages = new DoMCForms.ShowPreformImages();

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
            var newValue = DoMCForms.Dialogs.DigitalInput.ShowIntegerDialog("Введите номер гнезда", false, (int)nudArchiveSocketNumber.Value);
            if (newValue >= 0)
                nudArchiveSocketNumber.Value = newValue;
        }

        private void nudArchiveFrom_DoubleClick(object sender, EventArgs e)
        {
            var newValue = DoMCForms.Dialogs.DigitalInput.ShowIntegerDialog("Введите час начала периода", false, (int)nudArchiveFrom.Value);
            if (newValue >= 0)
                nudArchiveFrom.Value = newValue;
        }

        private void nudArchiveTo_DoubleClick(object sender, EventArgs e)
        {
            var newValue = DoMCForms.Dialogs.DigitalInput.ShowIntegerDialog("Введите час конца периода", false, (int)nudArchiveTo.Value);
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
                RemoteArchiveCycles = RemoteArchiveCycles.FindAll(lc => lc.IsSocketsGood.Any(s => !s));


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

        private void btnReport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                var dtArchiveFrom = dtpArchiveFrom.Value.Date.AddHours((double)nudArchiveFrom.Value);
                var dtArchiveTo = dtpArchiveTo.Value.Date.AddHours((double)nudArchiveTo.Value + 1);

                /*List<DisplayCycleData> ArchiveCycles = new List<DisplayCycleData>();
                List<CycleData> LocalArchiveCycles;
                List<CycleData> RemoteArchiveCycles;


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
                */

                List<BoxDB> boxes = new List<BoxDB>();


                var localboxes = DS.LocalGetBox(dtArchiveFrom, dtArchiveTo).OrderBy(b => b.CompletedTime).ToList();
                if (localboxes != null)
                    boxes.AddRange(localboxes);
                var remoteboxes = DS.RemoteGetBox(dtArchiveFrom, dtArchiveTo).OrderBy(b => b.CompletedTime).ToList();
                if (remoteboxes != null)
                    boxes.AddRange(remoteboxes);

                boxes = boxes.OrderBy(b => b.CompletedTime).Where(b => b.CompletedTime != DateTime.MinValue).ToList();


                List<ReportStatistics> report = new List<ReportStatistics>();
                var ArchiveGroup = boxes.GroupBy(b => new Shift(b.CompletedTime));
                foreach (var groupElement in ArchiveGroup)
                {
                    var shiftStat = new ReportStatistics();
                    var shift = groupElement.Key;
                    shiftStat.Shift = shift;
                    var boxList = groupElement.OrderBy(c => c.CompletedTime).ToList();
                    foreach (var cycle in boxList)
                    {
                        shiftStat.TotalBoxCount++;
                        shiftStat.DefectsInBoxCount += cycle.BadCyclesCount;
                    }

                    report.Add(shiftStat);
                }
                var monthReport = report.GroupBy(r => new { r.Shift.ShiftDate.Year, r.Shift.ShiftDate.Month }).Select(g => new MonthReportStatistics
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalBoxCount = g.Sum(s => s.TotalBoxCount),
                    DefectsInBoxCount = g.Sum(s => s.DefectsInBoxCount)
                }).ToList();
                StringBuilder sb = new StringBuilder();
                //Отчет по сменам в csv файле
                // Смена;Дата смены;Всего коробов;Всего дефектных съемов
                sb.AppendLine("Смена;Дата смены;Всего коробов;Всего дефектных съемов;Процент дефектных съемов");
                foreach (var r in report)
                {
                    sb.AppendLine($"{r.Shift.GetShiftNumber()}{(r.Shift.IsNight ? "Н" : "Д")};{r.Shift.ShiftDate:dd-MM-yyyy};{r.TotalBoxCount};{r.DefectsInBoxCount};{r.DefectsInBoxCount / (r.TotalBoxCount * 105d) * 100d:F3}%");
                }
                sb.AppendLine();
                //отчет по месяцам в csv файле
                sb.AppendLine("Год;Месяц;Всего коробов;Всего дефектных съемов;Процент дефектных съемов");
                foreach (var r in monthReport)
                {
                    sb.AppendLine($"{r.Year};{r.Month};{r.TotalBoxCount};{r.DefectsInBoxCount};{r.DefectsInBoxCount / (r.TotalBoxCount * 105d) * 100d:F3}%");
                }


                #region Statistics
                /*
                var data = new double[boxes.Count, 8];
                for (int i = 0; i < boxes.Count; i++)
                {
                    var shift = new Shift(boxes[i].CompletedTime);     // смена с 8 до 20 и с 20 до 8
                    data[i, 0] = shift.GetShiftNumber();              // Номер смены
                    data[i, 1] = shift.IsNight ? 0 : 1;              // Ночная смена?
                    data[i, 2] = boxes[i].CompletedTime.Hour;              // Час производства
                    data[i, 3] = boxes[i].CompletedTime.Minute;            // Минута (важна для циклов!)
                    if (i > 0)
                    {
                        var boxFillTime = Math.Round((boxes[i].CompletedTime - boxes[i - 1].CompletedTime).TotalMinutes);
                        data[i, 4] = Math.Min(boxFillTime, 120);
                    }
                    else
                        data[i, 4] = 120;
                    data[i, 5] = boxes[i].CompletedTime.DayOfWeek >= DayOfWeek.Saturday ? 1 : 0;    // выходной?

                    // Близость к пересменке (0 - начало, 1 - конец)
                    data[i, 6] = (boxes[i].CompletedTime - shift.ShiftStartsAt()).Hours;
                    data[i, 7] = boxes[i].BadCyclesCount;                  // ЦЕЛЕВАЯ ПЕРЕМЕННАЯ (наш "брак")
                }
                int targetIndex = 7;
                var matrix = DenseMatrix.OfArray(data);
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    var col = matrix.Column(j);
                    double mean = col.Mean();
                    double stdDev = col.StandardDeviation();

                    if (stdDev > 0)
                    {
                        matrix.SetColumn(j, (col - mean) / stdDev);
                    }
                }
                var correlationMatrix = (matrix.Transpose() * matrix) / (matrix.RowCount - 1);
                var evd = correlationMatrix.Evd();

                // 3. Получаем результат
                Vector<System.Numerics.Complex> values = evd.EigenValues;

                // 3. Берем вектор самой главной компоненты (последний столбец)
                var mainVector = evd.EigenVectors.Column(targetIndex);

                sb.AppendLine();
                sb.AppendLine($"Собственные значения");

                // 4. Смотрим веса (влияние) каждого поля в этой компоненте
                string[] labels = { "Shift", "IsNight", "Hour", "Minute", "SecondsFromLastBox", "IsWeekend", "RateFromShiftStart", "Defects" };
                for (int i = 0; i < labels.Length; i++)
                {
                    sb.AppendLine($"{labels[i]}: {mainVector[i]:F4}");
                }

                // 2. Индекс колонки с процентом брака (твое поле №8)

                var rateColumn = matrix.Column(targetIndex);

                sb.AppendLine($"--- Корреляция с Процентом брака ({labels[targetIndex]}) ---");

                for (int i = 0; i < labels.Length; i++)
                {
                    if (i == targetIndex) continue;

                    // Считаем корреляцию Пирсона между текущим столбцом и браком
                    double correlation = Correlation.Pearson(matrix.Column(i), rateColumn);

                    // Интерпретируем результат
                    string influence = correlation > 0 ? "Растет брак" : "Снижается брак";
                    if (Math.Abs(correlation) < 0.1) influence = "Почти не влияет";

                    sb.AppendLine($"{labels[i].PadRight(12)}: {correlation:F4} ({influence})");
                }

                sb.AppendLine($"--- Распределение ({labels[targetIndex]}) ---");

                for (int i = 0; i < labels.Length; i++)
                {
                    if (i == targetIndex) continue;
                    // в словарь в ключ записываем значение поля i, а в значение: сумму процентов брака для этих записей
                    Dictionary<double, double> distribution = new Dictionary<double, double>();
                    for (int j = 0; j < data.GetLength(0); j++)
                    {
                        double key = data[j, i];
                        double value = data[j, targetIndex];
                        if (!distribution.ContainsKey(key))
                            distribution[key] = 0;
                        distribution[key] += value;
                    }
                    // сортируем словарь по ключу и записываем результат в sb построчно
                    foreach (var kvp in distribution.OrderBy(k => k.Key))
                    {
                        sb.AppendLine($"{labels[i].PadRight(12)} = {kvp.Key:F4} -> {kvp.Value:F4}");
                    }

                }
                */
                #endregion Statistics



                //var eigenvalues=EigenSimple.GetPrincipalEigen(dataToAnalize);

                this.Cursor = Cursors.Default;
                //выбираем файл для сохранения
                var sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Отчет сохранен");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}");
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }

    public class ReportStatistics
    {
        /// <summary>
        /// Смена
        /// </summary>
        public Shift Shift;
        /// <summary>
        /// Всего коробов выпущено
        /// </summary>
        public int TotalBoxCount;
        /// <summary>
        /// количество девектных съемов в коробе
        /// </summary>
        public int DefectsInBoxCount;
    }
    public class MonthReportStatistics
    {
        /// <summary>
        /// Год
        /// </summary>
        public int Year;
        /// <summary>
        /// Месяц
        /// </summary>
        public int Month;
        /// <summary>
        /// Всего коробов выпущено
        /// </summary>
        public int TotalBoxCount;
        /// <summary>
        /// количество девектных съемов в коробе
        /// </summary>
        public int DefectsInBoxCount;
    }

    /*public class EigenSimple
    {
        public static (double eigenvalue, double[] eigenvector) GetPrincipalEigen(double[,] matrix, int iterations = 100)
        {
            Center(ref matrix);
            int n = matrix.GetLength(0);
            double[] v = new double[n];
            for (int i = 0; i < n; i++) v[i] = 1.0; // Начальное приближение

            double eigenvalue = 0;

            for (int iter = 0; iter < iterations; iter++)
            {
                // 1. Умножаем матрицу на вектор: w = A * v
                double[] w = new double[n];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        w[i] += matrix[i, j] * v[j];

                // 2. Находим норму (длину) вектора — это наше приближенное lambda
                eigenvalue = 0;
                for (int i = 0; i < n; i++) eigenvalue += w[i] * w[i];
                eigenvalue = Math.Sqrt(eigenvalue);

                // 3. Нормализуем вектор для следующей итерации
                for (int i = 0; i < n; i++) v[i] = w[i] / eigenvalue;
            }

            return (eigenvalue, v);
        }

        public static void Center(ref double[,] values)
        {
            for(int j=0; j<values.GetLength(1); j++)
            {
                double sum = 0;
                for (int i = 0; i < values.GetLength(0); i++)
                    sum += values[i, j];
                double mean = sum / values.GetLength(0);
                for (int i = 0; i < values.GetLength(0); i++)
                    values[i, j] -= mean;
            }
        }
    }*/
}
