namespace DoMC.Forms
{
    partial class DoMCSocketOnOffForm
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
            dgvCardNumberText = new DataGridViewTextBoxColumn();
            dgvBtnOn = new DataGridViewButtonColumn();
            dgvBtnOff = new DataGridViewButtonColumn();
            label1 = new Label();
            pnlSockets = new Panel();
            bntReset = new Button();
            btnOK = new Button();
            btnCancel = new Button();
            btnSetAll = new Button();
            lblSocketQuantity = new Label();
            dgvCardNumbers = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvCardNumbers).BeginInit();
            SuspendLayout();
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
            dgvCardNumbers.Location = new Point(604, 58);
            dgvCardNumbers.MultiSelect = false;
            dgvCardNumbers.Name = "dgvCardNumbers";
            dgvCardNumbers.RowHeadersVisible = false;
            dgvCardNumbers.Size = new Size(293, 444);
            dgvCardNumbers.TabIndex = 9;
            dgvCardNumbers.CellContentClick += dgvCardNumbers_CellContentClick;
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 21);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(107, 15);
            label1.TabIndex = 0;
            label1.Text = "Количество гнезд:";
            // 
            // pnlSockets
            // 
            pnlSockets.Location = new Point(18, 58);
            pnlSockets.Margin = new Padding(4, 3, 4, 3);
            pnlSockets.Name = "pnlSockets";
            pnlSockets.Size = new Size(580, 444);
            pnlSockets.TabIndex = 2;
            // 
            // bntReset
            // 
            bntReset.Location = new Point(313, 3);
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
            btnOK.Location = new Point(14, 509);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 27);
            btnOK.TabIndex = 4;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(808, 509);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSetAll
            // 
            btnSetAll.Location = new Point(454, 3);
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
            lblSocketQuantity.Location = new Point(139, 21);
            lblSocketQuantity.Margin = new Padding(4, 0, 4, 0);
            lblSocketQuantity.Name = "lblSocketQuantity";
            lblSocketQuantity.Size = new Size(13, 15);
            lblSocketQuantity.TabIndex = 7;
            lblSocketQuantity.Text = "0";
            // 
            // DoMCSocketOnOffForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(909, 539);
            Controls.Add(dgvCardNumbers);
            Controls.Add(lblSocketQuantity);
            Controls.Add(btnSetAll);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(bntReset);
            Controls.Add(pnlSockets);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DoMCSocketOnOffForm";
            Text = "Сохранение изображений гнезд";
            ((System.ComponentModel.ISupportInitialize)dgvCardNumbers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
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
    }
}