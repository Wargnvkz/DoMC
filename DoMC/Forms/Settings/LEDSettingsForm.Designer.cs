namespace DoMCLib.Forms
{
    partial class LEDSettingsForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.txbCurrent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txbPreformLength = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txbDelayLength = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.epError = new System.Windows.Forms.ErrorProvider(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.txbLCBKoefficient = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.epError)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOK.Location = new System.Drawing.Point(12, 176);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 35);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(218, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 35);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(296, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 24);
            this.label4.TabIndex = 10;
            this.label4.Text = "мА";
            // 
            // txbCurrent
            // 
            this.txbCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbCurrent.Location = new System.Drawing.Point(218, 12);
            this.txbCurrent.Name = "txbCurrent";
            this.txbCurrent.Size = new System.Drawing.Size(72, 29);
            this.txbCurrent.TabIndex = 9;
            this.txbCurrent.DoubleClick += new System.EventHandler(this.txbInput_DoubleClick);
            this.txbCurrent.Validating += new System.ComponentModel.CancelEventHandler(this.txb_intValidating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(8, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Ток:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(296, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 24);
            this.label1.TabIndex = 13;
            this.label1.Text = "мм";
            // 
            // txbPreformLength
            // 
            this.txbPreformLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbPreformLength.Location = new System.Drawing.Point(218, 47);
            this.txbPreformLength.Name = "txbPreformLength";
            this.txbPreformLength.Size = new System.Drawing.Size(72, 29);
            this.txbPreformLength.TabIndex = 12;
            this.txbPreformLength.DoubleClick += new System.EventHandler(this.txbInput_DoubleClick);
            this.txbPreformLength.Validating += new System.ComponentModel.CancelEventHandler(this.txb_intValidating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(8, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 24);
            this.label2.TabIndex = 11;
            this.label2.Text = "Длина преформы:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(296, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 24);
            this.label5.TabIndex = 16;
            this.label5.Text = "мм";
            // 
            // txbDelayLength
            // 
            this.txbDelayLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbDelayLength.Location = new System.Drawing.Point(218, 82);
            this.txbDelayLength.Name = "txbDelayLength";
            this.txbDelayLength.Size = new System.Drawing.Size(72, 29);
            this.txbDelayLength.TabIndex = 15;
            this.txbDelayLength.DoubleClick += new System.EventHandler(this.txbInput_DoubleClick);
            this.txbDelayLength.Validating += new System.ComponentModel.CancelEventHandler(this.txb_intValidating);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(8, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(164, 24);
            this.label6.TabIndex = 14;
            this.label6.Text = "Длина задержки:";
            // 
            // epError
            // 
            this.epError.ContainerControl = this;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(249, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 24);
            this.label7.TabIndex = 19;
            this.label7.Text = "имп/мм";
            // 
            // txbLCBKoefficient
            // 
            this.txbLCBKoefficient.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbLCBKoefficient.Location = new System.Drawing.Point(156, 131);
            this.txbLCBKoefficient.Name = "txbLCBKoefficient";
            this.txbLCBKoefficient.Size = new System.Drawing.Size(87, 29);
            this.txbLCBKoefficient.TabIndex = 18;
            this.txbLCBKoefficient.DoubleClick += new System.EventHandler(this.txbInput_DoubleClick);
            this.txbLCBKoefficient.Validating += new System.ComponentModel.CancelEventHandler(this.txb_doubleValidating);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(8, 136);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 24);
            this.label8.TabIndex = 17;
            this.label8.Text = "Коэффициент:";
            // 
            // LEDSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 220);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txbLCBKoefficient);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txbDelayLength);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbPreformLength);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txbCurrent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "LEDSettingsForm";
            this.Text = "Параметры БУС";
            ((System.ComponentModel.ISupportInitialize)(this.epError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbCurrent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbPreformLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbDelayLength;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider epError;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txbLCBKoefficient;
        private System.Windows.Forms.Label label8;
    }
}