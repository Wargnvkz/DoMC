namespace DoMCLib.Forms
{
    partial class DoMCAdditionalParametersForm
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
            this.lblAverageToHaveImage = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txbAverageImage = new System.Windows.Forms.TextBox();
            this.cbLogPackets = new System.Windows.Forms.CheckBox();
            this.cbRegisterEmptyImages = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblAverageToHaveImage
            // 
            this.lblAverageToHaveImage.AutoSize = true;
            this.lblAverageToHaveImage.Location = new System.Drawing.Point(13, 9);
            this.lblAverageToHaveImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAverageToHaveImage.Name = "lblAverageToHaveImage";
            this.lblAverageToHaveImage.Size = new System.Drawing.Size(259, 20);
            this.lblAverageToHaveImage.TabIndex = 0;
            this.lblAverageToHaveImage.Text = "Среднее значение изображения:";
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Среднее значение изображения, чтобы считать, что изображение прочитано";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(12, 133);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(557, 133);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txbAverageImage
            // 
            this.txbAverageImage.Location = new System.Drawing.Point(279, 6);
            this.txbAverageImage.Name = "txbAverageImage";
            this.txbAverageImage.Size = new System.Drawing.Size(122, 26);
            this.txbAverageImage.TabIndex = 3;
            // 
            // cbLogPackets
            // 
            this.cbLogPackets.AutoSize = true;
            this.cbLogPackets.Location = new System.Drawing.Point(17, 38);
            this.cbLogPackets.Name = "cbLogPackets";
            this.cbLogPackets.Size = new System.Drawing.Size(447, 24);
            this.cbLogPackets.TabIndex = 5;
            this.cbLogPackets.Text = "Записывать все принятые сетевые пакеты в лог-файл";
            this.cbLogPackets.UseVisualStyleBackColor = true;
            // 
            // cbRegisterEmptyImages
            // 
            this.cbRegisterEmptyImages.AutoSize = true;
            this.cbRegisterEmptyImages.Location = new System.Drawing.Point(17, 68);
            this.cbRegisterEmptyImages.Name = "cbRegisterEmptyImages";
            this.cbRegisterEmptyImages.Size = new System.Drawing.Size(424, 24);
            this.cbRegisterEmptyImages.TabIndex = 6;
            this.cbRegisterEmptyImages.Text = "Отмечать непрочитанные изображения, как ошибку";
            this.cbRegisterEmptyImages.UseVisualStyleBackColor = true;
            // 
            // DoMCAdditionalParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 180);
            this.Controls.Add(this.cbRegisterEmptyImages);
            this.Controls.Add(this.cbLogPackets);
            this.Controls.Add(this.txbAverageImage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblAverageToHaveImage);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DoMCAdditionalParametersForm";
            this.Text = "DoMCAdditionalParametersForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAverageToHaveImage;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txbAverageImage;
        private System.Windows.Forms.CheckBox cbLogPackets;
        private System.Windows.Forms.CheckBox cbRegisterEmptyImages;
    }
}