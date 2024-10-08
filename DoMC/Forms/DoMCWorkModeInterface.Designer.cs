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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.msWorkingModeMenu = new System.Windows.Forms.MenuStrip();
            this.tsmiStandards = new System.Windows.Forms.ToolStripMenuItem();
            this.miLoadStandard = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateNewStandard = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.miResetStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogsArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.miMainInterfaceLogsArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.miLCBLogsArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.miRDPBLogsArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.miDBLogsArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.miInterfaceLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.miLCBLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.miRDPBLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.miDBLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.miInnerVariables = new System.Windows.Forms.ToolStripMenuItem();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tabWorkAndArchive = new System.Windows.Forms.TabControl();
            this.tbpCurrent = new System.Windows.Forms.TabPage();
            this.lvBoxes = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblTotalDefectCyclesCaption = new System.Windows.Forms.Label();
            this.lblTotalDefectCycles = new System.Windows.Forms.Label();
            this.lblCurrentBoxDefectCyclesCaption = new System.Windows.Forms.Label();
            this.lblCurrentBoxDefectCycles = new System.Windows.Forms.Label();
            this.lblCurrentLastHourSumBadCaption = new System.Windows.Forms.Label();
            this.lblCycleDurationValue = new System.Windows.Forms.Label();
            this.lblCycleDurationCaption = new System.Windows.Forms.Label();
            this.lblErrors = new System.Windows.Forms.Label();
            this.lbCurrentErrors = new System.Windows.Forms.ListBox();
            this.chCurrentLastHourSumBad = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlCurrentSockets = new System.Windows.Forms.Panel();
            this.tbpArchive = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.miSocketsSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.pbStartStop = new DoMC.PressButton();
            this.pbLCB = new DoMC.PressButton();
            this.pbLocalDB = new DoMC.PressButton();
            this.pbRemoteDB = new DoMC.PressButton();
            this.pbRDPB = new DoMC.PressButton();
            this.pbCurrentShowStatistics = new DoMC.PressButton();
            this.msWorkingModeMenu.SuspendLayout();
            this.tabWorkAndArchive.SuspendLayout();
            this.tbpCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chCurrentLastHourSumBad)).BeginInit();
            this.SuspendLayout();
            // 
            // msWorkingModeMenu
            // 
            this.msWorkingModeMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msWorkingModeMenu.AutoSize = false;
            this.msWorkingModeMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.msWorkingModeMenu.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.msWorkingModeMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.msWorkingModeMenu.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.msWorkingModeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStandards,
            this.tsmiStatistics,
            this.tsmiLogsArchive,
            this.tsmiLogs,
            this.miInnerVariables,
            this.miSocketsSettings,
            this.miSettings});
            this.msWorkingModeMenu.Location = new System.Drawing.Point(0, 0);
            this.msWorkingModeMenu.Name = "msWorkingModeMenu";
            this.msWorkingModeMenu.Size = new System.Drawing.Size(1176, 33);
            this.msWorkingModeMenu.TabIndex = 2;
            this.msWorkingModeMenu.Text = "menuStrip1";
            // 
            // tsmiStandards
            // 
            this.tsmiStandards.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLoadStandard,
            this.miCreateNewStandard,
            this.сохранитьToolStripMenuItem});
            this.tsmiStandards.Name = "tsmiStandards";
            this.tsmiStandards.Size = new System.Drawing.Size(99, 29);
            this.tsmiStandards.Text = "Эталоны";
            // 
            // miLoadStandard
            // 
            this.miLoadStandard.Name = "miLoadStandard";
            this.miLoadStandard.Size = new System.Drawing.Size(189, 30);
            this.miLoadStandard.Text = "Загрузить...";
            this.miLoadStandard.Click += new System.EventHandler(this.miLoadStandard_Click);
            // 
            // miCreateNewStandard
            // 
            this.miCreateNewStandard.Name = "miCreateNewStandard";
            this.miCreateNewStandard.Size = new System.Drawing.Size(189, 30);
            this.miCreateNewStandard.Text = "Создать...";
            this.miCreateNewStandard.Click += new System.EventHandler(this.miCreateNewStandard_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(189, 30);
            this.сохранитьToolStripMenuItem.Text = "Сохранить...";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.miSaveStandard_Click);
            // 
            // tsmiStatistics
            // 
            this.tsmiStatistics.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miResetStatistics,
            this.обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem});
            this.tsmiStatistics.Name = "tsmiStatistics";
            this.tsmiStatistics.Size = new System.Drawing.Size(120, 29);
            this.tsmiStatistics.Text = "Статистика";
            // 
            // miResetStatistics
            // 
            this.miResetStatistics.Name = "miResetStatistics";
            this.miResetStatistics.Size = new System.Drawing.Size(492, 30);
            this.miResetStatistics.Text = "Обнулить счетчики гнезд";
            this.miResetStatistics.Click += new System.EventHandler(this.miResetStatistics_Click);
            // 
            // обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem
            // 
            this.обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem.Name = "обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem";
            this.обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem.Size = new System.Drawing.Size(492, 30);
            this.обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem.Text = "Обнулить общий счетчик сброшенных съемов";
            this.обнулитьОбщийСчетчикСброшенныхСъемовToolStripMenuItem.Click += new System.EventHandler(this.miResetCounter_Click);
            // 
            // tsmiLogsArchive
            // 
            this.tsmiLogsArchive.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMainInterfaceLogsArchive,
            this.miLCBLogsArchive,
            this.miRDPBLogsArchive,
            this.miDBLogsArchive});
            this.tsmiLogsArchive.Name = "tsmiLogsArchive";
            this.tsmiLogsArchive.Size = new System.Drawing.Size(181, 29);
            this.tsmiLogsArchive.Text = "Архивы журналов";
            // 
            // miMainInterfaceLogsArchive
            // 
            this.miMainInterfaceLogsArchive.Name = "miMainInterfaceLogsArchive";
            this.miMainInterfaceLogsArchive.Size = new System.Drawing.Size(427, 30);
            this.miMainInterfaceLogsArchive.Text = "Папка журналов работы программы";
            this.miMainInterfaceLogsArchive.Click += new System.EventHandler(this.miMainInterfaceLogsArchive_Click);
            // 
            // miLCBLogsArchive
            // 
            this.miLCBLogsArchive.Name = "miLCBLogsArchive";
            this.miLCBLogsArchive.Size = new System.Drawing.Size(427, 30);
            this.miLCBLogsArchive.Text = "Папка журналов БУС";
            this.miLCBLogsArchive.Click += new System.EventHandler(this.miLCBLogsArchive_Click);
            // 
            // miRDPBLogsArchive
            // 
            this.miRDPBLogsArchive.Name = "miRDPBLogsArchive";
            this.miRDPBLogsArchive.Size = new System.Drawing.Size(427, 30);
            this.miRDPBLogsArchive.Text = "Папка журналов бракёра";
            this.miRDPBLogsArchive.Click += new System.EventHandler(this.miRDPBLogsArchive_Click);
            // 
            // miDBLogsArchive
            // 
            this.miDBLogsArchive.Name = "miDBLogsArchive";
            this.miDBLogsArchive.Size = new System.Drawing.Size(427, 30);
            this.miDBLogsArchive.Text = "Папка журналов базы данных и архива";
            this.miDBLogsArchive.Click += new System.EventHandler(this.miDBLogsArchive_Click);
            // 
            // tsmiLogs
            // 
            this.tsmiLogs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miInterfaceLogs,
            this.miLCBLogs,
            this.miRDPBLogs,
            this.miDBLogs});
            this.tsmiLogs.Name = "tsmiLogs";
            this.tsmiLogs.Size = new System.Drawing.Size(104, 29);
            this.tsmiLogs.Text = "Журналы";
            // 
            // miInterfaceLogs
            // 
            this.miInterfaceLogs.Name = "miInterfaceLogs";
            this.miInterfaceLogs.Size = new System.Drawing.Size(343, 30);
            this.miInterfaceLogs.Text = "Работа программы";
            this.miInterfaceLogs.Click += new System.EventHandler(this.miInterfaceLogs_Click);
            // 
            // miLCBLogs
            // 
            this.miLCBLogs.Name = "miLCBLogs";
            this.miLCBLogs.Size = new System.Drawing.Size(343, 30);
            this.miLCBLogs.Text = "Работы БУС";
            this.miLCBLogs.Click += new System.EventHandler(this.miLCBLogs_Click);
            // 
            // miRDPBLogs
            // 
            this.miRDPBLogs.Name = "miRDPBLogs";
            this.miRDPBLogs.Size = new System.Drawing.Size(343, 30);
            this.miRDPBLogs.Text = "Работа бракёра";
            this.miRDPBLogs.Click += new System.EventHandler(this.miRDPBLogs_Click);
            // 
            // miDBLogs
            // 
            this.miDBLogs.Name = "miDBLogs";
            this.miDBLogs.Size = new System.Drawing.Size(343, 30);
            this.miDBLogs.Text = "Работа базы данных и архива";
            this.miDBLogs.Click += new System.EventHandler(this.miDBLogs_Click);
            // 
            // miInnerVariables
            // 
            this.miInnerVariables.Name = "miInnerVariables";
            this.miInnerVariables.Size = new System.Drawing.Size(242, 29);
            this.miInnerVariables.Text = "Внутренние переменные";
            this.miInnerVariables.Click += new System.EventHandler(this.miInnerVariables_Click);
            // 
            // miSettings
            // 
            this.miSettings.Name = "miSettings";
            this.miSettings.Size = new System.Drawing.Size(117, 29);
            this.miSettings.Text = "Настройки";
            this.miSettings.Click += new System.EventHandler(this.miSettings_Click);
            // 
            // tabWorkAndArchive
            // 
            this.tabWorkAndArchive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabWorkAndArchive.Controls.Add(this.tbpCurrent);
            this.tabWorkAndArchive.Controls.Add(this.tbpArchive);
            this.tabWorkAndArchive.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabWorkAndArchive.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabWorkAndArchive.Location = new System.Drawing.Point(126, 36);
            this.tabWorkAndArchive.Name = "tabWorkAndArchive";
            this.tabWorkAndArchive.SelectedIndex = 0;
            this.tabWorkAndArchive.Size = new System.Drawing.Size(1054, 600);
            this.tabWorkAndArchive.TabIndex = 3;
            this.tabWorkAndArchive.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabWorkAndArchive_DrawItem);
            // 
            // tbpCurrent
            // 
            this.tbpCurrent.BackColor = System.Drawing.SystemColors.Control;
            this.tbpCurrent.Controls.Add(this.lvBoxes);
            this.tbpCurrent.Controls.Add(this.lblTotalDefectCyclesCaption);
            this.tbpCurrent.Controls.Add(this.lblTotalDefectCycles);
            this.tbpCurrent.Controls.Add(this.lblCurrentBoxDefectCyclesCaption);
            this.tbpCurrent.Controls.Add(this.lblCurrentBoxDefectCycles);
            this.tbpCurrent.Controls.Add(this.lblCurrentLastHourSumBadCaption);
            this.tbpCurrent.Controls.Add(this.pbLCB);
            this.tbpCurrent.Controls.Add(this.pbLocalDB);
            this.tbpCurrent.Controls.Add(this.pbRemoteDB);
            this.tbpCurrent.Controls.Add(this.pbRDPB);
            this.tbpCurrent.Controls.Add(this.lblCycleDurationValue);
            this.tbpCurrent.Controls.Add(this.lblCycleDurationCaption);
            this.tbpCurrent.Controls.Add(this.pbCurrentShowStatistics);
            this.tbpCurrent.Controls.Add(this.lblErrors);
            this.tbpCurrent.Controls.Add(this.lbCurrentErrors);
            this.tbpCurrent.Controls.Add(this.chCurrentLastHourSumBad);
            this.tbpCurrent.Controls.Add(this.pnlCurrentSockets);
            this.tbpCurrent.Location = new System.Drawing.Point(4, 47);
            this.tbpCurrent.Name = "tbpCurrent";
            this.tbpCurrent.Padding = new System.Windows.Forms.Padding(3);
            this.tbpCurrent.Size = new System.Drawing.Size(1046, 549);
            this.tbpCurrent.TabIndex = 0;
            this.tbpCurrent.Text = "Текущий";
            // 
            // lvBoxes
            // 
            this.lvBoxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvBoxes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvBoxes.HideSelection = false;
            this.lvBoxes.Location = new System.Drawing.Point(767, 429);
            this.lvBoxes.Name = "lvBoxes";
            this.lvBoxes.Size = new System.Drawing.Size(271, 274);
            this.lvBoxes.TabIndex = 17;
            this.lvBoxes.UseCompatibleStateImageBehavior = false;
            this.lvBoxes.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Время короба";
            this.columnHeader1.Width = 320;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Плохих съемов";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Сторона";
            this.columnHeader3.Width = 200;
            // 
            // lblTotalDefectCyclesCaption
            // 
            this.lblTotalDefectCyclesCaption.AutoSize = true;
            this.lblTotalDefectCyclesCaption.Location = new System.Drawing.Point(760, 387);
            this.lblTotalDefectCyclesCaption.Name = "lblTotalDefectCyclesCaption";
            this.lblTotalDefectCyclesCaption.Size = new System.Drawing.Size(354, 39);
            this.lblTotalDefectCyclesCaption.TabIndex = 16;
            this.lblTotalDefectCyclesCaption.Text = "Сброшенных циклов:";
            // 
            // lblTotalDefectCycles
            // 
            this.lblTotalDefectCycles.BackColor = System.Drawing.SystemColors.Control;
            this.lblTotalDefectCycles.Location = new System.Drawing.Point(1417, 376);
            this.lblTotalDefectCycles.Name = "lblTotalDefectCycles";
            this.lblTotalDefectCycles.Size = new System.Drawing.Size(109, 60);
            this.lblTotalDefectCycles.TabIndex = 15;
            this.lblTotalDefectCycles.Text = "0";
            this.lblTotalDefectCycles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentBoxDefectCyclesCaption
            // 
            this.lblCurrentBoxDefectCyclesCaption.AutoSize = true;
            this.lblCurrentBoxDefectCyclesCaption.Location = new System.Drawing.Point(760, 338);
            this.lblCurrentBoxDefectCyclesCaption.Name = "lblCurrentBoxDefectCyclesCaption";
            this.lblCurrentBoxDefectCyclesCaption.Size = new System.Drawing.Size(651, 39);
            this.lblCurrentBoxDefectCyclesCaption.TabIndex = 14;
            this.lblCurrentBoxDefectCyclesCaption.Text = "Сброшенных циклов в текущем коробе:";
            // 
            // lblCurrentBoxDefectCycles
            // 
            this.lblCurrentBoxDefectCycles.BackColor = System.Drawing.SystemColors.Control;
            this.lblCurrentBoxDefectCycles.Location = new System.Drawing.Point(1417, 333);
            this.lblCurrentBoxDefectCycles.Name = "lblCurrentBoxDefectCycles";
            this.lblCurrentBoxDefectCycles.Size = new System.Drawing.Size(109, 49);
            this.lblCurrentBoxDefectCycles.TabIndex = 13;
            this.lblCurrentBoxDefectCycles.Text = "0";
            this.lblCurrentBoxDefectCycles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentLastHourSumBadCaption
            // 
            this.lblCurrentLastHourSumBadCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentLastHourSumBadCaption.AutoSize = true;
            this.lblCurrentLastHourSumBadCaption.Location = new System.Drawing.Point(6, 289);
            this.lblCurrentLastHourSumBadCaption.Name = "lblCurrentLastHourSumBadCaption";
            this.lblCurrentLastHourSumBadCaption.Size = new System.Drawing.Size(574, 39);
            this.lblCurrentLastHourSumBadCaption.TabIndex = 12;
            this.lblCurrentLastHourSumBadCaption.Text = "Счетчик ошибок за последний час:";
            // 
            // lblCycleDurationValue
            // 
            this.lblCycleDurationValue.AutoSize = true;
            this.lblCycleDurationValue.Location = new System.Drawing.Point(640, 62);
            this.lblCycleDurationValue.Name = "lblCycleDurationValue";
            this.lblCycleDurationValue.Size = new System.Drawing.Size(36, 39);
            this.lblCycleDurationValue.TabIndex = 10;
            this.lblCycleDurationValue.Text = "0";
            // 
            // lblCycleDurationCaption
            // 
            this.lblCycleDurationCaption.AutoSize = true;
            this.lblCycleDurationCaption.Location = new System.Drawing.Point(407, 62);
            this.lblCycleDurationCaption.Name = "lblCycleDurationCaption";
            this.lblCycleDurationCaption.Size = new System.Drawing.Size(232, 39);
            this.lblCycleDurationCaption.TabIndex = 9;
            this.lblCycleDurationCaption.Text = "Время цикла:";
            // 
            // lblErrors
            // 
            this.lblErrors.AutoSize = true;
            this.lblErrors.Location = new System.Drawing.Point(760, 12);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(153, 39);
            this.lblErrors.TabIndex = 3;
            this.lblErrors.Text = "Ошибки:";
            // 
            // lbCurrentErrors
            // 
            this.lbCurrentErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCurrentErrors.BackColor = System.Drawing.SystemColors.Control;
            this.lbCurrentErrors.FormattingEnabled = true;
            this.lbCurrentErrors.ItemHeight = 38;
            this.lbCurrentErrors.Location = new System.Drawing.Point(767, 62);
            this.lbCurrentErrors.Name = "lbCurrentErrors";
            this.lbCurrentErrors.Size = new System.Drawing.Size(271, 270);
            this.lbCurrentErrors.TabIndex = 2;
            // 
            // chCurrentLastHourSumBad
            // 
            this.chCurrentLastHourSumBad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.AxisX.Maximum = 100D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.MinorGrid.Enabled = true;
            chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea2.AxisX.MinorTickMark.Enabled = true;
            chartArea2.AxisX.MinorTickMark.LineColor = System.Drawing.Color.LightGray;
            chartArea2.Name = "ChartArea1";
            this.chCurrentLastHourSumBad.ChartAreas.Add(chartArea2);
            this.chCurrentLastHourSumBad.Location = new System.Drawing.Point(6, 331);
            this.chCurrentLastHourSumBad.Name = "chCurrentLastHourSumBad";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.chCurrentLastHourSumBad.Series.Add(series2);
            this.chCurrentLastHourSumBad.Size = new System.Drawing.Size(1034, 212);
            this.chCurrentLastHourSumBad.TabIndex = 1;
            this.chCurrentLastHourSumBad.Text = "chart1";
            // 
            // pnlCurrentSockets
            // 
            this.pnlCurrentSockets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlCurrentSockets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCurrentSockets.Location = new System.Drawing.Point(6, 62);
            this.pnlCurrentSockets.Name = "pnlCurrentSockets";
            this.pnlCurrentSockets.Size = new System.Drawing.Size(395, 212);
            this.pnlCurrentSockets.TabIndex = 0;
            this.pnlCurrentSockets.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCurrentSockets_Paint);
            // 
            // tbpArchive
            // 
            this.tbpArchive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbpArchive.Location = new System.Drawing.Point(4, 47);
            this.tbpArchive.Name = "tbpArchive";
            this.tbpArchive.Padding = new System.Windows.Forms.Padding(3);
            this.tbpArchive.Size = new System.Drawing.Size(1046, 549);
            this.tbpArchive.TabIndex = 1;
            this.tbpArchive.Text = "Архив";
            this.tbpArchive.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // miSocketsSettings
            // 
            this.miSocketsSettings.Name = "miSocketsSettings";
            this.miSocketsSettings.Size = new System.Drawing.Size(168, 29);
            this.miSocketsSettings.Text = "Настройки гнезд";
            this.miSocketsSettings.Click += new System.EventHandler(this.miSocketsSettings_Click);
            // 
            // pbStartStop
            // 
            this.pbStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pbStartStop.IsPressed = false;
            this.pbStartStop.Location = new System.Drawing.Point(6, 38);
            this.pbStartStop.Name = "pbStartStop";
            this.pbStartStop.Size = new System.Drawing.Size(120, 92);
            this.pbStartStop.TabIndex = 4;
            this.pbStartStop.Text = "Пуск";
            this.pbStartStop.UseVisualStyleBackColor = true;
            this.pbStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // pbLCB
            // 
            this.pbLCB.IsPressed = false;
            this.pbLCB.Location = new System.Drawing.Point(414, 455);
            this.pbLCB.Name = "pbLCB";
            this.pbLCB.Size = new System.Drawing.Size(190, 111);
            this.pbLCB.TabIndex = 11;
            this.pbLCB.Text = "БУС";
            this.pbLCB.UseVisualStyleBackColor = true;
            this.pbLCB.Visible = false;
            this.pbLCB.Click += new System.EventHandler(this.pbDevices_Click);
            // 
            // pbLocalDB
            // 
            this.pbLocalDB.IsPressed = false;
            this.pbLocalDB.Location = new System.Drawing.Point(414, 221);
            this.pbLocalDB.Name = "pbLocalDB";
            this.pbLocalDB.Size = new System.Drawing.Size(190, 111);
            this.pbLocalDB.TabIndex = 3;
            this.pbLocalDB.Text = "База данных";
            this.pbLocalDB.UseVisualStyleBackColor = true;
            this.pbLocalDB.Click += new System.EventHandler(this.pbDevices_Click);
            // 
            // pbRemoteDB
            // 
            this.pbRemoteDB.IsPressed = false;
            this.pbRemoteDB.Location = new System.Drawing.Point(414, 338);
            this.pbRemoteDB.Name = "pbRemoteDB";
            this.pbRemoteDB.Size = new System.Drawing.Size(190, 111);
            this.pbRemoteDB.TabIndex = 4;
            this.pbRemoteDB.Text = "Архив";
            this.pbRemoteDB.UseVisualStyleBackColor = true;
            this.pbRemoteDB.Click += new System.EventHandler(this.pbDevices_Click);
            // 
            // pbRDPB
            // 
            this.pbRDPB.IsPressed = false;
            this.pbRDPB.Location = new System.Drawing.Point(414, 104);
            this.pbRDPB.Name = "pbRDPB";
            this.pbRDPB.Size = new System.Drawing.Size(190, 111);
            this.pbRDPB.TabIndex = 2;
            this.pbRDPB.Text = "Бракёр";
            this.pbRDPB.UseVisualStyleBackColor = true;
            this.pbRDPB.Click += new System.EventHandler(this.pbDevices_Click);
            // 
            // pbCurrentShowStatistics
            // 
            this.pbCurrentShowStatistics.IsPressed = false;
            this.pbCurrentShowStatistics.Location = new System.Drawing.Point(6, 6);
            this.pbCurrentShowStatistics.Name = "pbCurrentShowStatistics";
            this.pbCurrentShowStatistics.Size = new System.Drawing.Size(395, 50);
            this.pbCurrentShowStatistics.TabIndex = 7;
            this.pbCurrentShowStatistics.Text = "Показать статистику";
            this.pbCurrentShowStatistics.UseVisualStyleBackColor = true;
            this.pbCurrentShowStatistics.Click += new System.EventHandler(this.pbCurrentShowStatistics_Click);
            // 
            // DoMCWorkModeInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 648);
            this.Controls.Add(this.pbStartStop);
            this.Controls.Add(this.tabWorkAndArchive);
            this.Controls.Add(this.msWorkingModeMenu);
            this.Name = "DoMCWorkModeInterface";
            this.Text = "Рабочий режим ПМК";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DoMCWorkModeInterface_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DoMCWorkModeInterface_FormClosed);
            this.Resize += new System.EventHandler(this.DoMCWorkModeInterface_Resize);
            this.msWorkingModeMenu.ResumeLayout(false);
            this.msWorkingModeMenu.PerformLayout();
            this.tabWorkAndArchive.ResumeLayout(false);
            this.tbpCurrent.ResumeLayout(false);
            this.tbpCurrent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chCurrentLastHourSumBad)).EndInit();
            this.ResumeLayout(false);

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
        private PressButton pbCurrentShowStatistics;
        private PressButton pbRemoteDB;
        private PressButton pbLocalDB;
        private PressButton pbRDPB;
        private PressButton pbStartStop;
        private System.Windows.Forms.Label lblCycleDurationValue;
        private System.Windows.Forms.Label lblCycleDurationCaption;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogsArchive;
        private System.Windows.Forms.ToolStripMenuItem miMainInterfaceLogsArchive;
        private System.Windows.Forms.ToolStripMenuItem miLCBLogsArchive;
        private System.Windows.Forms.ToolStripMenuItem miRDPBLogsArchive;
        private System.Windows.Forms.ToolStripMenuItem miDBLogsArchive;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogs;
        private System.Windows.Forms.ToolStripMenuItem miInterfaceLogs;
        private System.Windows.Forms.ToolStripMenuItem miLCBLogs;
        private System.Windows.Forms.ToolStripMenuItem miRDPBLogs;
        private System.Windows.Forms.ToolStripMenuItem miDBLogs;
        private System.Windows.Forms.ToolStripMenuItem miInnerVariables;
        private PressButton pbLCB;
        private System.Windows.Forms.Label lblCurrentLastHourSumBadCaption;
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
    }
}