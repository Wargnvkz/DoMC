namespace Emulator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            clbEquipment = new CheckedListBox();
            btnCCDStart = new Button();
            listBoxLog = new ListBox();
            btnCCDStop = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // clbEquipment
            // 
            clbEquipment.FormattingEnabled = true;
            clbEquipment.Location = new Point(3, 1);
            clbEquipment.Name = "clbEquipment";
            clbEquipment.Size = new Size(213, 238);
            clbEquipment.TabIndex = 0;
            // 
            // btnCCDStart
            // 
            btnCCDStart.Location = new Point(3, 245);
            btnCCDStart.Name = "btnCCDStart";
            btnCCDStart.Size = new Size(88, 28);
            btnCCDStart.TabIndex = 1;
            btnCCDStart.Text = "Запуск CCD";
            btnCCDStart.UseVisualStyleBackColor = true;
            btnCCDStart.Click += btnCCDStart_Click;
            // 
            // listBoxLog
            // 
            listBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxLog.FormattingEnabled = true;
            listBoxLog.ItemHeight = 15;
            listBoxLog.Location = new Point(3, 279);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(795, 169);
            listBoxLog.TabIndex = 2;
            // 
            // btnCCDStop
            // 
            btnCCDStop.Location = new Point(97, 245);
            btnCCDStop.Name = "btnCCDStop";
            btnCCDStop.Size = new Size(119, 28);
            btnCCDStop.TabIndex = 3;
            btnCCDStop.Text = "Остановка CCD";
            btnCCDStop.UseVisualStyleBackColor = true;
            btnCCDStop.Click += btnCCDStop_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 15000;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCCDStop);
            Controls.Add(listBoxLog);
            Controls.Add(btnCCDStart);
            Controls.Add(clbEquipment);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox clbEquipment;
        private Button btnCCDStart;
        private ListBox listBoxLog;
        private Button btnCCDStop;
        private System.Windows.Forms.Timer timer1;
    }
}
