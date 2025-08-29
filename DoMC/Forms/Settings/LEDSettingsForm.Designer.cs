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
            components = new System.ComponentModel.Container();
            btnOK = new Button();
            btnCancel = new Button();
            label4 = new Label();
            txbCurrent = new TextBox();
            label3 = new Label();
            label1 = new Label();
            txbPreformLength = new TextBox();
            label2 = new Label();
            label5 = new Label();
            txbDelayLength = new TextBox();
            label6 = new Label();
            epError = new ErrorProvider(components);
            label7 = new Label();
            txbLCBKoefficient = new TextBox();
            label8 = new Label();
            ((System.ComponentModel.ISupportInitialize)epError).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnOK.Location = new Point(14, 203);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(125, 40);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.CausesValidation = false;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnCancel.Location = new Point(254, 203);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(125, 40);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(345, 17);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(36, 24);
            label4.TabIndex = 10;
            label4.Text = "мА";
            // 
            // txbCurrent
            // 
            txbCurrent.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbCurrent.Location = new Point(254, 14);
            txbCurrent.Margin = new Padding(4, 3, 4, 3);
            txbCurrent.Name = "txbCurrent";
            txbCurrent.Size = new Size(83, 29);
            txbCurrent.TabIndex = 9;
            txbCurrent.DoubleClick += txbInput_DoubleClick;
            txbCurrent.Validating += txb_intValidating;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(9, 17);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(47, 24);
            label3.TabIndex = 8;
            label3.Text = "Ток:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(345, 58);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(45, 24);
            label1.TabIndex = 13;
            label1.Text = "имп";
            // 
            // txbPreformLength
            // 
            txbPreformLength.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbPreformLength.Location = new Point(254, 54);
            txbPreformLength.Margin = new Padding(4, 3, 4, 3);
            txbPreformLength.Name = "txbPreformLength";
            txbPreformLength.Size = new Size(83, 29);
            txbPreformLength.TabIndex = 12;
            txbPreformLength.DoubleClick += txbInput_DoubleClick;
            txbPreformLength.Validating += txb_intValidating;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(9, 58);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(171, 24);
            label2.TabIndex = 11;
            label2.Text = "Длина преформы:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(345, 98);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(45, 24);
            label5.TabIndex = 16;
            label5.Text = "имп";
            // 
            // txbDelayLength
            // 
            txbDelayLength.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbDelayLength.Location = new Point(254, 95);
            txbDelayLength.Margin = new Padding(4, 3, 4, 3);
            txbDelayLength.Name = "txbDelayLength";
            txbDelayLength.Size = new Size(83, 29);
            txbDelayLength.TabIndex = 15;
            txbDelayLength.DoubleClick += txbInput_DoubleClick;
            txbDelayLength.Validating += txb_intValidating;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.Location = new Point(9, 98);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(164, 24);
            label6.TabIndex = 14;
            label6.Text = "Длина задержки:";
            // 
            // epError
            // 
            epError.ContainerControl = this;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label7.Location = new Point(290, 157);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(76, 24);
            label7.TabIndex = 19;
            label7.Text = "имп/мм";
            // 
            // txbLCBKoefficient
            // 
            txbLCBKoefficient.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbLCBKoefficient.Location = new Point(182, 151);
            txbLCBKoefficient.Margin = new Padding(4, 3, 4, 3);
            txbLCBKoefficient.Name = "txbLCBKoefficient";
            txbLCBKoefficient.Size = new Size(101, 29);
            txbLCBKoefficient.TabIndex = 18;
            txbLCBKoefficient.Validating += txb_doubleValidating;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label8.Location = new Point(9, 157);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(141, 24);
            label8.TabIndex = 17;
            label8.Text = "Коэффициент:";
            // 
            // LEDSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(393, 254);
            Controls.Add(label7);
            Controls.Add(txbLCBKoefficient);
            Controls.Add(label8);
            Controls.Add(label5);
            Controls.Add(txbDelayLength);
            Controls.Add(label6);
            Controls.Add(label1);
            Controls.Add(txbPreformLength);
            Controls.Add(label2);
            Controls.Add(label4);
            Controls.Add(txbCurrent);
            Controls.Add(label3);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Margin = new Padding(4, 3, 4, 3);
            Name = "LEDSettingsForm";
            Text = "Параметры БУС";
            ((System.ComponentModel.ISupportInitialize)epError).EndInit();
            ResumeLayout(false);
            PerformLayout();
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