namespace DoMCLib.Forms
{
    partial class DoMCGeneralSettingsForm
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
            groupBox1 = new GroupBox();
            nudStandardPercent = new NumericUpDown();
            nudCycles = new NumericUpDown();
            label4 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudStandardPercent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCycles).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(14, 74);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 27);
            btnOK.TabIndex = 6;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(443, 74);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(nudStandardPercent);
            groupBox1.Controls.Add(nudCycles);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(14, 14);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(516, 48);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Коррекция эталона";
            // 
            // nudStandardPercent
            // 
            nudStandardPercent.Font = new Font("Microsoft Sans Serif", 9F);
            nudStandardPercent.Location = new Point(248, 19);
            nudStandardPercent.Margin = new Padding(6, 3, 6, 3);
            nudStandardPercent.Name = "nudStandardPercent";
            nudStandardPercent.Size = new Size(74, 21);
            nudStandardPercent.TabIndex = 55;
            nudStandardPercent.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudStandardPercent.DoubleClick += num_DoubleClick;
            // 
            // nudCycles
            // 
            nudCycles.Font = new Font("Microsoft Sans Serif", 9F);
            nudCycles.Location = new Point(59, 19);
            nudCycles.Margin = new Padding(6, 3, 6, 3);
            nudCycles.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudCycles.Name = "nudCycles";
            nudCycles.Size = new Size(74, 21);
            nudCycles.TabIndex = 54;
            nudCycles.Value = new decimal(new int[] { 10, 0, 0, 0 });
            nudCycles.DoubleClick += num_DoubleClick;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(332, 22);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(144, 15);
            label4.TabIndex = 10;
            label4.Text = "% изначального эталона";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(134, 22);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(104, 15);
            label2.TabIndex = 8;
            label2.Text = "циклов останется";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 22);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 6;
            label1.Text = "Через";
            // 
            // DoMCGeneralSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(545, 114);
            Controls.Add(groupBox1);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Margin = new Padding(4, 3, 4, 3);
            Name = "DoMCGeneralSettingsForm";
            Text = "Настройки обновления эталона";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudStandardPercent).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCycles).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private NumericUpDown nudCycles;
        private NumericUpDown nudStandardPercent;
    }
}