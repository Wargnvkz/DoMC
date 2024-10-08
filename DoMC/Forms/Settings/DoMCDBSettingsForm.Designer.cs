namespace DoMCLib.Forms
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txbLocalConnectionString = new System.Windows.Forms.TextBox();
            this.btnLocalDBBrowse = new System.Windows.Forms.Button();
            this.btnRemoteDBBrowse = new System.Windows.Forms.Button();
            this.txbRemoteConnectionString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTimeBeforeMoveDataToArchive = new System.Windows.Forms.Label();
            this.txbDelayBeforeMoveDataToArchive = new System.Windows.Forms.TextBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(18, 286);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(112, 35);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(827, 286);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Строка подключения локальной БД:";
            // 
            // txbLocalConnectionString
            // 
            this.txbLocalConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbLocalConnectionString.Location = new System.Drawing.Point(22, 38);
            this.txbLocalConnectionString.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbLocalConnectionString.Name = "txbLocalConnectionString";
            this.txbLocalConnectionString.Size = new System.Drawing.Size(793, 26);
            this.txbLocalConnectionString.TabIndex = 3;
            // 
            // btnLocalDBBrowse
            // 
            this.btnLocalDBBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocalDBBrowse.Location = new System.Drawing.Point(827, 35);
            this.btnLocalDBBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLocalDBBrowse.Name = "btnLocalDBBrowse";
            this.btnLocalDBBrowse.Size = new System.Drawing.Size(112, 35);
            this.btnLocalDBBrowse.TabIndex = 4;
            this.btnLocalDBBrowse.Text = "Обзор...";
            this.btnLocalDBBrowse.UseVisualStyleBackColor = true;
            this.btnLocalDBBrowse.Click += new System.EventHandler(this.btnLocalDBBrowse_Click);
            // 
            // btnRemoteDBBrowse
            // 
            this.btnRemoteDBBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoteDBBrowse.Location = new System.Drawing.Point(827, 129);
            this.btnRemoteDBBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemoteDBBrowse.Name = "btnRemoteDBBrowse";
            this.btnRemoteDBBrowse.Size = new System.Drawing.Size(112, 35);
            this.btnRemoteDBBrowse.TabIndex = 7;
            this.btnRemoteDBBrowse.Text = "Обзор...";
            this.btnRemoteDBBrowse.UseVisualStyleBackColor = true;
            this.btnRemoteDBBrowse.Click += new System.EventHandler(this.btnRemoteDBBrowse_Click);
            // 
            // txbRemoteConnectionString
            // 
            this.txbRemoteConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbRemoteConnectionString.Location = new System.Drawing.Point(22, 132);
            this.txbRemoteConnectionString.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbRemoteConnectionString.Name = "txbRemoteConnectionString";
            this.txbRemoteConnectionString.Size = new System.Drawing.Size(793, 26);
            this.txbRemoteConnectionString.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 108);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(289, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Строка подключения удаленной БД:";
            // 
            // lblTimeBeforeMoveDataToArchive
            // 
            this.lblTimeBeforeMoveDataToArchive.AutoSize = true;
            this.lblTimeBeforeMoveDataToArchive.Location = new System.Drawing.Point(18, 215);
            this.lblTimeBeforeMoveDataToArchive.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimeBeforeMoveDataToArchive.Name = "lblTimeBeforeMoveDataToArchive";
            this.lblTimeBeforeMoveDataToArchive.Size = new System.Drawing.Size(281, 20);
            this.lblTimeBeforeMoveDataToArchive.TabIndex = 8;
            this.lblTimeBeforeMoveDataToArchive.Text = "Время до переноса данных в архив:";
            // 
            // txbDelayBeforeMoveDataToArchive
            // 
            this.txbDelayBeforeMoveDataToArchive.Location = new System.Drawing.Point(312, 211);
            this.txbDelayBeforeMoveDataToArchive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbDelayBeforeMoveDataToArchive.Name = "txbDelayBeforeMoveDataToArchive";
            this.txbDelayBeforeMoveDataToArchive.Size = new System.Drawing.Size(148, 26);
            this.txbDelayBeforeMoveDataToArchive.TabIndex = 9;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(471, 215);
            this.lblSeconds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(34, 20);
            this.lblSeconds.TabIndex = 10;
            this.lblSeconds.Text = "сек";
            // 
            // DoMCDBSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 340);
            this.Controls.Add(this.lblSeconds);
            this.Controls.Add(this.txbDelayBeforeMoveDataToArchive);
            this.Controls.Add(this.lblTimeBeforeMoveDataToArchive);
            this.Controls.Add(this.btnRemoteDBBrowse);
            this.Controls.Add(this.txbRemoteConnectionString);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnLocalDBBrowse);
            this.Controls.Add(this.txbLocalConnectionString);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DoMCDBSettingsForm";
            this.Text = "Настройка строк подключения к БД";
            this.ResumeLayout(false);
            this.PerformLayout();

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