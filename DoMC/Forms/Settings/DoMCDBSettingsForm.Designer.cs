namespace DoMC.Forms.Settings
{
    partial class DoMCDBSettingsForm
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
            label1 = new Label();
            txbLocalConnectionString = new TextBox();
            btnLocalDBBrowse = new Button();
            btnRemoteDBBrowse = new Button();
            txbRemoteConnectionString = new TextBox();
            label2 = new Label();
            lblTimeBeforeMoveDataToArchive = new Label();
            txbDelayBeforeMoveDataToArchive = new TextBox();
            lblSeconds = new Label();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(18, 286);
            btnOK.Margin = new Padding(4, 5, 4, 5);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(112, 35);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(827, 286);
            btnCancel.Margin = new Padding(4, 5, 4, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 35);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 14);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(161, 20);
            label1.TabIndex = 2;
            label1.Text = "Папка хранения БД:";
            // 
            // txbLocalConnectionString
            // 
            txbLocalConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbLocalConnectionString.Location = new Point(22, 38);
            txbLocalConnectionString.Margin = new Padding(4, 5, 4, 5);
            txbLocalConnectionString.Name = "txbLocalConnectionString";
            txbLocalConnectionString.Size = new Size(793, 26);
            txbLocalConnectionString.TabIndex = 3;
            // 
            // btnLocalDBBrowse
            // 
            btnLocalDBBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLocalDBBrowse.Location = new Point(827, 35);
            btnLocalDBBrowse.Margin = new Padding(4, 5, 4, 5);
            btnLocalDBBrowse.Name = "btnLocalDBBrowse";
            btnLocalDBBrowse.Size = new Size(112, 35);
            btnLocalDBBrowse.TabIndex = 4;
            btnLocalDBBrowse.Text = "Обзор...";
            btnLocalDBBrowse.UseVisualStyleBackColor = true;
            btnLocalDBBrowse.Click += btnLocalDBBrowse_Click;
            // 
            // btnRemoteDBBrowse
            // 
            btnRemoteDBBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemoteDBBrowse.Location = new Point(827, 129);
            btnRemoteDBBrowse.Margin = new Padding(4, 5, 4, 5);
            btnRemoteDBBrowse.Name = "btnRemoteDBBrowse";
            btnRemoteDBBrowse.Size = new Size(112, 35);
            btnRemoteDBBrowse.TabIndex = 7;
            btnRemoteDBBrowse.Text = "Обзор...";
            btnRemoteDBBrowse.UseVisualStyleBackColor = true;
            btnRemoteDBBrowse.Click += btnRemoteDBBrowse_Click;
            // 
            // txbRemoteConnectionString
            // 
            txbRemoteConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbRemoteConnectionString.Location = new Point(22, 132);
            txbRemoteConnectionString.Margin = new Padding(4, 5, 4, 5);
            txbRemoteConnectionString.Name = "txbRemoteConnectionString";
            txbRemoteConnectionString.Size = new Size(793, 26);
            txbRemoteConnectionString.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 108);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(217, 20);
            label2.TabIndex = 5;
            label2.Text = "Папка хранения архива БД:";
            // 
            // lblTimeBeforeMoveDataToArchive
            // 
            lblTimeBeforeMoveDataToArchive.AutoSize = true;
            lblTimeBeforeMoveDataToArchive.Location = new Point(18, 215);
            lblTimeBeforeMoveDataToArchive.Margin = new Padding(4, 0, 4, 0);
            lblTimeBeforeMoveDataToArchive.Name = "lblTimeBeforeMoveDataToArchive";
            lblTimeBeforeMoveDataToArchive.Size = new Size(281, 20);
            lblTimeBeforeMoveDataToArchive.TabIndex = 8;
            lblTimeBeforeMoveDataToArchive.Text = "Время до переноса данных в архив:";
            // 
            // txbDelayBeforeMoveDataToArchive
            // 
            txbDelayBeforeMoveDataToArchive.Location = new Point(312, 211);
            txbDelayBeforeMoveDataToArchive.Margin = new Padding(4, 5, 4, 5);
            txbDelayBeforeMoveDataToArchive.Name = "txbDelayBeforeMoveDataToArchive";
            txbDelayBeforeMoveDataToArchive.Size = new Size(148, 26);
            txbDelayBeforeMoveDataToArchive.TabIndex = 9;
            // 
            // lblSeconds
            // 
            lblSeconds.AutoSize = true;
            lblSeconds.Location = new Point(471, 215);
            lblSeconds.Margin = new Padding(4, 0, 4, 0);
            lblSeconds.Name = "lblSeconds";
            lblSeconds.Size = new Size(34, 20);
            lblSeconds.TabIndex = 10;
            lblSeconds.Text = "сек";
            // 
            // DoMCDBSettingsForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(957, 340);
            Controls.Add(lblSeconds);
            Controls.Add(txbDelayBeforeMoveDataToArchive);
            Controls.Add(lblTimeBeforeMoveDataToArchive);
            Controls.Add(btnRemoteDBBrowse);
            Controls.Add(txbRemoteConnectionString);
            Controls.Add(label2);
            Controls.Add(btnLocalDBBrowse);
            Controls.Add(txbLocalConnectionString);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4, 5, 4, 5);
            Name = "DoMCDBSettingsForm";
            Text = "Настройка подключения к БД";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbLocalConnectionString;
        private System.Windows.Forms.Button btnLocalDBBrowse;
        private System.Windows.Forms.Button btnRemoteDBBrowse;
        private System.Windows.Forms.TextBox txbRemoteConnectionString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTimeBeforeMoveDataToArchive;
        private System.Windows.Forms.TextBox txbDelayBeforeMoveDataToArchive;
        private System.Windows.Forms.Label lblSeconds;
    }
}