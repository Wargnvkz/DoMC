namespace DoMCLib.Forms
{
    partial class DoMCStatusForm
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
            this.pnlSocketStatus = new System.Windows.Forms.Panel();
            this.chSocketErrors = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chSocketErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSocketStatus
            // 
            this.pnlSocketStatus.Location = new System.Drawing.Point(12, 12);
            this.pnlSocketStatus.Name = "pnlSocketStatus";
            this.pnlSocketStatus.Size = new System.Drawing.Size(237, 271);
            this.pnlSocketStatus.TabIndex = 0;
            // 
            // chSocketErrors
            // 
            this.chSocketErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chSocketErrors.ChartAreas.Add(chartArea1);
            this.chSocketErrors.Location = new System.Drawing.Point(10, 289);
            this.chSocketErrors.Name = "chSocketErrors";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chSocketErrors.Series.Add(series1);
            this.chSocketErrors.Size = new System.Drawing.Size(761, 111);
            this.chSocketErrors.TabIndex = 1;
            this.chSocketErrors.Text = "chart1";
            // 
            // DoMCStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 408);
            this.Controls.Add(this.chSocketErrors);
            this.Controls.Add(this.pnlSocketStatus);
            this.Name = "DoMCStatusForm";
            this.Text = "Статус ПМК";
            ((System.ComponentModel.ISupportInitialize)(this.chSocketErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSocketStatus;
        private System.Windows.Forms.DataVisualization.Charting.Chart chSocketErrors;
    }
}