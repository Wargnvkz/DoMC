namespace DoMCLib.Forms
{
    partial class RDPSettingsForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRDPIP = new System.Windows.Forms.Label();
            this.lblCoolingBlock = new System.Windows.Forms.Label();
            this.txbRDPIP = new System.Windows.Forms.TextBox();
            this.cbCoolingBlock = new System.Windows.Forms.ComboBox();
            this.lblIPPort = new System.Windows.Forms.Label();
            this.txbIPPort = new System.Windows.Forms.TextBox();
            this.epError = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblMachineNumber = new System.Windows.Forms.Label();
            this.txbMachineNumber = new System.Windows.Forms.TextBox();
            this.cbRDPBSendBadCycle = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.epError)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOK.Location = new System.Drawing.Point(12, 251);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(79, 31);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(228, 251);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 31);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblRDPIP
            // 
            this.lblRDPIP.AutoSize = true;
            this.lblRDPIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRDPIP.Location = new System.Drawing.Point(8, 9);
            this.lblRDPIP.Name = "lblRDPIP";
            this.lblRDPIP.Size = new System.Drawing.Size(144, 20);
            this.lblRDPIP.TabIndex = 2;
            this.lblRDPIP.Text = "IP адрес бракера:";
            // 
            // lblCoolingBlock
            // 
            this.lblCoolingBlock.AutoSize = true;
            this.lblCoolingBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCoolingBlock.Location = new System.Drawing.Point(8, 74);
            this.lblCoolingBlock.Name = "lblCoolingBlock";
            this.lblCoolingBlock.Size = new System.Drawing.Size(273, 20);
            this.lblCoolingBlock.TabIndex = 4;
            this.lblCoolingBlock.Text = "Количество охлаждающих блоков:";
            // 
            // txbRDPIP
            // 
            this.txbRDPIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbRDPIP.Location = new System.Drawing.Point(12, 32);
            this.txbRDPIP.Name = "txbRDPIP";
            this.txbRDPIP.Size = new System.Drawing.Size(158, 26);
            this.txbRDPIP.TabIndex = 5;
            this.txbRDPIP.Text = "192.168.0.171";
            this.txbRDPIP.Validating += new System.ComponentModel.CancelEventHandler(this.txbRDPIP_Validating);
            // 
            // cbCoolingBlock
            // 
            this.cbCoolingBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCoolingBlock.FormattingEnabled = true;
            this.cbCoolingBlock.Location = new System.Drawing.Point(12, 97);
            this.cbCoolingBlock.Name = "cbCoolingBlock";
            this.cbCoolingBlock.Size = new System.Drawing.Size(121, 28);
            this.cbCoolingBlock.TabIndex = 6;
            this.cbCoolingBlock.Validating += new System.ComponentModel.CancelEventHandler(this.cbCoolingBlock_Validating);
            // 
            // lblIPPort
            // 
            this.lblIPPort.AutoSize = true;
            this.lblIPPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblIPPort.Location = new System.Drawing.Point(184, 9);
            this.lblIPPort.Name = "lblIPPort";
            this.lblIPPort.Size = new System.Drawing.Size(118, 20);
            this.lblIPPort.TabIndex = 7;
            this.lblIPPort.Text = "Порт бракера:";
            // 
            // txbIPPort
            // 
            this.txbIPPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbIPPort.Location = new System.Drawing.Point(188, 32);
            this.txbIPPort.Name = "txbIPPort";
            this.txbIPPort.Size = new System.Drawing.Size(114, 26);
            this.txbIPPort.TabIndex = 8;
            this.txbIPPort.Text = "4001";
            this.txbIPPort.Validating += new System.ComponentModel.CancelEventHandler(this.txbIPPort_Validating);
            // 
            // epError
            // 
            this.epError.ContainerControl = this;
            // 
            // lblMachineNumber
            // 
            this.lblMachineNumber.AutoSize = true;
            this.lblMachineNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMachineNumber.Location = new System.Drawing.Point(8, 137);
            this.lblMachineNumber.Name = "lblMachineNumber";
            this.lblMachineNumber.Size = new System.Drawing.Size(129, 20);
            this.lblMachineNumber.TabIndex = 9;
            this.lblMachineNumber.Text = "Номер машины:";
            // 
            // txbMachineNumber
            // 
            this.txbMachineNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbMachineNumber.Location = new System.Drawing.Point(12, 160);
            this.txbMachineNumber.Name = "txbMachineNumber";
            this.txbMachineNumber.Size = new System.Drawing.Size(158, 26);
            this.txbMachineNumber.TabIndex = 10;
            this.txbMachineNumber.Text = "11";
            this.txbMachineNumber.DoubleClick += new System.EventHandler(this.txbMachineNumber_DoubleClick);
            // 
            // cbRDPBSendBadCycle
            // 
            this.cbRDPBSendBadCycle.AutoSize = true;
            this.cbRDPBSendBadCycle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbRDPBSendBadCycle.Location = new System.Drawing.Point(12, 202);
            this.cbRDPBSendBadCycle.Name = "cbRDPBSendBadCycle";
            this.cbRDPBSendBadCycle.Size = new System.Drawing.Size(278, 19);
            this.cbRDPBSendBadCycle.TabIndex = 11;
            this.cbRDPBSendBadCycle.Text = "Отправлять ли на бракёр, что съем плохой";
            this.cbRDPBSendBadCycle.UseVisualStyleBackColor = true;
            // 
            // RDPSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 294);
            this.Controls.Add(this.cbRDPBSendBadCycle);
            this.Controls.Add(this.txbMachineNumber);
            this.Controls.Add(this.lblMachineNumber);
            this.Controls.Add(this.txbIPPort);
            this.Controls.Add(this.lblIPPort);
            this.Controls.Add(this.cbCoolingBlock);
            this.Controls.Add(this.txbRDPIP);
            this.Controls.Add(this.lblCoolingBlock);
            this.Controls.Add(this.lblRDPIP);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "RDPSettingsForm";
            this.Text = "Параметры бракера";
            this.Validating += new System.ComponentModel.CancelEventHandler(this.RDPSettingsForm_Validating);
            ((System.ComponentModel.ISupportInitialize)(this.epError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRDPIP;
        private System.Windows.Forms.Label lblCoolingBlock;
        private System.Windows.Forms.TextBox txbRDPIP;
        private System.Windows.Forms.ComboBox cbCoolingBlock;
        private System.Windows.Forms.Label lblIPPort;
        private System.Windows.Forms.TextBox txbIPPort;
        private System.Windows.Forms.ErrorProvider epError;
        private System.Windows.Forms.TextBox txbMachineNumber;
        private System.Windows.Forms.Label lblMachineNumber;
        private System.Windows.Forms.CheckBox cbRDPBSendBadCycle;
    }
}