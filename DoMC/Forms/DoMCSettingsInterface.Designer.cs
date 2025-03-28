namespace DoMC
{
    partial class DoMCSettingsInterface
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            miSettings = new ToolStripMenuItem();
            miLEDSettings = new ToolStripMenuItem();
            miReadParameters = new ToolStripMenuItem();
            miStandardRecalcSetting = new ToolStripMenuItem();
            miSetCheckSockets = new ToolStripMenuItem();
            miDBSettings = new ToolStripMenuItem();
            miRDPSettings = new ToolStripMenuItem();
            miPhysicToDisplaySocket = new ToolStripMenuItem();
            дополнительныеПараметрыToolStripMenuItem = new ToolStripMenuItem();
            эталонToolStripMenuItem = new ToolStripMenuItem();
            miSaveStandard = new ToolStripMenuItem();
            miLoadStandard = new ToolStripMenuItem();
            конфигурацияToolStripMenuItem = new ToolStripMenuItem();
            сохранитьToolStripMenuItem = new ToolStripMenuItem();
            загрузитьToolStripMenuItem = new ToolStripMenuItem();
            tsmiLogsArchive = new ToolStripMenuItem();
            tsmiTechnicalData = new ToolStripMenuItem();
            tsmiReadImageStatistics = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tbSettingsCheck = new TabPage();
            tbGetStandard = new TabPage();
            tbTestLCB = new TabPage();
            tbCCDTest = new TabPage();
            tbShowPreformImages = new TabPage();
            tbTestRDPB_uc = new TabPage();
            tbDB = new TabPage();
            btnMoveToArchive = new Button();
            tbArchive = new TabPage();
            menuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tbDB.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.ControlDark;
            menuStrip1.Font = new Font("Segoe UI", 14F);
            menuStrip1.Items.AddRange(new ToolStripItem[] { miSettings, эталонToolStripMenuItem, конфигурацияToolStripMenuItem, tsmiLogsArchive, tsmiTechnicalData });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(9, 4, 0, 4);
            menuStrip1.Size = new Size(1820, 37);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // miSettings
            // 
            miSettings.DropDownItems.AddRange(new ToolStripItem[] { miLEDSettings, miReadParameters, miStandardRecalcSetting, miSetCheckSockets, miDBSettings, miRDPSettings, miPhysicToDisplaySocket, дополнительныеПараметрыToolStripMenuItem });
            miSettings.Name = "miSettings";
            miSettings.Size = new Size(117, 29);
            miSettings.Text = "Настройки";
            // 
            // miLEDSettings
            // 
            miLEDSettings.Checked = true;
            miLEDSettings.CheckState = CheckState.Checked;
            miLEDSettings.Name = "miLEDSettings";
            miLEDSettings.Size = new Size(535, 30);
            miLEDSettings.Text = "Настройка режима работы БУС...";
            miLEDSettings.Click += miLEDSettings_Click;
            // 
            // miReadParameters
            // 
            miReadParameters.Name = "miReadParameters";
            miReadParameters.Size = new Size(535, 30);
            miReadParameters.Text = "Настройки параметров чтения гнезд платами ПЗС...";
            miReadParameters.Click += miReadParameters_Click;
            // 
            // miStandardRecalcSetting
            // 
            miStandardRecalcSetting.Name = "miStandardRecalcSetting";
            miStandardRecalcSetting.Size = new Size(535, 30);
            miStandardRecalcSetting.Text = "Настройки обновления эталона...";
            miStandardRecalcSetting.Click += miWorkModeSettings_Click;
            // 
            // miSetCheckSockets
            // 
            miSetCheckSockets.Name = "miSetCheckSockets";
            miSetCheckSockets.Size = new Size(535, 30);
            miSetCheckSockets.Text = "Включение проверки гнезд...";
            miSetCheckSockets.Click += miSetCheckSockets_Click;
            // 
            // miDBSettings
            // 
            miDBSettings.Name = "miDBSettings";
            miDBSettings.Size = new Size(535, 30);
            miDBSettings.Text = "Настройка БД...";
            miDBSettings.Click += miDBSettings_Click;
            // 
            // miRDPSettings
            // 
            miRDPSettings.Name = "miRDPSettings";
            miRDPSettings.Size = new Size(535, 30);
            miRDPSettings.Text = "Параметры бракера...";
            miRDPSettings.Click += miRDPSettings_Click;
            // 
            // miPhysicToDisplaySocket
            // 
            miPhysicToDisplaySocket.Name = "miPhysicToDisplaySocket";
            miPhysicToDisplaySocket.Size = new Size(535, 30);
            miPhysicToDisplaySocket.Text = "Соответствие отображаемых и физических гнезд...";
            miPhysicToDisplaySocket.Click += соответствиеГнездToolStripMenuItem_Click;
            // 
            // дополнительныеПараметрыToolStripMenuItem
            // 
            дополнительныеПараметрыToolStripMenuItem.Name = "дополнительныеПараметрыToolStripMenuItem";
            дополнительныеПараметрыToolStripMenuItem.Size = new Size(535, 30);
            дополнительныеПараметрыToolStripMenuItem.Text = "Дополнительные параметры...";
            дополнительныеПараметрыToolStripMenuItem.Click += дополнительныеПараметрыToolStripMenuItem_Click;
            // 
            // эталонToolStripMenuItem
            // 
            эталонToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { miSaveStandard, miLoadStandard });
            эталонToolStripMenuItem.Name = "эталонToolStripMenuItem";
            эталонToolStripMenuItem.Size = new Size(86, 29);
            эталонToolStripMenuItem.Text = "Эталон";
            // 
            // miSaveStandard
            // 
            miSaveStandard.Name = "miSaveStandard";
            miSaveStandard.Size = new Size(189, 30);
            miSaveStandard.Text = "Сохранить...";
            miSaveStandard.Click += miSaveStandard_Click;
            // 
            // miLoadStandard
            // 
            miLoadStandard.Name = "miLoadStandard";
            miLoadStandard.Size = new Size(189, 30);
            miLoadStandard.Text = "Загрузить...";
            miLoadStandard.Click += miLoadStandard_Click;
            // 
            // конфигурацияToolStripMenuItem
            // 
            конфигурацияToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { сохранитьToolStripMenuItem, загрузитьToolStripMenuItem });
            конфигурацияToolStripMenuItem.Name = "конфигурацияToolStripMenuItem";
            конфигурацияToolStripMenuItem.Size = new Size(150, 29);
            конфигурацияToolStripMenuItem.Text = "Конфигурация";
            // 
            // сохранитьToolStripMenuItem
            // 
            сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            сохранитьToolStripMenuItem.Size = new Size(189, 30);
            сохранитьToolStripMenuItem.Text = "Сохранить...";
            сохранитьToolStripMenuItem.Click += сохранитьToolStripMenuItem_Click;
            // 
            // загрузитьToolStripMenuItem
            // 
            загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            загрузитьToolStripMenuItem.Size = new Size(189, 30);
            загрузитьToolStripMenuItem.Text = "Загрузить...";
            загрузитьToolStripMenuItem.Click += загрузитьToolStripMenuItem_Click;
            // 
            // tsmiLogsArchive
            // 
            tsmiLogsArchive.Name = "tsmiLogsArchive";
            tsmiLogsArchive.Size = new Size(169, 29);
            tsmiLogsArchive.Text = "Папка журналов";
            tsmiLogsArchive.Click += tsmiLogsArchive_Click;
            // 
            // tsmiTechnicalData
            // 
            tsmiTechnicalData.DropDownItems.AddRange(new ToolStripItem[] { tsmiReadImageStatistics });
            tsmiTechnicalData.Name = "tsmiTechnicalData";
            tsmiTechnicalData.Size = new Size(205, 29);
            tsmiTechnicalData.Text = "Технические данные";
            // 
            // tsmiReadImageStatistics
            // 
            tsmiReadImageStatistics.Name = "tsmiReadImageStatistics";
            tsmiReadImageStatistics.Size = new Size(430, 30);
            tsmiReadImageStatistics.Text = "Окно статистики чтения изображений...";
            tsmiReadImageStatistics.Visible = false;
            tsmiReadImageStatistics.Click += tsmiReadImageStatistics_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tbSettingsCheck);
            tabControl1.Controls.Add(tbGetStandard);
            tabControl1.Controls.Add(tbCCDTest);
            tabControl1.Controls.Add(tbTestLCB);
            tabControl1.Controls.Add(tbShowPreformImages);
            tabControl1.Controls.Add(tbTestRDPB_uc);
            tabControl1.Controls.Add(tbDB);
            tabControl1.Controls.Add(tbArchive);
            tabControl1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tabControl1.Location = new Point(0, 43);
            tabControl1.Margin = new Padding(4, 5, 4, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1816, 509);
            tabControl1.TabIndex = 2;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tbSettingsCheck
            // 
            tbSettingsCheck.Location = new Point(4, 29);
            tbSettingsCheck.Name = "tbSettingsCheck";
            tbSettingsCheck.Size = new Size(1808, 476);
            tbSettingsCheck.TabIndex = 12;
            tbSettingsCheck.Text = "Проверка настроек";
            tbSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // tbGetStandard
            // 
            tbGetStandard.Location = new Point(4, 29);
            tbGetStandard.Margin = new Padding(3, 4, 3, 4);
            tbGetStandard.Name = "tbGetStandard";
            tbGetStandard.Size = new Size(1808, 476);
            tbGetStandard.TabIndex = 10;
            tbGetStandard.Text = "Получение эталона";
            tbGetStandard.UseVisualStyleBackColor = true;
            // 
            // tbTestLCB
            // 
            tbTestLCB.Location = new Point(4, 29);
            tbTestLCB.Margin = new Padding(3, 4, 3, 4);
            tbTestLCB.Name = "tbTestLCB";
            tbTestLCB.Size = new Size(1808, 476);
            tbTestLCB.TabIndex = 11;
            tbTestLCB.Text = "Тест БУС";
            tbTestLCB.UseVisualStyleBackColor = true;
            // 
            // tbCCDTest
            // 
            tbCCDTest.Location = new Point(4, 29);
            tbCCDTest.Margin = new Padding(3, 4, 3, 4);
            tbCCDTest.Name = "tbCCDTest";
            tbCCDTest.Size = new Size(1808, 476);
            tbCCDTest.TabIndex = 9;
            tbCCDTest.Text = "Тест ПЗС";
            tbCCDTest.UseVisualStyleBackColor = true;
            // 
            // tbShowPreformImages
            // 
            tbShowPreformImages.Font = new Font("Segoe UI", 9F);
            tbShowPreformImages.Location = new Point(4, 29);
            tbShowPreformImages.Margin = new Padding(4, 5, 4, 5);
            tbShowPreformImages.Name = "tbShowPreformImages";
            tbShowPreformImages.Size = new Size(1808, 476);
            tbShowPreformImages.TabIndex = 5;
            tbShowPreformImages.Text = "Тест изображений";
            tbShowPreformImages.UseVisualStyleBackColor = true;
            // 
            // tbTestRDPB_uc
            // 
            tbTestRDPB_uc.Location = new Point(4, 29);
            tbTestRDPB_uc.Name = "tbTestRDPB_uc";
            tbTestRDPB_uc.Size = new Size(1808, 476);
            tbTestRDPB_uc.TabIndex = 13;
            tbTestRDPB_uc.Text = "Тест бракера";
            tbTestRDPB_uc.UseVisualStyleBackColor = true;
            // 
            // tbDB
            // 
            tbDB.Controls.Add(btnMoveToArchive);
            tbDB.Location = new Point(4, 29);
            tbDB.Margin = new Padding(4, 5, 4, 5);
            tbDB.Name = "tbDB";
            tbDB.Size = new Size(1808, 476);
            tbDB.TabIndex = 7;
            tbDB.Text = "База данных";
            tbDB.UseVisualStyleBackColor = true;
            // 
            // btnMoveToArchive
            // 
            btnMoveToArchive.Location = new Point(9, 5);
            btnMoveToArchive.Margin = new Padding(4, 5, 4, 5);
            btnMoveToArchive.Name = "btnMoveToArchive";
            btnMoveToArchive.Size = new Size(290, 97);
            btnMoveToArchive.TabIndex = 4;
            btnMoveToArchive.Text = "Переместить данные в архив";
            btnMoveToArchive.UseVisualStyleBackColor = true;
            btnMoveToArchive.Click += btnMoveToArchive_Click;
            // 
            // tbArchive
            // 
            tbArchive.Location = new Point(4, 29);
            tbArchive.Margin = new Padding(3, 4, 3, 4);
            tbArchive.Name = "tbArchive";
            tbArchive.Size = new Size(1808, 476);
            tbArchive.TabIndex = 8;
            tbArchive.Text = "Архив съемов";
            tbArchive.UseVisualStyleBackColor = true;
            // 
            // DoMCSettingsInterface
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1820, 553);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            Font = new Font("Segoe UI", 12F);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(918, 592);
            Name = "DoMCSettingsInterface";
            Text = "Управление ПМК";
            FormClosed += DoMCMainInterface_FormClosed;
            Load += DoMCMainInterface_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tbDB.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miReadParameters;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem эталонToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miSaveStandard;
        private System.Windows.Forms.ToolStripMenuItem miLoadStandard;
        private System.Windows.Forms.ToolStripMenuItem miStandardRecalcSetting;
        private System.Windows.Forms.ToolStripMenuItem miLEDSettings;
        private System.Windows.Forms.TabPage tbShowPreformImages;
        private System.Windows.Forms.ToolStripMenuItem miSetCheckSockets;
        private System.Windows.Forms.ToolStripMenuItem miDBSettings;
        private System.Windows.Forms.ToolStripMenuItem miRDPSettings;
        private System.Windows.Forms.TabPage tbDB;
        private System.Windows.Forms.ToolStripMenuItem miPhysicToDisplaySocket;
        private System.Windows.Forms.ToolStripMenuItem конфигурацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogsArchive;
        private System.Windows.Forms.Button btnMoveToArchive;
        private System.Windows.Forms.ToolStripMenuItem дополнительныеПараметрыToolStripMenuItem;
        private System.Windows.Forms.TabPage tbArchive;
        private System.Windows.Forms.ToolStripMenuItem tsmiReadImageStatistics;
        private System.Windows.Forms.ToolStripMenuItem tsmiTechnicalData;
        private TabPage tbCCDTest;
        private TabPage tbGetStandard;
        private TabPage tbTestLCB;
        private TabPage tbSettingsCheck;
        private TabPage tbTestRDPB_uc;
    }
}

