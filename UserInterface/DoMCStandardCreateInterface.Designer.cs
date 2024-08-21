namespace DoMCInterface
{
    partial class DoMCStandardCreateInterface
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
            this.btnCreateStandard = new System.Windows.Forms.Button();
            this.pbStandard1 = new System.Windows.Forms.PictureBox();
            this.lblStandard1Ready = new System.Windows.Forms.Label();
            this.lblStandard2Ready = new System.Windows.Forms.Label();
            this.pbStandard2 = new System.Windows.Forms.PictureBox();
            this.lblStandard3Ready = new System.Windows.Forms.Label();
            this.pbStandard3 = new System.Windows.Forms.PictureBox();
            this.lblTotalStandard = new System.Windows.Forms.Label();
            this.pbStandardSum = new System.Windows.Forms.PictureBox();
            this.tmCheckSignShow = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbStandard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStandard2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStandard3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStandardSum)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreateStandard
            // 
            this.btnCreateStandard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateStandard.Location = new System.Drawing.Point(13, 14);
            this.btnCreateStandard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCreateStandard.Name = "btnCreateStandard";
            this.btnCreateStandard.Size = new System.Drawing.Size(395, 35);
            this.btnCreateStandard.TabIndex = 0;
            this.btnCreateStandard.Text = "Создать эталон";
            this.btnCreateStandard.UseVisualStyleBackColor = true;
            this.btnCreateStandard.Click += new System.EventHandler(this.btnCreateStandard_Click);
            // 
            // pbStandard1
            // 
            this.pbStandard1.Location = new System.Drawing.Point(102, 57);
            this.pbStandard1.Name = "pbStandard1";
            this.pbStandard1.Size = new System.Drawing.Size(100, 100);
            this.pbStandard1.TabIndex = 1;
            this.pbStandard1.TabStop = false;
            // 
            // lblStandard1Ready
            // 
            this.lblStandard1Ready.AutoSize = true;
            this.lblStandard1Ready.Location = new System.Drawing.Point(12, 57);
            this.lblStandard1Ready.Name = "lblStandard1Ready";
            this.lblStandard1Ready.Size = new System.Drawing.Size(84, 20);
            this.lblStandard1Ready.TabIndex = 2;
            this.lblStandard1Ready.Text = "Эталон 1:";
            // 
            // lblStandard2Ready
            // 
            this.lblStandard2Ready.AutoSize = true;
            this.lblStandard2Ready.Location = new System.Drawing.Point(12, 163);
            this.lblStandard2Ready.Name = "lblStandard2Ready";
            this.lblStandard2Ready.Size = new System.Drawing.Size(84, 20);
            this.lblStandard2Ready.TabIndex = 4;
            this.lblStandard2Ready.Text = "Эталон 2:";
            // 
            // pbStandard2
            // 
            this.pbStandard2.Location = new System.Drawing.Point(102, 163);
            this.pbStandard2.Name = "pbStandard2";
            this.pbStandard2.Size = new System.Drawing.Size(100, 100);
            this.pbStandard2.TabIndex = 3;
            this.pbStandard2.TabStop = false;
            // 
            // lblStandard3Ready
            // 
            this.lblStandard3Ready.AutoSize = true;
            this.lblStandard3Ready.Location = new System.Drawing.Point(12, 269);
            this.lblStandard3Ready.Name = "lblStandard3Ready";
            this.lblStandard3Ready.Size = new System.Drawing.Size(84, 20);
            this.lblStandard3Ready.TabIndex = 6;
            this.lblStandard3Ready.Text = "Эталон 3:";
            // 
            // pbStandard3
            // 
            this.pbStandard3.Location = new System.Drawing.Point(102, 269);
            this.pbStandard3.Name = "pbStandard3";
            this.pbStandard3.Size = new System.Drawing.Size(100, 100);
            this.pbStandard3.TabIndex = 5;
            this.pbStandard3.TabStop = false;
            // 
            // lblTotalStandard
            // 
            this.lblTotalStandard.AutoSize = true;
            this.lblTotalStandard.Location = new System.Drawing.Point(261, 137);
            this.lblTotalStandard.Name = "lblTotalStandard";
            this.lblTotalStandard.Size = new System.Drawing.Size(124, 20);
            this.lblTotalStandard.TabIndex = 8;
            this.lblTotalStandard.Text = "Общий эталон:";
            // 
            // pbStandardSum
            // 
            this.pbStandardSum.Location = new System.Drawing.Point(275, 160);
            this.pbStandardSum.Name = "pbStandardSum";
            this.pbStandardSum.Size = new System.Drawing.Size(100, 100);
            this.pbStandardSum.TabIndex = 7;
            this.pbStandardSum.TabStop = false;
            // 
            // tmCheckSignShow
            // 
            this.tmCheckSignShow.Tick += new System.EventHandler(this.tmCheckSignShow_Tick);
            // 
            // DoMCStandardCreateInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 381);
            this.Controls.Add(this.lblTotalStandard);
            this.Controls.Add(this.pbStandardSum);
            this.Controls.Add(this.lblStandard3Ready);
            this.Controls.Add(this.pbStandard3);
            this.Controls.Add(this.lblStandard2Ready);
            this.Controls.Add(this.pbStandard2);
            this.Controls.Add(this.lblStandard1Ready);
            this.Controls.Add(this.pbStandard1);
            this.Controls.Add(this.btnCreateStandard);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DoMCStandardCreateInterface";
            this.Text = "Создание эталона";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DoMCStandardCreateInterface_FormClosed);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DoMCStandardCreateInterface_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pbStandard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStandard2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStandard3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStandardSum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateStandard;
        private System.Windows.Forms.PictureBox pbStandard1;
        private System.Windows.Forms.Label lblStandard1Ready;
        private System.Windows.Forms.Label lblStandard2Ready;
        private System.Windows.Forms.PictureBox pbStandard2;
        private System.Windows.Forms.Label lblStandard3Ready;
        private System.Windows.Forms.PictureBox pbStandard3;
        private System.Windows.Forms.Label lblTotalStandard;
        private System.Windows.Forms.PictureBox pbStandardSum;
        private System.Windows.Forms.Timer tmCheckSignShow;
    }
}