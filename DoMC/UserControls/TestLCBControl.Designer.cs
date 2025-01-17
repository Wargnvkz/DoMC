namespace DoMC.UserControls
{
    partial class TestLCBInterface
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
            panel4 = new Panel();
            btnLCBSaveToConfig = new Button();
            btnLCBLoadFromConfig = new Button();
            btnTestLCBStop = new Button();
            pnlTestLCBSetWorkMode = new Panel();
            chbTestLCBWorkMode = new CheckBox();
            panel3 = new Panel();
            lblTestLCBCurrentHorizontalStrokeUnitMm = new Label();
            txbTestLCBCurrentHorizontalStrokeMm = new TextBox();
            lblTestLCBCurrentHorizontalStrokeUnit = new Label();
            btnTestLCBGetCurrentPosition = new Button();
            txbTestLCBCurrentHorizontalStroke = new TextBox();
            lblTestLCBCurrentHorizontalStroke = new Label();
            panel2 = new Panel();
            lblTestLCBMaximumHorizontalStrokeUnitMm = new Label();
            txbTestLCBMaximumHorizontalStrokeMm = new TextBox();
            lblTestLCBMaximumHorizontalStrokeUnit = new Label();
            btnTestLCBGetMaxPosition = new Button();
            txbTestLCBMaximumHorizontalStroke = new TextBox();
            lblTestLCBMaximumHorizontalStroke = new Label();
            panel1 = new Panel();
            lblTestLCBDelayLengthValueMm = new Label();
            lblTestLCBPreformLengthValueMm = new Label();
            txbTestLCBDelayLengthMm = new TextBox();
            txbTestLCBPreformLengthMm = new TextBox();
            lblTestLCBDelayLengthValue = new Label();
            lblTestLCBPreformLengthValue = new Label();
            txbTestLCBDelayLength = new TextBox();
            lblTestLCBDelayLength = new Label();
            btnTestLCBGetMovementParameters = new Button();
            btnTestLCBSetMovementParameters = new Button();
            txbTestLCBPreformLength = new TextBox();
            lblTestLCBPreformLength = new Label();
            pnlTestLCBCurrent = new Panel();
            lblTestLCBCurrentUnit = new Label();
            btnTestLCBGetCurrent = new Button();
            btnTestLCBSetCurrent = new Button();
            txbTestLCBCurrent = new TextBox();
            lblTestLCBCurrent = new Label();
            pnlTestLCBStatus = new Panel();
            btnTestLCBFullTest = new Button();
            btnTestLCBWriteStatuses = new Button();
            gbLEDs = new GroupBox();
            cbTestLCBLED11 = new CheckBox();
            cbTestLCBLED10 = new CheckBox();
            cbTestLCBLED9 = new CheckBox();
            cbTestLCBLED8 = new CheckBox();
            cbTestLCBLED7 = new CheckBox();
            cbTestLCBLED6 = new CheckBox();
            cbTestLCBLED5 = new CheckBox();
            cbTestLCBLED4 = new CheckBox();
            cbTestLCBLED3 = new CheckBox();
            cbTestLCBLED2 = new CheckBox();
            cbTestLCBLED1 = new CheckBox();
            cbTestLCBLED0 = new CheckBox();
            btnTestLCBReadStatuses = new Button();
            gbTestLCBInputs = new GroupBox();
            cbTestLCBInput7 = new CheckBox();
            cbTestLCBInput6 = new CheckBox();
            cbTestLCBInput5 = new CheckBox();
            cbTestLCBInput4 = new CheckBox();
            cbTestLCBInput3 = new CheckBox();
            cbTestLCBInput2 = new CheckBox();
            cbTestLCBInput1 = new CheckBox();
            cbTestLCBInput0 = new CheckBox();
            gbTestLCBOutputs = new GroupBox();
            cbTestLCBOutput5 = new CheckBox();
            cbTestLCBOutput4 = new CheckBox();
            cbTestLCBOutput3 = new CheckBox();
            cbTestLCBOutput2 = new CheckBox();
            cbTestLCBOutput1 = new CheckBox();
            cbTestLCBOutput0 = new CheckBox();
            btnTestLCBClearAll = new Button();
            btnTestLCBSetAll = new Button();
            btnTestLCBInit = new Button();
            cbTestLCBSynchrosignal = new CheckBox();
            timer1 = new System.Windows.Forms.Timer(components);
            panel4.SuspendLayout();
            pnlTestLCBSetWorkMode.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            pnlTestLCBCurrent.SuspendLayout();
            pnlTestLCBStatus.SuspendLayout();
            gbLEDs.SuspendLayout();
            gbTestLCBInputs.SuspendLayout();
            gbTestLCBOutputs.SuspendLayout();
            SuspendLayout();
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(btnLCBSaveToConfig);
            panel4.Controls.Add(btnLCBLoadFromConfig);
            panel4.Location = new Point(1144, 599);
            panel4.Margin = new Padding(4, 5, 4, 5);
            panel4.Name = "panel4";
            panel4.Size = new Size(276, 119);
            panel4.TabIndex = 50;
            // 
            // btnLCBSaveToConfig
            // 
            btnLCBSaveToConfig.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnLCBSaveToConfig.Location = new Point(8, 5);
            btnLCBSaveToConfig.Margin = new Padding(4, 5, 4, 5);
            btnLCBSaveToConfig.Name = "btnLCBSaveToConfig";
            btnLCBSaveToConfig.Size = new Size(262, 43);
            btnLCBSaveToConfig.TabIndex = 28;
            btnLCBSaveToConfig.Text = "Сохранить параметры преформы в конфигурацию";
            btnLCBSaveToConfig.UseVisualStyleBackColor = true;
            btnLCBSaveToConfig.Click += btnLCBSaveToConfig_Click;
            // 
            // btnLCBLoadFromConfig
            // 
            btnLCBLoadFromConfig.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnLCBLoadFromConfig.Location = new Point(8, 59);
            btnLCBLoadFromConfig.Margin = new Padding(4, 5, 4, 5);
            btnLCBLoadFromConfig.Name = "btnLCBLoadFromConfig";
            btnLCBLoadFromConfig.Size = new Size(262, 43);
            btnLCBLoadFromConfig.TabIndex = 27;
            btnLCBLoadFromConfig.Text = "Прочитать параметры преформы из конфигурации";
            btnLCBLoadFromConfig.UseVisualStyleBackColor = true;
            btnLCBLoadFromConfig.Click += btnLCBLoadFromConfig_Click;
            // 
            // btnTestLCBStop
            // 
            btnTestLCBStop.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBStop.Location = new Point(1160, 0);
            btnTestLCBStop.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBStop.Name = "btnTestLCBStop";
            btnTestLCBStop.Size = new Size(262, 80);
            btnTestLCBStop.TabIndex = 49;
            btnTestLCBStop.Text = "Отключить связь";
            btnTestLCBStop.UseVisualStyleBackColor = true;
            btnTestLCBStop.Click += btnTestLCBStop_Click;
            // 
            // pnlTestLCBSetWorkMode
            // 
            pnlTestLCBSetWorkMode.BorderStyle = BorderStyle.FixedSingle;
            pnlTestLCBSetWorkMode.Controls.Add(chbTestLCBWorkMode);
            pnlTestLCBSetWorkMode.Location = new Point(680, 727);
            pnlTestLCBSetWorkMode.Margin = new Padding(4, 5, 4, 5);
            pnlTestLCBSetWorkMode.Name = "pnlTestLCBSetWorkMode";
            pnlTestLCBSetWorkMode.Size = new Size(742, 114);
            pnlTestLCBSetWorkMode.TabIndex = 48;
            // 
            // chbTestLCBWorkMode
            // 
            chbTestLCBWorkMode.Appearance = Appearance.Button;
            chbTestLCBWorkMode.CheckAlign = ContentAlignment.MiddleCenter;
            chbTestLCBWorkMode.Location = new Point(9, 7);
            chbTestLCBWorkMode.Margin = new Padding(4, 5, 4, 5);
            chbTestLCBWorkMode.Name = "chbTestLCBWorkMode";
            chbTestLCBWorkMode.Size = new Size(262, 96);
            chbTestLCBWorkMode.TabIndex = 31;
            chbTestLCBWorkMode.Text = "Рабочий режим";
            chbTestLCBWorkMode.TextAlign = ContentAlignment.MiddleCenter;
            chbTestLCBWorkMode.UseVisualStyleBackColor = true;
            chbTestLCBWorkMode.CheckedChanged += chbTestLCBWorkMode_CheckedChanged;
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(lblTestLCBCurrentHorizontalStrokeUnitMm);
            panel3.Controls.Add(txbTestLCBCurrentHorizontalStrokeMm);
            panel3.Controls.Add(lblTestLCBCurrentHorizontalStrokeUnit);
            panel3.Controls.Add(btnTestLCBGetCurrentPosition);
            panel3.Controls.Add(txbTestLCBCurrentHorizontalStroke);
            panel3.Controls.Add(lblTestLCBCurrentHorizontalStroke);
            panel3.Location = new Point(0, 851);
            panel3.Margin = new Padding(4, 5, 4, 5);
            panel3.Name = "panel3";
            panel3.Size = new Size(674, 114);
            panel3.TabIndex = 47;
            // 
            // lblTestLCBCurrentHorizontalStrokeUnitMm
            // 
            lblTestLCBCurrentHorizontalStrokeUnitMm.AutoSize = true;
            lblTestLCBCurrentHorizontalStrokeUnitMm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBCurrentHorizontalStrokeUnitMm.Location = new Point(627, 41);
            lblTestLCBCurrentHorizontalStrokeUnitMm.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBCurrentHorizontalStrokeUnitMm.Name = "lblTestLCBCurrentHorizontalStrokeUnitMm";
            lblTestLCBCurrentHorizontalStrokeUnitMm.Size = new Size(26, 17);
            lblTestLCBCurrentHorizontalStrokeUnitMm.TabIndex = 47;
            lblTestLCBCurrentHorizontalStrokeUnitMm.Text = "мм";
            // 
            // txbTestLCBCurrentHorizontalStrokeMm
            // 
            txbTestLCBCurrentHorizontalStrokeMm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBCurrentHorizontalStrokeMm.Location = new Point(542, 35);
            txbTestLCBCurrentHorizontalStrokeMm.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBCurrentHorizontalStrokeMm.Name = "txbTestLCBCurrentHorizontalStrokeMm";
            txbTestLCBCurrentHorizontalStrokeMm.ReadOnly = true;
            txbTestLCBCurrentHorizontalStrokeMm.Size = new Size(74, 23);
            txbTestLCBCurrentHorizontalStrokeMm.TabIndex = 46;
            // 
            // lblTestLCBCurrentHorizontalStrokeUnit
            // 
            lblTestLCBCurrentHorizontalStrokeUnit.AutoSize = true;
            lblTestLCBCurrentHorizontalStrokeUnit.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBCurrentHorizontalStrokeUnit.Location = new Point(496, 41);
            lblTestLCBCurrentHorizontalStrokeUnit.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBCurrentHorizontalStrokeUnit.Name = "lblTestLCBCurrentHorizontalStrokeUnit";
            lblTestLCBCurrentHorizontalStrokeUnit.Size = new Size(33, 17);
            lblTestLCBCurrentHorizontalStrokeUnit.TabIndex = 45;
            lblTestLCBCurrentHorizontalStrokeUnit.Text = "имп";
            // 
            // btnTestLCBGetCurrentPosition
            // 
            btnTestLCBGetCurrentPosition.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBGetCurrentPosition.Location = new Point(4, 7);
            btnTestLCBGetCurrentPosition.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBGetCurrentPosition.Name = "btnTestLCBGetCurrentPosition";
            btnTestLCBGetCurrentPosition.Size = new Size(262, 96);
            btnTestLCBGetCurrentPosition.TabIndex = 44;
            btnTestLCBGetCurrentPosition.Text = "Прочитать текущий ход по горизонтали";
            btnTestLCBGetCurrentPosition.UseVisualStyleBackColor = true;
            btnTestLCBGetCurrentPosition.Click += btnTestLCBGetCurrentPosition_Click;
            // 
            // txbTestLCBCurrentHorizontalStroke
            // 
            txbTestLCBCurrentHorizontalStroke.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBCurrentHorizontalStroke.Location = new Point(411, 35);
            txbTestLCBCurrentHorizontalStroke.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBCurrentHorizontalStroke.Name = "txbTestLCBCurrentHorizontalStroke";
            txbTestLCBCurrentHorizontalStroke.ReadOnly = true;
            txbTestLCBCurrentHorizontalStroke.Size = new Size(74, 23);
            txbTestLCBCurrentHorizontalStroke.TabIndex = 43;
            // 
            // lblTestLCBCurrentHorizontalStroke
            // 
            lblTestLCBCurrentHorizontalStroke.AutoSize = true;
            lblTestLCBCurrentHorizontalStroke.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBCurrentHorizontalStroke.Location = new Point(284, 42);
            lblTestLCBCurrentHorizontalStroke.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBCurrentHorizontalStroke.Name = "lblTestLCBCurrentHorizontalStroke";
            lblTestLCBCurrentHorizontalStroke.Size = new Size(96, 17);
            lblTestLCBCurrentHorizontalStroke.TabIndex = 42;
            lblTestLCBCurrentHorizontalStroke.Text = "Текущий ход:";
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(lblTestLCBMaximumHorizontalStrokeUnitMm);
            panel2.Controls.Add(txbTestLCBMaximumHorizontalStrokeMm);
            panel2.Controls.Add(lblTestLCBMaximumHorizontalStrokeUnit);
            panel2.Controls.Add(btnTestLCBGetMaxPosition);
            panel2.Controls.Add(txbTestLCBMaximumHorizontalStroke);
            panel2.Controls.Add(lblTestLCBMaximumHorizontalStroke);
            panel2.Location = new Point(0, 727);
            panel2.Margin = new Padding(4, 5, 4, 5);
            panel2.Name = "panel2";
            panel2.Size = new Size(674, 114);
            panel2.TabIndex = 46;
            // 
            // lblTestLCBMaximumHorizontalStrokeUnitMm
            // 
            lblTestLCBMaximumHorizontalStrokeUnitMm.AutoSize = true;
            lblTestLCBMaximumHorizontalStrokeUnitMm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBMaximumHorizontalStrokeUnitMm.Location = new Point(627, 37);
            lblTestLCBMaximumHorizontalStrokeUnitMm.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBMaximumHorizontalStrokeUnitMm.Name = "lblTestLCBMaximumHorizontalStrokeUnitMm";
            lblTestLCBMaximumHorizontalStrokeUnitMm.Size = new Size(26, 17);
            lblTestLCBMaximumHorizontalStrokeUnitMm.TabIndex = 39;
            lblTestLCBMaximumHorizontalStrokeUnitMm.Text = "мм";
            // 
            // txbTestLCBMaximumHorizontalStrokeMm
            // 
            txbTestLCBMaximumHorizontalStrokeMm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBMaximumHorizontalStrokeMm.Location = new Point(542, 34);
            txbTestLCBMaximumHorizontalStrokeMm.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBMaximumHorizontalStrokeMm.Name = "txbTestLCBMaximumHorizontalStrokeMm";
            txbTestLCBMaximumHorizontalStrokeMm.ReadOnly = true;
            txbTestLCBMaximumHorizontalStrokeMm.Size = new Size(74, 23);
            txbTestLCBMaximumHorizontalStrokeMm.TabIndex = 38;
            // 
            // lblTestLCBMaximumHorizontalStrokeUnit
            // 
            lblTestLCBMaximumHorizontalStrokeUnit.AutoSize = true;
            lblTestLCBMaximumHorizontalStrokeUnit.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBMaximumHorizontalStrokeUnit.Location = new Point(496, 37);
            lblTestLCBMaximumHorizontalStrokeUnit.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBMaximumHorizontalStrokeUnit.Name = "lblTestLCBMaximumHorizontalStrokeUnit";
            lblTestLCBMaximumHorizontalStrokeUnit.Size = new Size(33, 17);
            lblTestLCBMaximumHorizontalStrokeUnit.TabIndex = 37;
            lblTestLCBMaximumHorizontalStrokeUnit.Text = "имп";
            // 
            // btnTestLCBGetMaxPosition
            // 
            btnTestLCBGetMaxPosition.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBGetMaxPosition.Location = new Point(6, 7);
            btnTestLCBGetMaxPosition.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBGetMaxPosition.Name = "btnTestLCBGetMaxPosition";
            btnTestLCBGetMaxPosition.Size = new Size(262, 96);
            btnTestLCBGetMaxPosition.TabIndex = 36;
            btnTestLCBGetMaxPosition.Text = "Прочитать максимальный ход по горизонтали";
            btnTestLCBGetMaxPosition.UseVisualStyleBackColor = true;
            btnTestLCBGetMaxPosition.Click += btnTestLCBGetMaxPosition_Click;
            // 
            // txbTestLCBMaximumHorizontalStroke
            // 
            txbTestLCBMaximumHorizontalStroke.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBMaximumHorizontalStroke.Location = new Point(411, 34);
            txbTestLCBMaximumHorizontalStroke.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBMaximumHorizontalStroke.Name = "txbTestLCBMaximumHorizontalStroke";
            txbTestLCBMaximumHorizontalStroke.ReadOnly = true;
            txbTestLCBMaximumHorizontalStroke.Size = new Size(74, 23);
            txbTestLCBMaximumHorizontalStroke.TabIndex = 35;
            // 
            // lblTestLCBMaximumHorizontalStroke
            // 
            lblTestLCBMaximumHorizontalStroke.AutoSize = true;
            lblTestLCBMaximumHorizontalStroke.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBMaximumHorizontalStroke.Location = new Point(285, 41);
            lblTestLCBMaximumHorizontalStroke.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBMaximumHorizontalStroke.Name = "lblTestLCBMaximumHorizontalStroke";
            lblTestLCBMaximumHorizontalStroke.Size = new Size(75, 17);
            lblTestLCBMaximumHorizontalStroke.TabIndex = 34;
            lblTestLCBMaximumHorizontalStroke.Text = "Макс. ход:";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lblTestLCBDelayLengthValueMm);
            panel1.Controls.Add(lblTestLCBPreformLengthValueMm);
            panel1.Controls.Add(txbTestLCBDelayLengthMm);
            panel1.Controls.Add(txbTestLCBPreformLengthMm);
            panel1.Controls.Add(lblTestLCBDelayLengthValue);
            panel1.Controls.Add(lblTestLCBPreformLengthValue);
            panel1.Controls.Add(txbTestLCBDelayLength);
            panel1.Controls.Add(lblTestLCBDelayLength);
            panel1.Controls.Add(btnTestLCBGetMovementParameters);
            panel1.Controls.Add(btnTestLCBSetMovementParameters);
            panel1.Controls.Add(txbTestLCBPreformLength);
            panel1.Controls.Add(lblTestLCBPreformLength);
            panel1.Location = new Point(524, 599);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(612, 119);
            panel1.TabIndex = 45;
            // 
            // lblTestLCBDelayLengthValueMm
            // 
            lblTestLCBDelayLengthValueMm.AutoSize = true;
            lblTestLCBDelayLengthValueMm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBDelayLengthValueMm.Location = new Point(579, 68);
            lblTestLCBDelayLengthValueMm.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBDelayLengthValueMm.Name = "lblTestLCBDelayLengthValueMm";
            lblTestLCBDelayLengthValueMm.Size = new Size(26, 17);
            lblTestLCBDelayLengthValueMm.TabIndex = 36;
            lblTestLCBDelayLengthValueMm.Text = "мм";
            // 
            // lblTestLCBPreformLengthValueMm
            // 
            lblTestLCBPreformLengthValueMm.AutoSize = true;
            lblTestLCBPreformLengthValueMm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBPreformLengthValueMm.Location = new Point(579, 22);
            lblTestLCBPreformLengthValueMm.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBPreformLengthValueMm.Name = "lblTestLCBPreformLengthValueMm";
            lblTestLCBPreformLengthValueMm.Size = new Size(26, 17);
            lblTestLCBPreformLengthValueMm.TabIndex = 35;
            lblTestLCBPreformLengthValueMm.Text = "мм";
            // 
            // txbTestLCBDelayLengthMm
            // 
            txbTestLCBDelayLengthMm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBDelayLengthMm.Location = new Point(490, 64);
            txbTestLCBDelayLengthMm.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBDelayLengthMm.Name = "txbTestLCBDelayLengthMm";
            txbTestLCBDelayLengthMm.Size = new Size(79, 23);
            txbTestLCBDelayLengthMm.TabIndex = 34;
            // 
            // txbTestLCBPreformLengthMm
            // 
            txbTestLCBPreformLengthMm.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBPreformLengthMm.Location = new Point(490, 20);
            txbTestLCBPreformLengthMm.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBPreformLengthMm.Name = "txbTestLCBPreformLengthMm";
            txbTestLCBPreformLengthMm.Size = new Size(79, 23);
            txbTestLCBPreformLengthMm.TabIndex = 33;
            // 
            // lblTestLCBDelayLengthValue
            // 
            lblTestLCBDelayLengthValue.AutoSize = true;
            lblTestLCBDelayLengthValue.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBDelayLengthValue.Location = new Point(458, 68);
            lblTestLCBDelayLengthValue.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBDelayLengthValue.Name = "lblTestLCBDelayLengthValue";
            lblTestLCBDelayLengthValue.Size = new Size(33, 17);
            lblTestLCBDelayLengthValue.TabIndex = 32;
            lblTestLCBDelayLengthValue.Text = "имп";
            // 
            // lblTestLCBPreformLengthValue
            // 
            lblTestLCBPreformLengthValue.AutoSize = true;
            lblTestLCBPreformLengthValue.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBPreformLengthValue.Location = new Point(447, 22);
            lblTestLCBPreformLengthValue.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBPreformLengthValue.Name = "lblTestLCBPreformLengthValue";
            lblTestLCBPreformLengthValue.Size = new Size(33, 17);
            lblTestLCBPreformLengthValue.TabIndex = 31;
            lblTestLCBPreformLengthValue.Text = "имп";
            // 
            // txbTestLCBDelayLength
            // 
            txbTestLCBDelayLength.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBDelayLength.Location = new Point(375, 64);
            txbTestLCBDelayLength.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBDelayLength.Name = "txbTestLCBDelayLength";
            txbTestLCBDelayLength.Size = new Size(79, 23);
            txbTestLCBDelayLength.TabIndex = 30;
            // 
            // lblTestLCBDelayLength
            // 
            lblTestLCBDelayLength.AutoSize = true;
            lblTestLCBDelayLength.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBDelayLength.Location = new Point(219, 68);
            lblTestLCBDelayLength.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBDelayLength.Name = "lblTestLCBDelayLength";
            lblTestLCBDelayLength.Size = new Size(157, 17);
            lblTestLCBDelayLength.TabIndex = 29;
            lblTestLCBDelayLength.Text = "Расстояние задержки:";
            // 
            // btnTestLCBGetMovementParameters
            // 
            btnTestLCBGetMovementParameters.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBGetMovementParameters.Location = new Point(9, 12);
            btnTestLCBGetMovementParameters.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBGetMovementParameters.Name = "btnTestLCBGetMovementParameters";
            btnTestLCBGetMovementParameters.Size = new Size(200, 43);
            btnTestLCBGetMovementParameters.TabIndex = 28;
            btnTestLCBGetMovementParameters.Text = "Прочитать параметры";
            btnTestLCBGetMovementParameters.UseVisualStyleBackColor = true;
            btnTestLCBGetMovementParameters.Click += btnTestLCBGetMovementParameters_Click;
            // 
            // btnTestLCBSetMovementParameters
            // 
            btnTestLCBSetMovementParameters.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBSetMovementParameters.Location = new Point(9, 64);
            btnTestLCBSetMovementParameters.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBSetMovementParameters.Name = "btnTestLCBSetMovementParameters";
            btnTestLCBSetMovementParameters.Size = new Size(200, 43);
            btnTestLCBSetMovementParameters.TabIndex = 27;
            btnTestLCBSetMovementParameters.Text = "Установить параметры";
            btnTestLCBSetMovementParameters.UseVisualStyleBackColor = true;
            btnTestLCBSetMovementParameters.Click += btnTestLCBSetMovementParameters_Click;
            // 
            // txbTestLCBPreformLength
            // 
            txbTestLCBPreformLength.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBPreformLength.Location = new Point(358, 20);
            txbTestLCBPreformLength.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBPreformLength.Name = "txbTestLCBPreformLength";
            txbTestLCBPreformLength.Size = new Size(79, 23);
            txbTestLCBPreformLength.TabIndex = 26;
            // 
            // lblTestLCBPreformLength
            // 
            lblTestLCBPreformLength.AutoSize = true;
            lblTestLCBPreformLength.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBPreformLength.Location = new Point(219, 22);
            lblTestLCBPreformLength.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBPreformLength.Name = "lblTestLCBPreformLength";
            lblTestLCBPreformLength.Size = new Size(129, 17);
            lblTestLCBPreformLength.TabIndex = 25;
            lblTestLCBPreformLength.Text = "Длина преформы:";
            // 
            // pnlTestLCBCurrent
            // 
            pnlTestLCBCurrent.BorderStyle = BorderStyle.FixedSingle;
            pnlTestLCBCurrent.Controls.Add(lblTestLCBCurrentUnit);
            pnlTestLCBCurrent.Controls.Add(btnTestLCBGetCurrent);
            pnlTestLCBCurrent.Controls.Add(btnTestLCBSetCurrent);
            pnlTestLCBCurrent.Controls.Add(txbTestLCBCurrent);
            pnlTestLCBCurrent.Controls.Add(lblTestLCBCurrent);
            pnlTestLCBCurrent.Location = new Point(2, 599);
            pnlTestLCBCurrent.Margin = new Padding(4, 5, 4, 5);
            pnlTestLCBCurrent.Name = "pnlTestLCBCurrent";
            pnlTestLCBCurrent.Size = new Size(516, 119);
            pnlTestLCBCurrent.TabIndex = 44;
            // 
            // lblTestLCBCurrentUnit
            // 
            lblTestLCBCurrentUnit.AutoSize = true;
            lblTestLCBCurrentUnit.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBCurrentUnit.Location = new Point(484, 42);
            lblTestLCBCurrentUnit.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBCurrentUnit.Name = "lblTestLCBCurrentUnit";
            lblTestLCBCurrentUnit.Size = new Size(26, 17);
            lblTestLCBCurrentUnit.TabIndex = 27;
            lblTestLCBCurrentUnit.Text = "мА";
            // 
            // btnTestLCBGetCurrent
            // 
            btnTestLCBGetCurrent.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBGetCurrent.Location = new Point(4, 12);
            btnTestLCBGetCurrent.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBGetCurrent.Name = "btnTestLCBGetCurrent";
            btnTestLCBGetCurrent.Size = new Size(262, 43);
            btnTestLCBGetCurrent.TabIndex = 26;
            btnTestLCBGetCurrent.Text = "Прочитать ток";
            btnTestLCBGetCurrent.UseVisualStyleBackColor = true;
            btnTestLCBGetCurrent.Click += btnTestLCBGetCurrent_Click;
            // 
            // btnTestLCBSetCurrent
            // 
            btnTestLCBSetCurrent.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBSetCurrent.Location = new Point(4, 64);
            btnTestLCBSetCurrent.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBSetCurrent.Name = "btnTestLCBSetCurrent";
            btnTestLCBSetCurrent.Size = new Size(262, 43);
            btnTestLCBSetCurrent.TabIndex = 25;
            btnTestLCBSetCurrent.Text = "Установить ток";
            btnTestLCBSetCurrent.UseVisualStyleBackColor = true;
            btnTestLCBSetCurrent.Click += btnTestLCBSetCurrent_Click;
            // 
            // txbTestLCBCurrent
            // 
            txbTestLCBCurrent.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txbTestLCBCurrent.Location = new Point(408, 38);
            txbTestLCBCurrent.Margin = new Padding(4, 5, 4, 5);
            txbTestLCBCurrent.Name = "txbTestLCBCurrent";
            txbTestLCBCurrent.Size = new Size(76, 23);
            txbTestLCBCurrent.TabIndex = 24;
            // 
            // lblTestLCBCurrent
            // 
            lblTestLCBCurrent.AutoSize = true;
            lblTestLCBCurrent.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblTestLCBCurrent.Location = new Point(274, 42);
            lblTestLCBCurrent.Margin = new Padding(4, 0, 4, 0);
            lblTestLCBCurrent.Name = "lblTestLCBCurrent";
            lblTestLCBCurrent.Size = new Size(124, 17);
            lblTestLCBCurrent.TabIndex = 23;
            lblTestLCBCurrent.Text = "Ток светодиодов:";
            // 
            // pnlTestLCBStatus
            // 
            pnlTestLCBStatus.BorderStyle = BorderStyle.FixedSingle;
            pnlTestLCBStatus.Controls.Add(btnTestLCBFullTest);
            pnlTestLCBStatus.Controls.Add(btnTestLCBWriteStatuses);
            pnlTestLCBStatus.Controls.Add(gbLEDs);
            pnlTestLCBStatus.Controls.Add(btnTestLCBReadStatuses);
            pnlTestLCBStatus.Controls.Add(gbTestLCBInputs);
            pnlTestLCBStatus.Controls.Add(gbTestLCBOutputs);
            pnlTestLCBStatus.Controls.Add(btnTestLCBClearAll);
            pnlTestLCBStatus.Controls.Add(btnTestLCBSetAll);
            pnlTestLCBStatus.Location = new Point(0, 86);
            pnlTestLCBStatus.Margin = new Padding(4, 5, 4, 5);
            pnlTestLCBStatus.Name = "pnlTestLCBStatus";
            pnlTestLCBStatus.Size = new Size(1421, 505);
            pnlTestLCBStatus.TabIndex = 43;
            // 
            // btnTestLCBFullTest
            // 
            btnTestLCBFullTest.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBFullTest.Location = new Point(4, 406);
            btnTestLCBFullTest.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBFullTest.Name = "btnTestLCBFullTest";
            btnTestLCBFullTest.Size = new Size(262, 80);
            btnTestLCBFullTest.TabIndex = 16;
            btnTestLCBFullTest.Text = "Полная проверка";
            btnTestLCBFullTest.UseVisualStyleBackColor = true;
            btnTestLCBFullTest.Click += btnTestLCBFullTest_Click;
            // 
            // btnTestLCBWriteStatuses
            // 
            btnTestLCBWriteStatuses.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBWriteStatuses.Location = new Point(4, 186);
            btnTestLCBWriteStatuses.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBWriteStatuses.Name = "btnTestLCBWriteStatuses";
            btnTestLCBWriteStatuses.Size = new Size(262, 80);
            btnTestLCBWriteStatuses.TabIndex = 15;
            btnTestLCBWriteStatuses.Text = "Установить состояние";
            btnTestLCBWriteStatuses.UseVisualStyleBackColor = true;
            btnTestLCBWriteStatuses.Click += btnTestLCBWriteStatuses_Click;
            // 
            // gbLEDs
            // 
            gbLEDs.Controls.Add(cbTestLCBLED11);
            gbLEDs.Controls.Add(cbTestLCBLED10);
            gbLEDs.Controls.Add(cbTestLCBLED9);
            gbLEDs.Controls.Add(cbTestLCBLED8);
            gbLEDs.Controls.Add(cbTestLCBLED7);
            gbLEDs.Controls.Add(cbTestLCBLED6);
            gbLEDs.Controls.Add(cbTestLCBLED5);
            gbLEDs.Controls.Add(cbTestLCBLED4);
            gbLEDs.Controls.Add(cbTestLCBLED3);
            gbLEDs.Controls.Add(cbTestLCBLED2);
            gbLEDs.Controls.Add(cbTestLCBLED1);
            gbLEDs.Controls.Add(cbTestLCBLED0);
            gbLEDs.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            gbLEDs.Location = new Point(280, 7);
            gbLEDs.Margin = new Padding(4, 5, 4, 5);
            gbLEDs.Name = "gbLEDs";
            gbLEDs.Padding = new Padding(4, 5, 4, 5);
            gbLEDs.Size = new Size(336, 486);
            gbLEDs.TabIndex = 14;
            gbLEDs.TabStop = false;
            gbLEDs.Text = "Светодиоды";
            // 
            // cbTestLCBLED11
            // 
            cbTestLCBLED11.AutoSize = true;
            cbTestLCBLED11.Location = new Point(10, 440);
            cbTestLCBLED11.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED11.Name = "cbTestLCBLED11";
            cbTestLCBLED11.Size = new Size(118, 21);
            cbTestLCBLED11.TabIndex = 11;
            cbTestLCBLED11.Text = "Светодиод 12";
            cbTestLCBLED11.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED10
            // 
            cbTestLCBLED10.AutoSize = true;
            cbTestLCBLED10.Location = new Point(10, 402);
            cbTestLCBLED10.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED10.Name = "cbTestLCBLED10";
            cbTestLCBLED10.Size = new Size(118, 21);
            cbTestLCBLED10.TabIndex = 10;
            cbTestLCBLED10.Text = "Светодиод 11";
            cbTestLCBLED10.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED9
            // 
            cbTestLCBLED9.AutoSize = true;
            cbTestLCBLED9.Location = new Point(10, 366);
            cbTestLCBLED9.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED9.Name = "cbTestLCBLED9";
            cbTestLCBLED9.Size = new Size(118, 21);
            cbTestLCBLED9.TabIndex = 9;
            cbTestLCBLED9.Text = "Светодиод 10";
            cbTestLCBLED9.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED8
            // 
            cbTestLCBLED8.AutoSize = true;
            cbTestLCBLED8.Location = new Point(10, 329);
            cbTestLCBLED8.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED8.Name = "cbTestLCBLED8";
            cbTestLCBLED8.Size = new Size(110, 21);
            cbTestLCBLED8.TabIndex = 8;
            cbTestLCBLED8.Text = "Светодиод 9";
            cbTestLCBLED8.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED7
            // 
            cbTestLCBLED7.AutoSize = true;
            cbTestLCBLED7.Location = new Point(10, 292);
            cbTestLCBLED7.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED7.Name = "cbTestLCBLED7";
            cbTestLCBLED7.Size = new Size(110, 21);
            cbTestLCBLED7.TabIndex = 7;
            cbTestLCBLED7.Text = "Светодиод 8";
            cbTestLCBLED7.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED6
            // 
            cbTestLCBLED6.AutoSize = true;
            cbTestLCBLED6.Location = new Point(10, 255);
            cbTestLCBLED6.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED6.Name = "cbTestLCBLED6";
            cbTestLCBLED6.Size = new Size(110, 21);
            cbTestLCBLED6.TabIndex = 6;
            cbTestLCBLED6.Text = "Светодиод 7";
            cbTestLCBLED6.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED5
            // 
            cbTestLCBLED5.AutoSize = true;
            cbTestLCBLED5.Location = new Point(10, 219);
            cbTestLCBLED5.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED5.Name = "cbTestLCBLED5";
            cbTestLCBLED5.Size = new Size(110, 21);
            cbTestLCBLED5.TabIndex = 5;
            cbTestLCBLED5.Text = "Светодиод 6";
            cbTestLCBLED5.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED4
            // 
            cbTestLCBLED4.AutoSize = true;
            cbTestLCBLED4.Location = new Point(10, 182);
            cbTestLCBLED4.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED4.Name = "cbTestLCBLED4";
            cbTestLCBLED4.Size = new Size(110, 21);
            cbTestLCBLED4.TabIndex = 4;
            cbTestLCBLED4.Text = "Светодиод 5";
            cbTestLCBLED4.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED3
            // 
            cbTestLCBLED3.AutoSize = true;
            cbTestLCBLED3.Location = new Point(10, 144);
            cbTestLCBLED3.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED3.Name = "cbTestLCBLED3";
            cbTestLCBLED3.Size = new Size(110, 21);
            cbTestLCBLED3.TabIndex = 3;
            cbTestLCBLED3.Text = "Светодиод 4";
            cbTestLCBLED3.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED2
            // 
            cbTestLCBLED2.AutoSize = true;
            cbTestLCBLED2.Location = new Point(10, 107);
            cbTestLCBLED2.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED2.Name = "cbTestLCBLED2";
            cbTestLCBLED2.Size = new Size(110, 21);
            cbTestLCBLED2.TabIndex = 2;
            cbTestLCBLED2.Text = "Светодиод 3";
            cbTestLCBLED2.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED1
            // 
            cbTestLCBLED1.AutoSize = true;
            cbTestLCBLED1.Location = new Point(10, 70);
            cbTestLCBLED1.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED1.Name = "cbTestLCBLED1";
            cbTestLCBLED1.Size = new Size(110, 21);
            cbTestLCBLED1.TabIndex = 1;
            cbTestLCBLED1.Text = "Светодиод 2";
            cbTestLCBLED1.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBLED0
            // 
            cbTestLCBLED0.AutoSize = true;
            cbTestLCBLED0.Location = new Point(10, 32);
            cbTestLCBLED0.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBLED0.Name = "cbTestLCBLED0";
            cbTestLCBLED0.Size = new Size(110, 21);
            cbTestLCBLED0.TabIndex = 0;
            cbTestLCBLED0.Text = "Светодиод 1";
            cbTestLCBLED0.UseVisualStyleBackColor = true;
            // 
            // btnTestLCBReadStatuses
            // 
            btnTestLCBReadStatuses.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBReadStatuses.Location = new Point(4, 64);
            btnTestLCBReadStatuses.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBReadStatuses.Name = "btnTestLCBReadStatuses";
            btnTestLCBReadStatuses.Size = new Size(262, 80);
            btnTestLCBReadStatuses.TabIndex = 13;
            btnTestLCBReadStatuses.Text = "Читать состояние";
            btnTestLCBReadStatuses.UseVisualStyleBackColor = true;
            btnTestLCBReadStatuses.Click += btnTestLCBReadStatuses_Click;
            // 
            // gbTestLCBInputs
            // 
            gbTestLCBInputs.Controls.Add(cbTestLCBInput7);
            gbTestLCBInputs.Controls.Add(cbTestLCBInput6);
            gbTestLCBInputs.Controls.Add(cbTestLCBInput5);
            gbTestLCBInputs.Controls.Add(cbTestLCBInput4);
            gbTestLCBInputs.Controls.Add(cbTestLCBInput3);
            gbTestLCBInputs.Controls.Add(cbTestLCBInput2);
            gbTestLCBInputs.Controls.Add(cbTestLCBInput1);
            gbTestLCBInputs.Controls.Add(cbTestLCBInput0);
            gbTestLCBInputs.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            gbTestLCBInputs.Location = new Point(970, 7);
            gbTestLCBInputs.Margin = new Padding(4, 5, 4, 5);
            gbTestLCBInputs.Name = "gbTestLCBInputs";
            gbTestLCBInputs.Padding = new Padding(4, 5, 4, 5);
            gbTestLCBInputs.Size = new Size(441, 356);
            gbTestLCBInputs.TabIndex = 12;
            gbTestLCBInputs.TabStop = false;
            gbTestLCBInputs.Text = "Входы";
            // 
            // cbTestLCBInput7
            // 
            cbTestLCBInput7.AutoSize = true;
            cbTestLCBInput7.Location = new Point(10, 292);
            cbTestLCBInput7.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBInput7.Name = "cbTestLCBInput7";
            cbTestLCBInput7.Size = new Size(153, 21);
            cbTestLCBInput7.TabIndex = 7;
            cbTestLCBInput7.Text = "Маячки выдвинуты";
            cbTestLCBInput7.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBInput6
            // 
            cbTestLCBInput6.AutoSize = true;
            cbTestLCBInput6.Location = new Point(10, 255);
            cbTestLCBInput6.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBInput6.Name = "cbTestLCBInput6";
            cbTestLCBInput6.Size = new Size(145, 21);
            cbTestLCBInput6.TabIndex = 6;
            cbTestLCBInput6.Text = "Маячки спрятаны";
            cbTestLCBInput6.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBInput5
            // 
            cbTestLCBInput5.AutoSize = true;
            cbTestLCBInput5.Location = new Point(10, 219);
            cbTestLCBInput5.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBInput5.Name = "cbTestLCBInput5";
            cbTestLCBInput5.Size = new Size(99, 21);
            cbTestLCBInput5.TabIndex = 5;
            cbTestLCBInput5.Text = "Запас PG1";
            cbTestLCBInput5.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBInput4
            // 
            cbTestLCBInput4.AutoSize = true;
            cbTestLCBInput4.Location = new Point(10, 182);
            cbTestLCBInput4.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBInput4.Name = "cbTestLCBInput4";
            cbTestLCBInput4.Size = new Size(208, 21);
            cbTestLCBInput4.TabIndex = 4;
            cbTestLCBInput4.Text = "Датчик положения захвата";
            cbTestLCBInput4.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBInput3
            // 
            cbTestLCBInput3.AutoSize = true;
            cbTestLCBInput3.Location = new Point(10, 144);
            cbTestLCBInput3.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBInput3.Name = "cbTestLCBInput3";
            cbTestLCBInput3.Size = new Size(189, 21);
            cbTestLCBInput3.TabIndex = 3;
            cbTestLCBInput3.Text = "Блокировка работы БУС";
            cbTestLCBInput3.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBInput2
            // 
            cbTestLCBInput2.AutoSize = true;
            cbTestLCBInput2.Location = new Point(10, 107);
            cbTestLCBInput2.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBInput2.Name = "cbTestLCBInput2";
            cbTestLCBInput2.Size = new Size(96, 21);
            cbTestLCBInput2.TabIndex = 2;
            cbTestLCBInput2.Text = "Энкодер B";
            cbTestLCBInput2.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBInput1
            // 
            cbTestLCBInput1.AutoSize = true;
            cbTestLCBInput1.Location = new Point(10, 70);
            cbTestLCBInput1.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBInput1.Name = "cbTestLCBInput1";
            cbTestLCBInput1.Size = new Size(96, 21);
            cbTestLCBInput1.TabIndex = 1;
            cbTestLCBInput1.Text = "Энкодер A";
            cbTestLCBInput1.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBInput0
            // 
            cbTestLCBInput0.AutoSize = true;
            cbTestLCBInput0.Location = new Point(10, 32);
            cbTestLCBInput0.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBInput0.Name = "cbTestLCBInput0";
            cbTestLCBInput0.Size = new Size(49, 21);
            cbTestLCBInput0.TabIndex = 0;
            cbTestLCBInput0.Text = "AiN";
            cbTestLCBInput0.UseVisualStyleBackColor = true;
            // 
            // gbTestLCBOutputs
            // 
            gbTestLCBOutputs.Controls.Add(cbTestLCBOutput5);
            gbTestLCBOutputs.Controls.Add(cbTestLCBOutput4);
            gbTestLCBOutputs.Controls.Add(cbTestLCBOutput3);
            gbTestLCBOutputs.Controls.Add(cbTestLCBOutput2);
            gbTestLCBOutputs.Controls.Add(cbTestLCBOutput1);
            gbTestLCBOutputs.Controls.Add(cbTestLCBOutput0);
            gbTestLCBOutputs.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            gbTestLCBOutputs.Location = new Point(626, 7);
            gbTestLCBOutputs.Margin = new Padding(4, 5, 4, 5);
            gbTestLCBOutputs.Name = "gbTestLCBOutputs";
            gbTestLCBOutputs.Padding = new Padding(4, 5, 4, 5);
            gbTestLCBOutputs.Size = new Size(336, 356);
            gbTestLCBOutputs.TabIndex = 11;
            gbTestLCBOutputs.TabStop = false;
            gbTestLCBOutputs.Text = "Выходы";
            // 
            // cbTestLCBOutput5
            // 
            cbTestLCBOutput5.AutoSize = true;
            cbTestLCBOutput5.Location = new Point(10, 219);
            cbTestLCBOutput5.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBOutput5.Name = "cbTestLCBOutput5";
            cbTestLCBOutput5.Size = new Size(105, 21);
            cbTestLCBOutput5.TabIndex = 5;
            cbTestLCBOutput5.Text = "Запас PC12";
            cbTestLCBOutput5.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBOutput4
            // 
            cbTestLCBOutput4.AutoSize = true;
            cbTestLCBOutput4.Location = new Point(10, 182);
            cbTestLCBOutput4.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBOutput4.Name = "cbTestLCBOutput4";
            cbTestLCBOutput4.Size = new Size(129, 21);
            cbTestLCBOutput4.TabIndex = 4;
            cbTestLCBOutput4.Text = "Синхронизация";
            cbTestLCBOutput4.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBOutput3
            // 
            cbTestLCBOutput3.AutoSize = true;
            cbTestLCBOutput3.Location = new Point(10, 144);
            cbTestLCBOutput3.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBOutput3.Name = "cbTestLCBOutput3";
            cbTestLCBOutput3.Size = new Size(110, 21);
            cbTestLCBOutput3.TabIndex = 3;
            cbTestLCBOutput3.Text = "Авария БУС ";
            cbTestLCBOutput3.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBOutput2
            // 
            cbTestLCBOutput2.AutoSize = true;
            cbTestLCBOutput2.Location = new Point(10, 107);
            cbTestLCBOutput2.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBOutput2.Name = "cbTestLCBOutput2";
            cbTestLCBOutput2.Size = new Size(127, 21);
            cbTestLCBOutput2.TabIndex = 2;
            cbTestLCBOutput2.Text = "Электромагнит";
            cbTestLCBOutput2.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBOutput1
            // 
            cbTestLCBOutput1.AutoSize = true;
            cbTestLCBOutput1.Location = new Point(10, 70);
            cbTestLCBOutput1.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBOutput1.Name = "cbTestLCBOutput1";
            cbTestLCBOutput1.Size = new Size(101, 21);
            cbTestLCBOutput1.TabIndex = 1;
            cbTestLCBOutput1.Text = "Клапан ПЦ";
            cbTestLCBOutput1.UseVisualStyleBackColor = true;
            // 
            // cbTestLCBOutput0
            // 
            cbTestLCBOutput0.AutoSize = true;
            cbTestLCBOutput0.Location = new Point(10, 32);
            cbTestLCBOutput0.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBOutput0.Name = "cbTestLCBOutput0";
            cbTestLCBOutput0.Size = new Size(156, 21);
            cbTestLCBOutput0.TabIndex = 0;
            cbTestLCBOutput0.Text = "HL5 - LED на плате";
            cbTestLCBOutput0.UseVisualStyleBackColor = true;
            // 
            // btnTestLCBClearAll
            // 
            btnTestLCBClearAll.Location = new Point(1299, 450);
            btnTestLCBClearAll.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBClearAll.Name = "btnTestLCBClearAll";
            btnTestLCBClearAll.Size = new Size(112, 37);
            btnTestLCBClearAll.TabIndex = 9;
            btnTestLCBClearAll.Text = "Очистить";
            btnTestLCBClearAll.UseVisualStyleBackColor = true;
            btnTestLCBClearAll.Click += btnTestLCBClearAll_Click;
            // 
            // btnTestLCBSetAll
            // 
            btnTestLCBSetAll.Location = new Point(1176, 450);
            btnTestLCBSetAll.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBSetAll.Name = "btnTestLCBSetAll";
            btnTestLCBSetAll.Size = new Size(112, 37);
            btnTestLCBSetAll.TabIndex = 11;
            btnTestLCBSetAll.Text = "Установить все";
            btnTestLCBSetAll.UseVisualStyleBackColor = true;
            btnTestLCBSetAll.Click += btnTestLCBSetAll_Click;
            // 
            // btnTestLCBInit
            // 
            btnTestLCBInit.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTestLCBInit.Location = new Point(0, 0);
            btnTestLCBInit.Margin = new Padding(4, 5, 4, 5);
            btnTestLCBInit.Name = "btnTestLCBInit";
            btnTestLCBInit.Size = new Size(262, 80);
            btnTestLCBInit.TabIndex = 42;
            btnTestLCBInit.Text = "Инициализация связи";
            btnTestLCBInit.UseVisualStyleBackColor = true;
            btnTestLCBInit.Click += btnTestLCBInit_Click;
            // 
            // cbTestLCBSynchrosignal
            // 
            cbTestLCBSynchrosignal.AutoSize = true;
            cbTestLCBSynchrosignal.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbTestLCBSynchrosignal.Location = new Point(291, 31);
            cbTestLCBSynchrosignal.Margin = new Padding(4, 5, 4, 5);
            cbTestLCBSynchrosignal.Name = "cbTestLCBSynchrosignal";
            cbTestLCBSynchrosignal.Size = new Size(118, 21);
            cbTestLCBSynchrosignal.TabIndex = 51;
            cbTestLCBSynchrosignal.Text = "Синхросигнал";
            cbTestLCBSynchrosignal.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
            // 
            // TestLCBInterface
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cbTestLCBSynchrosignal);
            Controls.Add(panel4);
            Controls.Add(btnTestLCBStop);
            Controls.Add(pnlTestLCBSetWorkMode);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(pnlTestLCBCurrent);
            Controls.Add(pnlTestLCBStatus);
            Controls.Add(btnTestLCBInit);
            Name = "TestLCBInterface";
            Size = new Size(1491, 973);
            panel4.ResumeLayout(false);
            pnlTestLCBSetWorkMode.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            pnlTestLCBCurrent.ResumeLayout(false);
            pnlTestLCBCurrent.PerformLayout();
            pnlTestLCBStatus.ResumeLayout(false);
            gbLEDs.ResumeLayout(false);
            gbLEDs.PerformLayout();
            gbTestLCBInputs.ResumeLayout(false);
            gbTestLCBInputs.PerformLayout();
            gbTestLCBOutputs.ResumeLayout(false);
            gbTestLCBOutputs.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel4;
        private Button btnLCBSaveToConfig;
        private Button btnLCBLoadFromConfig;
        private Button btnTestLCBStop;
        private Panel pnlTestLCBSetWorkMode;
        private CheckBox chbTestLCBWorkMode;
        private Panel panel3;
        private Label lblTestLCBCurrentHorizontalStrokeUnitMm;
        private TextBox txbTestLCBCurrentHorizontalStrokeMm;
        private Label lblTestLCBCurrentHorizontalStrokeUnit;
        private Button btnTestLCBGetCurrentPosition;
        private TextBox txbTestLCBCurrentHorizontalStroke;
        private Label lblTestLCBCurrentHorizontalStroke;
        private Panel panel2;
        private Label lblTestLCBMaximumHorizontalStrokeUnitMm;
        private TextBox txbTestLCBMaximumHorizontalStrokeMm;
        private Label lblTestLCBMaximumHorizontalStrokeUnit;
        private Button btnTestLCBGetMaxPosition;
        private TextBox txbTestLCBMaximumHorizontalStroke;
        private Label lblTestLCBMaximumHorizontalStroke;
        private Panel panel1;
        private Label lblTestLCBDelayLengthValueMm;
        private Label lblTestLCBPreformLengthValueMm;
        private TextBox txbTestLCBDelayLengthMm;
        private TextBox txbTestLCBPreformLengthMm;
        private Label lblTestLCBDelayLengthValue;
        private Label lblTestLCBPreformLengthValue;
        private TextBox txbTestLCBDelayLength;
        private Label lblTestLCBDelayLength;
        private Button btnTestLCBGetMovementParameters;
        private Button btnTestLCBSetMovementParameters;
        private TextBox txbTestLCBPreformLength;
        private Label lblTestLCBPreformLength;
        private Panel pnlTestLCBCurrent;
        private Label lblTestLCBCurrentUnit;
        private Button btnTestLCBGetCurrent;
        private Button btnTestLCBSetCurrent;
        private TextBox txbTestLCBCurrent;
        private Label lblTestLCBCurrent;
        private Panel pnlTestLCBStatus;
        private Button btnTestLCBFullTest;
        private Button btnTestLCBWriteStatuses;
        private GroupBox gbLEDs;
        private CheckBox cbTestLCBLED11;
        private CheckBox cbTestLCBLED10;
        private CheckBox cbTestLCBLED9;
        private CheckBox cbTestLCBLED8;
        private CheckBox cbTestLCBLED7;
        private CheckBox cbTestLCBLED6;
        private CheckBox cbTestLCBLED5;
        private CheckBox cbTestLCBLED4;
        private CheckBox cbTestLCBLED3;
        private CheckBox cbTestLCBLED2;
        private CheckBox cbTestLCBLED1;
        private CheckBox cbTestLCBLED0;
        private Button btnTestLCBReadStatuses;
        private GroupBox gbTestLCBInputs;
        private CheckBox cbTestLCBInput7;
        private CheckBox cbTestLCBInput6;
        private CheckBox cbTestLCBInput5;
        private CheckBox cbTestLCBInput4;
        private CheckBox cbTestLCBInput3;
        private CheckBox cbTestLCBInput2;
        private CheckBox cbTestLCBInput1;
        private CheckBox cbTestLCBInput0;
        private GroupBox gbTestLCBOutputs;
        private CheckBox cbTestLCBOutput5;
        private CheckBox cbTestLCBOutput4;
        private CheckBox cbTestLCBOutput3;
        private CheckBox cbTestLCBOutput2;
        private CheckBox cbTestLCBOutput1;
        private CheckBox cbTestLCBOutput0;
        private Button btnTestLCBClearAll;
        private Button btnTestLCBSetAll;
        private Button btnTestLCBInit;
        private CheckBox cbTestLCBSynchrosignal;
        private System.Windows.Forms.Timer timer1;
    }
}
