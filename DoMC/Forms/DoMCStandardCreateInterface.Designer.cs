namespace DoMC
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
            components = new System.ComponentModel.Container();
            btnCreateStandard = new Button();
            pbStandard1 = new PictureBox();
            lblStandard1Ready = new Label();
            lblStandard2Ready = new Label();
            pbStandard2 = new PictureBox();
            lblStandard3Ready = new Label();
            pbStandard3 = new PictureBox();
            lblTotalStandard = new Label();
            pbStandardSum = new PictureBox();
            tmCheckSignShow = new System.Windows.Forms.Timer(components);
            statusStrip1 = new StatusStrip();
            pbGettingStandard = new ToolStripProgressBar();
            lblWorkStatus = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)pbStandard1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStandardSum).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnCreateStandard
            // 
            btnCreateStandard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnCreateStandard.Location = new Point(13, 14);
            btnCreateStandard.Margin = new Padding(4, 5, 4, 5);
            btnCreateStandard.Name = "btnCreateStandard";
            btnCreateStandard.Size = new Size(395, 35);
            btnCreateStandard.TabIndex = 0;
            btnCreateStandard.Text = "Создать эталон";
            btnCreateStandard.UseVisualStyleBackColor = true;
            btnCreateStandard.Click += btnCreateStandard_Click;
            // 
            // pbStandard1
            // 
            pbStandard1.Location = new Point(102, 57);
            pbStandard1.Name = "pbStandard1";
            pbStandard1.Size = new Size(100, 100);
            pbStandard1.TabIndex = 1;
            pbStandard1.TabStop = false;
            // 
            // lblStandard1Ready
            // 
            lblStandard1Ready.AutoSize = true;
            lblStandard1Ready.Location = new Point(12, 57);
            lblStandard1Ready.Name = "lblStandard1Ready";
            lblStandard1Ready.Size = new Size(84, 20);
            lblStandard1Ready.TabIndex = 2;
            lblStandard1Ready.Text = "Эталон 1:";
            // 
            // lblStandard2Ready
            // 
            lblStandard2Ready.AutoSize = true;
            lblStandard2Ready.Location = new Point(12, 163);
            lblStandard2Ready.Name = "lblStandard2Ready";
            lblStandard2Ready.Size = new Size(84, 20);
            lblStandard2Ready.TabIndex = 4;
            lblStandard2Ready.Text = "Эталон 2:";
            // 
            // pbStandard2
            // 
            pbStandard2.Location = new Point(102, 163);
            pbStandard2.Name = "pbStandard2";
            pbStandard2.Size = new Size(100, 100);
            pbStandard2.TabIndex = 3;
            pbStandard2.TabStop = false;
            // 
            // lblStandard3Ready
            // 
            lblStandard3Ready.AutoSize = true;
            lblStandard3Ready.Location = new Point(12, 269);
            lblStandard3Ready.Name = "lblStandard3Ready";
            lblStandard3Ready.Size = new Size(84, 20);
            lblStandard3Ready.TabIndex = 6;
            lblStandard3Ready.Text = "Эталон 3:";
            // 
            // pbStandard3
            // 
            pbStandard3.Location = new Point(102, 269);
            pbStandard3.Name = "pbStandard3";
            pbStandard3.Size = new Size(100, 100);
            pbStandard3.TabIndex = 5;
            pbStandard3.TabStop = false;
            // 
            // lblTotalStandard
            // 
            lblTotalStandard.AutoSize = true;
            lblTotalStandard.Location = new Point(261, 137);
            lblTotalStandard.Name = "lblTotalStandard";
            lblTotalStandard.Size = new Size(124, 20);
            lblTotalStandard.TabIndex = 8;
            lblTotalStandard.Text = "Общий эталон:";
            // 
            // pbStandardSum
            // 
            pbStandardSum.Location = new Point(275, 160);
            pbStandardSum.Name = "pbStandardSum";
            pbStandardSum.Size = new Size(100, 100);
            pbStandardSum.TabIndex = 7;
            pbStandardSum.TabStop = false;
            // 
            // tmCheckSignShow
            // 
            tmCheckSignShow.Tick += tmCheckSignShow_Tick;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { pbGettingStandard, lblWorkStatus });
            statusStrip1.Location = new Point(0, 379);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(421, 22);
            statusStrip1.TabIndex = 9;
            statusStrip1.Text = "statusStrip1";
            // 
            // pbGettingStandard
            // 
            pbGettingStandard.Name = "pbGettingStandard";
            pbGettingStandard.Size = new Size(100, 16);
            // 
            // lblWorkStatus
            // 
            lblWorkStatus.Name = "lblWorkStatus";
            lblWorkStatus.Size = new Size(0, 17);
            // 
            // DoMCStandardCreateInterface
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(421, 401);
            Controls.Add(statusStrip1);
            Controls.Add(lblTotalStandard);
            Controls.Add(pbStandardSum);
            Controls.Add(lblStandard3Ready);
            Controls.Add(pbStandard3);
            Controls.Add(lblStandard2Ready);
            Controls.Add(pbStandard2);
            Controls.Add(lblStandard1Ready);
            Controls.Add(pbStandard1);
            Controls.Add(btnCreateStandard);
            Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DoMCStandardCreateInterface";
            Text = "Создание эталона";
            FormClosed += DoMCStandardCreateInterface_FormClosed;
            Paint += DoMCStandardCreateInterface_Paint;
            ((System.ComponentModel.ISupportInitialize)pbStandard1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStandardSum).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private StatusStrip statusStrip1;
        private ToolStripProgressBar pbGettingStandard;
        private ToolStripStatusLabel lblWorkStatus;
    }
}