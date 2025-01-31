namespace DoMC.Forms
{
    partial class ShowFrameForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pbImg = new System.Windows.Forms.PictureBox();
            this.cmsFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsiSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.cbVertical = new System.Windows.Forms.CheckBox();
            this.chMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chFreq = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbFullMax = new System.Windows.Forms.CheckBox();
            this.lblNumLineCaption = new System.Windows.Forms.Label();
            this.lblDevAvgCaption = new System.Windows.Forms.Label();
            this.lblDevDevCaption = new System.Windows.Forms.Label();
            this.lblDevAvgText = new System.Windows.Forms.Label();
            this.lblDevDevText = new System.Windows.Forms.Label();
            this.lblPercentText = new System.Windows.Forms.Label();
            this.cmsiSaveCompressedFile = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).BeginInit();
            this.cmsFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFreq)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImg
            // 
            this.pbImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImg.ContextMenuStrip = this.cmsFiles;
            this.pbImg.Location = new System.Drawing.Point(1, 1);
            this.pbImg.Name = "pbImg";
            this.pbImg.Size = new System.Drawing.Size(512, 512);
            this.pbImg.TabIndex = 0;
            this.pbImg.TabStop = false;
            this.pbImg.Paint += new System.Windows.Forms.PaintEventHandler(this.pbImg_Paint);
            this.pbImg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbImg_MouseUp);
            // 
            // cmsFiles
            // 
            this.cmsFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsiSaveFile,
            this.cmsiSaveCompressedFile});
            this.cmsFiles.Name = "cmsFiles";
            this.cmsFiles.Size = new System.Drawing.Size(294, 70);
            // 
            // cmsiSaveFile
            // 
            this.cmsiSaveFile.Name = "cmsiSaveFile";
            this.cmsiSaveFile.Size = new System.Drawing.Size(293, 22);
            this.cmsiSaveFile.Text = "Сохранить изображение...";
            this.cmsiSaveFile.Click += new System.EventHandler(this.cmsiSaveFile_Click);
            // 
            // numFrame
            // 
            this.numFrame.Location = new System.Drawing.Point(847, 4);
            this.numFrame.Maximum = new decimal(new int[] {
            511,
            0,
            0,
            0});
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(70, 20);
            this.numFrame.TabIndex = 6;
            this.numFrame.ValueChanged += new System.EventHandler(this.numFrame_ValueChanged);
            // 
            // cbVertical
            // 
            this.cbVertical.AutoSize = true;
            this.cbVertical.Location = new System.Drawing.Point(973, 5);
            this.cbVertical.Name = "cbVertical";
            this.cbVertical.Size = new System.Drawing.Size(131, 17);
            this.cbVertical.TabIndex = 7;
            this.cbVertical.Text = "Вертикальная линия";
            this.cbVertical.UseVisualStyleBackColor = true;
            this.cbVertical.CheckedChanged += new System.EventHandler(this.cbVertical_CheckedChanged);
            // 
            // chMain
            // 
            this.chMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.AxisX.Interval = 32D;
            chartArea3.AxisX.Minimum = 0D;
            chartArea3.Name = "ChartArea1";
            this.chMain.ChartAreas.Add(chartArea3);
            this.chMain.Location = new System.Drawing.Point(519, 28);
            this.chMain.Name = "chMain";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.CustomProperties = "IsXAxisQuantitative=True";
            series3.Name = "Series1";
            this.chMain.Series.Add(series3);
            this.chMain.Size = new System.Drawing.Size(670, 291);
            this.chMain.TabIndex = 9;
            this.chMain.Text = "chart1";
            this.chMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chMain_MouseMove);
            // 
            // chFreq
            // 
            this.chFreq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea4.Name = "ChartArea1";
            this.chFreq.ChartAreas.Add(chartArea4);
            this.chFreq.Location = new System.Drawing.Point(519, 352);
            this.chFreq.Name = "chFreq";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Name = "Series1";
            this.chFreq.Series.Add(series4);
            this.chFreq.Size = new System.Drawing.Size(670, 161);
            this.chFreq.TabIndex = 10;
            this.chFreq.Text = "chart1";
            this.chFreq.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chMain_MouseMove);
            // 
            // cbFullMax
            // 
            this.cbFullMax.AutoSize = true;
            this.cbFullMax.Location = new System.Drawing.Point(519, 5);
            this.cbFullMax.Name = "cbFullMax";
            this.cbFullMax.Size = new System.Drawing.Size(150, 17);
            this.cbFullMax.TabIndex = 11;
            this.cbFullMax.Text = "Максимальное по кадру";
            this.cbFullMax.UseVisualStyleBackColor = true;
            this.cbFullMax.CheckedChanged += new System.EventHandler(this.cbFullMax_CheckedChanged);
            // 
            // lblNumLineCaption
            // 
            this.lblNumLineCaption.AutoSize = true;
            this.lblNumLineCaption.Location = new System.Drawing.Point(764, 6);
            this.lblNumLineCaption.Name = "lblNumLineCaption";
            this.lblNumLineCaption.Size = new System.Drawing.Size(77, 13);
            this.lblNumLineCaption.TabIndex = 12;
            this.lblNumLineCaption.Text = "Номер линии:";
            // 
            // lblDevAvgCaption
            // 
            this.lblDevAvgCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDevAvgCaption.AutoSize = true;
            this.lblDevAvgCaption.Location = new System.Drawing.Point(519, 322);
            this.lblDevAvgCaption.Name = "lblDevAvgCaption";
            this.lblDevAvgCaption.Size = new System.Drawing.Size(53, 13);
            this.lblDevAvgCaption.TabIndex = 13;
            this.lblDevAvgCaption.Text = "Среднее:";
            // 
            // lblDevDevCaption
            // 
            this.lblDevDevCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDevDevCaption.AutoSize = true;
            this.lblDevDevCaption.Location = new System.Drawing.Point(519, 336);
            this.lblDevDevCaption.Name = "lblDevDevCaption";
            this.lblDevDevCaption.Size = new System.Drawing.Size(71, 13);
            this.lblDevDevCaption.TabIndex = 14;
            this.lblDevDevCaption.Text = "Отклонение:";
            // 
            // lblDevAvgText
            // 
            this.lblDevAvgText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDevAvgText.AutoSize = true;
            this.lblDevAvgText.Location = new System.Drawing.Point(588, 322);
            this.lblDevAvgText.Name = "lblDevAvgText";
            this.lblDevAvgText.Size = new System.Drawing.Size(0, 13);
            this.lblDevAvgText.TabIndex = 15;
            // 
            // lblDevDevText
            // 
            this.lblDevDevText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDevDevText.AutoSize = true;
            this.lblDevDevText.Location = new System.Drawing.Point(588, 336);
            this.lblDevDevText.Name = "lblDevDevText";
            this.lblDevDevText.Size = new System.Drawing.Size(0, 13);
            this.lblDevDevText.TabIndex = 16;
            // 
            // lblPercentText
            // 
            this.lblPercentText.AutoSize = true;
            this.lblPercentText.Location = new System.Drawing.Point(696, 336);
            this.lblPercentText.Name = "lblPercentText";
            this.lblPercentText.Size = new System.Drawing.Size(0, 13);
            this.lblPercentText.TabIndex = 17;
            // 
            // cmsiSaveCompressedFile
            // 
            this.cmsiSaveCompressedFile.Name = "cmsiSaveCompressedFile";
            this.cmsiSaveCompressedFile.Size = new System.Drawing.Size(293, 22);
            this.cmsiSaveCompressedFile.Text = "Сохранить упакованное изображение...";
            this.cmsiSaveCompressedFile.Click += new System.EventHandler(this.cmsiSaveCompressedFile_Click);
            // 
            // ShowFrameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 521);
            this.Controls.Add(this.lblPercentText);
            this.Controls.Add(this.lblDevDevText);
            this.Controls.Add(this.lblDevAvgText);
            this.Controls.Add(this.lblDevDevCaption);
            this.Controls.Add(this.lblDevAvgCaption);
            this.Controls.Add(this.lblNumLineCaption);
            this.Controls.Add(this.cbFullMax);
            this.Controls.Add(this.chFreq);
            this.Controls.Add(this.chMain);
            this.Controls.Add(this.cbVertical);
            this.Controls.Add(this.numFrame);
            this.Controls.Add(this.pbImg);
            this.Name = "ShowFrameForm";
            this.Text = " ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.ShowFrameForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).EndInit();
            this.cmsFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFreq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImg;
        private System.Windows.Forms.NumericUpDown numFrame;
        private System.Windows.Forms.CheckBox cbVertical;
        private System.Windows.Forms.DataVisualization.Charting.Chart chMain;
        private System.Windows.Forms.DataVisualization.Charting.Chart chFreq;
        private System.Windows.Forms.CheckBox cbFullMax;
        private System.Windows.Forms.Label lblNumLineCaption;
        private System.Windows.Forms.Label lblDevAvgCaption;
        private System.Windows.Forms.Label lblDevDevCaption;
        private System.Windows.Forms.Label lblDevAvgText;
        private System.Windows.Forms.Label lblDevDevText;
        private System.Windows.Forms.Label lblPercentText;
        private System.Windows.Forms.ContextMenuStrip cmsFiles;
        private System.Windows.Forms.ToolStripMenuItem cmsiSaveFile;
        private System.Windows.Forms.ToolStripMenuItem cmsiSaveCompressedFile;
    }
}

