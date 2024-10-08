namespace DoMCLib.Forms
{
    partial class DoMCSocketSettingsForm
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
            this.nudMeasureDelay = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.nudDataType = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nudFilterModule = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudFrameDuration = new System.Windows.Forms.NumericUpDown();
            this.nudExposition = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.epError = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnImageParameters = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterModule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrameDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epError)).BeginInit();
            this.SuspendLayout();
            // 
            // nudMeasureDelay
            // 
            this.nudMeasureDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudMeasureDelay.Location = new System.Drawing.Point(239, 147);
            this.nudMeasureDelay.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nudMeasureDelay.Name = "nudMeasureDelay";
            this.nudMeasureDelay.Size = new System.Drawing.Size(75, 29);
            this.nudMeasureDelay.TabIndex = 52;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(12, 149);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(138, 24);
            this.label11.TabIndex = 51;
            this.label11.Text = "Задержка, мс:";
            // 
            // nudDataType
            // 
            this.nudDataType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudDataType.Location = new System.Drawing.Point(239, 111);
            this.nudDataType.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nudDataType.Name = "nudDataType";
            this.nudDataType.Size = new System.Drawing.Size(75, 29);
            this.nudDataType.TabIndex = 49;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(12, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 24);
            this.label10.TabIndex = 48;
            this.label10.Text = "Тип данных:";
            // 
            // nudFilterModule
            // 
            this.nudFilterModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudFilterModule.Location = new System.Drawing.Point(239, 75);
            this.nudFilterModule.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudFilterModule.Name = "nudFilterModule";
            this.nudFilterModule.Size = new System.Drawing.Size(75, 29);
            this.nudFilterModule.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(12, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(199, 24);
            this.label4.TabIndex = 36;
            this.label4.Text = "Модуль фильтрации:";
            // 
            // nudFrameDuration
            // 
            this.nudFrameDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudFrameDuration.Location = new System.Drawing.Point(239, 41);
            this.nudFrameDuration.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudFrameDuration.Name = "nudFrameDuration";
            this.nudFrameDuration.Size = new System.Drawing.Size(75, 29);
            this.nudFrameDuration.TabIndex = 34;
            this.nudFrameDuration.Value = new decimal(new int[] {
            1300,
            0,
            0,
            0});
            this.nudFrameDuration.DoubleClick += new System.EventHandler(this.numExposition_DoubleClick);
            // 
            // nudExposition
            // 
            this.nudExposition.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudExposition.Location = new System.Drawing.Point(239, 6);
            this.nudExposition.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudExposition.Name = "nudExposition";
            this.nudExposition.Size = new System.Drawing.Size(75, 29);
            this.nudExposition.TabIndex = 33;
            this.nudExposition.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudExposition.DoubleClick += new System.EventHandler(this.numExposition_DoubleClick);
            this.nudExposition.Validating += new System.ComponentModel.CancelEventHandler(this.nudExposition_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(12, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 24);
            this.label3.TabIndex = 32;
            this.label3.Text = "Длина фрейма:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 24);
            this.label2.TabIndex = 31;
            this.label2.Text = "Экспозиция:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOK.Location = new System.Drawing.Point(12, 274);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(106, 37);
            this.btnOK.TabIndex = 53;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(208, 274);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 37);
            this.btnCancel.TabIndex = 54;
            this.btnCancel.Text = " Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // epError
            // 
            this.epError.ContainerControl = this;
            // 
            // btnImageParameters
            // 
            this.btnImageParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnImageParameters.Location = new System.Drawing.Point(16, 182);
            this.btnImageParameters.Name = "btnImageParameters";
            this.btnImageParameters.Size = new System.Drawing.Size(298, 68);
            this.btnImageParameters.TabIndex = 83;
            this.btnImageParameters.Text = "Настройки приятия решения";
            this.btnImageParameters.UseVisualStyleBackColor = true;
            this.btnImageParameters.Click += new System.EventHandler(this.btnImageParameters_Click);
            // 
            // DoMCSocketSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 323);
            this.Controls.Add(this.btnImageParameters);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.nudMeasureDelay);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.nudDataType);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.nudFilterModule);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudFrameDuration);
            this.Controls.Add(this.nudExposition);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "DoMCSocketSettingsForm";
            this.Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterModule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrameDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown nudMeasureDelay;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nudDataType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudFilterModule;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudFrameDuration;
        private System.Windows.Forms.NumericUpDown nudExposition;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider epError;
        private System.Windows.Forms.Button btnImageParameters;
    }
}