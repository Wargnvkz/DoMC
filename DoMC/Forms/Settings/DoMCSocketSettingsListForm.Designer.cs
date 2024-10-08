namespace DoMCLib.Forms
{
    partial class DoMCSocketSettingsListForm
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
            this.ddlSocketQuantity = new System.Windows.Forms.ComboBox();
            this.pnlSockets = new System.Windows.Forms.Panel();
            this.bntCopy = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество гнезд:";
            // 
            // ddlSocketQuantity
            // 
            this.ddlSocketQuantity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSocketQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ddlSocketQuantity.FormattingEnabled = true;
            this.ddlSocketQuantity.Items.AddRange(new object[] {
            "1",
            "2",
            "8",
            "32",
            "48",
            "72",
            "96"});
            this.ddlSocketQuantity.Location = new System.Drawing.Point(198, 15);
            this.ddlSocketQuantity.Name = "ddlSocketQuantity";
            this.ddlSocketQuantity.Size = new System.Drawing.Size(55, 32);
            this.ddlSocketQuantity.TabIndex = 1;
            this.ddlSocketQuantity.SelectedIndexChanged += new System.EventHandler(this.ddlSocketQuantity_SelectedIndexChanged);
            // 
            // pnlSockets
            // 
            this.pnlSockets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSockets.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pnlSockets.Location = new System.Drawing.Point(15, 74);
            this.pnlSockets.Name = "pnlSockets";
            this.pnlSockets.Size = new System.Drawing.Size(540, 491);
            this.pnlSockets.TabIndex = 2;
            // 
            // bntCopy
            // 
            this.bntCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bntCopy.Location = new System.Drawing.Point(259, 3);
            this.bntCopy.Name = "bntCopy";
            this.bntCopy.Size = new System.Drawing.Size(167, 65);
            this.bntCopy.TabIndex = 3;
            this.bntCopy.Text = "Копировать первое гнездо";
            this.bntCopy.UseVisualStyleBackColor = true;
            this.bntCopy.Click += new System.EventHandler(this.bntCopy_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOK.Location = new System.Drawing.Point(15, 571);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(123, 38);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(432, 571);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(123, 38);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClear.Location = new System.Drawing.Point(432, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(123, 64);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Очистить параметры всех гнезд";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // DoMCSocketSettingsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 612);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.bntCopy);
            this.Controls.Add(this.pnlSockets);
            this.Controls.Add(this.ddlSocketQuantity);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DoMCSocketSettingsListForm";
            this.Text = "Настройка параметров чтения гнезд";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlSocketQuantity;
        private System.Windows.Forms.Panel pnlSockets;
        private System.Windows.Forms.Button bntCopy;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClear;
    }
}