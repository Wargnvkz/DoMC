namespace DoMC.Forms
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
            nudArchiveTo = new NumericUpDown();
            lblArchiveDateEnding = new Label();
            btnArchiveShow = new Button();
            lvArchiveSavedSockets = new ListView();
            chCycleTime = new ColumnHeader();
            chArchiveByTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblArchiveSocket = new Label();
            nudArchiveSocketNumber = new NumericUpDown();
            btnArchiveSelect = new Button();
            chArchiveSumBad = new System.Windows.Forms.DataVisualization.Charting.Chart();
            nudArchiveFrom = new NumericUpDown();
            lblTo = new Label();
            lblFrom = new Label();
            dtpArchiveTo = new DateTimePicker();
            dtpArchiveFrom = new DateTimePicker();
            btnArchiveSelectBad = new Button();
            lblSocketHoursCaption = new Label();
            chEvents = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblEventsByTimeCaption = new Label();
            lblBoxesCaption = new Label();
            lvBoxes = new ListView();
            chBoxTime = new ColumnHeader();
            chBoxBadCycles = new ColumnHeader();
            lvDefects = new ListView();
            chDefectTime = new ColumnHeader();
            chDefectSocket = new ColumnHeader();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)nudArchiveTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chArchiveByTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudArchiveSocketNumber).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chArchiveSumBad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudArchiveFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chEvents).BeginInit();
            SuspendLayout();
            // 
            // nudArchiveTo
            // 
            nudArchiveTo.Font = new Font("Microsoft Sans Serif", 25F);
            nudArchiveTo.Location = new Point(1261, 5);
            nudArchiveTo.Margin = new Padding(5, 3, 5, 3);
            nudArchiveTo.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            nudArchiveTo.Name = "nudArchiveTo";
            nudArchiveTo.Size = new Size(104, 45);
            nudArchiveTo.TabIndex = 19;
            nudArchiveTo.DoubleClick += nudArchiveTo_DoubleClick;
            // 
            // lblArchiveDateEnding
            // 
            lblArchiveDateEnding.AutoSize = true;
            lblArchiveDateEnding.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblArchiveDateEnding.Location = new Point(1374, 9);
            lblArchiveDateEnding.Margin = new Padding(4, 0, 4, 0);
            lblArchiveDateEnding.Name = "lblArchiveDateEnding";
            lblArchiveDateEnding.Size = new Size(164, 39);
            lblArchiveDateEnding.TabIndex = 27;
            lblArchiveDateEnding.Text = ":00 часов";
            // 
            // btnArchiveShow
            // 
            btnArchiveShow.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnArchiveShow.Location = new Point(476, 706);
            btnArchiveShow.Margin = new Padding(4, 3, 4, 3);
            btnArchiveShow.Name = "btnArchiveShow";
            btnArchiveShow.Size = new Size(250, 106);
            btnArchiveShow.TabIndex = 26;
            btnArchiveShow.Text = "Показать цикл";
            btnArchiveShow.UseVisualStyleBackColor = true;
            btnArchiveShow.Click += btnArchiveShow_Click;
            // 
            // lvArchiveSavedSockets
            // 
            lvArchiveSavedSockets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lvArchiveSavedSockets.Columns.AddRange(new ColumnHeader[] { chCycleTime });
            lvArchiveSavedSockets.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lvArchiveSavedSockets.FullRowSelect = true;
            lvArchiveSavedSockets.Location = new Point(18, 700);
            lvArchiveSavedSockets.Margin = new Padding(4, 3, 4, 3);
            lvArchiveSavedSockets.MultiSelect = false;
            lvArchiveSavedSockets.Name = "lvArchiveSavedSockets";
            lvArchiveSavedSockets.Size = new Size(447, 360);
            lvArchiveSavedSockets.TabIndex = 25;
            lvArchiveSavedSockets.UseCompatibleStateImageBehavior = false;
            lvArchiveSavedSockets.View = View.Details;
            // 
            // chCycleTime
            // 
            chCycleTime.Text = "Время цикла";
            chCycleTime.Width = 347;
            // 
            // chArchiveByTime
            // 
            chArchiveByTime.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            chartArea1.AxisX.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.LineColor = Color.LightGray;
            chartArea1.AxisX.MinorTickMark.Enabled = true;
            chartArea1.AxisX.MinorTickMark.LineColor = Color.LightGray;
            chartArea1.AxisX2.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea1.Name = "ChartArea1";
            chArchiveByTime.ChartAreas.Add(chartArea1);
            chArchiveByTime.Location = new Point(18, 455);
            chArchiveByTime.Margin = new Padding(4, 3, 4, 3);
            chArchiveByTime.Name = "chArchiveByTime";
            chArchiveByTime.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            chArchiveByTime.PaletteCustomColors = new Color[]
    {
    Color.OrangeRed
    };
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            chArchiveByTime.Series.Add(series1);
            chArchiveByTime.Size = new Size(1492, 245);
            chArchiveByTime.TabIndex = 24;
            chArchiveByTime.Text = "chart1";
            chArchiveByTime.MouseDown += chArchiveByTime_MouseDown;
            // 
            // lblArchiveSocket
            // 
            lblArchiveSocket.AutoSize = true;
            lblArchiveSocket.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblArchiveSocket.Location = new Point(27, 395);
            lblArchiveSocket.Margin = new Padding(4, 0, 4, 0);
            lblArchiveSocket.Name = "lblArchiveSocket";
            lblArchiveSocket.Size = new Size(139, 39);
            lblArchiveSocket.TabIndex = 23;
            lblArchiveSocket.Text = "Гнездо:";
            // 
            // nudArchiveSocketNumber
            // 
            nudArchiveSocketNumber.Font = new Font("Microsoft Sans Serif", 30F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudArchiveSocketNumber.Location = new Point(206, 387);
            nudArchiveSocketNumber.Margin = new Padding(4, 3, 4, 3);
            nudArchiveSocketNumber.Maximum = new decimal(new int[] { 96, 0, 0, 0 });
            nudArchiveSocketNumber.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudArchiveSocketNumber.Name = "nudArchiveSocketNumber";
            nudArchiveSocketNumber.Size = new Size(132, 53);
            nudArchiveSocketNumber.TabIndex = 22;
            nudArchiveSocketNumber.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudArchiveSocketNumber.ValueChanged += nudArchiveSocketNumber_ValueChanged;
            nudArchiveSocketNumber.DoubleClick += nudArchiveSocketNumber_DoubleClick;
            // 
            // btnArchiveSelect
            // 
            btnArchiveSelect.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnArchiveSelect.Location = new Point(22, 70);
            btnArchiveSelect.Margin = new Padding(4, 3, 4, 3);
            btnArchiveSelect.Name = "btnArchiveSelect";
            btnArchiveSelect.Size = new Size(819, 58);
            btnArchiveSelect.TabIndex = 21;
            btnArchiveSelect.Text = "Показать сохраненные гнезда за период";
            btnArchiveSelect.UseVisualStyleBackColor = true;
            btnArchiveSelect.Click += btnArchiveSelect_Click;
            // 
            // chArchiveSumBad
            // 
            chArchiveSumBad.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            chartArea2.AxisX.Maximum = 100D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.MinorGrid.Enabled = true;
            chartArea2.AxisX.MinorGrid.LineColor = Color.LightGray;
            chartArea2.AxisX.MinorTickMark.Enabled = true;
            chartArea2.AxisX.MinorTickMark.LineColor = Color.LightGray;
            chartArea2.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.Name = "ChartArea1";
            chArchiveSumBad.ChartAreas.Add(chartArea2);
            chArchiveSumBad.Location = new Point(18, 142);
            chArchiveSumBad.Margin = new Padding(4, 3, 4, 3);
            chArchiveSumBad.Name = "chArchiveSumBad";
            chArchiveSumBad.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series2.Name = "Series1";
            chArchiveSumBad.Series.Add(series2);
            chArchiveSumBad.Size = new Size(1492, 245);
            chArchiveSumBad.TabIndex = 20;
            chArchiveSumBad.Text = "chart1";
            chArchiveSumBad.MouseDown += chArchiveSumBad_MouseDown;
            // 
            // nudArchiveFrom
            // 
            nudArchiveFrom.Font = new Font("Microsoft Sans Serif", 25F);
            nudArchiveFrom.Location = new Point(476, 5);
            nudArchiveFrom.Margin = new Padding(5, 3, 5, 3);
            nudArchiveFrom.Name = "nudArchiveFrom";
            nudArchiveFrom.Size = new Size(104, 45);
            nudArchiveFrom.TabIndex = 18;
            nudArchiveFrom.DoubleClick += nudArchiveFrom_DoubleClick;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTo.Location = new Point(589, 9);
            lblTo.Margin = new Padding(4, 0, 4, 0);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(211, 39);
            lblTo.TabIndex = 17;
            lblTo.Text = ":00 часов по";
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblFrom.Location = new Point(14, 10);
            lblFrom.Margin = new Padding(4, 0, 4, 0);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(42, 39);
            lblFrom.TabIndex = 16;
            lblFrom.Text = "С";
            // 
            // dtpArchiveTo
            // 
            dtpArchiveTo.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            dtpArchiveTo.Location = new Point(862, 6);
            dtpArchiveTo.Margin = new Padding(4, 3, 4, 3);
            dtpArchiveTo.Name = "dtpArchiveTo";
            dtpArchiveTo.Size = new Size(390, 45);
            dtpArchiveTo.TabIndex = 15;
            // 
            // dtpArchiveFrom
            // 
            dtpArchiveFrom.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            dtpArchiveFrom.Location = new Point(65, 5);
            dtpArchiveFrom.Margin = new Padding(4, 3, 4, 3);
            dtpArchiveFrom.Name = "dtpArchiveFrom";
            dtpArchiveFrom.Size = new Size(403, 45);
            dtpArchiveFrom.TabIndex = 14;
            // 
            // btnArchiveSelectBad
            // 
            btnArchiveSelectBad.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnArchiveSelectBad.Location = new Point(862, 70);
            btnArchiveSelectBad.Margin = new Padding(4, 3, 4, 3);
            btnArchiveSelectBad.Name = "btnArchiveSelectBad";
            btnArchiveSelectBad.Size = new Size(708, 58);
            btnArchiveSelectBad.TabIndex = 28;
            btnArchiveSelectBad.Text = "Показать плохие гнезда за период";
            btnArchiveSelectBad.UseVisualStyleBackColor = true;
            btnArchiveSelectBad.Click += btnArchiveSelectBad_Click;
            // 
            // lblSocketHoursCaption
            // 
            lblSocketHoursCaption.AutoSize = true;
            lblSocketHoursCaption.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblSocketHoursCaption.Location = new Point(580, 406);
            lblSocketHoursCaption.Margin = new Padding(4, 0, 4, 0);
            lblSocketHoursCaption.Name = "lblSocketHoursCaption";
            lblSocketHoursCaption.Size = new Size(612, 39);
            lblSocketHoursCaption.TabIndex = 30;
            lblSocketHoursCaption.Text = "Распределение измерений по часам:";
            // 
            // chEvents
            // 
            chEvents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartArea3.AxisX.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea3.AxisX.MajorGrid.Enabled = false;
            chartArea3.AxisX.MinorGrid.Enabled = true;
            chartArea3.AxisX.MinorGrid.LineColor = Color.LightGray;
            chartArea3.AxisX.MinorTickMark.Enabled = true;
            chartArea3.AxisX.MinorTickMark.LineColor = Color.LightGray;
            chartArea3.AxisX2.LabelStyle.Format = "dd.MM.yyyy HH:mm:ss";
            chartArea3.AxisY.MajorGrid.Enabled = false;
            chartArea3.Name = "ChartArea1";
            chEvents.ChartAreas.Add(chartArea3);
            chEvents.Location = new Point(757, 764);
            chEvents.Margin = new Padding(4, 3, 4, 3);
            chEvents.Name = "chEvents";
            chEvents.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            chEvents.PaletteCustomColors = new Color[]
    {
    Color.OrangeRed
    };
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series3.Name = "Series1";
            series3.YValuesPerPoint = 2;
            chEvents.Series.Add(series3);
            chEvents.Size = new Size(752, 301);
            chEvents.TabIndex = 31;
            chEvents.Text = "chart1";
            // 
            // lblEventsByTimeCaption
            // 
            lblEventsByTimeCaption.AutoSize = true;
            lblEventsByTimeCaption.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblEventsByTimeCaption.Location = new Point(749, 715);
            lblEventsByTimeCaption.Margin = new Padding(4, 0, 4, 0);
            lblEventsByTimeCaption.Name = "lblEventsByTimeCaption";
            lblEventsByTimeCaption.Size = new Size(652, 39);
            lblEventsByTimeCaption.TabIndex = 32;
            lblEventsByTimeCaption.Text = "Распределение измерений по времени:";
            // 
            // lblBoxesCaption
            // 
            lblBoxesCaption.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblBoxesCaption.AutoSize = true;
            lblBoxesCaption.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblBoxesCaption.Location = new Point(1518, 756);
            lblBoxesCaption.Margin = new Padding(4, 0, 4, 0);
            lblBoxesCaption.Name = "lblBoxesCaption";
            lblBoxesCaption.Size = new Size(202, 26);
            lblBoxesCaption.TabIndex = 33;
            lblBoxesCaption.Text = "Короба за период:";
            // 
            // lvBoxes
            // 
            lvBoxes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lvBoxes.Columns.AddRange(new ColumnHeader[] { chBoxTime, chBoxBadCycles });
            lvBoxes.Location = new Point(1517, 789);
            lvBoxes.Margin = new Padding(4, 3, 4, 3);
            lvBoxes.Name = "lvBoxes";
            lvBoxes.Size = new Size(290, 271);
            lvBoxes.TabIndex = 34;
            lvBoxes.UseCompatibleStateImageBehavior = false;
            lvBoxes.View = View.Details;
            // 
            // chBoxTime
            // 
            chBoxTime.Text = "Время короба";
            chBoxTime.Width = 170;
            // 
            // chBoxBadCycles
            // 
            chBoxBadCycles.Text = "Плохих съемов";
            // 
            // lvDefects
            // 
            lvDefects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lvDefects.Columns.AddRange(new ColumnHeader[] { chDefectTime, chDefectSocket });
            lvDefects.Location = new Point(1517, 175);
            lvDefects.Margin = new Padding(4, 3, 4, 3);
            lvDefects.Name = "lvDefects";
            lvDefects.Size = new Size(290, 576);
            lvDefects.TabIndex = 36;
            lvDefects.UseCompatibleStateImageBehavior = false;
            lvDefects.View = View.Details;
            // 
            // chDefectTime
            // 
            chDefectTime.Text = "Время дефекта";
            chDefectTime.Width = 170;
            // 
            // chDefectSocket
            // 
            chDefectSocket.Text = "Гнезда";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(1517, 142);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(200, 26);
            label1.TabIndex = 35;
            label1.Text = "Съемы за период:";
            // 
            // DoMCArchiveForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1821, 1061);
            Controls.Add(lvDefects);
            Controls.Add(label1);
            Controls.Add(lvBoxes);
            Controls.Add(lblBoxesCaption);
            Controls.Add(lblEventsByTimeCaption);
            Controls.Add(chEvents);
            Controls.Add(lblSocketHoursCaption);
            Controls.Add(btnArchiveSelectBad);
            Controls.Add(nudArchiveTo);
            Controls.Add(lblArchiveDateEnding);
            Controls.Add(btnArchiveShow);
            Controls.Add(lvArchiveSavedSockets);
            Controls.Add(chArchiveByTime);
            Controls.Add(lblArchiveSocket);
            Controls.Add(nudArchiveSocketNumber);
            Controls.Add(btnArchiveSelect);
            Controls.Add(chArchiveSumBad);
            Controls.Add(nudArchiveFrom);
            Controls.Add(lblTo);
            Controls.Add(lblFrom);
            Controls.Add(dtpArchiveTo);
            Controls.Add(dtpArchiveFrom);
            Margin = new Padding(4, 3, 4, 3);
            Name = "DoMCArchiveForm";
            Text = "DoMCArchive";
            ((System.ComponentModel.ISupportInitialize)nudArchiveTo).EndInit();
            ((System.ComponentModel.ISupportInitialize)chArchiveByTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudArchiveSocketNumber).EndInit();
            ((System.ComponentModel.ISupportInitialize)chArchiveSumBad).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudArchiveFrom).EndInit();
            ((System.ComponentModel.ISupportInitialize)chEvents).EndInit();
            ResumeLayout(false);
            PerformLayout();
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