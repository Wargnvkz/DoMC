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
            label1 = new Label();
            cmbSocketQuantity = new ComboBox();
            pnlSockets = new Panel();
            bntCopy = new Button();
            btnOK = new Button();
            btnCancel = new Button();
            btnClear = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(9, 21);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(180, 24);
            label1.TabIndex = 0;
            label1.Text = "Количество гнезд:";
            // 
            // cmbSocketQuantity
            // 
            cmbSocketQuantity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSocketQuantity.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cmbSocketQuantity.FormattingEnabled = true;
            cmbSocketQuantity.Items.AddRange(new object[] { "96", "72", "48", "32" });
            cmbSocketQuantity.Location = new Point(231, 17);
            cmbSocketQuantity.Margin = new Padding(4, 3, 4, 3);
            cmbSocketQuantity.Name = "cmbSocketQuantity";
            cmbSocketQuantity.Size = new Size(63, 32);
            cmbSocketQuantity.TabIndex = 1;
            cmbSocketQuantity.SelectedIndexChanged += cmbSocketQuantity_SelectedIndexChanged;
            // 
            // pnlSockets
            // 
            pnlSockets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlSockets.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            pnlSockets.Location = new Point(18, 85);
            pnlSockets.Margin = new Padding(4, 3, 4, 3);
            pnlSockets.Name = "pnlSockets";
            pnlSockets.Size = new Size(630, 567);
            pnlSockets.TabIndex = 2;
            // 
            // bntCopy
            // 
            bntCopy.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            bntCopy.Location = new Point(302, 3);
            bntCopy.Margin = new Padding(4, 3, 4, 3);
            bntCopy.Name = "bntCopy";
            bntCopy.Size = new Size(195, 75);
            bntCopy.TabIndex = 3;
            bntCopy.Text = "Копировать первое гнездо";
            bntCopy.UseVisualStyleBackColor = true;
            bntCopy.Click += bntCopy_Click;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnOK.Location = new Point(18, 659);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(144, 44);
            btnOK.TabIndex = 4;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnCancel.Location = new Point(504, 659);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(144, 44);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            btnClear.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnClear.Location = new Point(504, 5);
            btnClear.Margin = new Padding(4, 3, 4, 3);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(144, 74);
            btnClear.TabIndex = 6;
            btnClear.Text = "Очистить параметры всех гнезд";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // DoMCSocketSettingsListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(662, 706);
            Controls.Add(btnOK);
            Controls.Add(btnClear);
            Controls.Add(btnCancel);
            Controls.Add(bntCopy);
            Controls.Add(pnlSockets);
            Controls.Add(cmbSocketQuantity);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DoMCSocketSettingsListForm";
            Text = "Настройка параметров чтения гнезд";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSocketQuantity;
        private System.Windows.Forms.Panel pnlSockets;
        private System.Windows.Forms.Button bntCopy;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClear;
    }
}