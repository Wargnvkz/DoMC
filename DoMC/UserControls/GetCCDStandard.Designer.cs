using System.Configuration;

namespace DoMC.UserControls
{
    partial class GetCCDStandardInterface
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnGetAllStandards = new Button();
            btnReadImagesToGetStandardForOneSocket = new Button();
            lblStandardSocketNumber = new Label();
            lblSocketNumberText = new Label();
            btnMakeAverage = new Button();
            lblImageStandardText = new Label();
            label6 = new Label();
            label5 = new Label();
            lblImage1Text = new Label();
            pbAverage = new PictureBox();
            pbStandard3 = new PictureBox();
            pbStandard2 = new PictureBox();
            pbStandard1 = new PictureBox();
            cbExternalSignalForStandard = new CheckBox();
            pnlGetStandardSockets = new Panel();
            tmrRenew = new System.Windows.Forms.Timer(components);
            lblProgressCaption = new Label();
            statusStrip1 = new StatusStrip();
            pbGettingStandard = new ToolStripProgressBar();
            lblWorkStatus = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)pbAverage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnGetAllStandards
            // 
            btnGetAllStandards.Location = new Point(407, 51);
            btnGetAllStandards.Margin = new Padding(5);
            btnGetAllStandards.Name = "btnGetAllStandards";
            btnGetAllStandards.Size = new Size(336, 38);
            btnGetAllStandards.TabIndex = 61;
            btnGetAllStandards.Text = "Получение эталонов по всем гнездам";
            btnGetAllStandards.UseVisualStyleBackColor = true;
            btnGetAllStandards.Click += btnGetAllStandards_Click;
            // 
            // btnReadImagesToGetStandardForOneSocket
            // 
            btnReadImagesToGetStandardForOneSocket.Location = new Point(411, 172);
            btnReadImagesToGetStandardForOneSocket.Margin = new Padding(5);
            btnReadImagesToGetStandardForOneSocket.Name = "btnReadImagesToGetStandardForOneSocket";
            btnReadImagesToGetStandardForOneSocket.Size = new Size(332, 38);
            btnReadImagesToGetStandardForOneSocket.TabIndex = 54;
            btnReadImagesToGetStandardForOneSocket.Text = "Чтение изображений выбранного гнезда";
            btnReadImagesToGetStandardForOneSocket.UseVisualStyleBackColor = true;
            btnReadImagesToGetStandardForOneSocket.Click += btnReadImagesToGetStandardForOneSocket_Click;
            // 
            // lblStandardSocketNumber
            // 
            lblStandardSocketNumber.AutoSize = true;
            lblStandardSocketNumber.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblStandardSocketNumber.Location = new Point(617, 137);
            lblStandardSocketNumber.Margin = new Padding(5, 0, 5, 0);
            lblStandardSocketNumber.Name = "lblStandardSocketNumber";
            lblStandardSocketNumber.Size = new Size(14, 17);
            lblStandardSocketNumber.TabIndex = 52;
            lblStandardSocketNumber.Text = "-";
            // 
            // lblSocketNumberText
            // 
            lblSocketNumberText.AutoSize = true;
            lblSocketNumberText.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblSocketNumberText.Location = new Point(407, 137);
            lblSocketNumberText.Margin = new Padding(5, 0, 5, 0);
            lblSocketNumberText.Name = "lblSocketNumberText";
            lblSocketNumberText.Size = new Size(134, 17);
            lblSocketNumberText.TabIndex = 50;
            lblSocketNumberText.Text = "Выбрано гнездо:";
            // 
            // btnMakeAverage
            // 
            btnMakeAverage.Location = new Point(753, 470);
            btnMakeAverage.Margin = new Padding(5);
            btnMakeAverage.Name = "btnMakeAverage";
            btnMakeAverage.Size = new Size(122, 38);
            btnMakeAverage.TabIndex = 49;
            btnMakeAverage.Text = "Усреднить 🡺";
            btnMakeAverage.UseVisualStyleBackColor = true;
            btnMakeAverage.Click += btnMakeAverage_Click;
            // 
            // lblImageStandardText
            // 
            lblImageStandardText.AutoSize = true;
            lblImageStandardText.Location = new Point(880, 297);
            lblImageStandardText.Margin = new Padding(5, 0, 5, 0);
            lblImageStandardText.Name = "lblImageStandardText";
            lblImageStandardText.Size = new Size(123, 15);
            lblImageStandardText.TabIndex = 47;
            lblImageStandardText.Text = "Усредненный эталон";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(407, 634);
            label6.Margin = new Padding(5, 0, 5, 0);
            label6.Name = "label6";
            label6.Size = new Size(92, 15);
            label6.TabIndex = 46;
            label6.Text = "Изображение 3";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(407, 427);
            label5.Margin = new Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new Size(92, 15);
            label5.TabIndex = 45;
            label5.Text = "Изображение 2";
            // 
            // lblImage1Text
            // 
            lblImage1Text.AutoSize = true;
            lblImage1Text.Location = new Point(407, 226);
            lblImage1Text.Margin = new Padding(5, 0, 5, 0);
            lblImage1Text.Name = "lblImage1Text";
            lblImage1Text.Size = new Size(92, 15);
            lblImage1Text.TabIndex = 44;
            lblImage1Text.Text = "Изображение 1";
            // 
            // pbAverage
            // 
            pbAverage.BorderStyle = BorderStyle.FixedSingle;
            pbAverage.Location = new Point(885, 327);
            pbAverage.Margin = new Padding(5);
            pbAverage.Name = "pbAverage";
            pbAverage.Size = new Size(383, 393);
            pbAverage.SizeMode = PictureBoxSizeMode.Zoom;
            pbAverage.TabIndex = 42;
            pbAverage.TabStop = false;
            // 
            // pbStandard3
            // 
            pbStandard3.BorderStyle = BorderStyle.FixedSingle;
            pbStandard3.Location = new Point(552, 632);
            pbStandard3.Margin = new Padding(5);
            pbStandard3.Name = "pbStandard3";
            pbStandard3.Size = new Size(191, 196);
            pbStandard3.SizeMode = PictureBoxSizeMode.Zoom;
            pbStandard3.TabIndex = 41;
            pbStandard3.TabStop = false;
            // 
            // pbStandard2
            // 
            pbStandard2.BorderStyle = BorderStyle.FixedSingle;
            pbStandard2.Location = new Point(552, 425);
            pbStandard2.Margin = new Padding(5);
            pbStandard2.Name = "pbStandard2";
            pbStandard2.Size = new Size(191, 196);
            pbStandard2.SizeMode = PictureBoxSizeMode.Zoom;
            pbStandard2.TabIndex = 40;
            pbStandard2.TabStop = false;
            // 
            // pbStandard1
            // 
            pbStandard1.BorderStyle = BorderStyle.FixedSingle;
            pbStandard1.Location = new Point(552, 220);
            pbStandard1.Margin = new Padding(5);
            pbStandard1.Name = "pbStandard1";
            pbStandard1.Size = new Size(191, 196);
            pbStandard1.SizeMode = PictureBoxSizeMode.Zoom;
            pbStandard1.TabIndex = 39;
            pbStandard1.TabStop = false;
            // 
            // cbExternalSignalForStandard
            // 
            cbExternalSignalForStandard.AutoSize = true;
            cbExternalSignalForStandard.Location = new Point(411, 9);
            cbExternalSignalForStandard.Margin = new Padding(5);
            cbExternalSignalForStandard.Name = "cbExternalSignalForStandard";
            cbExternalSignalForStandard.Size = new Size(188, 19);
            cbExternalSignalForStandard.TabIndex = 38;
            cbExternalSignalForStandard.Text = "Читать по внешнему сигналу";
            cbExternalSignalForStandard.UseVisualStyleBackColor = true;
            cbExternalSignalForStandard.CheckedChanged += cbExternalSignalForStandard_CheckedChanged;
            // 
            // pnlGetStandardSockets
            // 
            pnlGetStandardSockets.Location = new Point(5, 5);
            pnlGetStandardSockets.Margin = new Padding(5);
            pnlGetStandardSockets.Name = "pnlGetStandardSockets";
            pnlGetStandardSockets.Size = new Size(393, 503);
            pnlGetStandardSockets.TabIndex = 35;
            // 
            // tmrRenew
            // 
            tmrRenew.Enabled = true;
            tmrRenew.Interval = 300;
            tmrRenew.Tick += tmrRenew_Tick;
            // 
            // lblProgressCaption
            // 
            lblProgressCaption.AutoSize = true;
            lblProgressCaption.Location = new Point(5, 513);
            lblProgressCaption.Name = "lblProgressCaption";
            lblProgressCaption.Size = new Size(103, 15);
            lblProgressCaption.TabIndex = 63;
            lblProgressCaption.Text = "Чтение эталонов:";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { pbGettingStandard, lblWorkStatus });
            statusStrip1.Location = new Point(0, 837);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1444, 22);
            statusStrip1.TabIndex = 64;
            statusStrip1.Text = "statusStrip1";
            // 
            // pbGettingStandard
            // 
            pbGettingStandard.Name = "pbGettingStandard";
            pbGettingStandard.Size = new Size(150, 16);
            // 
            // lblWorkStatus
            // 
            lblWorkStatus.Name = "lblWorkStatus";
            lblWorkStatus.Size = new Size(12, 17);
            lblWorkStatus.Text = "-";
            // 
            // GetCCDStandardInterface
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(statusStrip1);
            Controls.Add(lblProgressCaption);
            Controls.Add(btnGetAllStandards);
            Controls.Add(btnReadImagesToGetStandardForOneSocket);
            Controls.Add(lblStandardSocketNumber);
            Controls.Add(lblSocketNumberText);
            Controls.Add(btnMakeAverage);
            Controls.Add(lblImageStandardText);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(lblImage1Text);
            Controls.Add(pbAverage);
            Controls.Add(pbStandard3);
            Controls.Add(pbStandard2);
            Controls.Add(pbStandard1);
            Controls.Add(cbExternalSignalForStandard);
            Controls.Add(pnlGetStandardSockets);
            Name = "GetCCDStandardInterface";
            Size = new Size(1444, 859);
            ((System.ComponentModel.ISupportInitialize)pbAverage).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStandard1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnGetAllStandards;
        private Button btnReadImagesToGetStandardForOneSocket;
        private Label lblStandardSocketNumber;
        private Label lblSocketNumberText;
        private Button btnMakeAverage;
        private Label lblImageStandardText;
        private Label label6;
        private Label label5;
        private Label lblImage1Text;
        private PictureBox pbAverage;
        private PictureBox pbStandard3;
        private PictureBox pbStandard2;
        private PictureBox pbStandard1;
        private CheckBox cbExternalSignalForStandard;
        private Panel pnlGetStandardSockets;
        private System.Windows.Forms.Timer tmrRenew;
        private Label lblProgressCaption;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar pbGettingStandard;
        private ToolStripStatusLabel lblWorkStatus;
    }
}
