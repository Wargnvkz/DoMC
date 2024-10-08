namespace DoMCLib.Forms
{
    partial class DoMCImageProcessSettingsListForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSockets = new System.Windows.Forms.Panel();
            this.btnExpandToAll = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblSocketQuantity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество гнезд:";
            // 
            // pnlSockets
            // 
            this.pnlSockets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSockets.Location = new System.Drawing.Point(15, 50);
            this.pnlSockets.Name = "pnlSockets";
            this.pnlSockets.Size = new System.Drawing.Size(497, 385);
            this.pnlSockets.TabIndex = 2;
            // 
            // btnExpandToAll
            // 
            this.btnExpandToAll.Location = new System.Drawing.Point(365, 3);
            this.btnExpandToAll.Name = "btnExpandToAll";
            this.btnExpandToAll.Size = new System.Drawing.Size(147, 43);
            this.btnExpandToAll.TabIndex = 3;
            this.btnExpandToAll.Text = "Копировать параметры гнезда 1";
            this.btnExpandToAll.UseVisualStyleBackColor = true;
            this.btnExpandToAll.Click += new System.EventHandler(this.btnExpandToAll_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(12, 441);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblSocketQuantity
            // 
            this.lblSocketQuantity.AutoSize = true;
            this.lblSocketQuantity.Location = new System.Drawing.Point(119, 18);
            this.lblSocketQuantity.Name = "lblSocketQuantity";
            this.lblSocketQuantity.Size = new System.Drawing.Size(13, 13);
            this.lblSocketQuantity.TabIndex = 7;
            this.lblSocketQuantity.Text = "0";
            // 
            // DoMCImageProcessSettingsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 467);
            this.Controls.Add(this.lblSocketQuantity);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnExpandToAll);
            this.Controls.Add(this.pnlSockets);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DoMCImageProcessSettingsListForm";
            this.Text = "Сохранение изображений гнезд";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlSockets;
        private System.Windows.Forms.Button btnExpandToAll;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblSocketQuantity;
    }
}