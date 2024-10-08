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
            this.components = new System.ComponentModel.Container();
            this.dgvSockets = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.epSockets = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSockets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epSockets)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSockets
            // 
            this.dgvSockets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSockets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSockets.Location = new System.Drawing.Point(0, 0);
            this.dgvSockets.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvSockets.Name = "dgvSockets";
            this.dgvSockets.Size = new System.Drawing.Size(1073, 357);
            this.dgvSockets.TabIndex = 0;
            this.dgvSockets.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSockets_CellFormatting);
            this.dgvSockets.Validating += new System.ComponentModel.CancelEventHandler(this.dgvSockets_Validating);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(12, 365);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 31);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(977, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 31);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // epSockets
            // 
            this.epSockets.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.epSockets.ContainerControl = this;
            // 
            // PhysicalSocketsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 408);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvSockets);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PhysicalSocketsForm";
            this.Text = "PhysicalSocketsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSockets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epSockets)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSockets;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider epSockets;
    }
}