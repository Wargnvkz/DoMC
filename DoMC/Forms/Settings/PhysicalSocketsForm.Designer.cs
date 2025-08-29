namespace DoMCLib.Forms
{
    partial class PhysicalSocketsForm
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
            dgvSockets = new DataGridView();
            btnOK = new Button();
            btnCancel = new Button();
            epSockets = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvSockets).BeginInit();
            ((System.ComponentModel.ISupportInitialize)epSockets).BeginInit();
            SuspendLayout();
            // 
            // dgvSockets
            // 
            dgvSockets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSockets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSockets.Location = new Point(0, 0);
            dgvSockets.Margin = new Padding(4, 5, 4, 5);
            dgvSockets.Name = "dgvSockets";
            dgvSockets.Size = new Size(1073, 357);
            dgvSockets.TabIndex = 0;
            dgvSockets.CellDoubleClick += dgvSockets_CellDoubleClick;
            dgvSockets.CellFormatting += dgvSockets_CellFormatting;
            dgvSockets.Validating += dgvSockets_Validating;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(12, 365);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(84, 31);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(977, 365);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(84, 31);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // epSockets
            // 
            epSockets.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
            epSockets.ContainerControl = this;
            // 
            // PhysicalSocketsForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1073, 408);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(dgvSockets);
            Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4, 5, 4, 5);
            Name = "PhysicalSocketsForm";
            Text = "PhysicalSocketsForm";
            ((System.ComponentModel.ISupportInitialize)dgvSockets).EndInit();
            ((System.ComponentModel.ISupportInitialize)epSockets).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSockets;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider epSockets;
    }
}