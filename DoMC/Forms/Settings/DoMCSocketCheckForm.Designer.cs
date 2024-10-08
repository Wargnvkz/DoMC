namespace DoMCLib.Forms
{
    partial class DoMCSocketOnOffForm
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
            this.bntReset = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSetAll = new System.Windows.Forms.Button();
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
            // bntReset
            // 
            this.bntReset.Location = new System.Drawing.Point(268, 3);
            this.bntReset.Name = "bntReset";
            this.bntReset.Size = new System.Drawing.Size(115, 43);
            this.bntReset.TabIndex = 3;
            this.bntReset.Text = "Сбросить все гнезда";
            this.bntReset.UseVisualStyleBackColor = true;
            this.bntReset.Click += new System.EventHandler(this.bntReset_Click);
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(437, 441);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSetAll
            // 
            this.btnSetAll.Location = new System.Drawing.Point(389, 3);
            this.btnSetAll.Name = "btnSetAll";
            this.btnSetAll.Size = new System.Drawing.Size(123, 43);
            this.btnSetAll.TabIndex = 6;
            this.btnSetAll.Text = "Установить все гнезда";
            this.btnSetAll.UseVisualStyleBackColor = true;
            this.btnSetAll.Click += new System.EventHandler(this.btnSetAll_Click);
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
            // DoMCSocketOnOffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 467);
            this.Controls.Add(this.lblSocketQuantity);
            this.Controls.Add(this.btnSetAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.bntReset);
            this.Controls.Add(this.pnlSockets);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DoMCSocketOnOffForm";
            this.Text = "Сохранение изображений гнезд";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlSockets;
        private System.Windows.Forms.Button bntReset;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSetAll;
        private System.Windows.Forms.Label lblSocketQuantity;
    }
}