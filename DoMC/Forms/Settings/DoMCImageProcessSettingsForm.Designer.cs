namespace DoMCLib.Forms
{
    partial class DoMCImageProcessSettingsForm
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
            label1 = new Label();
            nudRight = new NumericUpDown();
            nudLeft = new NumericUpDown();
            nudBottom = new NumericUpDown();
            nudTop = new NumericUpDown();
            btnCancel = new Button();
            btnOK = new Button();
            panel1 = new Panel();
            cbActionDefect1 = new ComboBox();
            nudActionDefectParameter1 = new NumericUpDown();
            label2 = new Label();
            label3 = new Label();
            nudActionColorParameter1 = new NumericUpDown();
            cbActionColor1 = new ComboBox();
            label4 = new Label();
            nudColorParameterResult = new NumericUpDown();
            cbColorResult = new ComboBox();
            label5 = new Label();
            nudDefectParameterResult = new NumericUpDown();
            cbDefectResult = new ComboBox();
            nudActionColorParameter2 = new NumericUpDown();
            cbActionColor2 = new ComboBox();
            nudActionDefectParameter2 = new NumericUpDown();
            cbActionDefect2 = new ComboBox();
            nudActionColorParameter3 = new NumericUpDown();
            cbActionColor3 = new ComboBox();
            nudActionDefectParameter3 = new NumericUpDown();
            cbActionDefect3 = new ComboBox();
            nudActionColorParameter4 = new NumericUpDown();
            cbActionColor4 = new ComboBox();
            nudActionDefectParameter4 = new NumericUpDown();
            cbActionDefect4 = new ComboBox();
            nudActionColorParameter5 = new NumericUpDown();
            cbActionColor5 = new ComboBox();
            nudActionDefectParameter5 = new NumericUpDown();
            cbActionDefect5 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)nudRight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLeft).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudBottom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudTop).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudColorParameterResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDefectParameterResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter5).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(43, 391);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(183, 24);
            label1.TabIndex = 96;
            label1.Text = "Область проверки:";
            // 
            // nudRight
            // 
            nudRight.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudRight.Location = new Point(410, 391);
            nudRight.Margin = new Padding(4, 3, 4, 3);
            nudRight.Maximum = new decimal(new int[] { 511, 0, 0, 0 });
            nudRight.Name = "nudRight";
            nudRight.Size = new Size(75, 29);
            nudRight.TabIndex = 90;
            nudRight.Value = new decimal(new int[] { 511, 0, 0, 0 });
            nudRight.DoubleClick += num_DoubleClick;
            // 
            // nudLeft
            // 
            nudLeft.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudLeft.Location = new Point(260, 391);
            nudLeft.Margin = new Padding(4, 3, 4, 3);
            nudLeft.Maximum = new decimal(new int[] { 511, 0, 0, 0 });
            nudLeft.Name = "nudLeft";
            nudLeft.Size = new Size(75, 29);
            nudLeft.TabIndex = 89;
            nudLeft.DoubleClick += num_DoubleClick;
            // 
            // nudBottom
            // 
            nudBottom.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudBottom.Location = new Point(332, 430);
            nudBottom.Margin = new Padding(4, 3, 4, 3);
            nudBottom.Maximum = new decimal(new int[] { 511, 0, 0, 0 });
            nudBottom.Name = "nudBottom";
            nudBottom.Size = new Size(75, 29);
            nudBottom.TabIndex = 88;
            nudBottom.Value = new decimal(new int[] { 511, 0, 0, 0 });
            nudBottom.DoubleClick += num_DoubleClick;
            // 
            // nudTop
            // 
            nudTop.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudTop.Location = new Point(332, 351);
            nudTop.Margin = new Padding(4, 3, 4, 3);
            nudTop.Maximum = new decimal(new int[] { 511, 0, 0, 0 });
            nudTop.Name = "nudTop";
            nudTop.Size = new Size(75, 29);
            nudTop.TabIndex = 87;
            nudTop.DoubleClick += num_DoubleClick;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.CausesValidation = false;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnCancel.Location = new Point(595, 478);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(124, 43);
            btnCancel.TabIndex = 84;
            btnCancel.Text = " Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnOK.Location = new Point(19, 478);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(124, 43);
            btnOK.TabIndex = 83;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(293, 363);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(158, 89);
            panel1.TabIndex = 95;
            // 
            // cbActionDefect1
            // 
            cbActionDefect1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionDefect1.FormattingEnabled = true;
            cbActionDefect1.Location = new Point(19, 42);
            cbActionDefect1.Margin = new Padding(4, 3, 4, 3);
            cbActionDefect1.Name = "cbActionDefect1";
            cbActionDefect1.Size = new Size(237, 32);
            cbActionDefect1.TabIndex = 97;
            // 
            // nudActionDefectParameter1
            // 
            nudActionDefectParameter1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionDefectParameter1.Location = new Point(264, 43);
            nudActionDefectParameter1.Margin = new Padding(4, 3, 4, 3);
            nudActionDefectParameter1.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionDefectParameter1.Name = "nudActionDefectParameter1";
            nudActionDefectParameter1.Size = new Size(92, 29);
            nudActionDefectParameter1.TabIndex = 98;
            nudActionDefectParameter1.DoubleClick += num_DoubleClick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(14, 10);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(209, 24);
            label2.TabIndex = 99;
            label2.Text = "Выявление дефектов:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(377, 10);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(183, 24);
            label3.TabIndex = 102;
            label3.Text = "Отклонение цвета:";
            // 
            // nudActionColorParameter1
            // 
            nudActionColorParameter1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionColorParameter1.Location = new Point(626, 43);
            nudActionColorParameter1.Margin = new Padding(4, 3, 4, 3);
            nudActionColorParameter1.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionColorParameter1.Name = "nudActionColorParameter1";
            nudActionColorParameter1.Size = new Size(88, 29);
            nudActionColorParameter1.TabIndex = 101;
            nudActionColorParameter1.DoubleClick += num_DoubleClick;
            // 
            // cbActionColor1
            // 
            cbActionColor1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionColor1.FormattingEnabled = true;
            cbActionColor1.Location = new Point(382, 42);
            cbActionColor1.Margin = new Padding(4, 3, 4, 3);
            cbActionColor1.Name = "cbActionColor1";
            cbActionColor1.Size = new Size(237, 32);
            cbActionColor1.TabIndex = 100;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(377, 262);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(183, 24);
            label4.TabIndex = 108;
            label4.Text = "Отклонение цвета:";
            // 
            // nudColorParameterResult
            // 
            nudColorParameterResult.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudColorParameterResult.Location = new Point(626, 294);
            nudColorParameterResult.Margin = new Padding(4, 3, 4, 3);
            nudColorParameterResult.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudColorParameterResult.Name = "nudColorParameterResult";
            nudColorParameterResult.Size = new Size(88, 29);
            nudColorParameterResult.TabIndex = 107;
            nudColorParameterResult.DoubleClick += num_DoubleClick;
            // 
            // cbColorResult
            // 
            cbColorResult.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbColorResult.FormattingEnabled = true;
            cbColorResult.Location = new Point(382, 293);
            cbColorResult.Margin = new Padding(4, 3, 4, 3);
            cbColorResult.Name = "cbColorResult";
            cbColorResult.Size = new Size(237, 32);
            cbColorResult.TabIndex = 106;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(14, 262);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(209, 24);
            label5.TabIndex = 105;
            label5.Text = "Выявление дефектов:";
            // 
            // nudDefectParameterResult
            // 
            nudDefectParameterResult.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudDefectParameterResult.Location = new Point(264, 294);
            nudDefectParameterResult.Margin = new Padding(4, 3, 4, 3);
            nudDefectParameterResult.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudDefectParameterResult.Name = "nudDefectParameterResult";
            nudDefectParameterResult.Size = new Size(92, 29);
            nudDefectParameterResult.TabIndex = 104;
            nudDefectParameterResult.DoubleClick += num_DoubleClick;
            // 
            // cbDefectResult
            // 
            cbDefectResult.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbDefectResult.FormattingEnabled = true;
            cbDefectResult.Location = new Point(19, 293);
            cbDefectResult.Margin = new Padding(4, 3, 4, 3);
            cbDefectResult.Name = "cbDefectResult";
            cbDefectResult.Size = new Size(237, 32);
            cbDefectResult.TabIndex = 103;
            // 
            // nudActionColorParameter2
            // 
            nudActionColorParameter2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionColorParameter2.Location = new Point(626, 87);
            nudActionColorParameter2.Margin = new Padding(4, 3, 4, 3);
            nudActionColorParameter2.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionColorParameter2.Name = "nudActionColorParameter2";
            nudActionColorParameter2.Size = new Size(88, 29);
            nudActionColorParameter2.TabIndex = 112;
            nudActionColorParameter2.DoubleClick += num_DoubleClick;
            // 
            // cbActionColor2
            // 
            cbActionColor2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionColor2.FormattingEnabled = true;
            cbActionColor2.Location = new Point(382, 85);
            cbActionColor2.Margin = new Padding(4, 3, 4, 3);
            cbActionColor2.Name = "cbActionColor2";
            cbActionColor2.Size = new Size(237, 32);
            cbActionColor2.TabIndex = 111;
            // 
            // nudActionDefectParameter2
            // 
            nudActionDefectParameter2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionDefectParameter2.Location = new Point(264, 87);
            nudActionDefectParameter2.Margin = new Padding(4, 3, 4, 3);
            nudActionDefectParameter2.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionDefectParameter2.Name = "nudActionDefectParameter2";
            nudActionDefectParameter2.Size = new Size(92, 29);
            nudActionDefectParameter2.TabIndex = 110;
            nudActionDefectParameter2.DoubleClick += num_DoubleClick;
            // 
            // cbActionDefect2
            // 
            cbActionDefect2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionDefect2.FormattingEnabled = true;
            cbActionDefect2.Location = new Point(19, 85);
            cbActionDefect2.Margin = new Padding(4, 3, 4, 3);
            cbActionDefect2.Name = "cbActionDefect2";
            cbActionDefect2.Size = new Size(237, 32);
            cbActionDefect2.TabIndex = 109;
            // 
            // nudActionColorParameter3
            // 
            nudActionColorParameter3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionColorParameter3.Location = new Point(626, 130);
            nudActionColorParameter3.Margin = new Padding(4, 3, 4, 3);
            nudActionColorParameter3.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionColorParameter3.Name = "nudActionColorParameter3";
            nudActionColorParameter3.Size = new Size(88, 29);
            nudActionColorParameter3.TabIndex = 116;
            nudActionColorParameter3.DoubleClick += num_DoubleClick;
            // 
            // cbActionColor3
            // 
            cbActionColor3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionColor3.FormattingEnabled = true;
            cbActionColor3.Location = new Point(382, 129);
            cbActionColor3.Margin = new Padding(4, 3, 4, 3);
            cbActionColor3.Name = "cbActionColor3";
            cbActionColor3.Size = new Size(237, 32);
            cbActionColor3.TabIndex = 115;
            // 
            // nudActionDefectParameter3
            // 
            nudActionDefectParameter3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionDefectParameter3.Location = new Point(264, 130);
            nudActionDefectParameter3.Margin = new Padding(4, 3, 4, 3);
            nudActionDefectParameter3.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionDefectParameter3.Name = "nudActionDefectParameter3";
            nudActionDefectParameter3.Size = new Size(92, 29);
            nudActionDefectParameter3.TabIndex = 114;
            nudActionDefectParameter3.DoubleClick += num_DoubleClick;
            // 
            // cbActionDefect3
            // 
            cbActionDefect3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionDefect3.FormattingEnabled = true;
            cbActionDefect3.Location = new Point(19, 129);
            cbActionDefect3.Margin = new Padding(4, 3, 4, 3);
            cbActionDefect3.Name = "cbActionDefect3";
            cbActionDefect3.Size = new Size(237, 32);
            cbActionDefect3.TabIndex = 113;
            // 
            // nudActionColorParameter4
            // 
            nudActionColorParameter4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionColorParameter4.Location = new Point(626, 174);
            nudActionColorParameter4.Margin = new Padding(4, 3, 4, 3);
            nudActionColorParameter4.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionColorParameter4.Name = "nudActionColorParameter4";
            nudActionColorParameter4.Size = new Size(88, 29);
            nudActionColorParameter4.TabIndex = 120;
            nudActionColorParameter4.DoubleClick += num_DoubleClick;
            // 
            // cbActionColor4
            // 
            cbActionColor4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionColor4.FormattingEnabled = true;
            cbActionColor4.Location = new Point(382, 173);
            cbActionColor4.Margin = new Padding(4, 3, 4, 3);
            cbActionColor4.Name = "cbActionColor4";
            cbActionColor4.Size = new Size(237, 32);
            cbActionColor4.TabIndex = 119;
            // 
            // nudActionDefectParameter4
            // 
            nudActionDefectParameter4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionDefectParameter4.Location = new Point(264, 174);
            nudActionDefectParameter4.Margin = new Padding(4, 3, 4, 3);
            nudActionDefectParameter4.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionDefectParameter4.Name = "nudActionDefectParameter4";
            nudActionDefectParameter4.Size = new Size(92, 29);
            nudActionDefectParameter4.TabIndex = 118;
            nudActionDefectParameter4.DoubleClick += num_DoubleClick;
            // 
            // cbActionDefect4
            // 
            cbActionDefect4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionDefect4.FormattingEnabled = true;
            cbActionDefect4.Location = new Point(19, 173);
            cbActionDefect4.Margin = new Padding(4, 3, 4, 3);
            cbActionDefect4.Name = "cbActionDefect4";
            cbActionDefect4.Size = new Size(237, 32);
            cbActionDefect4.TabIndex = 117;
            // 
            // nudActionColorParameter5
            // 
            nudActionColorParameter5.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionColorParameter5.Location = new Point(626, 218);
            nudActionColorParameter5.Margin = new Padding(4, 3, 4, 3);
            nudActionColorParameter5.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionColorParameter5.Name = "nudActionColorParameter5";
            nudActionColorParameter5.Size = new Size(88, 29);
            nudActionColorParameter5.TabIndex = 124;
            nudActionColorParameter5.DoubleClick += num_DoubleClick;
            // 
            // cbActionColor5
            // 
            cbActionColor5.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionColor5.FormattingEnabled = true;
            cbActionColor5.Location = new Point(382, 217);
            cbActionColor5.Margin = new Padding(4, 3, 4, 3);
            cbActionColor5.Name = "cbActionColor5";
            cbActionColor5.Size = new Size(237, 32);
            cbActionColor5.TabIndex = 123;
            // 
            // nudActionDefectParameter5
            // 
            nudActionDefectParameter5.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            nudActionDefectParameter5.Location = new Point(264, 218);
            nudActionDefectParameter5.Margin = new Padding(4, 3, 4, 3);
            nudActionDefectParameter5.Maximum = new decimal(new int[] { 32000, 0, 0, 0 });
            nudActionDefectParameter5.Name = "nudActionDefectParameter5";
            nudActionDefectParameter5.Size = new Size(92, 29);
            nudActionDefectParameter5.TabIndex = 122;
            nudActionDefectParameter5.DoubleClick += num_DoubleClick;
            // 
            // cbActionDefect5
            // 
            cbActionDefect5.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbActionDefect5.FormattingEnabled = true;
            cbActionDefect5.Location = new Point(19, 217);
            cbActionDefect5.Margin = new Padding(4, 3, 4, 3);
            cbActionDefect5.Name = "cbActionDefect5";
            cbActionDefect5.Size = new Size(237, 32);
            cbActionDefect5.TabIndex = 121;
            // 
            // DoMCImageProcessSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(733, 534);
            Controls.Add(nudActionColorParameter5);
            Controls.Add(cbActionColor5);
            Controls.Add(nudActionDefectParameter5);
            Controls.Add(cbActionDefect5);
            Controls.Add(nudActionColorParameter4);
            Controls.Add(cbActionColor4);
            Controls.Add(nudActionDefectParameter4);
            Controls.Add(cbActionDefect4);
            Controls.Add(nudActionColorParameter3);
            Controls.Add(cbActionColor3);
            Controls.Add(nudActionDefectParameter3);
            Controls.Add(cbActionDefect3);
            Controls.Add(nudActionColorParameter2);
            Controls.Add(cbActionColor2);
            Controls.Add(nudActionDefectParameter2);
            Controls.Add(cbActionDefect2);
            Controls.Add(label4);
            Controls.Add(nudColorParameterResult);
            Controls.Add(cbColorResult);
            Controls.Add(label5);
            Controls.Add(nudDefectParameterResult);
            Controls.Add(cbDefectResult);
            Controls.Add(label3);
            Controls.Add(nudActionColorParameter1);
            Controls.Add(cbActionColor1);
            Controls.Add(label2);
            Controls.Add(nudActionDefectParameter1);
            Controls.Add(cbActionDefect1);
            Controls.Add(label1);
            Controls.Add(nudRight);
            Controls.Add(nudLeft);
            Controls.Add(nudBottom);
            Controls.Add(nudTop);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(panel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "DoMCImageProcessSettingsForm";
            Text = "Настройка обработки изображений";
            ((System.ComponentModel.ISupportInitialize)nudRight).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudBottom).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudTop).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudColorParameterResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDefectParameterResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter3).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter3).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter4).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter4).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionColorParameter5).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudActionDefectParameter5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudRight;
        private System.Windows.Forms.NumericUpDown nudLeft;
        private System.Windows.Forms.NumericUpDown nudBottom;
        private System.Windows.Forms.NumericUpDown nudTop;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbActionDefect1;
        private System.Windows.Forms.NumericUpDown nudActionDefectParameter1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudActionColorParameter1;
        private System.Windows.Forms.ComboBox cbActionColor1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudColorParameterResult;
        private System.Windows.Forms.ComboBox cbColorResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudDefectParameterResult;
        private System.Windows.Forms.ComboBox cbDefectResult;
        private System.Windows.Forms.NumericUpDown nudActionColorParameter2;
        private System.Windows.Forms.ComboBox cbActionColor2;
        private System.Windows.Forms.NumericUpDown nudActionDefectParameter2;
        private System.Windows.Forms.ComboBox cbActionDefect2;
        private System.Windows.Forms.NumericUpDown nudActionColorParameter3;
        private System.Windows.Forms.ComboBox cbActionColor3;
        private System.Windows.Forms.NumericUpDown nudActionDefectParameter3;
        private System.Windows.Forms.ComboBox cbActionDefect3;
        private System.Windows.Forms.NumericUpDown nudActionColorParameter4;
        private System.Windows.Forms.ComboBox cbActionColor4;
        private System.Windows.Forms.NumericUpDown nudActionDefectParameter4;
        private System.Windows.Forms.ComboBox cbActionDefect4;
        private System.Windows.Forms.NumericUpDown nudActionColorParameter5;
        private System.Windows.Forms.ComboBox cbActionColor5;
        private System.Windows.Forms.NumericUpDown nudActionDefectParameter5;
        private System.Windows.Forms.ComboBox cbActionDefect5;
    }
}