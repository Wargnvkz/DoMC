namespace DoMCLib.Forms
{
    partial class ShowPreformImages
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

        #region Код, автоматически созданный конструктором форм Windows

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
            lblTestStandard = new Label();
            lblTestDifference = new Label();
            lblTestRead = new Label();
            pbTestStandard = new PictureBox();
            cmsStandardImage = new ContextMenuStrip(components);
            tsmiSaveStandardImage = new ToolStripMenuItem();
            pbTestDifference = new PictureBox();
            cmsResultImage = new ContextMenuStrip(components);
            tsmiSaveResultImage = new ToolStripMenuItem();
            pbTestReadImage = new PictureBox();
            cmsImage = new ContextMenuStrip(components);
            tsmiSaveImage = new ToolStripMenuItem();
            chTestStandard = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chTestDiff = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chTestReadLine = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblNumLineCaption = new Label();
            cbFullMax = new CheckBox();
            cbVertical = new CheckBox();
            numFrame = new NumericUpDown();
            cbFullAvg = new CheckBox();
            lblResultResultTitle = new Label();
            lblResultResult = new Label();
            cbInvertColors = new CheckBox();
            lblTimeDecision = new Label();
            lblTimeDecisionTitle = new Label();
            cmsCalcType = new ContextMenuStrip(components);
            спеднеквадратическоеОтклонениеToolStripMenuItem = new ToolStripMenuItem();
            обычнаяРазницаToolStripMenuItem = new ToolStripMenuItem();
            tsmiNormalize = new ToolStripMenuItem();
            отклонениеНормализованногоToolStripMenuItem = new ToolStripMenuItem();
            tsmiGradient = new ToolStripMenuItem();
            tsmiVNormalize = new ToolStripMenuItem();
            tsmiVNormalizeDenoise = new ToolStripMenuItem();
            lblAverageCaption = new Label();
            lblAverage = new Label();
            cbShowCheckArea = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pbTestStandard).BeginInit();
            cmsStandardImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbTestDifference).BeginInit();
            cmsResultImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbTestReadImage).BeginInit();
            cmsImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chTestStandard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chTestDiff).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chTestReadLine).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFrame).BeginInit();
            cmsCalcType.SuspendLayout();
            SuspendLayout();
            // 
            // lblTestStandard
            // 
            lblTestStandard.AutoSize = true;
            lblTestStandard.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestStandard.Location = new Point(265, 70);
            lblTestStandard.Margin = new Padding(4, 0, 4, 0);
            lblTestStandard.Name = "lblTestStandard";
            lblTestStandard.Size = new Size(60, 24);
            lblTestStandard.TabIndex = 48;
            lblTestStandard.Text = "label8";
            // 
            // lblTestDifference
            // 
            lblTestDifference.AutoSize = true;
            lblTestDifference.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestDifference.Location = new Point(141, 70);
            lblTestDifference.Margin = new Padding(4, 0, 4, 0);
            lblTestDifference.Name = "lblTestDifference";
            lblTestDifference.Size = new Size(60, 24);
            lblTestDifference.TabIndex = 47;
            lblTestDifference.Text = "label7";
            // 
            // lblTestRead
            // 
            lblTestRead.AutoSize = true;
            lblTestRead.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestRead.Location = new Point(14, 70);
            lblTestRead.Margin = new Padding(4, 0, 4, 0);
            lblTestRead.Name = "lblTestRead";
            lblTestRead.Size = new Size(60, 24);
            lblTestRead.TabIndex = 46;
            lblTestRead.Text = "label2";
            // 
            // pbTestStandard
            // 
            pbTestStandard.BorderStyle = BorderStyle.FixedSingle;
            pbTestStandard.ContextMenuStrip = cmsStandardImage;
            pbTestStandard.Location = new Point(261, 102);
            pbTestStandard.Margin = new Padding(4, 3, 4, 3);
            pbTestStandard.Name = "pbTestStandard";
            pbTestStandard.Size = new Size(116, 57);
            pbTestStandard.TabIndex = 45;
            pbTestStandard.TabStop = false;
            pbTestStandard.Paint += pbTestStandard_Paint;
            pbTestStandard.MouseUp += pbTestStandard_MouseUp;
            // 
            // cmsStandardImage
            // 
            cmsStandardImage.Items.AddRange(new ToolStripItem[] { tsmiSaveStandardImage });
            cmsStandardImage.Name = "contextMenuStrip1";
            cmsStandardImage.Size = new Size(143, 26);
            // 
            // tsmiSaveStandardImage
            // 
            tsmiSaveStandardImage.Name = "tsmiSaveStandardImage";
            tsmiSaveStandardImage.Size = new Size(142, 22);
            tsmiSaveStandardImage.Text = "Сохранить...";
            tsmiSaveStandardImage.Click += tsmiSaveImage_Click;
            // 
            // pbTestDifference
            // 
            pbTestDifference.BorderStyle = BorderStyle.FixedSingle;
            pbTestDifference.ContextMenuStrip = cmsResultImage;
            pbTestDifference.Location = new Point(138, 102);
            pbTestDifference.Margin = new Padding(4, 3, 4, 3);
            pbTestDifference.Name = "pbTestDifference";
            pbTestDifference.Size = new Size(116, 57);
            pbTestDifference.TabIndex = 44;
            pbTestDifference.TabStop = false;
            pbTestDifference.Paint += pbTestDifference_Paint;
            pbTestDifference.MouseUp += pbTestDifference_MouseUp;
            // 
            // cmsResultImage
            // 
            cmsResultImage.Items.AddRange(new ToolStripItem[] { tsmiSaveResultImage });
            cmsResultImage.Name = "contextMenuStrip1";
            cmsResultImage.Size = new Size(143, 26);
            // 
            // tsmiSaveResultImage
            // 
            tsmiSaveResultImage.Name = "tsmiSaveResultImage";
            tsmiSaveResultImage.Size = new Size(142, 22);
            tsmiSaveResultImage.Text = "Сохранить...";
            tsmiSaveResultImage.Click += tsmiSaveImage_Click;
            // 
            // pbTestReadImage
            // 
            pbTestReadImage.BorderStyle = BorderStyle.FixedSingle;
            pbTestReadImage.ContextMenuStrip = cmsImage;
            pbTestReadImage.Location = new Point(14, 102);
            pbTestReadImage.Margin = new Padding(4, 3, 4, 3);
            pbTestReadImage.Name = "pbTestReadImage";
            pbTestReadImage.Size = new Size(116, 57);
            pbTestReadImage.TabIndex = 43;
            pbTestReadImage.TabStop = false;
            pbTestReadImage.Paint += pbTestReadImage_Paint;
            pbTestReadImage.MouseUp += pbTestReadImage_MouseUp;
            // 
            // cmsImage
            // 
            cmsImage.Items.AddRange(new ToolStripItem[] { tsmiSaveImage });
            cmsImage.Name = "cmsImage";
            cmsImage.Size = new Size(143, 26);
            // 
            // tsmiSaveImage
            // 
            tsmiSaveImage.Name = "tsmiSaveImage";
            tsmiSaveImage.Size = new Size(142, 22);
            tsmiSaveImage.Text = "Сохранить...";
            tsmiSaveImage.Click += tsmiSaveImage_Click;
            // 
            // chTestStandard
            // 
            chartArea1.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea1.Name = "ChartArea1";
            chTestStandard.ChartAreas.Add(chartArea1);
            chTestStandard.Location = new Point(654, 314);
            chTestStandard.Margin = new Padding(4, 3, 4, 3);
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
            chTestStandard.Size = new Size(309, 216);
            chTestStandard.TabIndex = 52;
            chTestStandard.Text = "chart1";
            // 
            // chTestDiff
            // 
            chartArea2.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea2.Name = "ChartArea1";
            chTestDiff.ChartAreas.Add(chartArea2);
            chTestDiff.Location = new Point(356, 314);
            chTestDiff.Margin = new Padding(4, 3, 4, 3);
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
            chTestDiff.Size = new Size(304, 216);
            chTestDiff.TabIndex = 51;
            chTestDiff.Text = "chart1";
            // 
            // chTestReadLine
            // 
            chartArea3.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea3.Name = "ChartArea1";
            chTestReadLine.ChartAreas.Add(chartArea3);
            chTestReadLine.Location = new Point(14, 314);
            chTestReadLine.Margin = new Padding(4, 3, 4, 3);
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
            chTestReadLine.Size = new Size(364, 216);
            chTestReadLine.TabIndex = 50;
            chTestReadLine.Text = "chart1";
            // 
            // lblNumLineCaption
            // 
            lblNumLineCaption.AutoSize = true;
            lblNumLineCaption.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblNumLineCaption.Location = new Point(644, 6);
            lblNumLineCaption.Margin = new Padding(4, 0, 4, 0);
            lblNumLineCaption.Name = "lblNumLineCaption";
            lblNumLineCaption.Size = new Size(133, 24);
            lblNumLineCaption.TabIndex = 56;
            lblNumLineCaption.Text = "Номер линии:";
            // 
            // cbFullMax
            // 
            cbFullMax.AutoSize = true;
            cbFullMax.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbFullMax.Location = new Point(14, 5);
            cbFullMax.Margin = new Padding(4, 3, 4, 3);
            cbFullMax.Name = "cbFullMax";
            cbFullMax.Size = new Size(245, 28);
            cbFullMax.TabIndex = 55;
            cbFullMax.Text = "Максимальное по кадру";
            cbFullMax.UseVisualStyleBackColor = true;
            cbFullMax.CheckedChanged += cbFullMax_CheckedChanged;
            // 
            // cbVertical
            // 
            cbVertical.AutoSize = true;
            cbVertical.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbVertical.Location = new Point(926, 5);
            cbVertical.Margin = new Padding(4, 3, 4, 3);
            cbVertical.Name = "cbVertical";
            cbVertical.Size = new Size(213, 28);
            cbVertical.TabIndex = 54;
            cbVertical.Text = "Вертикальная линия";
            cbVertical.UseVisualStyleBackColor = true;
            cbVertical.CheckedChanged += cbVertical_CheckedChanged;
            // 
            // numFrame
            // 
            numFrame.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            numFrame.Location = new Point(811, 4);
            numFrame.Margin = new Padding(4, 3, 4, 3);
            numFrame.Maximum = new decimal(new int[] { 511, 0, 0, 0 });
            numFrame.Name = "numFrame";
            numFrame.Size = new Size(82, 29);
            numFrame.TabIndex = 53;
            numFrame.ValueChanged += numFrame_ValueChanged;
            // 
            // cbFullAvg
            // 
            cbFullAvg.AutoSize = true;
            cbFullAvg.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbFullAvg.Location = new Point(14, 36);
            cbFullAvg.Margin = new Padding(4, 3, 4, 3);
            cbFullAvg.Name = "cbFullAvg";
            cbFullAvg.Size = new Size(192, 28);
            cbFullAvg.TabIndex = 60;
            cbFullAvg.Text = "Среднее по кадру";
            cbFullAvg.UseVisualStyleBackColor = true;
            cbFullAvg.CheckedChanged += cbFullMax_CheckedChanged;
            // 
            // lblResultResultTitle
            // 
            lblResultResultTitle.AutoSize = true;
            lblResultResultTitle.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblResultResultTitle.Location = new Point(341, 6);
            lblResultResultTitle.Margin = new Padding(4, 0, 4, 0);
            lblResultResultTitle.Name = "lblResultResultTitle";
            lblResultResultTitle.Size = new Size(108, 24);
            lblResultResultTitle.TabIndex = 67;
            lblResultResultTitle.Text = "Результат:";
            // 
            // lblResultResult
            // 
            lblResultResult.AutoSize = true;
            lblResultResult.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblResultResult.Location = new Point(474, 6);
            lblResultResult.Margin = new Padding(4, 0, 4, 0);
            lblResultResult.Name = "lblResultResult";
            lblResultResult.Size = new Size(24, 24);
            lblResultResult.TabIndex = 68;
            lblResultResult.Text = "X";
            // 
            // cbInvertColors
            // 
            cbInvertColors.AutoSize = true;
            cbInvertColors.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbInvertColors.Location = new Point(926, 31);
            cbInvertColors.Margin = new Padding(4, 3, 4, 3);
            cbInvertColors.Name = "cbInvertColors";
            cbInvertColors.Size = new Size(187, 28);
            cbInvertColors.TabIndex = 70;
            cbInvertColors.Text = "Инверсия цветов";
            cbInvertColors.UseVisualStyleBackColor = true;
            cbInvertColors.CheckedChanged += cbInvertColors_CheckedChanged;
            // 
            // lblTimeDecision
            // 
            lblTimeDecision.AutoSize = true;
            lblTimeDecision.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTimeDecision.Location = new Point(564, 39);
            lblTimeDecision.Margin = new Padding(4, 0, 4, 0);
            lblTimeDecision.Name = "lblTimeDecision";
            lblTimeDecision.Size = new Size(24, 24);
            lblTimeDecision.TabIndex = 72;
            lblTimeDecision.Text = "X";
            // 
            // lblTimeDecisionTitle
            // 
            lblTimeDecisionTitle.AutoSize = true;
            lblTimeDecisionTitle.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTimeDecisionTitle.Location = new Point(341, 39);
            lblTimeDecisionTitle.Margin = new Padding(4, 0, 4, 0);
            lblTimeDecisionTitle.Name = "lblTimeDecisionTitle";
            lblTimeDecisionTitle.Size = new Size(185, 24);
            lblTimeDecisionTitle.TabIndex = 71;
            lblTimeDecisionTitle.Text = "Время вычисления:";
            // 
            // cmsCalcType
            // 
            cmsCalcType.Items.AddRange(new ToolStripItem[] { спеднеквадратическоеОтклонениеToolStripMenuItem, обычнаяРазницаToolStripMenuItem, tsmiNormalize, отклонениеНормализованногоToolStripMenuItem, tsmiGradient, tsmiVNormalize, tsmiVNormalizeDenoise });
            cmsCalcType.Name = "contextMenuStrip1";
            cmsCalcType.Size = new Size(346, 158);
            // 
            // спеднеквадратическоеОтклонениеToolStripMenuItem
            // 
            спеднеквадратическоеОтклонениеToolStripMenuItem.Name = "спеднеквадратическоеОтклонениеToolStripMenuItem";
            спеднеквадратическоеОтклонениеToolStripMenuItem.Size = new Size(345, 22);
            спеднеквадратическоеОтклонениеToolStripMenuItem.Text = "Обработка решения";
            спеднеквадратическоеОтклонениеToolStripMenuItem.Click += спеднеквадратическоеОтклонениеToolStripMenuItem_Click;
            // 
            // обычнаяРазницаToolStripMenuItem
            // 
            обычнаяРазницаToolStripMenuItem.Name = "обычнаяРазницаToolStripMenuItem";
            обычнаяРазницаToolStripMenuItem.Size = new Size(345, 22);
            обычнаяРазницаToolStripMenuItem.Text = "Обычная разница";
            обычнаяРазницаToolStripMenuItem.Click += tsmiPlainDifference_Click;
            // 
            // tsmiNormalize
            // 
            tsmiNormalize.Name = "tsmiNormalize";
            tsmiNormalize.Size = new Size(345, 22);
            tsmiNormalize.Text = "Нормализация";
            tsmiNormalize.Click += tsmiNormalize_Click;
            // 
            // отклонениеНормализованногоToolStripMenuItem
            // 
            отклонениеНормализованногоToolStripMenuItem.Name = "отклонениеНормализованногоToolStripMenuItem";
            отклонениеНормализованногоToolStripMenuItem.Size = new Size(345, 22);
            отклонениеНормализованногоToolStripMenuItem.Text = "Отклонение нормализованного";
            отклонениеНормализованногоToolStripMenuItem.Click += tsmiDeviationOfNormilized_Click;
            // 
            // tsmiGradient
            // 
            tsmiGradient.Name = "tsmiGradient";
            tsmiGradient.Size = new Size(345, 22);
            tsmiGradient.Text = "Градиент";
            tsmiGradient.Click += tsmiGradient_Click;
            // 
            // tsmiVNormalize
            // 
            tsmiVNormalize.Name = "tsmiVNormalize";
            tsmiVNormalize.Size = new Size(345, 22);
            tsmiVNormalize.Text = "Нормализовать по вертикали";
            tsmiVNormalize.Click += tsmiVNormalize_Click;
            // 
            // tsmiVNormalizeDenoise
            // 
            tsmiVNormalizeDenoise.Name = "tsmiVNormalizeDenoise";
            tsmiVNormalizeDenoise.Size = new Size(345, 22);
            tsmiVNormalizeDenoise.Text = "Нормализовать по вертикали с удалением шума";
            tsmiVNormalizeDenoise.Click += tsmiVNormalizeDenoise_Click;
            // 
            // lblAverageCaption
            // 
            lblAverageCaption.AutoSize = true;
            lblAverageCaption.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblAverageCaption.Location = new Point(1182, 6);
            lblAverageCaption.Margin = new Padding(4, 0, 4, 0);
            lblAverageCaption.Name = "lblAverageCaption";
            lblAverageCaption.Size = new Size(95, 24);
            lblAverageCaption.TabIndex = 73;
            lblAverageCaption.Text = "Среднее:";
            // 
            // lblAverage
            // 
            lblAverage.AutoSize = true;
            lblAverage.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblAverage.Location = new Point(1300, 6);
            lblAverage.Margin = new Padding(4, 0, 4, 0);
            lblAverage.Name = "lblAverage";
            lblAverage.Size = new Size(24, 24);
            lblAverage.TabIndex = 74;
            lblAverage.Text = "X";
            // 
            // cbShowCheckArea
            // 
            cbShowCheckArea.AutoSize = true;
            cbShowCheckArea.Checked = true;
            cbShowCheckArea.CheckState = CheckState.Checked;
            cbShowCheckArea.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbShowCheckArea.Location = new Point(1186, 31);
            cbShowCheckArea.Margin = new Padding(4, 3, 4, 3);
            cbShowCheckArea.Name = "cbShowCheckArea";
            cbShowCheckArea.Size = new Size(285, 28);
            cbShowCheckArea.TabIndex = 75;
            cbShowCheckArea.Text = "Показать границы проверки";
            cbShowCheckArea.UseVisualStyleBackColor = true;
            cbShowCheckArea.CheckedChanged += cbShowCheckSquare_CheckedChanged;
            // 
            // ShowPreformImages
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1373, 963);
            ContextMenuStrip = cmsCalcType;
            Controls.Add(cbShowCheckArea);
            Controls.Add(lblAverage);
            Controls.Add(lblAverageCaption);
            Controls.Add(lblTimeDecision);
            Controls.Add(lblTimeDecisionTitle);
            Controls.Add(cbInvertColors);
            Controls.Add(lblResultResult);
            Controls.Add(lblResultResultTitle);
            Controls.Add(cbFullAvg);
            Controls.Add(lblNumLineCaption);
            Controls.Add(cbFullMax);
            Controls.Add(cbVertical);
            Controls.Add(numFrame);
            Controls.Add(chTestStandard);
            Controls.Add(chTestDiff);
            Controls.Add(chTestReadLine);
            Controls.Add(lblTestStandard);
            Controls.Add(lblTestDifference);
            Controls.Add(lblTestRead);
            Controls.Add(pbTestStandard);
            Controls.Add(pbTestDifference);
            Controls.Add(pbTestReadImage);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ShowPreformImages";
            Text = "Просмотр результатов измерений";
            Resize += CheckPreformAlgorithmsForm_Resize;
            ((System.ComponentModel.ISupportInitialize)pbTestStandard).EndInit();
            cmsStandardImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbTestDifference).EndInit();
            cmsResultImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbTestReadImage).EndInit();
            cmsImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chTestStandard).EndInit();
            ((System.ComponentModel.ISupportInitialize)chTestDiff).EndInit();
            ((System.ComponentModel.ISupportInitialize)chTestReadLine).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFrame).EndInit();
            cmsCalcType.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label lblTestStandard;
        private System.Windows.Forms.Label lblTestDifference;
        private System.Windows.Forms.Label lblTestRead;
        private System.Windows.Forms.PictureBox pbTestStandard;
        private System.Windows.Forms.PictureBox pbTestDifference;
        private System.Windows.Forms.PictureBox pbTestReadImage;
        private System.Windows.Forms.DataVisualization.Charting.Chart chTestStandard;
        private System.Windows.Forms.DataVisualization.Charting.Chart chTestDiff;
        private System.Windows.Forms.DataVisualization.Charting.Chart chTestReadLine;
        private System.Windows.Forms.Label lblNumLineCaption;
        private System.Windows.Forms.CheckBox cbFullMax;
        private System.Windows.Forms.CheckBox cbVertical;
        private System.Windows.Forms.NumericUpDown numFrame;
        private System.Windows.Forms.CheckBox cbFullAvg;
        private System.Windows.Forms.Label lblResultResultTitle;
        private System.Windows.Forms.Label lblResultResult;
        private System.Windows.Forms.CheckBox cbInvertColors;
        private System.Windows.Forms.Label lblTimeDecision;
        private System.Windows.Forms.Label lblTimeDecisionTitle;
        private System.Windows.Forms.ContextMenuStrip cmsImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveImage;
        private System.Windows.Forms.ContextMenuStrip cmsStandardImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveStandardImage;
        private System.Windows.Forms.ContextMenuStrip cmsResultImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveResultImage;
        private System.Windows.Forms.ContextMenuStrip cmsCalcType;
        private System.Windows.Forms.ToolStripMenuItem спеднеквадратическоеОтклонениеToolStripMenuItem;
        private System.Windows.Forms.Label lblAverageCaption;
        private System.Windows.Forms.Label lblAverage;
        private System.Windows.Forms.ToolStripMenuItem обычнаяРазницаToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbShowCheckArea;
        private System.Windows.Forms.ToolStripMenuItem tsmiNormalize;
        private System.Windows.Forms.ToolStripMenuItem отклонениеНормализованногоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiGradient;
        private System.Windows.Forms.ToolStripMenuItem tsmiVNormalize;
        private System.Windows.Forms.ToolStripMenuItem tsmiVNormalizeDenoise;
    }
}

