namespace DoMC.Forms
{
    partial class DoMCSocketCopyParametersForm
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
            DataGridView dgvCardNumbers;
            dgvCardNumberText = new DataGridViewTextBoxColumn();
            dgvBtnOn = new DataGridViewButtonColumn();
            dgvBtnOff = new DataGridViewButtonColumn();
            lblSocketQuantityCaption = new Label();
            pnlSockets = new Panel();
            bntReset = new Button();
            btnOK = new Button();
            btnCancel = new Button();
            btnSetAll = new Button();
            lblSocketQuantity = new Label();
            gbReadingParameters = new GroupBox();
            cbFrameLength = new CheckBox();
            cbExposition = new CheckBox();
            gbCheckingParameters = new GroupBox();
            cbDecisionOperations = new CheckBox();
            cbCheckRectRight = new CheckBox();
            lblCheckRegionCaption = new Label();
            cbCheckRectLeft = new CheckBox();
            cbCheckRectBottom = new CheckBox();
            cbCheckRectTop = new CheckBox();
            panel1 = new Panel();
            lblWhereToCopyCaption = new Label();
            lblWhatToCopyCaption = new Label();
            dgvCardNumbers = new DataGridView();
            gbReadingParameters.SuspendLayout();
            gbCheckingParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCardNumbers).BeginInit();
            SuspendLayout();
            // 
            // dgvCardNumberText
            // 
            dgvCardNumberText.DataPropertyName = "CardName";
            dgvCardNumberText.HeaderText = "Плата";
            dgvCardNumberText.Name = "dgvCardNumberText";
            dgvCardNumberText.Resizable = DataGridViewTriState.False;
            // 
            // dgvBtnOn
            // 
            dgvBtnOn.DataPropertyName = "On";
            dgvBtnOn.HeaderText = "Включить";
            dgvBtnOn.Name = "dgvBtnOn";
            dgvBtnOn.Width = 80;
            // 
            // dgvBtnOff
            // 
            dgvBtnOff.DataPropertyName = "Off";
            dgvBtnOff.HeaderText = "Выключить";
            dgvBtnOff.Name = "dgvBtnOff";
            dgvBtnOff.Width = 80;
            // 
            // lblSocketQuantityCaption
            // 
            lblSocketQuantityCaption.AutoSize = true;
            lblSocketQuantityCaption.Location = new Point(238, 4);
            lblSocketQuantityCaption.Margin = new Padding(4, 0, 4, 0);
            lblSocketQuantityCaption.Name = "lblSocketQuantityCaption";
            lblSocketQuantityCaption.Size = new Size(107, 15);
            lblSocketQuantityCaption.TabIndex = 0;
            lblSocketQuantityCaption.Text = "Количество гнезд:";
            // 
            // pnlSockets
            // 
            pnlSockets.Location = new Point(238, 59);
            pnlSockets.Margin = new Padding(4, 3, 4, 3);
            pnlSockets.Name = "pnlSockets";
            pnlSockets.Size = new Size(580, 444);
            pnlSockets.TabIndex = 2;
            // 
            // bntReset
            // 
            bntReset.Location = new Point(533, 4);
            bntReset.Margin = new Padding(4, 3, 4, 3);
            bntReset.Name = "bntReset";
            bntReset.Size = new Size(134, 50);
            bntReset.TabIndex = 3;
            bntReset.Text = "Сбросить все гнезда";
            bntReset.UseVisualStyleBackColor = true;
            bntReset.Click += bntReset_Click;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(13, 509);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 27);
            btnOK.TabIndex = 4;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(1029, 509);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSetAll
            // 
            btnSetAll.Location = new Point(674, 4);
            btnSetAll.Margin = new Padding(4, 3, 4, 3);
            btnSetAll.Name = "btnSetAll";
            btnSetAll.Size = new Size(144, 50);
            btnSetAll.TabIndex = 6;
            btnSetAll.Text = "Установить все гнезда";
            btnSetAll.UseVisualStyleBackColor = true;
            btnSetAll.Click += btnSetAll_Click;
            // 
            // lblSocketQuantity
            // 
            lblSocketQuantity.AutoSize = true;
            lblSocketQuantity.Location = new Point(363, 4);
            lblSocketQuantity.Margin = new Padding(4, 0, 4, 0);
            lblSocketQuantity.Name = "lblSocketQuantity";
            lblSocketQuantity.Size = new Size(13, 15);
            lblSocketQuantity.TabIndex = 7;
            lblSocketQuantity.Text = "0";
            // 
            // gbReadingParameters
            // 
            gbReadingParameters.Controls.Add(cbFrameLength);
            gbReadingParameters.Controls.Add(cbExposition);
            gbReadingParameters.Location = new Point(12, 59);
            gbReadingParameters.Name = "gbReadingParameters";
            gbReadingParameters.Size = new Size(219, 88);
            gbReadingParameters.TabIndex = 10;
            gbReadingParameters.TabStop = false;
            gbReadingParameters.Text = "Праметры чтения";
            // 
            // cbFrameLength
            // 
            cbFrameLength.AutoSize = true;
            cbFrameLength.Location = new Point(6, 47);
            cbFrameLength.Name = "cbFrameLength";
            cbFrameLength.Size = new Size(108, 19);
            cbFrameLength.TabIndex = 1;
            cbFrameLength.Text = "Длина фрейма";
            cbFrameLength.UseVisualStyleBackColor = true;
            // 
            // cbExposition
            // 
            cbExposition.AutoSize = true;
            cbExposition.Location = new Point(6, 22);
            cbExposition.Name = "cbExposition";
            cbExposition.Size = new Size(91, 19);
            cbExposition.TabIndex = 0;
            cbExposition.Text = "Экспозиция";
            cbExposition.UseVisualStyleBackColor = true;
            // 
            // gbCheckingParameters
            // 
            gbCheckingParameters.Controls.Add(cbDecisionOperations);
            gbCheckingParameters.Controls.Add(cbCheckRectRight);
            gbCheckingParameters.Controls.Add(lblCheckRegionCaption);
            gbCheckingParameters.Controls.Add(cbCheckRectLeft);
            gbCheckingParameters.Controls.Add(cbCheckRectBottom);
            gbCheckingParameters.Controls.Add(cbCheckRectTop);
            gbCheckingParameters.Controls.Add(panel1);
            gbCheckingParameters.Location = new Point(12, 153);
            gbCheckingParameters.Name = "gbCheckingParameters";
            gbCheckingParameters.Size = new Size(219, 228);
            gbCheckingParameters.TabIndex = 11;
            gbCheckingParameters.TabStop = false;
            gbCheckingParameters.Text = "Параметры проверки гнезда";
            // 
            // cbDecisionOperations
            // 
            cbDecisionOperations.AutoSize = true;
            cbDecisionOperations.Location = new Point(6, 22);
            cbDecisionOperations.Name = "cbDecisionOperations";
            cbDecisionOperations.Size = new Size(138, 19);
            cbDecisionOperations.TabIndex = 108;
            cbDecisionOperations.Text = "Операции проверки";
            cbDecisionOperations.UseVisualStyleBackColor = true;
            // 
            // cbCheckRectRight
            // 
            cbCheckRectRight.AutoSize = true;
            cbCheckRectRight.Location = new Point(133, 125);
            cbCheckRectRight.Name = "cbCheckRectRight";
            cbCheckRectRight.Size = new Size(61, 19);
            cbCheckRectRight.TabIndex = 107;
            cbCheckRectRight.Text = "Право";
            cbCheckRectRight.UseVisualStyleBackColor = true;
            // 
            // lblCheckRegionCaption
            // 
            lblCheckRegionCaption.AutoSize = true;
            lblCheckRegionCaption.Location = new Point(16, 58);
            lblCheckRegionCaption.Name = "lblCheckRegionCaption";
            lblCheckRegionCaption.Size = new Size(112, 15);
            lblCheckRegionCaption.TabIndex = 106;
            lblCheckRegionCaption.Text = "Область проверки:";
            // 
            // cbCheckRectLeft
            // 
            cbCheckRectLeft.AutoSize = true;
            cbCheckRectLeft.Location = new Point(6, 125);
            cbCheckRectLeft.Name = "cbCheckRectLeft";
            cbCheckRectLeft.Size = new Size(53, 19);
            cbCheckRectLeft.TabIndex = 105;
            cbCheckRectLeft.Text = "Лево";
            cbCheckRectLeft.UseVisualStyleBackColor = true;
            // 
            // cbCheckRectBottom
            // 
            cbCheckRectBottom.AutoSize = true;
            cbCheckRectBottom.Location = new Point(80, 173);
            cbCheckRectBottom.Name = "cbCheckRectBottom";
            cbCheckRectBottom.Size = new Size(47, 19);
            cbCheckRectBottom.TabIndex = 104;
            cbCheckRectBottom.Text = "Низ";
            cbCheckRectBottom.UseVisualStyleBackColor = true;
            // 
            // cbCheckRectTop
            // 
            cbCheckRectTop.AutoSize = true;
            cbCheckRectTop.Location = new Point(80, 87);
            cbCheckRectTop.Name = "cbCheckRectTop";
            cbCheckRectTop.Size = new Size(52, 19);
            cbCheckRectTop.TabIndex = 103;
            cbCheckRectTop.Text = "Верх";
            cbCheckRectTop.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(31, 94);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(127, 89);
            panel1.TabIndex = 101;
            // 
            // dgvCardNumbers
            // 
            dgvCardNumbers.AllowUserToAddRows = false;
            dgvCardNumbers.AllowUserToDeleteRows = false;
            dgvCardNumbers.AllowUserToResizeRows = false;
            dgvCardNumbers.BackgroundColor = SystemColors.Control;
            dgvCardNumbers.BorderStyle = BorderStyle.None;
            dgvCardNumbers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCardNumbers.Columns.AddRange(new DataGridViewColumn[] { dgvCardNumberText, dgvBtnOn, dgvBtnOff });
            dgvCardNumbers.Location = new Point(824, 59);
            dgvCardNumbers.MultiSelect = false;
            dgvCardNumbers.Name = "dgvCardNumbers";
            dgvCardNumbers.RowHeadersVisible = false;
            dgvCardNumbers.Size = new Size(293, 444);
            dgvCardNumbers.TabIndex = 9;
            dgvCardNumbers.CellContentClick += dgvCardNumbers_CellContentClick;
            // 
            // lblWhereToCopyCaption
            // 
            lblWhereToCopyCaption.AutoSize = true;
            lblWhereToCopyCaption.Location = new Point(238, 41);
            lblWhereToCopyCaption.Name = "lblWhereToCopyCaption";
            lblWhereToCopyCaption.Size = new Size(156, 15);
            lblWhereToCopyCaption.TabIndex = 12;
            lblWhereToCopyCaption.Text = "В какие гнезда копировать:";
            // 
            // lblWhatToCopyCaption
            // 
            lblWhatToCopyCaption.AutoSize = true;
            lblWhatToCopyCaption.Location = new Point(12, 41);
            lblWhatToCopyCaption.Name = "lblWhatToCopyCaption";
            lblWhatToCopyCaption.Size = new Size(174, 15);
            lblWhatToCopyCaption.TabIndex = 13;
            lblWhatToCopyCaption.Text = "Какие параметры копировать:";
            // 
            // DoMCSocketCopyParametersForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1130, 539);
            Controls.Add(lblWhatToCopyCaption);
            Controls.Add(lblWhereToCopyCaption);
            Controls.Add(gbCheckingParameters);
            Controls.Add(gbReadingParameters);
            Controls.Add(dgvCardNumbers);
            Controls.Add(lblSocketQuantity);
            Controls.Add(btnSetAll);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(bntReset);
            Controls.Add(pnlSockets);
            Controls.Add(lblSocketQuantityCaption);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DoMCSocketCopyParametersForm";
            Text = "Копирование параметров гнезда 1";
            gbReadingParameters.ResumeLayout(false);
            gbReadingParameters.PerformLayout();
            gbCheckingParameters.ResumeLayout(false);
            gbCheckingParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCardNumbers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblSocketQuantityCaption;
        private System.Windows.Forms.Panel pnlSockets;
        private System.Windows.Forms.Button bntReset;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSetAll;
        private System.Windows.Forms.Label lblSocketQuantity;
        private DataGridView dgvCardNumbers;
        private DataGridViewTextBoxColumn dgvCardNumberText;
        private DataGridViewButtonColumn dgvBtnOn;
        private DataGridViewButtonColumn dgvBtnOff;
        private GroupBox gbReadingParameters;
        private GroupBox gbCheckingParameters;
        private CheckBox cbExposition;
        private CheckBox cbFrameLength;
        private CheckBox cbCheckRectRight;
        private Label lblCheckRegionCaption;
        private CheckBox cbCheckRectLeft;
        private CheckBox cbCheckRectBottom;
        private CheckBox cbCheckRectTop;
        private Panel panel1;
        private CheckBox cbDecisionOperations;
        private Label lblWhereToCopyCaption;
        private Label lblWhatToCopyCaption;
    }
}