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
        LEDDataExchangeStatus LEDStatus = new LEDDataExchangeStatus();
        bool IsWorkingModeStarted = false;
        bool LCBSettingsPreformLengthGotFromConfig = false;
        bool LCBSettingsDelayLengthGotFromConfig = false;

        public TestLCBInterface(IMainController Controller, ILogger logger, DoMC.Classes.IDoMCSettingsUpdatedProvider settingsUpdateProvider)
        {
            InitializeComponent();
            MainController = Controller;
            WorkingLog = logger;
            SettingsUpdateProvider = settingsUpdateProvider;
            SettingsUpdateProvider.SettingsUpdated += SettingsUpdateProvider_SettingsUpdated;
            CurrentObserver = MainController.GetObserver();
            CurrentObserver.NotificationReceivers += Observer_NotificationReceivers;
            Disposed += OnDispose;

            //MainController.GetObserver().NotificationReceivers += GetCCDStandardInterface_NotificationReceivers;
        }

        private void Observer_NotificationReceivers(string EventName, object? data)
        {
            if (EventName.EndsWith($"{LEDCommandType.LEDSynchrosignalResponse}.{EventType.Received}"))
            {
                LastSinchrosignal = DateTime.Now;
            }
        }

        private void SettingsUpdateProvider_SettingsUpdated(object? sender, EventArgs e)
        {
            var context = SettingsUpdateProvider.GetContext();
            if (TestLCBConnected)
                TestLCBStop();
            ApplyNewContext(context);
        }
        private void OnDispose(object? sender, EventArgs e)
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
        }
        private void ApplyNewContext(DoMCLib.Classes.DoMCApplicationContext context)
        {
            CurrentContext = context;
            FillPage();
        }
        private void FillPage()
        {

        }

        #region TestLCB
        CheckBox[] TestLCBOutputs;
        CheckBox[] TestLCBInputs;
        CheckBox[] TestLCBLEDs;
        private void btnTestLCBReadStatuses_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;

            var start = DateTime.Now;

            var res=MainController.CreateCommandInstance(typeof(GetLCBEquipmentStatusCommand))
                .Wait(this, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out object _);
            //GetLCBEquipmentStatusCommand

            /*SendCommand(ModuleCommand.GetLCBEquipmentStatusRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => { return LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBEquipmentStatusResponse && 
                    LEDStatus.LastCommandResponseReceived > start; });*/

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

            var start = DateTime.Now;
            var res = MainController.CreateCommandInstance(typeof(SetLCBEquipmentStatusCommand))
                .Wait(this, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out object _);
            SendCommand(ModuleCommand.SetLCBEquipmentStatusRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => { return LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBEquipmentStatusResponse && 
                    LEDStatus.LastCommandResponseReceived > start; });
            if (!res || !LEDStatus.LastCommandReceivedStatusIsOK)
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
            if (!CurrentContext.SetLCBWorkingParameters(MainController, WorkingLog))
            {
                btnTestLCBInit.BackColor = Color.Red;
                //DoMCNotInitializedErrorMessage();
                return;
            }
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
            if (LEDStatus.LEDStatuses == null) LEDStatus.LEDStatuses = new bool[12];
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
            if (LEDStatus.LEDStatuses == null) LEDStatus.LEDStatuses = new bool[12];
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
                var start = DateTime.Now;
                SendCommand(ModuleCommand.SetLCBEquipmentStatusRequest);
                var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                    () => { return LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBEquipmentStatusResponse && 
                        LEDStatus.LastCommandResponseReceived > start; });
                if (!res || !LEDStatus.LastCommandReceivedStatusIsOK)
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
            var start = DateTime.Now;
            SendCommand(ModuleCommand.GetLCBCurrentRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => { return LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLEDCurrentResponse && 
                    LEDStatus.LastCommandResponseReceived > start; });
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            txbTestLCBCurrent.Text = (LEDStatus.LEDCurrent).ToString();
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
            var start = DateTime.Now;
            //LEDStatus.LEDCurrent = current ;
            CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LEDCurrent = current;
            SendCommand(ModuleCommand.SetLCBCurrentRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => { return LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLEDCurrentResponse && 
                    LEDStatus.LastCommandResponseReceived > start; });
            if (!res || !LEDStatus.LastCommandReceivedStatusIsOK)
            {
                MessageBox.Show("Не удалось передать данные");
                return;
            }
        }

        private void btnTestLCBGetMovementParameters_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            var start = DateTime.Now;
            SendCommand(ModuleCommand.GetLCBMovementParametersRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => { return LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBMovementParametersResponse && 
                    LEDStatus.LastCommandResponseReceived > start; });
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            SetImpulsesToTextBoxes(txbTestLCBPreformLength, txbTestLCBPreformLengthMm, LEDStatus.PreformLength);
            SetImpulsesToTextBoxes(txbTestLCBDelayLength, txbTestLCBDelayLengthMm, LEDStatus.DelayLength);

            txbTestLCBPreformLength.Text = LEDStatus.PreformLength.ToString();
            txbTestLCBDelayLength.Text = LEDStatus.DelayLength.ToString();

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
            CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.PreformLength = preformLength;
            CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.DelayLength = delayLength;
            var start = DateTime.Now;
            SendCommand(ModuleCommand.SetLCBMovementParametersRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds
                , () =>
                {
                    return LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBMovementParametersResponse && 
                    LEDStatus.LastCommandResponseReceived > start;
                });
            if (!res || !LEDStatus.LastCommandReceivedStatusIsOK)
            {
                MessageBox.Show("Не удалось передать данные");
                return;
            }
        }

        private void TestLCBSetWorkMode()
        {
            var start = DateTime.Now;
            IsWorkingModeStarted = true;
            SendCommand(ModuleCommand.SetLCBWorkModeRequest);
            IsWorkingModeStarted = false;
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBWorkModeResponse && 
                LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            //txbTestLCBMaximumHorizontalStroke.Text = (LEDStatus.MaximumHorizontalStroke).ToString();
        }

        private void TestLCBSetSettingMode()
        {
            var start = DateTime.Now;
            IsWorkingModeStarted = false;
            SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBWorkModeResponse && 
                LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            //txbTestLCBMaximumHorizontalStroke.Text = (LEDStatus.MaximumHorizontalStroke).ToString();
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
            var start = DateTime.Now;
            SendCommand(ModuleCommand.GetLCBMaxPositionRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBMaxHorizontalStrokeResponse && 
                LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            txbTestLCBMaximumHorizontalStroke.Text = (LEDStatus.MaximumHorizontalStroke).ToString();
            if (CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient > 0)
            {
                txbTestLCBMaximumHorizontalStrokeMm.Text = (LEDStatus.MaximumHorizontalStroke / CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient).ToString();
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
            var start = DateTime.Now;
            SendCommand(ModuleCommand.GetLCBCurrentPositionRequest);
            var res = UserInterfaceControls.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, 
                () => LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBCurrentHorizontalStrokeResponse && 
                LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                return false;
            }
            txbTestLCBCurrentHorizontalStroke.Text = (LEDStatus.CurrentHorizontalStroke).ToString();
            if (CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient > 0)
            {
                txbTestLCBCurrentHorizontalStrokeMm.Text = (LEDStatus.CurrentHorizontalStroke / CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient).ToString();
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

        #endregion TestLCB


        private void btnLCBSaveToConfig_Click(object sender, EventArgs e)
        {

        }

        private void btnLCBLoadFromConfig_Click(object sender, EventArgs e)
        {

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
    }
}
