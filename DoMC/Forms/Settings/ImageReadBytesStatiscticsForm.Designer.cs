namespace DoMCLib.Forms
{
    partial class ImageReadBytesStatiscticsForm
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
            this.lvImagesReadStatistics = new System.Windows.Forms.ListView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lvImagesReadStatistics
            // 
            this.lvImagesReadStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvImagesReadStatistics.HideSelection = false;
            this.lvImagesReadStatistics.Location = new System.Drawing.Point(1, 2);
            this.lvImagesReadStatistics.Name = "lvImagesReadStatistics";
            this.lvImagesReadStatistics.Size = new System.Drawing.Size(974, 389);
            this.lvImagesReadStatistics.TabIndex = 0;
            this.lvImagesReadStatistics.UseCompatibleStateImageBehavior = false;
            this.lvImagesReadStatistics.View = System.Windows.Forms.View.Details;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ImageReadBytesStatiscticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 392);
            this.Controls.Add(this.lvImagesReadStatistics);
            this.Name = "ImageReadBytesStatiscticsForm";
            this.Text = "Статистика чтения изображений";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvImagesReadStatistics;
        private System.Windows.Forms.Timer timer1;
    }
}