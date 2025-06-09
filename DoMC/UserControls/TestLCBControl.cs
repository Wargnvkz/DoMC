using DoMC.Classes;
using DoMCModuleControl.Logging;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoMCLib.Classes.Module.CCD;
using DoMC.Tools;
using DoMCLib.Classes.Module.LCB;
using static DoMCLib.Classes.Module.LCB.LCBModule;
using System.Security.Cryptography;
using DoMCLib.Classes;

namespace DoMC.UserControls
{
    public partial class TestLCBInterface : UserControl
    {
        IMainController MainController;
        ILogger WorkingLog;
        IDoMCSettingsUpdatedProvider SettingsUpdateProvider;
        Observer CurrentObserver;
        DoMCLib.Classes.DoMCApplicationContext CurrentContext;
        bool?[] CardsChecks = new bool?[12];
        bool TestLCBConnected;
        bool TestLCBTestStarted = false;
        DateTime LastSinchrosignal;
        TimeSpan SinchroSignalDisplayTimeout = new TimeSpan(0, 0, 2);
        //LEDDataExchangeStatus LEDParameters = new LEDDataExchangeStatus();
        bool IsWorkingModeStarted = false;
        bool LCBSettingsPreformLengthGotFromConfig = false;
        bool LCBSettingsDelayLengthGotFromConfig = false;

        CheckBox[] TestLCBOutputs;
        CheckBox[] TestLCBInputs;
        CheckBox[] TestLCBLEDs;



        public TestLCBInterface(IMainController Controller, ILogger logger, DoMC.Classes.IDoMCSettingsUpdatedProvider settingsUpdateProvider)
        {
            InitializeComponent();
            InitControls();
            MainController = Controller;
            WorkingLog = logger;
            SettingsUpdateProvider = settingsUpdateProvider;
            SettingsUpdateProvider.SettingsUpdated += SettingsUpdateProvider_SettingsUpdated;
            CurrentObserver = MainController.GetObserver();
            CurrentObserver.NotificationReceivers += Observer_NotificationReceivers;
            //Disposed += OnDispose;

            //MainController.GetObserver().NotificationReceivers += GetCCDStandardInterface_NotificationReceivers;
        }

        private async Task Observer_NotificationReceivers(string EventName, object? data)
        {
            if (EventName.EndsWith($"{LEDCommandType.LEDSynchrosignalResponse}.{EventType.Received}"))
            {
                LastSinchrosignal = DateTime.Now;
            }
            if (EventName== DoMCApplicationContext.SettingsInterfaceClosedEventName)
            {
                TestLCBSetSettingMode();
                TestLCBStop();
            }
        }

        private void SettingsUpdateProvider_SettingsUpdated(object? sender, EventArgs e)
        {
            var context = SettingsUpdateProvider.GetContext();
            if (TestLCBConnected)
                TestLCBStop();
            ApplyNewContext(context);
        }
        /*private void OnDispose(object? sender, EventArgs e)
        {
            if (SettingsUpdateProvider != null)
            {
                SettingsUpdateProvider.SettingsUpdated -= SettingsUpdateProvider_SettingsUpdated;
            }
            try
            {
                //MainController.GetObserver().NotificationReceivers -= GetCCDStandardInterface_NotificationReceivers;
            }
            catch { }
        }*/
        private void ApplyNewContext(DoMCLib.Classes.DoMCApplicationContext context)
        {
            CurrentContext = context;
            FillPage();
        }
        private void FillPage()
        {

        }

        private void InitControls()
        {

            TestLCBOutputs = new CheckBox[] { cbTestLCBOutput0, cbTestLCBOutput1, cbTestLCBOutput2, cbTestLCBOutput3, cbTestLCBOutput4, cbTestLCBOutput5 };
            TestLCBInputs = new CheckBox[] { cbTestLCBInput0, cbTestLCBInput1, cbTestLCBInput2, cbTestLCBInput3, cbTestLCBInput4, cbTestLCBInput5, cbTestLCBInput6, cbTestLCBInput7 };
            TestLCBLEDs = new CheckBox[] { cbTestLCBLED0, cbTestLCBLED1, cbTestLCBLED2, cbTestLCBLED3, cbTestLCBLED4, cbTestLCBLED5, cbTestLCBLED6, cbTestLCBLED7, cbTestLCBLED8, cbTestLCBLED9, cbTestLCBLED10, cbTestLCBLED11 };

        }



