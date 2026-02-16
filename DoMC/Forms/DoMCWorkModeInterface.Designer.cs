namespace DoMC
{
    partial class DoMCWorkModeInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            msWorkingModeMenu = new MenuStrip();
            tsmiStandards = new ToolStripMenuItem();
            miLoadStandard = new ToolStripMenuItem();
            miCreateNewStandard = new ToolStripMenuItem();
            сохранитьToolStripMenuItem = new ToolStripMenuItem();
            tsmiStatistics = new ToolStripMenuItem();
            miResetStatistics = new ToolStripMenuItem();
            обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem = new ToolStripMenuItem();
            tsmiLogsArchive = new ToolStripMenuItem();
            miSocketsSettings = new ToolStripMenuItem();
            miSettings = new ToolStripMenuItem();
            tabWorkAndArchive = new TabControl();
            tbpCurrent = new TabPage();
            lvBoxes = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            lblTotalDefectCyclesCaption = new Label();
            lblTotalDefectCycles = new Label();
            lblCurrentBoxDefectCyclesCaption = new Label();
            lblCurrentBoxDefectCycles = new Label();
            pbLCB = new DoMC.UserControls.PressButton();
            pbLocalDB = new DoMC.UserControls.PressButton();
            pbRemoteDB = new DoMC.UserControls.PressButton();
            pbRDPB = new DoMC.UserControls.PressButton();
            lblCycleDurationValue = new Label();
            lblCycleDurationCaption = new Label();
            pbCurrentShowStatistics = new DoMC.UserControls.PressButton();
            lblErrors = new Label();
            lbCurrentErrors = new ListBox();
            pnlCurrentSockets = new Panel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            nudAveragePriod = new NumericUpDown();
            lblAveragePeriod = new Label();
            nudAverageSocket = new NumericUpDown();
            lblAvergaSocketCaption = new Label();
            chAverageOfSocketByTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            tabPage2 = new TabPage();
            chCurrentLastHourSumBad = new System.Windows.Forms.DataVisualization.Charting.Chart();
            tbpArchive = new TabPage();
            timer1 = new System.Windows.Forms.Timer(components);
            pbStartStop = new DoMC.UserControls.PressButton();
            ssFooter = new StatusStrip();
            lblFooterStep = new ToolStripStatusLabel();
            msWorkingModeMenu.SuspendLayout();
            tabWorkAndArchive.SuspendLayout();
            tbpCurrent.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAveragePriod).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAverageSocket).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chAverageOfSocketByTime).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chCurrentLastHourSumBad).BeginInit();
            ssFooter.SuspendLayout();
            SuspendLayout();
            // 
            // msWorkingModeMenu
            // 
            msWorkingModeMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            msWorkingModeMenu.AutoSize = false;
            msWorkingModeMenu.Dock = DockStyle.None;
            msWorkingModeMenu.Font = new Font("Segoe UI", 14F);
            msWorkingModeMenu.GripStyle = ToolStripGripStyle.Visible;
            msWorkingModeMenu.ImageScalingSize = new Size(28, 28);
            msWorkingModeMenu.Items.AddRange(new ToolStripItem[] { tsmiStandards, tsmiStatistics, tsmiLogsArchive, miSocketsSettings, miSettings });
            msWorkingModeMenu.Location = new Point(0, 0);
            msWorkingModeMenu.Name = "msWorkingModeMenu";
            msWorkingModeMenu.Padding = new Padding(4, 2, 0, 2);
            msWorkingModeMenu.Size = new Size(1794, 38);
            msWorkingModeMenu.TabIndex = 2;
            msWorkingModeMenu.Text = "menuStrip1";
            // 
            // tsmiStandards
            // 
            tsmiStandards.DropDownItems.AddRange(new ToolStripItem[] { miLoadStandard, miCreateNewStandard, сохранитьToolStripMenuItem });
            tsmiStandards.Name = "tsmiStandards";
            tsmiStandards.Size = new Size(99, 34);
            tsmiStandards.Text = "Эталоны";
            // 
            // miLoadStandard
            // 
            miLoadStandard.Name = "miLoadStandard";
            miLoadStandard.Size = new Size(189, 30);
            miLoadStandard.Text = "Загрузить...";
            miLoadStandard.Click += miLoadStandard_Click;
            // 
            // miCreateNewStandard
            // 
            miCreateNewStandard.Name = "miCreateNewStandard";
            miCreateNewStandard.Size = new Size(189, 30);
            miCreateNewStandard.Text = "Создать...";
            miCreateNewStandard.Click += miCreateNewStandard_Click;
            // 
            // сохранитьToolStripMenuItem
            // 
            сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            сохранитьToolStripMenuItem.Size = new Size(189, 30);
            сохранитьToolStripMenuItem.Text = "Сохранить...";
            сохранитьToolStripMenuItem.Click += miSaveStandard_Click;
            // 
            // tsmiStatistics
            // 
            tsmiStatistics.DropDownItems.AddRange(new ToolStripItem[] { miResetStatistics, обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem });
            tsmiStatistics.Name = "tsmiStatistics";
            tsmiStatistics.Size = new Size(120, 34);
            tsmiStatistics.Text = "Статистика";
            // 
            // miResetStatistics
            // 
            miResetStatistics.Name = "miResetStatistics";
            miResetStatistics.Size = new Size(492, 30);
            miResetStatistics.Text = "Обнулить счетчики гнезд";
            miResetStatistics.Click += miResetStatistics_Click;
            // 
            // обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem
            // 
            обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem.Name = "обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem";
            обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem.Size = new Size(492, 30);
            обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem.Text = "Обнулить общий счетчик сброшенных съемов";
            обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem.Click += miResetCounter_Click;
            // 
            // tsmiLogsArchive
            // 
            tsmiLogsArchive.Name = "tsmiLogsArchive";
            tsmiLogsArchive.Size = new Size(169, 34);
            tsmiLogsArchive.Text = "Папка журналов";
            tsmiLogsArchive.Click += tsmiLogsArchive_Click;
            // 
            // miSocketsSettings
            // 
            miSocketsSettings.Name = "miSocketsSettings";
            miSocketsSettings.Size = new Size(168, 34);
            miSocketsSettings.Text = "Настройки гнезд";
            miSocketsSettings.Click += miSocketsSettings_Click;
            // 
            // miSettings
            // 
            miSettings.Name = "miSettings";
            miSettings.Size = new Size(117, 34);
            miSettings.Text = "Настройки";
            miSettings.Click += miSettings_Click;
            // 
            // tabWorkAndArchive
            // 
            tabWorkAndArchive.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabWorkAndArchive.Controls.Add(tbpCurrent);
            tabWorkAndArchive.Controls.Add(tbpArchive);
            tabWorkAndArchive.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabWorkAndArchive.Font = new Font("Microsoft Sans Serif", 15F);
            tabWorkAndArchive.Location = new Point(147, 42);
            tabWorkAndArchive.Margin = new Padding(4, 3, 4, 3);
            tabWorkAndArchive.Name = "tabWorkAndArchive";
            tabWorkAndArchive.SelectedIndex = 0;
            tabWorkAndArchive.Size = new Size(1652, 865);
            tabWorkAndArchive.TabIndex = 3;
            tabWorkAndArchive.DrawItem += tabWorkAndArchive_DrawItem;
            // 
            // tbpCurrent
            // 
            tbpCurrent.BackColor = SystemColors.Control;
            tbpCurrent.Controls.Add(lvBoxes);
            tbpCurrent.Controls.Add(lblTotalDefectCyclesCaption);
            tbpCurrent.Controls.Add(lblTotalDefectCycles);
            tbpCurrent.Controls.Add(lblCurrentBoxDefectCyclesCaption);
            tbpCurrent.Controls.Add(lblCurrentBoxDefectCycles);
            tbpCurrent.Controls.Add(pbLCB);
            tbpCurrent.Controls.Add(pbLocalDB);
            tbpCurrent.Controls.Add(pbRemoteDB);
            tbpCurrent.Controls.Add(pbRDPB);
            tbpCurrent.Controls.Add(lblCycleDurationValue);
            tbpCurrent.Controls.Add(lblCycleDurationCaption);
            tbpCurrent.Controls.Add(pbCurrentShowStatistics);
            tbpCurrent.Controls.Add(lblErrors);
            tbpCurrent.Controls.Add(lbCurrentErrors);
            tbpCurrent.Controls.Add(pnlCurrentSockets);
            tbpCurrent.Controls.Add(tabControl1);
            tbpCurrent.Location = new Point(4, 34);
            tbpCurrent.Margin = new Padding(4, 3, 4, 3);
            tbpCurrent.Name = "tbpCurrent";
            tbpCurrent.Padding = new Padding(4, 3, 4, 3);
            tbpCurrent.Size = new Size(1644, 827);
            tbpCurrent.TabIndex = 0;
            tbpCurrent.Text = "Текущий";
            // 
            // lvBoxes
            // 
            lvBoxes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lvBoxes.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvBoxes.Location = new Point(884, 335);
            lvBoxes.Margin = new Padding(4, 3, 4, 3);
            lvBoxes.Name = "lvBoxes";
            lvBoxes.Size = new Size(738, 188);
            lvBoxes.TabIndex = 17;
            lvBoxes.UseCompatibleStateImageBehavior = false;
            lvBoxes.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Время короба";
            columnHeader1.Width = 320;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Плохих съемов";
            columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Сторона";
            columnHeader3.Width = 200;
            // 
            // lblTotalDefectCyclesCaption
            // 
            lblTotalDefectCyclesCaption.AutoSize = true;
            lblTotalDefectCyclesCaption.Location = new Point(884, 307);
            lblTotalDefectCyclesCaption.Margin = new Padding(4, 0, 4, 0);
            lblTotalDefectCyclesCaption.Name = "lblTotalDefectCyclesCaption";
            lblTotalDefectCyclesCaption.Size = new Size(208, 25);
            lblTotalDefectCyclesCaption.TabIndex = 16;
            lblTotalDefectCyclesCaption.Text = "Сброшенных циклов:";
            // 
            // lblTotalDefectCycles
            // 
            lblTotalDefectCycles.BackColor = SystemColors.Control;
            lblTotalDefectCycles.Location = new Point(1277, 302);
            lblTotalDefectCycles.Margin = new Padding(4, 0, 4, 0);
            lblTotalDefectCycles.Name = "lblTotalDefectCycles";
            lblTotalDefectCycles.Size = new Size(67, 34);
            lblTotalDefectCycles.TabIndex = 15;
            lblTotalDefectCycles.Text = "0";
            lblTotalDefectCycles.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCurrentBoxDefectCyclesCaption
            // 
            lblCurrentBoxDefectCyclesCaption.AutoSize = true;
            lblCurrentBoxDefectCyclesCaption.Location = new Point(884, 277);
            lblCurrentBoxDefectCyclesCaption.Margin = new Padding(4, 0, 4, 0);
            lblCurrentBoxDefectCyclesCaption.Name = "lblCurrentBoxDefectCyclesCaption";
            lblCurrentBoxDefectCyclesCaption.Size = new Size(385, 25);
            lblCurrentBoxDefectCyclesCaption.TabIndex = 14;
            lblCurrentBoxDefectCyclesCaption.Text = "Сброшенных циклов в текущем коробе:";
            // 
            // lblCurrentBoxDefectCycles
            // 
            lblCurrentBoxDefectCycles.BackColor = SystemColors.Control;
            lblCurrentBoxDefectCycles.Location = new Point(1277, 271);
            lblCurrentBoxDefectCycles.Margin = new Padding(4, 0, 4, 0);
            lblCurrentBoxDefectCycles.Name = "lblCurrentBoxDefectCycles";
            lblCurrentBoxDefectCycles.Size = new Size(67, 36);
            lblCurrentBoxDefectCycles.TabIndex = 13;
            lblCurrentBoxDefectCycles.Text = "0";
            lblCurrentBoxDefectCycles.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pbLCB
            // 
            pbLCB.IsPressed = false;
            pbLCB.Location = new Point(483, 525);
            pbLCB.Margin = new Padding(4, 3, 4, 3);
            pbLCB.Name = "pbLCB";
            pbLCB.Size = new Size(222, 128);
            pbLCB.TabIndex = 11;
            pbLCB.Text = "БУС";
            pbLCB.Visible = false;
            pbLCB.Click += pbDevices_Click;
            // 
            // pbLocalDB
            // 
            pbLocalDB.IsPressed = false;
            pbLocalDB.Location = new Point(483, 255);
            pbLocalDB.Margin = new Padding(4, 3, 4, 3);
            pbLocalDB.Name = "pbLocalDB";
            pbLocalDB.Size = new Size(222, 128);
            pbLocalDB.TabIndex = 3;
            pbLocalDB.Text = "База данных";
            pbLocalDB.Click += pbDevices_Click;
            // 
            // pbRemoteDB
            // 
            pbRemoteDB.IsPressed = false;
            pbRemoteDB.Location = new Point(483, 390);
            pbRemoteDB.Margin = new Padding(4, 3, 4, 3);
            pbRemoteDB.Name = "pbRemoteDB";
            pbRemoteDB.Size = new Size(222, 128);
            pbRemoteDB.TabIndex = 4;
            pbRemoteDB.Text = "Архив";
            pbRemoteDB.Visible = false;
            pbRemoteDB.Click += pbDevices_Click;
            // 
            // pbRDPB
            // 
            pbRDPB.IsPressed = false;
            pbRDPB.Location = new Point(483, 120);
            pbRDPB.Margin = new Padding(4, 3, 4, 3);
            pbRDPB.Name = "pbRDPB";
            pbRDPB.Size = new Size(222, 128);
            pbRDPB.TabIndex = 2;
            pbRDPB.Text = "Бракёр";
            pbRDPB.Click += pbDevices_Click;
            // 
            // lblCycleDurationValue
            // 
            lblCycleDurationValue.AutoSize = true;
            lblCycleDurationValue.Location = new Point(670, 72);
            lblCycleDurationValue.Margin = new Padding(4, 0, 4, 0);
            lblCycleDurationValue.Name = "lblCycleDurationValue";
            lblCycleDurationValue.Size = new Size(23, 25);
            lblCycleDurationValue.TabIndex = 10;
            lblCycleDurationValue.Text = "0";
            // 
            // lblCycleDurationCaption
            // 
            lblCycleDurationCaption.AutoSize = true;
            lblCycleDurationCaption.Location = new Point(475, 72);
            lblCycleDurationCaption.Margin = new Padding(4, 0, 4, 0);
            lblCycleDurationCaption.Name = "lblCycleDurationCaption";
            lblCycleDurationCaption.Size = new Size(140, 25);
            lblCycleDurationCaption.TabIndex = 9;
            lblCycleDurationCaption.Text = "Время цикла:";
            // 
            // pbCurrentShowStatistics
            // 
            pbCurrentShowStatistics.IsPressed = false;
            pbCurrentShowStatistics.Location = new Point(7, 7);
            pbCurrentShowStatistics.Margin = new Padding(4, 3, 4, 3);
            pbCurrentShowStatistics.Name = "pbCurrentShowStatistics";
            pbCurrentShowStatistics.Size = new Size(411, 58);
            pbCurrentShowStatistics.TabIndex = 7;
            pbCurrentShowStatistics.Text = "Показать статистику";
            pbCurrentShowStatistics.Click += pbCurrentShowStatistics_Click;
            // 
            // lblErrors
            // 
            lblErrors.AutoSize = true;
            lblErrors.Location = new Point(884, 7);
            lblErrors.Margin = new Padding(4, 0, 4, 0);
            lblErrors.Name = "lblErrors";
            lblErrors.Size = new Size(92, 25);
            lblErrors.TabIndex = 3;
            lblErrors.Text = "Ошибки:";
            // 
            // lbCurrentErrors
            // 
            lbCurrentErrors.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lbCurrentErrors.BackColor = SystemColors.Control;
            lbCurrentErrors.FormattingEnabled = true;
            lbCurrentErrors.ItemHeight = 25;
            lbCurrentErrors.Location = new Point(884, 35);
            lbCurrentErrors.Margin = new Padding(4, 3, 4, 3);
            lbCurrentErrors.Name = "lbCurrentErrors";
            lbCurrentErrors.Size = new Size(738, 229);
            lbCurrentErrors.TabIndex = 2;
            // 
            // pnlCurrentSockets
            // 
            pnlCurrentSockets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlCurrentSockets.BorderStyle = BorderStyle.FixedSingle;
            pnlCurrentSockets.Location = new Point(7, 72);
            pnlCurrentSockets.Margin = new Padding(4, 3, 4, 3);
            pnlCurrentSockets.Name = "pnlCurrentSockets";
            pnlCurrentSockets.Size = new Size(411, 430);
            pnlCurrentSockets.TabIndex = 0;
            pnlCurrentSockets.Paint += pnlCurrentSockets_Paint;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(7, 524);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1636, 300);
            tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(nudAveragePriod);
            tabPage1.Controls.Add(lblAveragePeriod);
            tabPage1.Controls.Add(nudAverageSocket);
            tabPage1.Controls.Add(lblAvergaSocketCaption);
            tabPage1.Controls.Add(chAverageOfSocketByTime);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1628, 262);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Прозрачность по гнездам";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // nudAveragePriod
            // 
            nudAveragePriod.Location = new Point(431, 12);
            nudAveragePriod.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            nudAveragePriod.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudAveragePriod.Name = "nudAveragePriod";
            nudAveragePriod.Size = new Size(93, 30);
            nudAveragePriod.TabIndex = 6;
            nudAveragePriod.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudAveragePriod.ValueChanged += nudAverageParameter_ValueChanged;
            // 
            // lblAveragePeriod
            // 
            lblAveragePeriod.AutoSize = true;
            lblAveragePeriod.Location = new Point(316, 14);
            lblAveragePeriod.Name = "lblAveragePeriod";
            lblAveragePeriod.Size = new Size(109, 25);
            lblAveragePeriod.TabIndex = 5;
            lblAveragePeriod.Text = "Период, ч:";
            // 
            // nudAverageSocket
            // 
            nudAverageSocket.Location = new Point(161, 12);
            nudAverageSocket.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudAverageSocket.Name = "nudAverageSocket";
            nudAverageSocket.Size = new Size(93, 30);
            nudAverageSocket.TabIndex = 4;
            nudAverageSocket.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudAverageSocket.ValueChanged += nudAverageParameter_ValueChanged;
            // 
            // lblAvergaSocketCaption
            // 
            lblAvergaSocketCaption.AutoSize = true;
            lblAvergaSocketCaption.Location = new Point(6, 14);
            lblAvergaSocketCaption.Name = "lblAvergaSocketCaption";
            lblAvergaSocketCaption.Size = new Size(149, 25);
            lblAvergaSocketCaption.TabIndex = 3;
            lblAvergaSocketCaption.Text = "Номер гнезда:";
            // 
            // chAverageOfSocketByTime
            // 
            chAverageOfSocketByTime.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartArea1.AxisX.LabelStyle.Format = "HH:mm";
            chartArea1.AxisX.Maximum = 100D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.LineColor = Color.LightGray;
            chartArea1.AxisX.MinorTickMark.Enabled = true;
            chartArea1.AxisX.MinorTickMark.LineColor = Color.LightGray;
            chartArea1.Name = "ChartArea1";
            chAverageOfSocketByTime.ChartAreas.Add(chartArea1);
            chAverageOfSocketByTime.Location = new Point(4, 42);
            chAverageOfSocketByTime.Margin = new Padding(4, 3, 4, 3);
            chAverageOfSocketByTime.Name = "chAverageOfSocketByTime";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            chAverageOfSocketByTime.Series.Add(series1);
            chAverageOfSocketByTime.Size = new Size(1620, 217);
            chAverageOfSocketByTime.TabIndex = 2;
            chAverageOfSocketByTime.Text = "chart1";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(chCurrentLastHourSumBad);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1628, 272);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Счетчик ошибок за последний час";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // chCurrentLastHourSumBad
            // 
            chCurrentLastHourSumBad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartArea2.AxisX.Maximum = 100D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.MinorGrid.Enabled = true;
            chartArea2.AxisX.MinorGrid.LineColor = Color.LightGray;
            chartArea2.AxisX.MinorTickMark.Enabled = true;
            chartArea2.AxisX.MinorTickMark.LineColor = Color.LightGray;
            chartArea2.Name = "ChartArea1";
            chCurrentLastHourSumBad.ChartAreas.Add(chartArea2);
            chCurrentLastHourSumBad.Location = new Point(1, 13);
            chCurrentLastHourSumBad.Margin = new Padding(4, 3, 4, 3);
            chCurrentLastHourSumBad.Name = "chCurrentLastHourSumBad";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            chCurrentLastHourSumBad.Series.Add(series2);
            chCurrentLastHourSumBad.Size = new Size(1620, 233);
            chCurrentLastHourSumBad.TabIndex = 1;
            chCurrentLastHourSumBad.Text = "chart1";
            // 
            // tbpArchive
            // 
            tbpArchive.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tbpArchive.Location = new Point(4, 34);
            tbpArchive.Margin = new Padding(4, 3, 4, 3);
            tbpArchive.Name = "tbpArchive";
            tbpArchive.Padding = new Padding(4, 3, 4, 3);
            tbpArchive.Size = new Size(1644, 827);
            tbpArchive.TabIndex = 1;
            tbpArchive.Text = "Архив";
            tbpArchive.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // pbStartStop
            // 
            pbStartStop.BackColor = SystemColors.Control;
            pbStartStop.Font = new Font("Microsoft Sans Serif", 25F);
            pbStartStop.IsPressed = false;
            pbStartStop.Location = new Point(7, 44);
            pbStartStop.Margin = new Padding(4, 3, 4, 3);
            pbStartStop.Name = "pbStartStop";
            pbStartStop.Size = new Size(140, 106);
            pbStartStop.TabIndex = 4;
            pbStartStop.Text = "Пуск";
            pbStartStop.UseVisualStyleBackColor = false;
            pbStartStop.Click += btnStartStop_Click;
            // 
            // ssFooter
            // 
            ssFooter.Items.AddRange(new ToolStripItem[] { lblFooterStep });
            ssFooter.Location = new Point(0, 910);
            ssFooter.Name = "ssFooter";
            ssFooter.Size = new Size(1799, 22);
            ssFooter.TabIndex = 5;
            ssFooter.Text = "statusStrip1";
            // 
            // lblFooterStep
            // 
            lblFooterStep.AutoSize = false;
            lblFooterStep.Name = "lblFooterStep";
            lblFooterStep.Size = new Size(500, 17);
            lblFooterStep.Text = "Текущий шаг:";
            lblFooterStep.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DoMCWorkModeInterface
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1799, 932);
            Controls.Add(ssFooter);
            Controls.Add(pbStartStop);
            Controls.Add(tabWorkAndArchive);
            Controls.Add(msWorkingModeMenu);
            Margin = new Padding(4, 3, 4, 3);
            Name = "DoMCWorkModeInterface";
            Text = "Рабочий режим ПМК";
            WindowState = FormWindowState.Maximized;
            FormClosing += DoMCWorkModeInterface_FormClosing;
            FormClosed += DoMCWorkModeInterface_FormClosed;
            Resize += DoMCWorkModeInterface_Resize;
            msWorkingModeMenu.ResumeLayout(false);
            msWorkingModeMenu.PerformLayout();
            tabWorkAndArchive.ResumeLayout(false);
            tbpCurrent.ResumeLayout(false);
            tbpCurrent.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudAveragePriod).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAverageSocket).EndInit();
            ((System.ComponentModel.ISupportInitialize)chAverageOfSocketByTime).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chCurrentLastHourSumBad).EndInit();
            ssFooter.ResumeLayout(false);
            ssFooter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.MenuStrip msWorkingModeMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiStandards;
        private System.Windows.Forms.ToolStripMenuItem miLoadStandard;
        private System.Windows.Forms.TabControl tabWorkAndArchive;
        private System.Windows.Forms.TabPage tbpCurrent;
        private System.Windows.Forms.TabPage tbpArchive;
        private System.Windows.Forms.Panel pnlCurrentSockets;
        private System.Windows.Forms.DataVisualization.Charting.Chart chCurrentLastHourSumBad;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblErrors;
        private System.Windows.Forms.ListBox lbCurrentErrors;
        private System.Windows.Forms.ToolStripMenuItem tsmiStatistics;
        private System.Windows.Forms.ToolStripMenuItem miResetStatistics;
        private DoMC.UserControls.PressButton pbCurrentShowStatistics;
        private DoMC.UserControls.PressButton pbRemoteDB;
        private DoMC.UserControls.PressButton pbLocalDB;
        private DoMC.UserControls.PressButton pbRDPB;
        private DoMC.UserControls.PressButton pbStartStop;
        private DoMC.UserControls.PressButton pbLCB;
        private System.Windows.Forms.Label lblCycleDurationValue;
        private System.Windows.Forms.Label lblCycleDurationCaption;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogsArchive;
        private System.Windows.Forms.ToolStripMenuItem обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem;
        private System.Windows.Forms.Label lblCurrentBoxDefectCyclesCaption;
        private System.Windows.Forms.Label lblCurrentBoxDefectCycles;
        private System.Windows.Forms.Label lblTotalDefectCyclesCaption;
        private System.Windows.Forms.Label lblTotalDefectCycles;
        private System.Windows.Forms.ListView lvBoxes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miCreateNewStandard;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miSocketsSettings;
        private StatusStrip ssFooter;
        private ToolStripStatusLabel lblFooterStep;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private NumericUpDown nudAverageSocket;
        private Label lblAvergaSocketCaption;
        private System.Windows.Forms.DataVisualization.Charting.Chart chAverageOfSocketByTime;
        private NumericUpDown nudAveragePriod;
        private Label lblAveragePeriod;
    }
}