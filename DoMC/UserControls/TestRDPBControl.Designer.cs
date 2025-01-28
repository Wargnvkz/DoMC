namespace DoMC.UserControls
{
    partial class TestRDPBControl
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
            lblTestRDPBStatus = new Label();
            lvTestRDPBStatuses = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            btnTestRDPBSendCommand = new Button();
            txbTestRDPBManualCommand = new TextBox();
            lblTestRDPBCommand = new Label();
            txbTestRDPBCoolingBlocksStatus = new TextBox();
            cbTestRDPBCoolingBlocksQuantity = new ComboBox();
            lblCoolingBlockQuantity = new Label();
            btnTestRDPBN90 = new Button();
            btnTestRDPBN83 = new Button();
            btnTestRDPBN82 = new Button();
            btnTestRDPBN81 = new Button();
            btnTestRDPBN80 = new Button();
            btnRDPBTestConnect = new Button();
            SuspendLayout();
            // 
            // lblTestRDPBStatus
            // 
            lblTestRDPBStatus.AutoSize = true;
            lblTestRDPBStatus.Location = new Point(658, 25);
            lblTestRDPBStatus.Margin = new Padding(4, 0, 4, 0);
            lblTestRDPBStatus.Name = "lblTestRDPBStatus";
            lblTestRDPBStatus.Size = new Size(103, 15);
            lblTestRDPBStatus.TabIndex = 27;
            lblTestRDPBStatus.Text = "Статусы бракера:";
            // 
            // lvTestRDPBStatuses
            // 
            lvTestRDPBStatuses.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            lvTestRDPBStatuses.Location = new Point(662, 58);
            lvTestRDPBStatuses.Margin = new Padding(4, 5, 4, 5);
            lvTestRDPBStatuses.Name = "lvTestRDPBStatuses";
            lvTestRDPBStatuses.Size = new Size(708, 437);
            lvTestRDPBStatuses.TabIndex = 26;
            lvTestRDPBStatuses.UseCompatibleStateImageBehavior = false;
            lvTestRDPBStatuses.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Параметр";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Значение";
            columnHeader2.Width = 289;
            // 
            // btnTestRDPBSendCommand
            // 
            btnTestRDPBSendCommand.Location = new Point(424, 618);
            btnTestRDPBSendCommand.Margin = new Padding(4, 5, 4, 5);
            btnTestRDPBSendCommand.Name = "btnTestRDPBSendCommand";
            btnTestRDPBSendCommand.Size = new Size(123, 51);
            btnTestRDPBSendCommand.TabIndex = 25;
            btnTestRDPBSendCommand.Text = "Послать";
            btnTestRDPBSendCommand.UseVisualStyleBackColor = true;
            btnTestRDPBSendCommand.Click += btnTestRDPBSendCommand_Click;
            // 
            // txbTestRDPBManualCommand
            // 
            txbTestRDPBManualCommand.Location = new Point(4, 625);
            txbTestRDPBManualCommand.Margin = new Padding(4, 5, 4, 5);
            txbTestRDPBManualCommand.Name = "txbTestRDPBManualCommand";
            txbTestRDPBManualCommand.Size = new Size(409, 23);
            txbTestRDPBManualCommand.TabIndex = 24;
            // 
            // lblTestRDPBCommand
            // 
            lblTestRDPBCommand.AutoSize = true;
            lblTestRDPBCommand.Location = new Point(4, 593);
            lblTestRDPBCommand.Margin = new Padding(4, 0, 4, 0);
            lblTestRDPBCommand.Name = "lblTestRDPBCommand";
            lblTestRDPBCommand.Size = new Size(106, 15);
            lblTestRDPBCommand.TabIndex = 23;
            lblTestRDPBCommand.Text = "Команда в бракер";
            // 
            // txbTestRDPBCoolingBlocksStatus
            // 
            txbTestRDPBCoolingBlocksStatus.BackColor = SystemColors.Control;
            txbTestRDPBCoolingBlocksStatus.Enabled = false;
            txbTestRDPBCoolingBlocksStatus.Location = new Point(138, 518);
            txbTestRDPBCoolingBlocksStatus.Margin = new Padding(4, 5, 4, 5);
            txbTestRDPBCoolingBlocksStatus.Name = "txbTestRDPBCoolingBlocksStatus";
            txbTestRDPBCoolingBlocksStatus.Size = new Size(70, 23);
            txbTestRDPBCoolingBlocksStatus.TabIndex = 22;
            // 
            // cbTestRDPBCoolingBlocksQuantity
            // 
            cbTestRDPBCoolingBlocksQuantity.FormattingEnabled = true;
            cbTestRDPBCoolingBlocksQuantity.Items.AddRange(new object[] { "3", "4" });
            cbTestRDPBCoolingBlocksQuantity.Location = new Point(4, 518);
            cbTestRDPBCoolingBlocksQuantity.Margin = new Padding(4, 5, 4, 5);
            cbTestRDPBCoolingBlocksQuantity.Name = "cbTestRDPBCoolingBlocksQuantity";
            cbTestRDPBCoolingBlocksQuantity.Size = new Size(124, 23);
            cbTestRDPBCoolingBlocksQuantity.TabIndex = 21;
            cbTestRDPBCoolingBlocksQuantity.SelectedIndexChanged += cbTestRDPBCoolingBlocksQuantity_SelectedIndexChanged;
            // 
            // lblCoolingBlockQuantity
            // 
            lblCoolingBlockQuantity.AutoSize = true;
            lblCoolingBlockQuantity.Location = new Point(4, 492);
            lblCoolingBlockQuantity.Margin = new Padding(4, 0, 4, 0);
            lblCoolingBlockQuantity.Name = "lblCoolingBlockQuantity";
            lblCoolingBlockQuantity.Size = new Size(217, 15);
            lblCoolingBlockQuantity.TabIndex = 20;
            lblCoolingBlockQuantity.Text = "Число позиций охлаждающего блока";
            // 
            // btnTestRDPBN90
            // 
            btnTestRDPBN90.Location = new Point(4, 411);
            btnTestRDPBN90.Margin = new Padding(4, 5, 4, 5);
            btnTestRDPBN90.Name = "btnTestRDPBN90";
            btnTestRDPBN90.Size = new Size(204, 54);
            btnTestRDPBN90.TabIndex = 19;
            btnTestRDPBN90.Text = "(Получить статистику) N90->";
            btnTestRDPBN90.UseVisualStyleBackColor = true;
            btnTestRDPBN90.Click += btnTestRDPBN90_Click;
            // 
            // btnTestRDPBN83
            // 
            btnTestRDPBN83.Location = new Point(4, 349);
            btnTestRDPBN83.Margin = new Padding(4, 5, 4, 5);
            btnTestRDPBN83.Name = "btnTestRDPBN83";
            btnTestRDPBN83.Size = new Size(204, 54);
            btnTestRDPBN83.TabIndex = 18;
            btnTestRDPBN83.Text = "(Выключить бракер) N83->";
            btnTestRDPBN83.UseVisualStyleBackColor = true;
            btnTestRDPBN83.Click += btnTestRDPBN83_Click;
            // 
            // btnTestRDPBN82
            // 
            btnTestRDPBN82.Location = new Point(4, 285);
            btnTestRDPBN82.Margin = new Padding(4, 5, 4, 5);
            btnTestRDPBN82.Name = "btnTestRDPBN82";
            btnTestRDPBN82.Size = new Size(204, 54);
            btnTestRDPBN82.TabIndex = 17;
            btnTestRDPBN82.Text = "(Включить бракер) N82->";
            btnTestRDPBN82.UseVisualStyleBackColor = true;
            btnTestRDPBN82.Click += btnTestRDPBN82_Click;
            // 
            // btnTestRDPBN81
            // 
            btnTestRDPBN81.Location = new Point(4, 223);
            btnTestRDPBN81.Margin = new Padding(4, 5, 4, 5);
            btnTestRDPBN81.Name = "btnTestRDPBN81";
            btnTestRDPBN81.Size = new Size(204, 54);
            btnTestRDPBN81.TabIndex = 16;
            btnTestRDPBN81.Text = "(Съем плохой) N81->";
            btnTestRDPBN81.UseVisualStyleBackColor = true;
            btnTestRDPBN81.Click += btnTestRDPBN81_Click;
            // 
            // btnTestRDPBN80
            // 
            btnTestRDPBN80.Location = new Point(4, 159);
            btnTestRDPBN80.Margin = new Padding(4, 5, 4, 5);
            btnTestRDPBN80.Name = "btnTestRDPBN80";
            btnTestRDPBN80.Size = new Size(204, 54);
            btnTestRDPBN80.TabIndex = 15;
            btnTestRDPBN80.Text = "(Съем ОК) N80->";
            btnTestRDPBN80.UseVisualStyleBackColor = true;
            btnTestRDPBN80.Click += btnTestRDPBN80_Click;
            // 
            // btnRDPBTestConnect
            // 
            btnRDPBTestConnect.Location = new Point(4, 14);
            btnRDPBTestConnect.Margin = new Padding(4, 5, 4, 5);
            btnRDPBTestConnect.Name = "btnRDPBTestConnect";
            btnRDPBTestConnect.Size = new Size(204, 93);
            btnRDPBTestConnect.TabIndex = 14;
            btnRDPBTestConnect.Text = "Подключение";
            btnRDPBTestConnect.UseVisualStyleBackColor = true;
            btnRDPBTestConnect.Click += btnRDPBTestConnect_Click;
            // 
            // TestRDPBControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblTestRDPBStatus);
            Controls.Add(lvTestRDPBStatuses);
            Controls.Add(btnTestRDPBSendCommand);
            Controls.Add(txbTestRDPBManualCommand);
            Controls.Add(lblTestRDPBCommand);
            Controls.Add(txbTestRDPBCoolingBlocksStatus);
            Controls.Add(cbTestRDPBCoolingBlocksQuantity);
            Controls.Add(lblCoolingBlockQuantity);
            Controls.Add(btnTestRDPBN90);
            Controls.Add(btnTestRDPBN83);
            Controls.Add(btnTestRDPBN82);
            Controls.Add(btnTestRDPBN81);
            Controls.Add(btnTestRDPBN80);
            Controls.Add(btnRDPBTestConnect);
            Name = "TestRDPBControl";
            Size = new Size(1386, 722);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTestRDPBStatus;
        private ListView lvTestRDPBStatuses;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button btnTestRDPBSendCommand;
        private TextBox txbTestRDPBManualCommand;
        private Label lblTestRDPBCommand;
        private TextBox txbTestRDPBCoolingBlocksStatus;
        private ComboBox cbTestRDPBCoolingBlocksQuantity;
        private Label lblCoolingBlockQuantity;
        private Button btnTestRDPBN90;
        private Button btnTestRDPBN83;
        private Button btnTestRDPBN82;
        private Button btnTestRDPBN81;
        private Button btnTestRDPBN80;
        private Button btnRDPBTestConnect;
    }
}
