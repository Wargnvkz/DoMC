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
            this.components = new System.ComponentModel.Container();
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
            this.lblTestStandard = new System.Windows.Forms.Label();
            this.lblTestDifference = new System.Windows.Forms.Label();
            this.lblTestRead = new System.Windows.Forms.Label();
            this.pbTestStandard = new System.Windows.Forms.PictureBox();
            this.cmsStandardImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSaveStandardImage = new System.Windows.Forms.ToolStripMenuItem();
            this.pbTestDifference = new System.Windows.Forms.PictureBox();
            this.cmsResultImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSaveResultImage = new System.Windows.Forms.ToolStripMenuItem();
            this.pbTestReadImage = new System.Windows.Forms.PictureBox();
            this.cmsImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.chTestStandard = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chTestDiff = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chTestReadLine = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblNumLineCaption = new System.Windows.Forms.Label();
            this.cbFullMax = new System.Windows.Forms.CheckBox();
            this.cbVertical = new System.Windows.Forms.CheckBox();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.cbFullAvg = new System.Windows.Forms.CheckBox();
            this.lblResultResultTitle = new System.Windows.Forms.Label();
            this.lblResultResult = new System.Windows.Forms.Label();
            this.cbInvertColors = new System.Windows.Forms.CheckBox();
            this.lblTimeDecision = new System.Windows.Forms.Label();
            this.lblTimeDecisionTitle = new System.Windows.Forms.Label();
            this.cmsCalcType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.спеднеквадратическоеОтклонениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обычнаяРазницаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNormalize = new System.Windows.Forms.ToolStripMenuItem();
            this.отклонениеНормализованногоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGradient = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVNormalize = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVNormalizeDenoise = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAverageCaption = new System.Windows.Forms.Label();
            this.lblAverage = new System.Windows.Forms.Label();
            this.cbShowCheckArea = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbTestStandard)).BeginInit();
            this.cmsStandardImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTestDifference)).BeginInit();
            this.cmsResultImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTestReadImage)).BeginInit();
            this.cmsImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chTestStandard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chTestDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chTestReadLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.cmsCalcType.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTestStandard
            // 
            this.lblTestStandard.AutoSize = true;
            this.lblTestStandard.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTestStandard.Location = new System.Drawing.Point(227, 61);
            this.lblTestStandard.Name = "lblTestStandard";
            this.lblTestStandard.Size = new System.Drawing.Size(60, 24);
            this.lblTestStandard.TabIndex = 48;
            this.lblTestStandard.Text = "label8";
            // 
            // lblTestDifference
            // 
            this.lblTestDifference.AutoSize = true;
            this.lblTestDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTestDifference.Location = new System.Drawing.Point(121, 61);
            this.lblTestDifference.Name = "lblTestDifference";
            this.lblTestDifference.Size = new System.Drawing.Size(60, 24);
            this.lblTestDifference.TabIndex = 47;
            this.lblTestDifference.Text = "label7";
            // 
            // lblTestRead
            // 
            this.lblTestRead.AutoSize = true;
            this.lblTestRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTestRead.Location = new System.Drawing.Point(12, 61);
            this.lblTestRead.Name = "lblTestRead";
            this.lblTestRead.Size = new System.Drawing.Size(60, 24);
            this.lblTestRead.TabIndex = 46;
            this.lblTestRead.Text = "label2";
            // 
            // pbTestStandard
            // 
            this.pbTestStandard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTestStandard.ContextMenuStrip = this.cmsStandardImage;
            this.pbTestStandard.Location = new System.Drawing.Point(224, 88);
            this.pbTestStandard.Name = "pbTestStandard";
            this.pbTestStandard.Size = new System.Drawing.Size(100, 50);
            this.pbTestStandard.TabIndex = 45;
            this.pbTestStandard.TabStop = false;
            this.pbTestStandard.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTestStandard_Paint);
            this.pbTestStandard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbTestStandard_MouseUp);
            // 
            // cmsStandardImage
            // 
            this.cmsStandardImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSaveStandardImage});
            this.cmsStandardImage.Name = "contextMenuStrip1";
            this.cmsStandardImage.Size = new System.Drawing.Size(143, 26);
            // 
            // tsmiSaveStandardImage
            // 
            this.tsmiSaveStandardImage.Name = "tsmiSaveStandardImage";
            this.tsmiSaveStandardImage.Size = new System.Drawing.Size(142, 22);
            this.tsmiSaveStandardImage.Text = "Сохранить...";
            this.tsmiSaveStandardImage.Click += new System.EventHandler(this.tsmiSaveImage_Click);
            // 
            // pbTestDifference
            // 
            this.pbTestDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTestDifference.ContextMenuStrip = this.cmsResultImage;
            this.pbTestDifference.Location = new System.Drawing.Point(118, 88);
            this.pbTestDifference.Name = "pbTestDifference";
            this.pbTestDifference.Size = new System.Drawing.Size(100, 50);
            this.pbTestDifference.TabIndex = 44;
            this.pbTestDifference.TabStop = false;
            this.pbTestDifference.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTestDifference_Paint);
            this.pbTestDifference.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbTestDifference_MouseUp);
            // 
            // cmsResultImage
            // 
            this.cmsResultImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSaveResultImage});
            this.cmsResultImage.Name = "contextMenuStrip1";
            this.cmsResultImage.Size = new System.Drawing.Size(143, 26);
            // 
            // tsmiSaveResultImage
            // 
            this.tsmiSaveResultImage.Name = "tsmiSaveResultImage";
            this.tsmiSaveResultImage.Size = new System.Drawing.Size(142, 22);
            this.tsmiSaveResultImage.Text = "Сохранить...";
            this.tsmiSaveResultImage.Click += new System.EventHandler(this.tsmiSaveImage_Click);
            // 
            // pbTestReadImage
            // 
            this.pbTestReadImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTestReadImage.ContextMenuStrip = this.cmsImage;
            this.pbTestReadImage.Location = new System.Drawing.Point(12, 88);
            this.pbTestReadImage.Name = "pbTestReadImage";
            this.pbTestReadImage.Size = new System.Drawing.Size(100, 50);
            this.pbTestReadImage.TabIndex = 43;
            this.pbTestReadImage.TabStop = false;
            this.pbTestReadImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTestReadImage_Paint);
            this.pbTestReadImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbTestReadImage_MouseUp);
            // 
            // cmsImage
            // 
            this.cmsImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSaveImage});
            this.cmsImage.Name = "cmsImage";
            this.cmsImage.Size = new System.Drawing.Size(143, 26);
            // 
            // tsmiSaveImage
            // 
            this.tsmiSaveImage.Name = "tsmiSaveImage";
            this.tsmiSaveImage.Size = new System.Drawing.Size(142, 22);
            this.tsmiSaveImage.Text = "Сохранить...";
            this.tsmiSaveImage.Click += new System.EventHandler(this.tsmiSaveImage_Click);
            // 
            // chTestStandard
            // 
            chartArea1.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea1.Name = "ChartArea1";
            this.chTestStandard.ChartAreas.Add(chartArea1);
            this.chTestStandard.Location = new System.Drawing.Point(561, 272);
            this.chTestStandard.Name = "chTestStandard";
            this.chTestStandard.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
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
            this.chTestStandard.Series.Add(series1);
            this.chTestStandard.Series.Add(series2);
            this.chTestStandard.Series.Add(series3);
            this.chTestStandard.Size = new System.Drawing.Size(265, 187);
            this.chTestStandard.TabIndex = 52;
            this.chTestStandard.Text = "chart1";
            // 
            // chTestDiff
            // 
            chartArea2.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea2.Name = "ChartArea1";
            this.chTestDiff.ChartAreas.Add(chartArea2);
            this.chTestDiff.Location = new System.Drawing.Point(305, 272);
            this.chTestDiff.Name = "chTestDiff";
            this.chTestDiff.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
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
            this.chTestDiff.Series.Add(series4);
            this.chTestDiff.Series.Add(series5);
            this.chTestDiff.Series.Add(series6);
            this.chTestDiff.Size = new System.Drawing.Size(261, 187);
            this.chTestDiff.TabIndex = 51;
            this.chTestDiff.Text = "chart1";
            // 
            // chTestReadLine
            // 
            chartArea3.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea3.Name = "ChartArea1";
            this.chTestReadLine.ChartAreas.Add(chartArea3);
            this.chTestReadLine.Location = new System.Drawing.Point(12, 272);
            this.chTestReadLine.Name = "chTestReadLine";
            this.chTestReadLine.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
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
            this.chTestReadLine.Series.Add(series7);
            this.chTestReadLine.Series.Add(series8);
            this.chTestReadLine.Series.Add(series9);
            this.chTestReadLine.Size = new System.Drawing.Size(312, 187);
            this.chTestReadLine.TabIndex = 50;
            this.chTestReadLine.Text = "chart1";
            // 
            // lblNumLineCaption
            // 
            this.lblNumLineCaption.AutoSize = true;
            this.lblNumLineCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNumLineCaption.Location = new System.Drawing.Point(557, 9);
            this.lblNumLineCaption.Name = "lblNumLineCaption";
            this.lblNumLineCaption.Size = new System.Drawing.Size(133, 24);
            this.lblNumLineCaption.TabIndex = 56;
            this.lblNumLineCaption.Text = "Номер линии:";
            // 
            // cbFullMax
            // 
            this.cbFullMax.AutoSize = true;
            this.cbFullMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbFullMax.Location = new System.Drawing.Point(12, 4);
            this.cbFullMax.Name = "cbFullMax";
            this.cbFullMax.Size = new System.Drawing.Size(245, 28);
            this.cbFullMax.TabIndex = 55;
            this.cbFullMax.Text = "Максимальное по кадру";
            this.cbFullMax.UseVisualStyleBackColor = true;
            this.cbFullMax.CheckedChanged += new System.EventHandler(this.cbFullMax_CheckedChanged);
            // 
            // cbVertical
            // 
            this.cbVertical.AutoSize = true;
            this.cbVertical.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVertical.Location = new System.Drawing.Point(794, 4);
            this.cbVertical.Name = "cbVertical";
            this.cbVertical.Size = new System.Drawing.Size(213, 28);
            this.cbVertical.TabIndex = 54;
            this.cbVertical.Text = "Вертикальная линия";
            this.cbVertical.UseVisualStyleBackColor = true;
            this.cbVertical.CheckedChanged += new System.EventHandler(this.cbVertical_CheckedChanged);
            // 
            // numFrame
            // 
            this.numFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numFrame.Location = new System.Drawing.Point(696, 7);
            this.numFrame.Maximum = new decimal(new int[] {
            511,
            0,
            0,
            0});
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(70, 29);
            this.numFrame.TabIndex = 53;
            this.numFrame.ValueChanged += new System.EventHandler(this.numFrame_ValueChanged);
            // 
            // cbFullAvg
            // 
            this.cbFullAvg.AutoSize = true;
            this.cbFullAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbFullAvg.Location = new System.Drawing.Point(12, 31);
            this.cbFullAvg.Name = "cbFullAvg";
            this.cbFullAvg.Size = new System.Drawing.Size(192, 28);
            this.cbFullAvg.TabIndex = 60;
            this.cbFullAvg.Text = "Среднее по кадру";
            this.cbFullAvg.UseVisualStyleBackColor = true;
            this.cbFullAvg.CheckedChanged += new System.EventHandler(this.cbFullMax_CheckedChanged);
            // 
            // lblResultResultTitle
            // 
            this.lblResultResultTitle.AutoSize = true;
            this.lblResultResultTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResultResultTitle.Location = new System.Drawing.Point(292, 5);
            this.lblResultResultTitle.Name = "lblResultResultTitle";
            this.lblResultResultTitle.Size = new System.Drawing.Size(108, 24);
            this.lblResultResultTitle.TabIndex = 67;
            this.lblResultResultTitle.Text = "Результат:";
            // 
            // lblResultResult
            // 
            this.lblResultResult.AutoSize = true;
            this.lblResultResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResultResult.Location = new System.Drawing.Point(406, 5);
            this.lblResultResult.Name = "lblResultResult";
            this.lblResultResult.Size = new System.Drawing.Size(24, 24);
            this.lblResultResult.TabIndex = 68;
            this.lblResultResult.Text = "X";
            // 
            // cbInvertColors
            // 
            this.cbInvertColors.AutoSize = true;
            this.cbInvertColors.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbInvertColors.Location = new System.Drawing.Point(794, 27);
            this.cbInvertColors.Name = "cbInvertColors";
            this.cbInvertColors.Size = new System.Drawing.Size(187, 28);
            this.cbInvertColors.TabIndex = 70;
            this.cbInvertColors.Text = "Инверсия цветов";
            this.cbInvertColors.UseVisualStyleBackColor = true;
            this.cbInvertColors.CheckedChanged += new System.EventHandler(this.cbInvertColors_CheckedChanged);
            // 
            // lblTimeDecision
            // 
            this.lblTimeDecision.AutoSize = true;
            this.lblTimeDecision.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTimeDecision.Location = new System.Drawing.Point(483, 34);
            this.lblTimeDecision.Name = "lblTimeDecision";
            this.lblTimeDecision.Size = new System.Drawing.Size(24, 24);
            this.lblTimeDecision.TabIndex = 72;
            this.lblTimeDecision.Text = "X";
            // 
            // lblTimeDecisionTitle
            // 
            this.lblTimeDecisionTitle.AutoSize = true;
            this.lblTimeDecisionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTimeDecisionTitle.Location = new System.Drawing.Point(292, 34);
            this.lblTimeDecisionTitle.Name = "lblTimeDecisionTitle";
            this.lblTimeDecisionTitle.Size = new System.Drawing.Size(185, 24);
            this.lblTimeDecisionTitle.TabIndex = 71;
            this.lblTimeDecisionTitle.Text = "Время вычисления:";
            // 
            // cmsCalcType
            // 
            this.cmsCalcType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.спеднеквадратическоеОтклонениеToolStripMenuItem,
            this.обычнаяРазницаToolStripMenuItem,
            this.tsmiNormalize,
            this.отклонениеНормализованногоToolStripMenuItem,
            this.tsmiGradient,
            this.tsmiVNormalize,
            this.tsmiVNormalizeDenoise});
            this.cmsCalcType.Name = "contextMenuStrip1";
            this.cmsCalcType.Size = new System.Drawing.Size(346, 180);
            // 
            // спеднеквадратическоеОтклонениеToolStripMenuItem
            // 
            this.спеднеквадратическоеОтклонениеToolStripMenuItem.Name = "спеднеквадратическоеОтклонениеToolStripMenuItem";
            this.спеднеквадратическоеОтклонениеToolStripMenuItem.Size = new System.Drawing.Size(345, 22);
            this.спеднеквадратическоеОтклонениеToolStripMenuItem.Text = "Обработка решения";
            this.спеднеквадратическоеОтклонениеToolStripMenuItem.Click += new System.EventHandler(this.спеднеквадратическоеОтклонениеToolStripMenuItem_Click);
            // 
            // обычнаяРазницаToolStripMenuItem
            // 
            this.обычнаяРазницаToolStripMenuItem.Name = "обычнаяРазницаToolStripMenuItem";
            this.обычнаяРазницаToolStripMenuItem.Size = new System.Drawing.Size(345, 22);
            this.обычнаяРазницаToolStripMenuItem.Text = "Обычная разница";
            this.обычнаяРазницаToolStripMenuItem.Click += new System.EventHandler(this.tsmiPlainDifference_Click);
            // 
            // tsmiNormalize
            // 
            this.tsmiNormalize.Name = "tsmiNormalize";
            this.tsmiNormalize.Size = new System.Drawing.Size(345, 22);
            this.tsmiNormalize.Text = "Нормализация";
            this.tsmiNormalize.Click += new System.EventHandler(this.tsmiNormalize_Click);
            // 
            // отклонениеНормализованногоToolStripMenuItem
            // 
            this.отклонениеНормализованногоToolStripMenuItem.Name = "отклонениеНормализованногоToolStripMenuItem";
            this.отклонениеНормализованногоToolStripMenuItem.Size = new System.Drawing.Size(345, 22);
            this.отклонениеНормализованногоToolStripMenuItem.Text = "Отклонение нормализованного";
            this.отклонениеНормализованногоToolStripMenuItem.Click += new System.EventHandler(this.tsmiDeviationOfNormilized_Click);
            // 
            // tsmiGradient
            // 
            this.tsmiGradient.Name = "tsmiGradient";
            this.tsmiGradient.Size = new System.Drawing.Size(345, 22);
            this.tsmiGradient.Text = "Градиент";
            this.tsmiGradient.Click += new System.EventHandler(this.tsmiGradient_Click);
            // 
            // tsmiVNormalize
            // 
            this.tsmiVNormalize.Name = "tsmiVNormalize";
            this.tsmiVNormalize.Size = new System.Drawing.Size(345, 22);
            this.tsmiVNormalize.Text = "Нормализовать по вертикали";
            this.tsmiVNormalize.Click += new System.EventHandler(this.tsmiVNormalize_Click);
            // 
            // tsmiVNormalizeDenoise
            // 
            this.tsmiVNormalizeDenoise.Name = "tsmiVNormalizeDenoise";
            this.tsmiVNormalizeDenoise.Size = new System.Drawing.Size(345, 22);
            this.tsmiVNormalizeDenoise.Text = "Нормализовать по вертикали с удалением шума";
            this.tsmiVNormalizeDenoise.Click += new System.EventHandler(this.tsmiVNormalizeDenoise_Click);
            // 
            // lblAverageCaption
            // 
            this.lblAverageCaption.AutoSize = true;
            this.lblAverageCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAverageCaption.Location = new System.Drawing.Point(1013, 5);
            this.lblAverageCaption.Name = "lblAverageCaption";
            this.lblAverageCaption.Size = new System.Drawing.Size(95, 24);
            this.lblAverageCaption.TabIndex = 73;
            this.lblAverageCaption.Text = "Среднее:";
            // 
            // lblAverage
            // 
            this.lblAverage.AutoSize = true;
            this.lblAverage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAverage.Location = new System.Drawing.Point(1114, 5);
            this.lblAverage.Name = "lblAverage";
            this.lblAverage.Size = new System.Drawing.Size(24, 24);
            this.lblAverage.TabIndex = 74;
            this.lblAverage.Text = "X";
            // 
            // cbShowCheckArea
            // 
            this.cbShowCheckArea.AutoSize = true;
            this.cbShowCheckArea.Checked = true;
            this.cbShowCheckArea.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowCheckArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbShowCheckArea.Location = new System.Drawing.Point(1017, 27);
            this.cbShowCheckArea.Name = "cbShowCheckArea";
            this.cbShowCheckArea.Size = new System.Drawing.Size(285, 28);
            this.cbShowCheckArea.TabIndex = 75;
            this.cbShowCheckArea.Text = "Показать границы проверки";
            this.cbShowCheckArea.UseVisualStyleBackColor = true;
            this.cbShowCheckArea.CheckedChanged += new System.EventHandler(this.cbShowCheckSquare_CheckedChanged);
            // 
            // ShowPreformImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1177, 835);
            this.ContextMenuStrip = this.cmsCalcType;
            this.Controls.Add(this.cbShowCheckArea);
            this.Controls.Add(this.lblAverage);
            this.Controls.Add(this.lblAverageCaption);
            this.Controls.Add(this.lblTimeDecision);
            this.Controls.Add(this.lblTimeDecisionTitle);
            this.Controls.Add(this.cbInvertColors);
            this.Controls.Add(this.lblResultResult);
            this.Controls.Add(this.lblResultResultTitle);
            this.Controls.Add(this.cbFullAvg);
            this.Controls.Add(this.lblNumLineCaption);
            this.Controls.Add(this.cbFullMax);
            this.Controls.Add(this.cbVertical);
            this.Controls.Add(this.numFrame);
            this.Controls.Add(this.chTestStandard);
            this.Controls.Add(this.chTestDiff);
            this.Controls.Add(this.chTestReadLine);
            this.Controls.Add(this.lblTestStandard);
            this.Controls.Add(this.lblTestDifference);
            this.Controls.Add(this.lblTestRead);
            this.Controls.Add(this.pbTestStandard);
            this.Controls.Add(this.pbTestDifference);
            this.Controls.Add(this.pbTestReadImage);
            this.Name = "ShowPreformImages";
            this.Text = "Просмотр результатов измерений";
            this.Resize += new System.EventHandler(this.CheckPreformAlgorithmsForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbTestStandard)).EndInit();
            this.cmsStandardImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTestDifference)).EndInit();
            this.cmsResultImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTestReadImage)).EndInit();
            this.cmsImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chTestStandard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chTestDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chTestReadLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            this.cmsCalcType.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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

