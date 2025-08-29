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
            components = new System.ComponentModel.Container();
            nudMeasureDelay = new NumericUpDown();
            label11 = new Label();
            nudDataType = new NumericUpDown();
            label10 = new Label();
            nudFilterModule = new NumericUpDown();
            label4 = new Label();
            nudFrameDuration = new NumericUpDown();
            nudExposition = new NumericUpDown();
            label3 = new Label();
            label2 = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            epError = new ErrorProvider(components);
            btnImageParameters = new Button();
            ((System.ComponentModel.ISupportInitialize)nudMeasureDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDataType).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterModule).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFrameDuration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudExposition).BeginInit();
            ((System.ComponentModel.ISupportInitialize)epError).BeginInit();
            SuspendLayout();
            // 
            // nudMeasureDelay
            // 
            nudMeasureDelay.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudMeasureDelay.Location = new Point(279, 170);
            nudMeasureDelay.Margin = new Padding(4, 3, 4, 3);
            nudMeasureDelay.Maximum = new decimal(new int[] { 32767, 0, 0, 0 });
            nudMeasureDelay.Name = "nudMeasureDelay";
            nudMeasureDelay.Size = new Size(88, 29);
            nudMeasureDelay.TabIndex = 52;
            nudMeasureDelay.DoubleClick += num_DoubleClick;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label11.Location = new Point(14, 172);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(138, 24);
            label11.TabIndex = 51;
            label11.Text = "Задержка, мс:";
            // 
            // nudDataType
            // 
            nudDataType.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudDataType.Location = new Point(279, 128);
            nudDataType.Margin = new Padding(4, 3, 4, 3);
            nudDataType.Maximum = new decimal(new int[] { 32767, 0, 0, 0 });
            nudDataType.Name = "nudDataType";
            nudDataType.Size = new Size(88, 29);
            nudDataType.TabIndex = 49;
            nudDataType.DoubleClick += num_DoubleClick;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label10.Location = new Point(14, 130);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(121, 24);
            label10.TabIndex = 48;
            label10.Text = "Тип данных:";
            // 
            // nudFilterModule
            // 
            nudFilterModule.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudFilterModule.Location = new Point(279, 87);
            nudFilterModule.Margin = new Padding(4, 3, 4, 3);
            nudFilterModule.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            nudFilterModule.Name = "nudFilterModule";
            nudFilterModule.Size = new Size(88, 29);
            nudFilterModule.TabIndex = 40;
            nudFilterModule.DoubleClick += num_DoubleClick;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(14, 89);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(199, 24);
            label4.TabIndex = 36;
            label4.Text = "Модуль фильтрации:";
            // 
            // nudFrameDuration
            // 
            nudFrameDuration.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudFrameDuration.Location = new Point(279, 47);
            nudFrameDuration.Margin = new Padding(4, 3, 4, 3);
            nudFrameDuration.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudFrameDuration.Name = "nudFrameDuration";
            nudFrameDuration.Size = new Size(88, 29);
            nudFrameDuration.TabIndex = 34;
            nudFrameDuration.Value = new decimal(new int[] { 1300, 0, 0, 0 });
            nudFrameDuration.DoubleClick += num_DoubleClick;
            // 
            // nudExposition
            // 
            nudExposition.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudExposition.Location = new Point(279, 7);
            nudExposition.Margin = new Padding(4, 3, 4, 3);
            nudExposition.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudExposition.Name = "nudExposition";
            nudExposition.Size = new Size(88, 29);
            nudExposition.TabIndex = 33;
            nudExposition.Value = new decimal(new int[] { 40, 0, 0, 0 });
            nudExposition.DoubleClick += num_DoubleClick;
            nudExposition.Validating += nudExposition_Validating;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(14, 50);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(146, 24);
            label3.TabIndex = 32;
            label3.Text = "Длина фрейма:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(14, 13);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(122, 24);
            label2.TabIndex = 31;
            label2.Text = "Экспозиция:";
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnOK.Location = new Point(14, 316);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(124, 43);
            btnOK.TabIndex = 53;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.CausesValidation = false;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnCancel.Location = new Point(243, 316);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(124, 43);
            btnCancel.TabIndex = 54;
            btnCancel.Text = " Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // epError
            // 
            epError.ContainerControl = this;
            // 
            // btnImageParameters
            // 
            btnImageParameters.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnImageParameters.Location = new Point(19, 210);
            btnImageParameters.Margin = new Padding(4, 3, 4, 3);
            btnImageParameters.Name = "btnImageParameters";
            btnImageParameters.Size = new Size(348, 78);
            btnImageParameters.TabIndex = 83;
            btnImageParameters.Text = "Настройки приятия решения";
            btnImageParameters.UseVisualStyleBackColor = true;
            btnImageParameters.Click += btnImageParameters_Click;
            // 
            // DoMCSocketSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 373);
            Controls.Add(btnImageParameters);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(nudMeasureDelay);
            Controls.Add(label11);
            Controls.Add(nudDataType);
            Controls.Add(label10);
            Controls.Add(nudFilterModule);
            Controls.Add(label4);
            Controls.Add(nudFrameDuration);
            Controls.Add(nudExposition);
            Controls.Add(label3);
            Controls.Add(label2);
            Margin = new Padding(4, 3, 4, 3);
            Name = "DoMCSocketSettingsForm";
            Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)nudMeasureDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDataType).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFilterModule).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFrameDuration).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudExposition).EndInit();
            ((System.ComponentModel.ISupportInitialize)epError).EndInit();
            ResumeLayout(false);
            PerformLayout();
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