        private void btnTestLCBReadStatuses_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;


            var res = MainController.CreateCommandInstance(typeof(GetLCBEquipmentStatusCommand))
                .Wait(this, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out LEDEquimpentStatus LEDStatus);
            //GetLCBEquipmentStatusCommand

            /*SendCommand(ModuleCommand.GetLCBEquipmentStatusRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => { return LEDParameters.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBEquipmentStatusResponse && 
                    LEDParameters.LastCommandResponseReceived > start; });*/

            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            for (int o = 0; o < TestLCBOutputs.Length; o++)
            {
                TestLCBOutputs[o].CheckState = LEDStatus.Outputs[o] ? CheckState.Indeterminate : CheckState.Unchecked;
            }
            for (int i = 0; i < TestLCBInputs.Length; i++)
            {
                TestLCBInputs[i].CheckState = LEDStatus.Inputs[i] ? CheckState.Indeterminate : CheckState.Unchecked;
            }
            if (LEDStatus.LEDStatuses == null) LEDStatus.LEDStatuses = new bool[12];
            for (int l = 0; l < TestLCBLEDs.Length; l++)
            {
                TestLCBLEDs[l].CheckState = LEDStatus.LEDStatuses[l] ? CheckState.Indeterminate : CheckState.Unchecked;
            }
        }

        private void btnTestLCBWriteStatuses_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            LEDEquimpentStatus LEDStatus = new LEDEquimpentStatus();
            for (int o = 0; o < TestLCBOutputs.Length; o++)
            {
                LEDStatus.Outputs[o] = TestLCBOutputs[o].CheckState != CheckState.Unchecked;
            }

            for (int i = 0; i < TestLCBInputs.Length; i++)
            {
                LEDStatus.Inputs[i] = false;
            }

            if (LEDStatus.LEDStatuses == null) LEDStatus.LEDStatuses = new bool[12];
            for (int l = 0; l < TestLCBLEDs.Length; l++)
            {
                LEDStatus.LEDStatuses[l] = TestLCBLEDs[l].CheckState != CheckState.Unchecked;
            }

