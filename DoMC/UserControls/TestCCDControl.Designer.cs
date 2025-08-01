﻿namespace DoMC.Forms
{
    partial class TestCCDControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            cbInvertColors = new CheckBox();
            btnCycleStop = new Button();
            btnCycleStart = new Button();
            chTestStandard = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chTestDiff = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblNumLineCaption = new Label();
            cbFullMax = new CheckBox();
            cbVertical = new CheckBox();
            numFrame = new NumericUpDown();
            chTestReadLine = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblTestStandard = new Label();
            lblTestDifference = new Label();
            lblTestRead = new Label();
            pbTestStandard = new PictureBox();
            pbTestDifference = new PictureBox();
            pbTestReadImage = new PictureBox();
            btnTest_ReadSelectedSocket = new Button();
            lblTestSelectedSocket = new Label();
            lblTestSelectedSocketLabel = new Label();
            pnlSockets = new Panel();
            cbTest_ExternalStart = new CheckBox();
            btnTest_ReadAllSocket = new Button();
            cbTestCCDMaxPointShow = new CheckBox();
            timer1 = new System.Windows.Forms.Timer(components);
            statusStrip1 = new StatusStrip();
            pbGettingStandard = new ToolStripProgressBar();
            lblWorkStatus = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)chTestStandard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chTestDiff).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chTestReadLine).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbTestStandard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbTestDifference).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbTestReadImage).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // cbInvertColors
            // 
            cbInvertColors.AutoSize = true;
            cbInvertColors.Location = new Point(1384, 9);
            cbInvertColors.Margin = new Padding(5);
            cbInvertColors.Name = "cbInvertColors";
            cbInvertColors.Size = new Size(120, 19);
            cbInvertColors.TabIndex = 94;
            cbInvertColors.Text = "Инверсия цветов";
            cbInvertColors.UseVisualStyleBackColor = true;
            cbInvertColors.CheckedChanged += cbInvertColors_CheckedChanged;
            // 
            // btnCycleStop
            // 
            btnCycleStop.Location = new Point(227, 589);
            btnCycleStop.Margin = new Padding(5);
            btnCycleStop.Name = "btnCycleStop";
            btnCycleStop.Size = new Size(212, 46);
            btnCycleStop.TabIndex = 93;
            btnCycleStop.Text = " Остановка цикла";
            btnCycleStop.UseVisualStyleBackColor = true;
            btnCycleStop.Click += btnCycleStop_Click;
            // 
            // btnCycleStart
            // 
            btnCycleStart.Location = new Point(8, 589);
            btnCycleStart.Margin = new Padding(5);
            btnCycleStart.Name = "btnCycleStart";
            btnCycleStart.Size = new Size(165, 46);
            btnCycleStart.TabIndex = 92;
            btnCycleStart.Text = "Запуск цикла";
            btnCycleStart.UseVisualStyleBackColor = true;
            btnCycleStart.Click += btnCycleStartStop_Click;
            // 
            // chTestStandard
            // 
            chartArea1.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea1.Name = "ChartArea1";
            chTestStandard.ChartAreas.Add(chartArea1);
            chTestStandard.Location = new Point(1257, 585);
            chTestStandard.Margin = new Padding(5);
            chTestStandard.Name = "chTestStandard";
            chTestStandard.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Name = "Series3";
            series3.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            chTestStandard.Series.Add(series1);
            chTestStandard.Series.Add(series2);
            chTestStandard.Series.Add(series3);
            chTestStandard.Size = new Size(398, 287);
            chTestStandard.TabIndex = 91;
            chTestStandard.Text = "chart1";
            // 
            // chTestDiff
            // 
            chartArea2.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea2.Name = "ChartArea1";
            chTestDiff.ChartAreas.Add(chartArea2);
            chTestDiff.Location = new Point(873, 589);
            chTestDiff.Margin = new Padding(5);
            chTestDiff.Name = "chTestDiff";
            chTestDiff.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Name = "Series1";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Name = "Series2";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Name = "Series3";
            series6.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            chTestDiff.Series.Add(series4);
            chTestDiff.Series.Add(series5);
            chTestDiff.Series.Add(series6);
            chTestDiff.Size = new Size(392, 287);
            chTestDiff.TabIndex = 90;
            chTestDiff.Text = "chart1";
            // 
            // lblNumLineCaption
            // 
            lblNumLineCaption.AutoSize = true;
            lblNumLineCaption.Location = new Point(927, 10);
            lblNumLineCaption.Margin = new Padding(5, 0, 5, 0);
            lblNumLineCaption.Name = "lblNumLineCaption";
            lblNumLineCaption.Size = new Size(86, 15);
            lblNumLineCaption.TabIndex = 89;
            lblNumLineCaption.Text = "Номер линии:";
            // 
            // cbFullMax
            // 
            cbFullMax.AutoSize = true;
            cbFullMax.Location = new Point(644, 9);
            cbFullMax.Margin = new Padding(5);
            cbFullMax.Name = "cbFullMax";
            cbFullMax.Size = new Size(161, 19);
            cbFullMax.TabIndex = 88;
            cbFullMax.Text = "Максимальное по кадру";
            cbFullMax.UseVisualStyleBackColor = true;
            cbFullMax.CheckedChanged += cbFullMax_CheckedChanged;
            // 
            // cbVertical
            // 
            cbVertical.AutoSize = true;
            cbVertical.Location = new Point(1165, 9);
            cbVertical.Margin = new Padding(5);
            cbVertical.Name = "cbVertical";
            cbVertical.Size = new Size(139, 19);
            cbVertical.TabIndex = 87;
            cbVertical.Text = "Вертикальная линия";
            cbVertical.UseVisualStyleBackColor = true;
            // 
            // numFrame
            // 
            numFrame.Location = new Point(1050, 6);
            numFrame.Margin = new Padding(5);
            numFrame.Maximum = new decimal(new int[] { 511, 0, 0, 0 });
            numFrame.Name = "numFrame";
            numFrame.Size = new Size(105, 23);
            numFrame.TabIndex = 86;
            numFrame.ValueChanged += numFrame_ValueChanged;
            numFrame.DoubleClick += numFrame_DoubleClick;
            // 
            // chTestReadLine
            // 
            chartArea3.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea3.Name = "ChartArea1";
            chTestReadLine.ChartAreas.Add(chartArea3);
            chTestReadLine.Location = new Point(434, 585);
            chTestReadLine.Margin = new Padding(5);
            chTestReadLine.Name = "chTestReadLine";
            chTestReadLine.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Name = "Series1";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Name = "Series2";
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Name = "Series3";
            series9.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            chTestReadLine.Series.Add(series7);
            chTestReadLine.Series.Add(series8);
            chTestReadLine.Series.Add(series9);
            chTestReadLine.Size = new Size(468, 287);
            chTestReadLine.TabIndex = 85;
            chTestReadLine.Text = "chart1";
            // 
            // lblTestStandard
            // 
            lblTestStandard.AutoSize = true;
            lblTestStandard.Location = new Point(768, 78);
            lblTestStandard.Margin = new Padding(5, 0, 5, 0);
            lblTestStandard.Name = "lblTestStandard";
            lblTestStandard.Size = new Size(38, 15);
            lblTestStandard.TabIndex = 84;
            lblTestStandard.Text = "label8";
            // 
            // lblTestDifference
            // 
            lblTestDifference.AutoSize = true;
            lblTestDifference.Location = new Point(609, 78);
            lblTestDifference.Margin = new Padding(5, 0, 5, 0);
            lblTestDifference.Name = "lblTestDifference";
            lblTestDifference.Size = new Size(38, 15);
            lblTestDifference.TabIndex = 83;
            lblTestDifference.Text = "label7";
            // 
            // lblTestRead
            // 
            lblTestRead.AutoSize = true;
            lblTestRead.Location = new Point(445, 78);
            lblTestRead.Margin = new Padding(5, 0, 5, 0);
            lblTestRead.Name = "lblTestRead";
            lblTestRead.Size = new Size(38, 15);
            lblTestRead.TabIndex = 82;
            lblTestRead.Text = "label2";
            // 
            // pbTestStandard
            // 
            pbTestStandard.BorderStyle = BorderStyle.FixedSingle;
            pbTestStandard.Location = new Point(768, 110);
            pbTestStandard.Margin = new Padding(5);
            pbTestStandard.Name = "pbTestStandard";
            pbTestStandard.Size = new Size(149, 76);
            pbTestStandard.TabIndex = 81;
            pbTestStandard.TabStop = false;
            pbTestStandard.Paint += pbTestStandard_Paint;
            pbTestStandard.DoubleClick += pbTestStandard_DoubleClick;
            pbTestStandard.MouseUp += pbTestStandard_MouseUp;
            // 
            // pbTestDifference
            // 
            pbTestDifference.BorderStyle = BorderStyle.FixedSingle;
            pbTestDifference.Location = new Point(609, 110);
            pbTestDifference.Margin = new Padding(5);
            pbTestDifference.Name = "pbTestDifference";
            pbTestDifference.Size = new Size(149, 76);
            pbTestDifference.TabIndex = 80;
            pbTestDifference.TabStop = false;
            pbTestDifference.Paint += pbTestDifference_Paint;
            pbTestDifference.DoubleClick += pbTestDifference_DoubleClick;
            pbTestDifference.MouseUp += pbTestDifference_MouseUp;
            // 
            // pbTestReadImage
            // 
            pbTestReadImage.BorderStyle = BorderStyle.FixedSingle;
            pbTestReadImage.Location = new Point(450, 110);
            pbTestReadImage.Margin = new Padding(5);
            pbTestReadImage.Name = "pbTestReadImage";
            pbTestReadImage.Size = new Size(149, 76);
            pbTestReadImage.TabIndex = 79;
            pbTestReadImage.TabStop = false;
            pbTestReadImage.Paint += pbTestReadImage_Paint;
            pbTestReadImage.DoubleClick += pbTestReadImage_DoubleClick;
            pbTestReadImage.MouseUp += pbTestReadImage_MouseUp;
            // 
            // btnTest_ReadSelectedSocket
            // 
            btnTest_ReadSelectedSocket.Location = new Point(249, 9);
            btnTest_ReadSelectedSocket.Margin = new Padding(5);
            btnTest_ReadSelectedSocket.Name = "btnTest_ReadSelectedSocket";
            btnTest_ReadSelectedSocket.Size = new Size(189, 66);
            btnTest_ReadSelectedSocket.TabIndex = 78;
            btnTest_ReadSelectedSocket.Text = "Читать выбранное гнездо";
            btnTest_ReadSelectedSocket.UseVisualStyleBackColor = true;
            btnTest_ReadSelectedSocket.Click += btnReadSelectedSocket_Click;
            // 
            // lblTestSelectedSocket
            // 
            lblTestSelectedSocket.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblTestSelectedSocket.Location = new Point(584, 10);
            lblTestSelectedSocket.Margin = new Padding(5, 0, 5, 0);
            lblTestSelectedSocket.Name = "lblTestSelectedSocket";
            lblTestSelectedSocket.Size = new Size(50, 20);
            lblTestSelectedSocket.TabIndex = 77;
            lblTestSelectedSocket.Text = "-";
            // 
            // lblTestSelectedSocketLabel
            // 
            lblTestSelectedSocketLabel.AutoSize = true;
            lblTestSelectedSocketLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestSelectedSocketLabel.Location = new Point(437, 10);
            lblTestSelectedSocketLabel.Margin = new Padding(5, 0, 5, 0);
            lblTestSelectedSocketLabel.Name = "lblTestSelectedSocketLabel";
            lblTestSelectedSocketLabel.Size = new Size(137, 20);
            lblTestSelectedSocketLabel.TabIndex = 76;
            lblTestSelectedSocketLabel.Text = "Выбрано гнездо:";
            // 
            // pnlSockets
            // 
            pnlSockets.Location = new Point(8, 110);
            pnlSockets.Margin = new Padding(5);
            pnlSockets.Name = "pnlSockets";
            pnlSockets.Size = new Size(429, 469);
            pnlSockets.TabIndex = 75;
            // 
            // cbTest_ExternalStart
            // 
            cbTest_ExternalStart.AutoSize = true;
            cbTest_ExternalStart.Location = new Point(9, 3);
            cbTest_ExternalStart.Margin = new Padding(5);
            cbTest_ExternalStart.Name = "cbTest_ExternalStart";
            cbTest_ExternalStart.Size = new Size(117, 19);
            cbTest_ExternalStart.TabIndex = 72;
            cbTest_ExternalStart.Text = "Внешний запуск";
            cbTest_ExternalStart.UseVisualStyleBackColor = true;
            // 
            // btnTest_ReadAllSocket
            // 
            btnTest_ReadAllSocket.Location = new Point(8, 35);
            btnTest_ReadAllSocket.Margin = new Padding(5);
            btnTest_ReadAllSocket.Name = "btnTest_ReadAllSocket";
            btnTest_ReadAllSocket.Size = new Size(165, 40);
            btnTest_ReadAllSocket.TabIndex = 71;
            btnTest_ReadAllSocket.Text = "Читать все гнезда";
            btnTest_ReadAllSocket.UseVisualStyleBackColor = true;
            btnTest_ReadAllSocket.Click += btnTest_ReadAllSockets_Click;
            // 
            // cbTestCCDMaxPointShow
            // 
            cbTestCCDMaxPointShow.AutoSize = true;
            cbTestCCDMaxPointShow.Location = new Point(1540, 10);
            cbTestCCDMaxPointShow.Margin = new Padding(5);
            cbTestCCDMaxPointShow.Name = "cbTestCCDMaxPointShow";
            cbTestCCDMaxPointShow.Size = new Size(198, 19);
            cbTestCCDMaxPointShow.TabIndex = 96;
            cbTestCCDMaxPointShow.Text = "Показать максимальную точку";
            cbTestCCDMaxPointShow.UseVisualStyleBackColor = true;
            cbTestCCDMaxPointShow.CheckedChanged += cbTestCCDMaxPointShow_CheckedChanged;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 300;
            timer1.Tick += timer1_Tick;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { pbGettingStandard, lblWorkStatus });
            statusStrip1.Location = new Point(0, 880);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1683, 22);
            statusStrip1.TabIndex = 97;
            statusStrip1.Text = "statusStrip1";
            // 
            // pbGettingStandard
            // 
            pbGettingStandard.Name = "pbGettingStandard";
            pbGettingStandard.Size = new Size(150, 16);
            // 
            // lblWorkStatus
            // 
            lblWorkStatus.Name = "lblWorkStatus";
            lblWorkStatus.Size = new Size(12, 17);
            lblWorkStatus.Text = "-";
            // 
            // TestCCDControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(statusStrip1);
            Controls.Add(cbTestCCDMaxPointShow);
            Controls.Add(cbInvertColors);
            Controls.Add(btnCycleStop);
            Controls.Add(btnCycleStart);
            Controls.Add(chTestStandard);
            Controls.Add(chTestDiff);
            Controls.Add(lblNumLineCaption);
            Controls.Add(cbFullMax);
            Controls.Add(cbVertical);
            Controls.Add(numFrame);
            Controls.Add(chTestReadLine);
            Controls.Add(lblTestStandard);
            Controls.Add(lblTestDifference);
            Controls.Add(lblTestRead);
            Controls.Add(pbTestStandard);
            Controls.Add(pbTestDifference);
            Controls.Add(pbTestReadImage);
            Controls.Add(btnTest_ReadSelectedSocket);
            Controls.Add(lblTestSelectedSocket);
            Controls.Add(lblTestSelectedSocketLabel);
            Controls.Add(pnlSockets);
            Controls.Add(cbTest_ExternalStart);
            Controls.Add(btnTest_ReadAllSocket);
            Name = "TestCCDControl";
            Size = new Size(1683, 902);
            Resize += TestCCDInterface_Resize;
            ((System.ComponentModel.ISupportInitialize)chTestStandard).EndInit();
            ((System.ComponentModel.ISupportInitialize)chTestDiff).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFrame).EndInit();
            ((System.ComponentModel.ISupportInitialize)chTestReadLine).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbTestStandard).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbTestDifference).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbTestReadImage).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox cbInvertColors;
        private Button btnCycleStop;
        private Button btnCycleStart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chTestStandard;
        private System.Windows.Forms.DataVisualization.Charting.Chart chTestDiff;
        private Label lblNumLineCaption;
        private CheckBox cbFullMax;
        private CheckBox cbVertical;
        private NumericUpDown numFrame;
        private System.Windows.Forms.DataVisualization.Charting.Chart chTestReadLine;
        private Label lblTestStandard;
        private Label lblTestDifference;
        private Label lblTestRead;
        private PictureBox pbTestStandard;
        private PictureBox pbTestDifference;
        private PictureBox pbTestReadImage;
        private Button btnTest_ReadSelectedSocket;
        private Label lblTestSelectedSocket;
        private Label lblTestSelectedSocketLabel;
        private Panel pnlSockets;
        private CheckBox cbTest_ExternalStart;
        private Button btnTest_ReadAllSocket;
        private CheckBox cbTestCCDMaxPointShow;
        private System.Windows.Forms.Timer timer1;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar pbGettingStandard;
        private ToolStripStatusLabel lblWorkStatus;
    }
}
