namespace DoMCRemoteControl
{
    partial class SettingsForm
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
            btnOK = new Button();
            btnCancel = new Button();
            lblIPCaption = new Label();
            label2 = new Label();
            nudPort = new NumericUpDown();
            txbIP = new TextBox();
            ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(12, 78);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 40);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(158, 78);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblIPCaption
            // 
            lblIPCaption.AutoSize = true;
            lblIPCaption.Font = new Font("Segoe UI", 12F);
            lblIPCaption.Location = new Point(12, 9);
            lblIPCaption.Name = "lblIPCaption";
            lblIPCaption.Size = new Size(26, 21);
            lblIPCaption.TabIndex = 2;
            lblIPCaption.Text = "IP:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(12, 46);
            label2.Name = "label2";
            label2.Size = new Size(49, 21);
            label2.TabIndex = 3;
            label2.Text = "Порт:";
            // 
            // nudPort
            // 
            nudPort.Font = new Font("Segoe UI", 12F);
            nudPort.Location = new Point(67, 44);
            nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudPort.Name = "nudPort";
            nudPort.Size = new Size(97, 29);
            nudPort.TabIndex = 7;
            nudPort.Value = new decimal(new int[] { 8080, 0, 0, 0 });
            // 
            // txbIP
            // 
            txbIP.Font = new Font("Segoe UI", 12F);
            txbIP.Location = new Point(67, 6);
            txbIP.Name = "txbIP";
            txbIP.Size = new Size(166, 29);
            txbIP.TabIndex = 8;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(245, 130);
            Controls.Add(txbIP);
            Controls.Add(nudPort);
            Controls.Add(label2);
            Controls.Add(lblIPCaption);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Name = "SettingsForm";
            Text = "Настройки";
            ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private Button btnCancel;
        private Label lblIPCaption;
        private Label label2;
        private NumericUpDown nudPort;
        private TextBox txbIP;
    }
}