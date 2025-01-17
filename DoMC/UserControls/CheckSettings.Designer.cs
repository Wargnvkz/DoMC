namespace DoMC.UserControls
{
    partial class CheckSettings
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
            btnCheckSettings = new Button();
            btnSettingsCheckCardStatus = new Button();
            lblSocketSettings = new Label();
            pnlSockets = new Panel();
            lblCaptionDoMCCards = new Label();
            lvDoMCCards = new ListView();
            chN = new ColumnHeader();
            chIsActive = new ColumnHeader();
            chIPAddress = new ColumnHeader();
            chListSockets = new ColumnHeader();
            SuspendLayout();
            // 
            // btnCheckSettings
            // 
            btnCheckSettings.Location = new Point(912, 569);
            btnCheckSettings.Margin = new Padding(3, 4, 3, 4);
            btnCheckSettings.Name = "btnCheckSettings";
            btnCheckSettings.Size = new Size(232, 63);
            btnCheckSettings.TabIndex = 13;
            btnCheckSettings.Text = "Проверить настройки";
            btnCheckSettings.UseVisualStyleBackColor = true;
            btnCheckSettings.Click += btnCheckSettings_Click;
            // 
            // btnSettingsCheckCardStatus
            // 
            btnSettingsCheckCardStatus.Location = new Point(8, 569);
            btnSettingsCheckCardStatus.Margin = new Padding(3, 4, 3, 4);
            btnSettingsCheckCardStatus.Name = "btnSettingsCheckCardStatus";
            btnSettingsCheckCardStatus.Size = new Size(308, 63);
            btnSettingsCheckCardStatus.TabIndex = 12;
            btnSettingsCheckCardStatus.Text = "Проверка работы плат";
            btnSettingsCheckCardStatus.UseVisualStyleBackColor = true;
            btnSettingsCheckCardStatus.Click += btnSettingsCheckCardStatus_Click;
            // 
            // lblSocketSettings
            // 
            lblSocketSettings.AutoSize = true;
            lblSocketSettings.Location = new Point(908, 0);
            lblSocketSettings.Margin = new Padding(4, 0, 4, 0);
            lblSocketSettings.Name = "lblSocketSettings";
            lblSocketSettings.Size = new Size(103, 15);
            lblSocketSettings.TabIndex = 11;
            lblSocketSettings.Text = "Параметры гнезд";
            // 
            // pnlSockets
            // 
            pnlSockets.Location = new Point(914, 33);
            pnlSockets.Margin = new Padding(4, 5, 4, 5);
            pnlSockets.Name = "pnlSockets";
            pnlSockets.Size = new Size(393, 528);
            pnlSockets.TabIndex = 10;
            // 
            // lblCaptionDoMCCards
            // 
            lblCaptionDoMCCards.AutoSize = true;
            lblCaptionDoMCCards.Location = new Point(4, 0);
            lblCaptionDoMCCards.Margin = new Padding(4, 0, 4, 0);
            lblCaptionDoMCCards.Name = "lblCaptionDoMCCards";
            lblCaptionDoMCCards.Size = new Size(165, 15);
            lblCaptionDoMCCards.TabIndex = 9;
            lblCaptionDoMCCards.Text = "Используемы платы чтения:";
            // 
            // lvDoMCCards
            // 
            lvDoMCCards.Columns.AddRange(new ColumnHeader[] { chN, chIsActive, chIPAddress, chListSockets });
            lvDoMCCards.Location = new Point(8, 33);
            lvDoMCCards.Margin = new Padding(4, 5, 4, 5);
            lvDoMCCards.Name = "lvDoMCCards";
            lvDoMCCards.Size = new Size(894, 526);
            lvDoMCCards.TabIndex = 8;
            lvDoMCCards.UseCompatibleStateImageBehavior = false;
            lvDoMCCards.View = View.Details;
            // 
            // chN
            // 
            chN.Text = "№ платы";
            chN.Width = 80;
            // 
            // chIsActive
            // 
            chIsActive.Text = "Активно";
            chIsActive.Width = 100;
            // 
            // chIPAddress
            // 
            chIPAddress.Text = "IP адрес";
            chIPAddress.Width = 160;
            // 
            // chListSockets
            // 
            chListSockets.Text = "Список гнезд";
            chListSockets.Width = 200;
            // 
            // CheckSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnCheckSettings);
            Controls.Add(btnSettingsCheckCardStatus);
            Controls.Add(lblSocketSettings);
            Controls.Add(pnlSockets);
            Controls.Add(lblCaptionDoMCCards);
            Controls.Add(lvDoMCCards);
            Name = "CheckSettings";
            Size = new Size(1397, 689);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCheckSettings;
        private Button btnSettingsCheckCardStatus;
        private Label lblSocketSettings;
        private Panel pnlSockets;
        private Label lblCaptionDoMCCards;
        private ListView lvDoMCCards;
        private ColumnHeader chN;
        private ColumnHeader chIsActive;
        private ColumnHeader chIPAddress;
        private ColumnHeader chListSockets;
    }
}
