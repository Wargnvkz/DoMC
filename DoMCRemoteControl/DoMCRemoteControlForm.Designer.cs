namespace DoMCRemoteControl
{
    partial class DoMCRemoteControlForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            chEvents = new System.Windows.Forms.DataVisualization.Charting.Chart();
            timer1 = new System.Windows.Forms.Timer(components);
            lblEventsByTimeCaption = new Label();
            lvDefects = new ListView();
            chDefectCounter = new ColumnHeader();
            chDefectTime = new ColumnHeader();
            chDefectSocket = new ColumnHeader();
            lblDefectedCyclesCaption = new Label();
            lvBoxes = new ListView();
            chBoxCounter = new ColumnHeader();
            chBoxTime = new ColumnHeader();
            chBoxBadCycles = new ColumnHeader();
            lblBoxesCaption = new Label();
            pnlCurrentSockets = new Panel();
            lblNoConnection = new Label();
            cbCurrentShowStatistics = new CheckBox();
            lblWorkingStatusCaption = new Label();
            lblCycleDurationCaption = new Label();
            btnStart = new Button();
            btnStop = new Button();
            lblWorkingStatus = new Label();
            lblCycleDuration = new Label();
            lblDefectsInLastHour = new Label();
            lblErrorCounter = new Label();
            menuStrip1 = new MenuStrip();
            tsmiSettings = new ToolStripMenuItem();
            tsmiStatistics = new ToolStripMenuItem();
            tsmiSocketCounter = new ToolStripMenuItem();
            tsmiResetCycles = new ToolStripMenuItem();
            lblCurrentBoxDefects = new Label();
            lblCurrentBoxDefectsCaption = new Label();
            ((System.ComponentModel.ISupportInitialize)chEvents).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // chEvents
            // 
            chEvents.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartArea1.AxisX.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.LineColor = Color.LightGray;
            chartArea1.AxisX.MinorTickMark.Enabled = true;
            chartArea1.AxisX.MinorTickMark.LineColor = Color.LightGray;
            chartArea1.AxisX2.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            chEvents.ChartAreas.Add(chartArea1);
            chEvents.Location = new Point(108, 469);
            chEvents.Margin = new Padding(4, 3, 4, 3);
            chEvents.Name = "chEvents";
            chEvents.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            chEvents.PaletteCustomColors = new Color[]
    {
    Color.OrangeRed
    };
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            chEvents.Series.Add(series1);
            chEvents.Size = new Size(1333, 235);
            chEvents.TabIndex = 32;
            chEvents.Text = "chart1";
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 5000;
            timer1.Tick += timer_Tick;
            // 
            // lblEventsByTimeCaption
            // 
            lblEventsByTimeCaption.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblEventsByTimeCaption.AutoSize = true;
            lblEventsByTimeCaption.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblEventsByTimeCaption.Location = new Point(108, 427);
            lblEventsByTimeCaption.Margin = new Padding(4, 0, 4, 0);
            lblEventsByTimeCaption.Name = "lblEventsByTimeCaption";
            lblEventsByTimeCaption.Size = new Size(652, 39);
            lblEventsByTimeCaption.TabIndex = 33;
            lblEventsByTimeCaption.Text = "Распределение измерений по времени:";
            // 
            // lvDefects
            // 
            lvDefects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lvDefects.Columns.AddRange(new ColumnHeader[] { chDefectCounter, chDefectTime, chDefectSocket });
            lvDefects.Location = new Point(1121, 60);
            lvDefects.Margin = new Padding(4, 3, 4, 3);
            lvDefects.Name = "lvDefects";
            lvDefects.Size = new Size(320, 403);
            lvDefects.TabIndex = 40;
            lvDefects.UseCompatibleStateImageBehavior = false;
            lvDefects.View = View.Details;
            // 
            // chDefectCounter
            // 
            chDefectCounter.Text = "№";
            chDefectCounter.Width = 40;
            // 
            // chDefectTime
            // 
            chDefectTime.Text = "Время дефекта";
            chDefectTime.Width = 170;
            // 
            // chDefectSocket
            // 
            chDefectSocket.Text = "Гнезда";
            chDefectSocket.Width = 80;
            // 
            // lblDefectedCyclesCaption
            // 
            lblDefectedCyclesCaption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDefectedCyclesCaption.AutoSize = true;
            lblDefectedCyclesCaption.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblDefectedCyclesCaption.Location = new Point(1121, 27);
            lblDefectedCyclesCaption.Margin = new Padding(4, 0, 4, 0);
            lblDefectedCyclesCaption.Name = "lblDefectedCyclesCaption";
            lblDefectedCyclesCaption.Size = new Size(322, 26);
            lblDefectedCyclesCaption.TabIndex = 39;
            lblDefectedCyclesCaption.Text = "Дефектные съемы за период:";
            // 
            // lvBoxes
            // 
            lvBoxes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lvBoxes.Columns.AddRange(new ColumnHeader[] { chBoxCounter, chBoxTime, chBoxBadCycles });
            lvBoxes.Location = new Point(799, 60);
            lvBoxes.Margin = new Padding(4, 3, 4, 3);
            lvBoxes.Name = "lvBoxes";
            lvBoxes.Size = new Size(314, 403);
            lvBoxes.TabIndex = 38;
            lvBoxes.UseCompatibleStateImageBehavior = false;
            lvBoxes.View = View.Details;
            // 
            // chBoxCounter
            // 
            chBoxCounter.Text = "№";
            chBoxCounter.Width = 40;
            // 
            // chBoxTime
            // 
            chBoxTime.Text = "Время короба";
            chBoxTime.Width = 170;
            // 
            // chBoxBadCycles
            // 
            chBoxBadCycles.Text = "Плохих съемов";
            chBoxBadCycles.Width = 80;
            // 
            // lblBoxesCaption
            // 
            lblBoxesCaption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBoxesCaption.AutoSize = true;
            lblBoxesCaption.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblBoxesCaption.Location = new Point(799, 27);
            lblBoxesCaption.Margin = new Padding(4, 0, 4, 0);
            lblBoxesCaption.Name = "lblBoxesCaption";
            lblBoxesCaption.Size = new Size(202, 26);
            lblBoxesCaption.TabIndex = 37;
            lblBoxesCaption.Text = "Короба за период:";
            // 
            // pnlCurrentSockets
            // 
            pnlCurrentSockets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlCurrentSockets.BorderStyle = BorderStyle.FixedSingle;
            pnlCurrentSockets.Location = new Point(108, 67);
            pnlCurrentSockets.Margin = new Padding(4, 3, 4, 3);
            pnlCurrentSockets.Name = "pnlCurrentSockets";
            pnlCurrentSockets.Size = new Size(406, 312);
            pnlCurrentSockets.TabIndex = 41;
            pnlCurrentSockets.Paint += pnlCurrentSockets_Paint;
            // 
            // lblNoConnection
            // 
            lblNoConnection.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblNoConnection.AutoSize = true;
            lblNoConnection.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblNoConnection.ForeColor = Color.Red;
            lblNoConnection.Location = new Point(522, 142);
            lblNoConnection.Name = "lblNoConnection";
            lblNoConnection.Size = new Size(238, 37);
            lblNoConnection.TabIndex = 52;
            lblNoConnection.Text = "Нет связи с ПМК";
            lblNoConnection.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbCurrentShowStatistics
            // 
            cbCurrentShowStatistics.Appearance = Appearance.Button;
            cbCurrentShowStatistics.CheckAlign = ContentAlignment.MiddleCenter;
            cbCurrentShowStatistics.Checked = true;
            cbCurrentShowStatistics.CheckState = CheckState.Checked;
            cbCurrentShowStatistics.Font = new Font("Segoe UI", 12F);
            cbCurrentShowStatistics.Location = new Point(108, 27);
            cbCurrentShowStatistics.Name = "cbCurrentShowStatistics";
            cbCurrentShowStatistics.Size = new Size(406, 35);
            cbCurrentShowStatistics.TabIndex = 42;
            cbCurrentShowStatistics.Text = "Показать статистику";
            cbCurrentShowStatistics.TextAlign = ContentAlignment.MiddleCenter;
            cbCurrentShowStatistics.UseVisualStyleBackColor = true;
            cbCurrentShowStatistics.CheckedChanged += cbCurrentShowStatistics_CheckedChanged;
            // 
            // lblWorkingStatusCaption
            // 
            lblWorkingStatusCaption.AutoSize = true;
            lblWorkingStatusCaption.Font = new Font("Segoe UI", 14F);
            lblWorkingStatusCaption.Location = new Point(520, 27);
            lblWorkingStatusCaption.Name = "lblWorkingStatusCaption";
            lblWorkingStatusCaption.Size = new Size(141, 25);
            lblWorkingStatusCaption.TabIndex = 43;
            lblWorkingStatusCaption.Text = "Статус работы:";
            // 
            // lblCycleDurationCaption
            // 
            lblCycleDurationCaption.AutoSize = true;
            lblCycleDurationCaption.Font = new Font("Segoe UI", 14F);
            lblCycleDurationCaption.Location = new Point(520, 65);
            lblCycleDurationCaption.Name = "lblCycleDurationCaption";
            lblCycleDurationCaption.Size = new Size(193, 25);
            lblCycleDurationCaption.TabIndex = 44;
            lblCycleDurationCaption.Text = "Длительность цикла:";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 27);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(90, 48);
            btnStart.TabIndex = 45;
            btnStart.Text = "Запустить";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Visible = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(12, 81);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(90, 48);
            btnStop.TabIndex = 46;
            btnStop.Text = "Остановить";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Visible = false;
            btnStop.Click += btnStop_Click;
            // 
            // lblWorkingStatus
            // 
            lblWorkingStatus.AutoSize = true;
            lblWorkingStatus.Font = new Font("Segoe UI", 14F);
            lblWorkingStatus.Location = new Point(667, 27);
            lblWorkingStatus.Name = "lblWorkingStatus";
            lblWorkingStatus.Size = new Size(20, 25);
            lblWorkingStatus.TabIndex = 47;
            lblWorkingStatus.Text = "-";
            // 
            // lblCycleDuration
            // 
            lblCycleDuration.AutoSize = true;
            lblCycleDuration.Font = new Font("Segoe UI", 14F);
            lblCycleDuration.Location = new Point(719, 65);
            lblCycleDuration.Name = "lblCycleDuration";
            lblCycleDuration.Size = new Size(20, 25);
            lblCycleDuration.TabIndex = 48;
            lblCycleDuration.Text = "-";
            // 
            // lblDefectsInLastHour
            // 
            lblDefectsInLastHour.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblDefectsInLastHour.AutoSize = true;
            lblDefectsInLastHour.Font = new Font("Segoe UI", 12F);
            lblDefectsInLastHour.Location = new Point(387, 382);
            lblDefectsInLastHour.Name = "lblDefectsInLastHour";
            lblDefectsInLastHour.Size = new Size(16, 21);
            lblDefectsInLastHour.TabIndex = 50;
            lblDefectsInLastHour.Text = "-";
            // 
            // lblErrorCounter
            // 
            lblErrorCounter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblErrorCounter.AutoSize = true;
            lblErrorCounter.Font = new Font("Segoe UI", 12F);
            lblErrorCounter.Location = new Point(108, 382);
            lblErrorCounter.Name = "lblErrorCounter";
            lblErrorCounter.Size = new Size(273, 21);
            lblErrorCounter.TabIndex = 49;
            lblErrorCounter.Text = "Счетчик дефектов за последний час:";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { tsmiSettings, tsmiStatistics });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1454, 24);
            menuStrip1.TabIndex = 51;
            menuStrip1.Text = "menuStrip1";
            // 
            // tsmiSettings
            // 
            tsmiSettings.Name = "tsmiSettings";
            tsmiSettings.Size = new Size(79, 20);
            tsmiSettings.Text = "Настройки";
            tsmiSettings.Click += tsmiSettings_Click;
            // 
            // tsmiStatistics
            // 
            tsmiStatistics.DropDownItems.AddRange(new ToolStripItem[] { tsmiSocketCounter, tsmiResetCycles });
            tsmiStatistics.Name = "tsmiStatistics";
            tsmiStatistics.Size = new Size(80, 20);
            tsmiStatistics.Text = "Статистика";
            // 
            // tsmiSocketCounter
            // 
            tsmiSocketCounter.Name = "tsmiSocketCounter";
            tsmiSocketCounter.Size = new Size(295, 22);
            tsmiSocketCounter.Text = "Обнулить счетчик гнезд";
            tsmiSocketCounter.Click += tsmiSocketCounter_Click;
            // 
            // tsmiResetCycles
            // 
            tsmiResetCycles.Name = "tsmiResetCycles";
            tsmiResetCycles.Size = new Size(295, 22);
            tsmiResetCycles.Text = "Обнулить счетчик сброшенных съемов";
            tsmiResetCycles.Click += tsmiResetCycles_Click;
            // 
            // lblCurrentBoxDefects
            // 
            lblCurrentBoxDefects.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCurrentBoxDefects.AutoSize = true;
            lblCurrentBoxDefects.Font = new Font("Segoe UI", 12F);
            lblCurrentBoxDefects.Location = new Point(409, 406);
            lblCurrentBoxDefects.Name = "lblCurrentBoxDefects";
            lblCurrentBoxDefects.Size = new Size(16, 21);
            lblCurrentBoxDefects.TabIndex = 54;
            lblCurrentBoxDefects.Text = "-";
            // 
            // lblCurrentBoxDefectsCaption
            // 
            lblCurrentBoxDefectsCaption.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCurrentBoxDefectsCaption.AutoSize = true;
            lblCurrentBoxDefectsCaption.Font = new Font("Segoe UI", 12F);
            lblCurrentBoxDefectsCaption.Location = new Point(108, 403);
            lblCurrentBoxDefectsCaption.Name = "lblCurrentBoxDefectsCaption";
            lblCurrentBoxDefectsCaption.Size = new Size(295, 21);
            lblCurrentBoxDefectsCaption.TabIndex = 53;
            lblCurrentBoxDefectsCaption.Text = "Счетчик дефектов в последнем коробе:";
            // 
            // DoMCRemoteControlForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1454, 714);
            Controls.Add(lblCurrentBoxDefects);
            Controls.Add(lblCurrentBoxDefectsCaption);
            Controls.Add(lblNoConnection);
            Controls.Add(lblDefectsInLastHour);
            Controls.Add(lblErrorCounter);
            Controls.Add(lblCycleDuration);
            Controls.Add(lblWorkingStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(lblCycleDurationCaption);
            Controls.Add(lblWorkingStatusCaption);
            Controls.Add(cbCurrentShowStatistics);
            Controls.Add(pnlCurrentSockets);
            Controls.Add(lvDefects);
            Controls.Add(lblDefectedCyclesCaption);
            Controls.Add(lvBoxes);
            Controls.Add(lblBoxesCaption);
            Controls.Add(lblEventsByTimeCaption);
            Controls.Add(chEvents);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "DoMCRemoteControlForm";
            Text = "Панель управления ПМК";
            WindowState = FormWindowState.Maximized;
            Load += DoMCRemoteControlForm_Load;
            Resize += DoMCRemoteControlForm_Resize;
            ((System.ComponentModel.ISupportInitialize)chEvents).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chEvents;
        private System.Windows.Forms.Timer timer1;
        private Label lblEventsByTimeCaption;
        private ListView lvDefects;
        private ColumnHeader chDefectTime;
        private ColumnHeader chDefectSocket;
        private Label lblDefectedCyclesCaption;
        private ListView lvBoxes;
        private ColumnHeader chBoxTime;
        private ColumnHeader chBoxBadCycles;
        private Label lblBoxesCaption;
        private Panel pnlCurrentSockets;
        private CheckBox cbCurrentShowStatistics;
        private Label lblWorkingStatusCaption;
        private Label lblCycleDurationCaption;
        private Button btnStart;
        private Button btnStop;
        private Label lblWorkingStatus;
        private Label lblCycleDuration;
        private Label lblDefectsInLastHour;
        private Label lblErrorCounter;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem tsmiSettings;
        private ToolStripMenuItem tsmiStatistics;
        private ToolStripMenuItem tsmiSocketCounter;
        private ToolStripMenuItem tsmiResetCycles;
        private Label lblNoConnection;
        private ColumnHeader chDefectCounter;
        private ColumnHeader chBoxCounter;
        private Label lblCurrentBoxDefects;
        private Label lblCurrentBoxDefectsCaption;
    }
}
