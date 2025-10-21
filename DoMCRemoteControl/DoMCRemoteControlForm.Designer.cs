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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            chEvents = new System.Windows.Forms.DataVisualization.Charting.Chart();
            timer1 = new System.Windows.Forms.Timer(components);
            lblEventsByTimeCaption = new Label();
            lvDefects = new ListView();
            chDefectTime = new ColumnHeader();
            chDefectSocket = new ColumnHeader();
            label1 = new Label();
            lvBoxes = new ListView();
            chBoxTime = new ColumnHeader();
            chBoxBadCycles = new ColumnHeader();
            lblBoxesCaption = new Label();
            pnlCurrentSockets = new Panel();
            cbCurrentShowStatistics = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)chEvents).BeginInit();
            SuspendLayout();
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
            chEvents.Location = new Point(108, 317);
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
            chEvents.Size = new Size(744, 235);
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
            lblEventsByTimeCaption.AutoSize = true;
            lblEventsByTimeCaption.Font = new Font("Microsoft Sans Serif", 25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblEventsByTimeCaption.Location = new Point(108, 275);
            lblEventsByTimeCaption.Margin = new Padding(4, 0, 4, 0);
            lblEventsByTimeCaption.Name = "lblEventsByTimeCaption";
            lblEventsByTimeCaption.Size = new Size(652, 39);
            lblEventsByTimeCaption.TabIndex = 33;
            lblEventsByTimeCaption.Text = "Распределение измерений по времени:";
            // 
            // lvDefects
            // 
            lvDefects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lvDefects.Columns.AddRange(new ColumnHeader[] { chDefectTime, chDefectSocket });
            lvDefects.Location = new Point(885, 42);
            lvDefects.Margin = new Padding(4, 3, 4, 3);
            lvDefects.Name = "lvDefects";
            lvDefects.Size = new Size(290, 286);
            lvDefects.TabIndex = 40;
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
            label1.Location = new Point(885, 9);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(200, 26);
            label1.TabIndex = 39;
            label1.Text = "Съемы за период:";
            // 
            // lvBoxes
            // 
            lvBoxes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lvBoxes.Columns.AddRange(new ColumnHeader[] { chBoxTime, chBoxBadCycles });
            lvBoxes.Location = new Point(885, 372);
            lvBoxes.Margin = new Padding(4, 3, 4, 3);
            lvBoxes.Name = "lvBoxes";
            lvBoxes.Size = new Size(290, 183);
            lvBoxes.TabIndex = 38;
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
            // lblBoxesCaption
            // 
            lblBoxesCaption.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblBoxesCaption.AutoSize = true;
            lblBoxesCaption.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblBoxesCaption.Location = new Point(885, 343);
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
            pnlCurrentSockets.Location = new Point(108, 42);
            pnlCurrentSockets.Margin = new Padding(4, 3, 4, 3);
            pnlCurrentSockets.Name = "pnlCurrentSockets";
            pnlCurrentSockets.Size = new Size(273, 230);
            pnlCurrentSockets.TabIndex = 41;
            // 
            // cbCurrentShowStatistics
            // 
            cbCurrentShowStatistics.Appearance = Appearance.Button;
            cbCurrentShowStatistics.Location = new Point(108, 9);
            cbCurrentShowStatistics.Name = "cbCurrentShowStatistics";
            cbCurrentShowStatistics.Size = new Size(273, 24);
            cbCurrentShowStatistics.TabIndex = 42;
            cbCurrentShowStatistics.Text = "Показать статистику";
            cbCurrentShowStatistics.UseVisualStyleBackColor = true;
            // 
            // DoMCRemoteControlForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1188, 562);
            Controls.Add(cbCurrentShowStatistics);
            Controls.Add(pnlCurrentSockets);
            Controls.Add(lvDefects);
            Controls.Add(label1);
            Controls.Add(lvBoxes);
            Controls.Add(lblBoxesCaption);
            Controls.Add(lblEventsByTimeCaption);
            Controls.Add(chEvents);
            Name = "DoMCRemoteControlForm";
            Text = "Панель управления ПМК";
            ((System.ComponentModel.ISupportInitialize)chEvents).EndInit();
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
        private Label label1;
        private ListView lvBoxes;
        private ColumnHeader chBoxTime;
        private ColumnHeader chBoxBadCycles;
        private Label lblBoxesCaption;
        private Panel pnlCurrentSockets;
        private CheckBox cbCurrentShowStatistics;
    }
}
