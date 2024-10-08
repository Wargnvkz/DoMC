namespace DoMC
{
    partial class DoMCArchiveForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.nudArchiveTo = new System.Windows.Forms.NumericUpDown();
            this.lblArchiveDateEnding = new System.Windows.Forms.Label();
            this.btnArchiveShow = new System.Windows.Forms.Button();
            this.lvArchiveSavedSockets = new System.Windows.Forms.ListView();
            this.chCycleTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chArchiveByTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblArchiveSocket = new System.Windows.Forms.Label();
            this.nudArchiveSocketNumber = new System.Windows.Forms.NumericUpDown();
            this.btnArchiveSelect = new System.Windows.Forms.Button();
            this.chArchiveSumBad = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.nudArchiveFrom = new System.Windows.Forms.NumericUpDown();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpArchiveTo = new System.Windows.Forms.DateTimePicker();
            this.dtpArchiveFrom = new System.Windows.Forms.DateTimePicker();
            this.btnArchiveSelectBad = new System.Windows.Forms.Button();
            this.lblSocketHoursCaption = new System.Windows.Forms.Label();
            this.chEvents = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblEventsByTimeCaption = new System.Windows.Forms.Label();
            this.lblBoxesCaption = new System.Windows.Forms.Label();
            this.lvBoxes = new System.Windows.Forms.ListView();
            this.chBoxTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBoxBadCycles = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvDefects = new System.Windows.Forms.ListView();
            this.chDefectTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDefectSocket = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudArchiveTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chArchiveByTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArchiveSocketNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chArchiveSumBad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArchiveFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // nudArchiveTo
            // 
            this.nudArchiveTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudArchiveTo.Location = new System.Drawing.Point(1080, 1);
            this.nudArchiveTo.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nudArchiveTo.Name = "nudArchiveTo";
            this.nudArchiveTo.Size = new System.Drawing.Size(108, 53);
            this.nudArchiveTo.TabIndex = 19;
            this.nudArchiveTo.DoubleClick += new System.EventHandler(this.nudArchiveTo_DoubleClick);
            // 
            // lblArchiveDateEnding
            // 
            this.lblArchiveDateEnding.AutoSize = true;
            this.lblArchiveDateEnding.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblArchiveDateEnding.Location = new System.Drawing.Point(1182, 9);
            this.lblArchiveDateEnding.Name = "lblArchiveDateEnding";
            this.lblArchiveDateEnding.Size = new System.Drawing.Size(164, 39);
            this.lblArchiveDateEnding.TabIndex = 27;
            this.lblArchiveDateEnding.Text = ":00 часов";
            // 
            // btnArchiveShow
            // 
            this.btnArchiveShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnArchiveShow.Location = new System.Drawing.Point(408, 612);
            this.btnArchiveShow.Name = "btnArchiveShow";
            this.btnArchiveShow.Size = new System.Drawing.Size(214, 92);
            this.btnArchiveShow.TabIndex = 26;
            this.btnArchiveShow.Text = "Показать цикл";
            this.btnArchiveShow.UseVisualStyleBackColor = true;
            this.btnArchiveShow.Click += new System.EventHandler(this.btnArchiveShow_Click);
            // 
            // lvArchiveSavedSockets
            // 
            this.lvArchiveSavedSockets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvArchiveSavedSockets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chCycleTime});
            this.lvArchiveSavedSockets.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvArchiveSavedSockets.FullRowSelect = true;
            this.lvArchiveSavedSockets.HideSelection = false;
            this.lvArchiveSavedSockets.Location = new System.Drawing.Point(15, 607);
            this.lvArchiveSavedSockets.MultiSelect = false;
            this.lvArchiveSavedSockets.Name = "lvArchiveSavedSockets";
            this.lvArchiveSavedSockets.Size = new System.Drawing.Size(384, 323);
            this.lvArchiveSavedSockets.TabIndex = 25;
            this.lvArchiveSavedSockets.UseCompatibleStateImageBehavior = false;
            this.lvArchiveSavedSockets.View = System.Windows.Forms.View.Details;
            // 
            // chCycleTime
            // 
            this.chCycleTime.Text = "Время цикла";
            this.chCycleTime.Width = 347;
            // 
            // chArchiveByTime
            // 
            this.chArchiveByTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisX.MinorTickMark.Enabled = true;
            chartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisX2.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea1.Name = "ChartArea1";
            this.chArchiveByTime.ChartAreas.Add(chartArea1);
            this.chArchiveByTime.Location = new System.Drawing.Point(15, 394);
            this.chArchiveByTime.Name = "chArchiveByTime";
            this.chArchiveByTime.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chArchiveByTime.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.OrangeRed};
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            this.chArchiveByTime.Series.Add(series1);
            this.chArchiveByTime.Size = new System.Drawing.Size(1279, 212);
            this.chArchiveByTime.TabIndex = 24;
            this.chArchiveByTime.Text = "chart1";
            this.chArchiveByTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chArchiveByTime_MouseDown);
            // 
            // lblArchiveSocket
            // 
            this.lblArchiveSocket.AutoSize = true;
            this.lblArchiveSocket.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblArchiveSocket.Location = new System.Drawing.Point(23, 342);
            this.lblArchiveSocket.Name = "lblArchiveSocket";
            this.lblArchiveSocket.Size = new System.Drawing.Size(139, 39);
            this.lblArchiveSocket.TabIndex = 23;
            this.lblArchiveSocket.Text = "Гнездо:";
            // 
            // nudArchiveSocketNumber
            // 
            this.nudArchiveSocketNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudArchiveSocketNumber.Location = new System.Drawing.Point(177, 335);
            this.nudArchiveSocketNumber.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.nudArchiveSocketNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudArchiveSocketNumber.Name = "nudArchiveSocketNumber";
            this.nudArchiveSocketNumber.Size = new System.Drawing.Size(113, 53);
            this.nudArchiveSocketNumber.TabIndex = 22;
            this.nudArchiveSocketNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudArchiveSocketNumber.ValueChanged += new System.EventHandler(this.nudArchiveSocketNumber_ValueChanged);
            this.nudArchiveSocketNumber.DoubleClick += new System.EventHandler(this.nudArchiveSocketNumber_DoubleClick);
            // 
            // btnArchiveSelect
            // 
            this.btnArchiveSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnArchiveSelect.Location = new System.Drawing.Point(19, 61);
            this.btnArchiveSelect.Name = "btnArchiveSelect";
            this.btnArchiveSelect.Size = new System.Drawing.Size(702, 50);
            this.btnArchiveSelect.TabIndex = 21;
            this.btnArchiveSelect.Text = "Показать сохраненные гнезда за период";
            this.btnArchiveSelect.UseVisualStyleBackColor = true;
            this.btnArchiveSelect.Click += new System.EventHandler(this.btnArchiveSelect_Click);
            // 
            // chArchiveSumBad
            // 
            this.chArchiveSumBad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.AxisX.Maximum = 100D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.MinorGrid.Enabled = true;
            chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea2.AxisX.MinorTickMark.Enabled = true;
            chartArea2.AxisX.MinorTickMark.LineColor = System.Drawing.Color.LightGray;
            chartArea2.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.Name = "ChartArea1";
            this.chArchiveSumBad.ChartAreas.Add(chartArea2);
            this.chArchiveSumBad.Location = new System.Drawing.Point(15, 123);
            this.chArchiveSumBad.Name = "chArchiveSumBad";
            this.chArchiveSumBad.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series2.Name = "Series1";
            this.chArchiveSumBad.Series.Add(series2);
            this.chArchiveSumBad.Size = new System.Drawing.Size(1279, 212);
            this.chArchiveSumBad.TabIndex = 20;
            this.chArchiveSumBad.Text = "chart1";
            this.chArchiveSumBad.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chArchiveSumBad_MouseDown);
            // 
            // nudArchiveFrom
            // 
            this.nudArchiveFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudArchiveFrom.Location = new System.Drawing.Point(408, 2);
            this.nudArchiveFrom.Name = "nudArchiveFrom";
            this.nudArchiveFrom.Size = new System.Drawing.Size(108, 53);
            this.nudArchiveFrom.TabIndex = 18;
            this.nudArchiveFrom.DoubleClick += new System.EventHandler(this.nudArchiveFrom_DoubleClick);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTo.Location = new System.Drawing.Point(510, 10);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(211, 39);
            this.lblTo.TabIndex = 17;
            this.lblTo.Text = ":00 часов по";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFrom.Location = new System.Drawing.Point(12, 9);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(42, 39);
            this.lblFrom.TabIndex = 16;
            this.lblFrom.Text = "С";
            // 
            // dtpArchiveTo
            // 
            this.dtpArchiveTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpArchiveTo.Location = new System.Drawing.Point(739, 5);
            this.dtpArchiveTo.Name = "dtpArchiveTo";
            this.dtpArchiveTo.Size = new System.Drawing.Size(335, 45);
            this.dtpArchiveTo.TabIndex = 15;
            // 
            // dtpArchiveFrom
            // 
            this.dtpArchiveFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpArchiveFrom.Location = new System.Drawing.Point(56, 4);
            this.dtpArchiveFrom.Name = "dtpArchiveFrom";
            this.dtpArchiveFrom.Size = new System.Drawing.Size(346, 45);
            this.dtpArchiveFrom.TabIndex = 14;
            // 
            // btnArchiveSelectBad
            // 
            this.btnArchiveSelectBad.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnArchiveSelectBad.Location = new System.Drawing.Point(739, 61);
            this.btnArchiveSelectBad.Name = "btnArchiveSelectBad";
            this.btnArchiveSelectBad.Size = new System.Drawing.Size(607, 50);
            this.btnArchiveSelectBad.TabIndex = 28;
            this.btnArchiveSelectBad.Text = "Показать плохие гнезда за период";
            this.btnArchiveSelectBad.UseVisualStyleBackColor = true;
            this.btnArchiveSelectBad.Click += new System.EventHandler(this.btnArchiveSelectBad_Click);
            // 
            // lblSocketHoursCaption
            // 
            this.lblSocketHoursCaption.AutoSize = true;
            this.lblSocketHoursCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSocketHoursCaption.Location = new System.Drawing.Point(497, 352);
            this.lblSocketHoursCaption.Name = "lblSocketHoursCaption";
            this.lblSocketHoursCaption.Size = new System.Drawing.Size(612, 39);
            this.lblSocketHoursCaption.TabIndex = 30;
            this.lblSocketHoursCaption.Text = "Распределение измерений по часам:";
            // 
            // chEvents
            // 
            this.chEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.AxisX.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea3.AxisX.MajorGrid.Enabled = false;
            chartArea3.AxisX.MinorGrid.Enabled = true;
            chartArea3.AxisX.MinorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea3.AxisX.MinorTickMark.Enabled = true;
            chartArea3.AxisX.MinorTickMark.LineColor = System.Drawing.Color.LightGray;
            chartArea3.AxisX2.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea3.AxisY.MajorGrid.Enabled = false;
            chartArea3.Name = "ChartArea1";
            this.chEvents.ChartAreas.Add(chartArea3);
            this.chEvents.Location = new System.Drawing.Point(649, 662);
            this.chEvents.Name = "chEvents";
            this.chEvents.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chEvents.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.OrangeRed};
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series3.Name = "Series1";
            series3.YValuesPerPoint = 2;
            this.chEvents.Series.Add(series3);
            this.chEvents.Size = new System.Drawing.Size(645, 261);
            this.chEvents.TabIndex = 31;
            this.chEvents.Text = "chart1";
            // 
            // lblEventsByTimeCaption
            // 
            this.lblEventsByTimeCaption.AutoSize = true;
            this.lblEventsByTimeCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEventsByTimeCaption.Location = new System.Drawing.Point(642, 620);
            this.lblEventsByTimeCaption.Name = "lblEventsByTimeCaption";
            this.lblEventsByTimeCaption.Size = new System.Drawing.Size(652, 39);
            this.lblEventsByTimeCaption.TabIndex = 32;
            this.lblEventsByTimeCaption.Text = "Распределение измерений по времени:";
            // 
            // lblBoxesCaption
            // 
            this.lblBoxesCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBoxesCaption.AutoSize = true;
            this.lblBoxesCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblBoxesCaption.Location = new System.Drawing.Point(1301, 655);
            this.lblBoxesCaption.Name = "lblBoxesCaption";
            this.lblBoxesCaption.Size = new System.Drawing.Size(202, 26);
            this.lblBoxesCaption.TabIndex = 33;
            this.lblBoxesCaption.Text = "Короба за период:";
            // 
            // lvBoxes
            // 
            this.lvBoxes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lvBoxes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chBoxTime,
            this.chBoxBadCycles});
            this.lvBoxes.HideSelection = false;
            this.lvBoxes.Location = new System.Drawing.Point(1300, 684);
            this.lvBoxes.Name = "lvBoxes";
            this.lvBoxes.Size = new System.Drawing.Size(249, 239);
            this.lvBoxes.TabIndex = 34;
            this.lvBoxes.UseCompatibleStateImageBehavior = false;
            this.lvBoxes.View = System.Windows.Forms.View.Details;
            // 
            // chBoxTime
            // 
            this.chBoxTime.Text = "Время короба";
            this.chBoxTime.Width = 170;
            // 
            // chBoxBadCycles
            // 
            this.chBoxBadCycles.Text = "Плохих съемов";
            // 
            // lvDefects
            // 
            this.lvDefects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDefects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDefectTime,
            this.chDefectSocket});
            this.lvDefects.HideSelection = false;
            this.lvDefects.Location = new System.Drawing.Point(1300, 152);
            this.lvDefects.Name = "lvDefects";
            this.lvDefects.Size = new System.Drawing.Size(249, 500);
            this.lvDefects.TabIndex = 36;
            this.lvDefects.UseCompatibleStateImageBehavior = false;
            this.lvDefects.View = System.Windows.Forms.View.Details;
            // 
            // chDefectTime
            // 
            this.chDefectTime.Text = "Время дефекта";
            this.chDefectTime.Width = 170;
            // 
            // chDefectSocket
            // 
            this.chDefectSocket.Text = "Гнезда";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(1300, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 26);
            this.label1.TabIndex = 35;
            this.label1.Text = "Съемы за период:";
            // 
            // DoMCArchiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1561, 935);
            this.Controls.Add(this.lvDefects);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvBoxes);
            this.Controls.Add(this.lblBoxesCaption);
            this.Controls.Add(this.lblEventsByTimeCaption);
            this.Controls.Add(this.chEvents);
            this.Controls.Add(this.lblSocketHoursCaption);
            this.Controls.Add(this.btnArchiveSelectBad);
            this.Controls.Add(this.nudArchiveTo);
            this.Controls.Add(this.lblArchiveDateEnding);
            this.Controls.Add(this.btnArchiveShow);
            this.Controls.Add(this.lvArchiveSavedSockets);
            this.Controls.Add(this.chArchiveByTime);
            this.Controls.Add(this.lblArchiveSocket);
            this.Controls.Add(this.nudArchiveSocketNumber);
            this.Controls.Add(this.btnArchiveSelect);
            this.Controls.Add(this.chArchiveSumBad);
            this.Controls.Add(this.nudArchiveFrom);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.dtpArchiveTo);
            this.Controls.Add(this.dtpArchiveFrom);
            this.Name = "DoMCArchiveForm";
            this.Text = "DoMCArchive";
            ((System.ComponentModel.ISupportInitialize)(this.nudArchiveTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chArchiveByTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArchiveSocketNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chArchiveSumBad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArchiveFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chEvents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudArchiveTo;
        private System.Windows.Forms.Label lblArchiveDateEnding;
        private System.Windows.Forms.Button btnArchiveShow;
        private System.Windows.Forms.ListView lvArchiveSavedSockets;
        private System.Windows.Forms.ColumnHeader chCycleTime;
        private System.Windows.Forms.DataVisualization.Charting.Chart chArchiveByTime;
        private System.Windows.Forms.Label lblArchiveSocket;
        private System.Windows.Forms.NumericUpDown nudArchiveSocketNumber;
        private System.Windows.Forms.Button btnArchiveSelect;
        private System.Windows.Forms.DataVisualization.Charting.Chart chArchiveSumBad;
        private System.Windows.Forms.NumericUpDown nudArchiveFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpArchiveTo;
        private System.Windows.Forms.DateTimePicker dtpArchiveFrom;
        private System.Windows.Forms.Button btnArchiveSelectBad;
        private System.Windows.Forms.Label lblSocketHoursCaption;
        private System.Windows.Forms.DataVisualization.Charting.Chart chEvents;
        private System.Windows.Forms.Label lblEventsByTimeCaption;
        private System.Windows.Forms.Label lblBoxesCaption;
        private System.Windows.Forms.ListView lvBoxes;
        private System.Windows.Forms.ColumnHeader chBoxTime;
        private System.Windows.Forms.ColumnHeader chBoxBadCycles;
        private System.Windows.Forms.ListView lvDefects;
        private System.Windows.Forms.ColumnHeader chDefectTime;
        private System.Windows.Forms.ColumnHeader chDefectSocket;
        private System.Windows.Forms.Label label1;
    }
}