            var res = MainController.CreateCommandInstance(typeof(SetLCBEquipmentStatusCommand))
                .Wait(LEDStatus, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out bool result);
            //SendCommand(ModuleCommand.SetLCBEquipmentStatusRequest);
            /*var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => { return LEDParameters.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBEquipmentStatusResponse && 
                    LEDParameters.LastCommandResponseReceived > start; });*/
            if (!res || !result)
            {
                MessageBox.Show("Не удалось передать данные");
                return;
            }
        }


        private void btnTestLCBInit_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted) return;
            if (TestLCBConnected)
            {
                TestLCBStop();
            }
            else
            {
                TestLCBStart();

            }
        }

        private void btnTestLCBStop_Click(object sender, EventArgs e)
        {
            TestLCBStop();
        }

        private void TestLCBStart()
        {
            btnTestLCBInit.BackColor = Color.Gray;
            //Configuration = Configuration;
            if (!CurrentContext.StartLCB(MainController, WorkingLog))
            {
                btnTestLCBInit.BackColor = Color.Red;
                //DoMCNotInitializedErrorMessage();
                return;

            }
            TestLCBConnected = true;
            //CardsConnection.PacketLogActive = false;
            btnTestLCBInit.BackColor = Color.Green;
        }

        private void TestLCBStop()
        {
            CurrentContext.StopLCB(MainController, WorkingLog);
            //SendCommand(ModuleCommand.LCBStop);
            TestLCBTestStarted = false;
            TestLCBConnected = false;
            btnTestLCBInit.BackColor = SystemColors.Control;
        }


        private void btnTestLCBClearAll_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;

            for (int o = 0; o < TestLCBOutputs.Length; o++)
            {
                TestLCBOutputs[o].CheckState = CheckState.Unchecked;
            }
            for (int i = 0; i < TestLCBInputs.Length; i++)
            {
                TestLCBInputs[i].CheckState = CheckState.Unchecked;
            }
            for (int l = 0; l < TestLCBLEDs.Length; l++)
            {
                TestLCBLEDs[l].CheckState = CheckState.Unchecked;
            }
        }

        private void btnTestLCBSetAll_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;

            for (int o = 0; o < TestLCBOutputs.Length; o++)
            {
                TestLCBOutputs[o].CheckState = CheckState.Checked;
            }
            for (int i = 0; i < TestLCBInputs.Length; i++)
            {
                TestLCBInputs[i].CheckState = CheckState.Checked;
            }
            for (int l = 0; l < TestLCBLEDs.Length; l++)
            {
                TestLCBLEDs[l].CheckState = CheckState.Checked;
            }
        }


        private void btnTestLCBFullTest_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            TestLCBTestStarted = true;
            List<CheckBox> checkboxes = new List<CheckBox>();
            checkboxes.AddRange(TestLCBLEDs);
            checkboxes.AddRange(TestLCBOutputs);
            checkboxes.Add(null);
            LEDEquimpentStatus LEDStatus = new LEDEquimpentStatus();
            for (int o1 = 0; o1 < checkboxes.Count; o1++)
            {
                for (int i1 = 0; i1 < checkboxes.Count; i1++)
                {
                    if (checkboxes[i1] != null)
                        checkboxes[i1].CheckState = CheckState.Unchecked;
                }
                if (checkboxes[o1] != null)
                    checkboxes[o1].CheckState = CheckState.Checked;

                for (int o = 0; o < TestLCBOutputs.Length; o++)
                {
                    LEDStatus.Outputs[o] = TestLCBOutputs[o].CheckState != CheckState.Unchecked;
                }

                for (int i = 0; i < TestLCBInputs.Length; i++)
                {
                    LEDStatus.Inputs[i] = false;
                }

                if (LEDStatus.LEDStatuses == null) LEDStatus.LEDStatuses = new bool[12];
                for (int l = 0; l < TestLCBLEDs.Length; l++)
                {
                    LEDStatus.LEDStatuses[l] = TestLCBLEDs[l].CheckState != CheckState.Unchecked;
                }

                Application.DoEvents();
                var res = MainController.CreateCommandInstance(typeof(SetLCBEquipmentStatusCommand))
                    .Wait(new LEDEquimpentStatus() { Inputs = LEDStatus.Inputs, LEDStatuses = LEDStatus.LEDStatuses, Magnets = LEDStatus.Magnets, Outputs = LEDStatus.Outputs, Valve = LEDStatus.Valve }, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out bool result);

                if (!res || !result)
                {
                    MessageBox.Show("Не удалось передать данные");
                    return;
                }
                Thread.Sleep(500);
            }
            TestLCBTestStarted = false;

        }



        private void btnTestLCBGetCurrent_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            var res = MainController.CreateCommandInstance(typeof(GetLCBCurrentCommand))
                .Wait(null, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out int current);

            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            txbTestLCBCurrent.Text = current.ToString();
        }

        private void btnTestLCBSetCurrent_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            if (!int.TryParse(txbTestLCBCurrent.Text, out int current))
            {
                MessageBox.Show("Значение тока должно быть целым числом");
                txbTestLCBCurrent.Focus();
                return;
            }
            //LEDParameters.LEDCurrent = current ;
            var res = MainController.CreateCommandInstance(typeof(SetLCBCurrentCommand))
                .Wait(current, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out bool result);
            if (!res || !result)
            {
                MessageBox.Show("Не удалось передать данные");
                return;
            }
        }

        private void btnTestLCBGetMovementParameters_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            var res = MainController.CreateCommandInstance(typeof(GetLCBMovementParametersCommand))
                .Wait(null, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out LEDMovementParameters LEDParameters);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            SetImpulsesToTextBoxes(txbTestLCBPreformLength, txbTestLCBPreformLengthMm, LEDParameters.PreformLengthImpulses);
            SetImpulsesToTextBoxes(txbTestLCBDelayLength, txbTestLCBDelayLengthMm, LEDParameters.DelayLengthImpulses);

            //txbTestLCBPreformLength.Text = LEDParameters.PreformLengthImpulses.ToString();
            //txbTestLCBDelayLength.Text = LEDParameters.DelayLengthImpulses.ToString();

        }

        private void btnTestLCBSetMovementParameters_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            if (!int.TryParse(txbTestLCBPreformLength.Text, out int preformLength))
            {
                MessageBox.Show("Значение длины преформы в импульсах должно быть целым числом");
                txbTestLCBPreformLength.Focus();
                return;
            }
            if (!int.TryParse(txbTestLCBDelayLength.Text, out int delayLength))
            {
                MessageBox.Show("Значение расстояния задержки в импульсах должно быть целым числом");
                txbTestLCBDelayLength.Focus();
                return;
            }

            var res = MainController.CreateCommandInstance(typeof(SetLCBMovementParametersCommand))
                .Wait((preformLength, delayLength), CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out bool result);
            if (!res || !res)
            {
                MessageBox.Show("Не удалось передать данные");
                return;
            }
        }

        private void TestLCBSetWorkMode()
        {
            var res = MainController.CreateCommandInstance(typeof(SetLCBWorkModeCommand))
                .Wait(null, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out bool result);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            IsWorkingModeStarted = true;
            //txbTestLCBMaximumHorizontalStroke.Text = (LEDParameters.MaximumHorizontalStroke).ToString();
        }

        private void TestLCBSetSettingMode()
        {
            var res = MainController.CreateCommandInstance(typeof(SetLCBNonWorkModeCommand))
                .Wait(null, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out bool result);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            IsWorkingModeStarted = false;
            //txbTestLCBMaximumHorizontalStroke.Text = (LEDParameters.MaximumHorizontalStroke).ToString();
        }
        private void chbTestLCBWorkMode_CheckedChanged(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            if (chbTestLCBWorkMode.Checked)
            {
                chbTestLCBWorkMode.BackColor = Color.Green;
                TestLCBSetWorkMode();
            }
            else
            {
                chbTestLCBWorkMode.BackColor = Color.Transparent;
                TestLCBSetSettingMode();
            }
        }
        private void btnTestLCBGetMaxPosition_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            var res = MainController.CreateCommandInstance(typeof(GetLCBMaxPositionCommand))
                .Wait(null, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out int MaxPosition);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            txbTestLCBMaximumHorizontalStroke.Text = MaxPosition.ToString();
            if (CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient > 0)
            {
                txbTestLCBMaximumHorizontalStrokeMm.Text = (MaxPosition / CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient).ToString();
            }
        }

        System.Windows.Forms.Timer TestLCBGetCurrentPositionTimer;
        private void btnTestLCBGetCurrentPosition_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            if (TestLCBGetCurrentPositionTimer == null)
            {
                TestLCBGetCurrentPositionTimer = new System.Windows.Forms.Timer();
                TestLCBGetCurrentPositionTimer.Interval = 200;
                TestLCBGetCurrentPositionTimer.Tick += TestLCBGetCurrentPositionTimer_Tick;
                TestLCBGetCurrentPositionTimer.Start();
                TestLCBSetGetCurrentButtonActive(true);
            }
            else
            {
                TestLCBGetCurrentPositionTimer.Stop();
                TestLCBGetCurrentPositionTimer = null;
                TestLCBSetGetCurrentButtonActive(false);
            }
        }

        private void TestLCBGetCurrentPositionTimer_Tick(object sender, EventArgs e)
        {
            if (!TestLCBGetCurrentPosition())
            {
                if (TestLCBGetCurrentPositionTimer != null)
                {
                    TestLCBGetCurrentPositionTimer.Stop();
                    TestLCBGetCurrentPositionTimer = null;
                }
                TestLCBSetGetCurrentButtonActive(false);
                MessageBox.Show("Не удалось получить данные");
            }
        }
        private void TestLCBSetGetCurrentButtonActive(bool active)
        {
            Color clr;
            if (active)
                clr = Color.Green;
            else
                clr = SystemColors.Control;

            btnTestLCBGetCurrentPosition.BackColor = clr;
        }

        private bool TestLCBGetCurrentPosition()
        {
            if (TestLCBTestStarted | !TestLCBConnected) return false;
            var res = MainController.CreateCommandInstance(typeof(GetLCBCurrentPositionCommand))
                .Wait(null, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out int CurrentPosition);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return false;
            }

            txbTestLCBCurrentHorizontalStroke.Text = (CurrentPosition).ToString();
            if (CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient > 0)
            {
                txbTestLCBCurrentHorizontalStrokeMm.Text = (CurrentPosition / CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient).ToString();
            }

            return true;
        }
        private void txbTestLCBPreformLength_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            if (ctrl.Focused || LCBSettingsPreformLengthGotFromConfig)
            {
                LCBSettingsPreformLengthGotFromConfig = false;
                if (StringToDouble(ctrl.Text, out double value))
                {
                    SetImpulsesToTextBoxes(null, txbTestLCBPreformLengthMm, value);
                }
            }
        }

        private void txbTestLCBPreformLengthMm_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            if (ctrl.Focused)
            {
                if (StringToDouble(ctrl.Text, out double value))
                {
                    SetmmToTextBoxes(txbTestLCBPreformLength, null, value);
                }
            }

        }

        private void txbTestLCBDelayLength_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            if (ctrl.Focused || LCBSettingsDelayLengthGotFromConfig)
            {
                LCBSettingsDelayLengthGotFromConfig = false;
                if (StringToDouble(ctrl.Text, out double value))
                {
                    SetImpulsesToTextBoxes(null, txbTestLCBDelayLengthMm, value);
                }
            }
        }

        private void txbTestLCBDelayLengthMm_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            if (ctrl.Focused)
            {
                if (StringToDouble(ctrl.Text, out double value))
                {
                    SetmmToTextBoxes(txbTestLCBDelayLength, null, value);
                }
            }
        }

        private void SetImpulsesToTextBoxes(TextBox impTxb, TextBox mmTxb, double impulses)
        {

            if (mmTxb != null)
            {
                if (CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient > 0)
                {
                    mmTxb.Text = (impulses / CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient).ToString("F4");
                }
            }
            if (impTxb != null)
            {
                impTxb.Text = ((int)impulses).ToString("F0");
            }

        }

        private bool StringToDouble(string str, out double value)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                value = 0;
                return true;
            }
            var currCulture = System.Globalization.CultureInfo.CurrentCulture;
            var invCulture = System.Globalization.CultureInfo.InvariantCulture;
            if (double.TryParse(str, System.Globalization.NumberStyles.Any, currCulture, out value)
                        ||
                        double.TryParse(str, System.Globalization.NumberStyles.Any, invCulture, out value)
                        )
            {
                return true;
            }
            return false;
        }

        private void SetmmToTextBoxes(TextBox impTxb, TextBox mmTxb, double mm)
        {
            if (impTxb != null)
            {
                if (CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient > 0)
                {
                    impTxb.Text = ((int)(mm * CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient)).ToString();
                }
            }
            if (mmTxb != null)
            {
                mmTxb.Text = mm.ToString("F4");
            }
        }



        private void btnLCBSaveToConfig_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txbTestLCBPreformLength.Text, out int preformLength))
            {
                MessageBox.Show("Значение длины преформы в импульсах должно быть целым числом");
                txbTestLCBPreformLength.Focus();
                return;
            }
            if (!int.TryParse(txbTestLCBDelayLength.Text, out int delayLength))
            {
                MessageBox.Show("Значение расстояния задержки в импульсах должно быть целым числом");
                txbTestLCBDelayLength.Focus();
                return;
            }
            if (!int.TryParse(txbTestLCBCurrent.Text, out int current))
            {
                MessageBox.Show("Значение тока должно быть целым числом");
                txbTestLCBCurrent.Focus();
                return;
            }
            if (CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings == null)
                CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings = new LCBSettings() { LCBKoefficient = 75.2 };
            CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LEDCurrent = current;
            CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.DelayLength = delayLength;
            CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.PreformLength = preformLength;
        }

        private void btnLCBLoadFromConfig_Click(object sender, EventArgs e)
        {
            txbTestLCBCurrent.Text = CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LEDCurrent.ToString();
            SetImpulsesToTextBoxes(txbTestLCBPreformLength, txbTestLCBPreformLengthMm, CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.PreformLength);
            SetImpulsesToTextBoxes(txbTestLCBDelayLength, txbTestLCBDelayLengthMm, CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.DelayLength);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - LastSinchrosignal < SinchroSignalDisplayTimeout)
            {
                cbTestLCBSynchrosignal.CheckState = CheckState.Indeterminate;
            }
            else
            {
                cbTestLCBSynchrosignal.CheckState = CheckState.Unchecked;

            }
        }

        private void txbInput_DoubleClick(object sender, EventArgs e)
        {
            var txb = (sender as TextBox);
            string title = "";
            if (txb == txbTestLCBCurrent) title = "тока";
            if (txb == txbTestLCBDelayLength) title = "длина задержка";
            if (txb == txbTestLCBDelayLengthMm) title = "длина задержки в мм";
            if (txb == txbTestLCBPreformLength) title = "длина преформы в имп";
            if (txb == txbTestLCBPreformLengthMm) title = "длина преформы в мм";
            int.TryParse(txb.Text, out int Current);
            var newvalue = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog($"Ввод значения {title}", false, Current);
            if (newvalue >= 0)
                txb.Text = newvalue.ToString();
        }

    }
}
