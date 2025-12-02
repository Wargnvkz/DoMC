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
            btnRemoteDBBrowse = new Button();
            txbRemoteConnectionString = new TextBox();
            label1 = new Label();
            btnLocalDBBrowse = new Button();
            txbLocalConnectionString = new TextBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(12, 216);
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
            btnCancel.Location = new Point(738, 216);
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
            // btnRemoteDBBrowse
            // 
            btnRemoteDBBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemoteDBBrowse.Location = new Point(700, 158);
            btnRemoteDBBrowse.Margin = new Padding(4, 5, 4, 5);
            btnRemoteDBBrowse.Name = "btnRemoteDBBrowse";
            btnRemoteDBBrowse.Size = new Size(112, 35);
            btnRemoteDBBrowse.TabIndex = 14;
            btnRemoteDBBrowse.Text = "Обзор...";
            btnRemoteDBBrowse.UseVisualStyleBackColor = true;
            btnRemoteDBBrowse.Click += btnRemoteDBBrowse_Click;
            // 
            // txbRemoteConnectionString
            // 
            txbRemoteConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbRemoteConnectionString.Location = new Point(12, 165);
            txbRemoteConnectionString.Margin = new Padding(4, 5, 4, 5);
            txbRemoteConnectionString.Name = "txbRemoteConnectionString";
            txbRemoteConnectionString.Size = new Size(680, 23);
            txbRemoteConnectionString.TabIndex = 13;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 145);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(158, 15);
            label1.TabIndex = 12;
            label1.Text = "Папка хранения архива БД:";
            // 
            // btnLocalDBBrowse
            // 
            btnLocalDBBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLocalDBBrowse.Location = new Point(700, 90);
            btnLocalDBBrowse.Margin = new Padding(4, 5, 4, 5);
            btnLocalDBBrowse.Name = "btnLocalDBBrowse";
            btnLocalDBBrowse.Size = new Size(112, 35);
            btnLocalDBBrowse.TabIndex = 11;
            btnLocalDBBrowse.Text = "Обзор...";
            btnLocalDBBrowse.UseVisualStyleBackColor = true;
            btnLocalDBBrowse.Click += btnLocalDBBrowse_Click;
            // 
            // txbLocalConnectionString
            // 
            txbLocalConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbLocalConnectionString.Location = new Point(12, 97);
            txbLocalConnectionString.Margin = new Padding(4, 5, 4, 5);
            txbLocalConnectionString.Name = "txbLocalConnectionString";
            txbLocalConnectionString.Size = new Size(680, 23);
            txbLocalConnectionString.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 77);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(117, 15);
            label3.TabIndex = 9;
            label3.Text = "Папка хранения БД:";
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(825, 268);
            Controls.Add(btnRemoteDBBrowse);
            Controls.Add(txbRemoteConnectionString);
            Controls.Add(label1);
            Controls.Add(btnLocalDBBrowse);
            Controls.Add(txbLocalConnectionString);
            Controls.Add(label3);
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
        private Button btnRemoteDBBrowse;
        private TextBox txbRemoteConnectionString;
        private Label label1;
        private Button btnLocalDBBrowse;
        private TextBox txbLocalConnectionString;
        private Label label3;
    }
}