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
using DoMC.Classes;
using DoMCLib.Classes.Module.LCB;
using static DoMCLib.Classes.Module.LCB.LCBModule;
using DoMCLib.Classes.Module.RDPB.Classes;

namespace DoMC.UserControls
{
    public partial class TestRDPBControl : UserControl
    {
        IMainController MainController;
        ILogger WorkingLog;
        IDoMCSettingsUpdatedProvider SettingsUpdateProvider;
        Observer CurrentObserver;
        DoMCLib.Classes.DoMCApplicationContext CurrentContext;
        bool TestRDPBConnected = false;
        private RDPBStatus CurrentStatus;
        bool IsStarted = false;

        public TestRDPBControl(IMainController Controller, ILogger logger, DoMC.Classes.IDoMCSettingsUpdatedProvider settingsUpdateProvider)
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
            var EventNameParts = EventName.Split('.');
            if (EventNameParts[0] == nameof(DoMCLib.Classes.Module.RDPB.RDPBModule))
            {
                if (Enum.TryParse(EventNameParts[2], out StatusStringProccessResult result))
                {
                    if (result == StatusStringProccessResult.OK)
                    {
                        if (Enum.TryParse(EventNameParts[1], out RDPBCommandType command))
                        {
                            if (data is RDPBStatus cs)
                            {
                                CurrentStatus = cs;
                            }
                        }
                    }
                }
            }
        }

        private void SettingsUpdateProvider_SettingsUpdated(object? sender, EventArgs e)
        {
            var context = SettingsUpdateProvider.GetContext();
            if (TestRDPBConnected)
                TestRDPBStop();
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

        private void TestRDPBStop()
        {
            var cmd = MainController.CreateCommandInstance(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.StopCommand));
            cmd.ExecuteCommand();
            if (!IsStarted)
            {
                TestRDPBConnected = false;
                btnRDPBTestConnect.BackColor = SystemColors.Control;
                btnRDPBTestConnect.Text = "Подключить";
            }

        }
        private void TestRDPBStart()
        {
            var cmd = MainController.CreateCommandInstance(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.StopCommand));
            cmd.ExecuteCommand();
            if (IsStarted)
            {
                TestRDPBConnected = true;
                btnRDPBTestConnect.BackColor = Color.Green;
                btnRDPBTestConnect.Text = "Отключить";
            }

        }

        private void btnRDPBTestConnect_Click(object sender, EventArgs e)
        {
            if (TestRDPBConnected)
            {
                TestRDPBStop();
            }
            else
            {
                TestRDPBStart();
                TestRDPBStatusFill();
            }
        }

        private void btnTestRDPBN80_Click(object sender, EventArgs e)
        {

            if (!TestRDPBConnected) return;
            this.Enabled = false;
            var cmd = MainController.CreateCommandInstance(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.SendSetIsOkCommand));



            Context.SendCommand(ModuleCommand.RDPBSetIsOK);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetIsOK && Context.RDPBCurrentStatus.IsCurrentStatusActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;

        }

        private void btnTestRDPBN81_Click(object sender, EventArgs e)
        {

            //if (!TestRDPBConnected) return;
            this.Enabled = false;
            Context.SendCommand(ModuleCommand.RDPBSetIsBad);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetIsBad && Context.RDPBCurrentStatus.IsCurrentStatusActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;

        }

        private void btnTestRDPBN82_Click(object sender, EventArgs e)
        {

            if (!TestRDPBConnected) return;
            Context.SendCommand(ModuleCommand.RDPBOn);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.On);
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;

        }

        private void btnTestRDPBN83_Click(object sender, EventArgs e)
        {

            if (!TestRDPBConnected) return;
            this.Enabled = false;
            Context.SendCommand(ModuleCommand.RDPBOff);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.Off);
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;

        }

        private void btnTestRDPBN90_Click(object sender, EventArgs e)
        {

            if (!TestRDPBConnected) return;
            this.Enabled = false;
            Context.SendCommand(ModuleCommand.RDPBGetParameters);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.GetParameters && Context.RDPBCurrentStatus.IsParametersActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;

        }

        private void btnTestRDPBSendCommand_Click(object sender, EventArgs e)
        {

            if (!TestRDPBConnected) return;
            Context.RDPBCurrentStatus.ManualCommand = txbTestRDPBManualCommand.Text;
            Context.SendCommand(ModuleCommand.RDPBSendManualCommand);
            Thread.Sleep(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds / 10);
            TestRDPBStatusFill();

        }

        private void cbTestRDPBCoolingBlocksQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!TestRDPBConnected) return;
            Context.RDPBCurrentStatus.CoolingBlocksQuantityToSet = int.Parse(cbTestRDPBCoolingBlocksQuantity.SelectedItem?.ToString() ?? "0");
            Context.SendCommand(ModuleCommand.RDPBSetCoolingBlockQuantity);
            UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetCoolingBlocks && Context.RDPBCurrentStatus.IsParametersActual());
            TestRDPBStatusFill();

        }

        private void TestRDPBStatusFill()
        {

            lvTestRDPBStatuses.Items.Clear();
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Короб",
                Context.RDPBCurrentStatus.BoxDirection == DoMCLib.Classes.BoxDirectionType.Left ? "Левый" :
                Context.RDPBCurrentStatus.BoxDirection == DoMCLib.Classes.BoxDirectionType.Right ? "Правый" :
                "Неизвестно"
            }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Номер короба", Context.RDPBCurrentStatus.BoxNumber.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Бракер ", (Context.RDPBCurrentStatus.BlockIsOn ? "Включен" : "Выключен") }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Охлаждающих блоков", Context.RDPBCurrentStatus.CoolingBlocksQuantity.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Номер цикла", Context.RDPBCurrentStatus.CycleNumber.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Забракованных съемов", Context.RDPBCurrentStatus.BadSetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Хороших съемов", Context.RDPBCurrentStatus.GoodSetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Съемов в коробе", Context.RDPBCurrentStatus.SetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Направление ленты",
                Context.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Stoped ? "Стоит" :
                Context.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Left ? "Влево" :
                Context.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Right ? "Вправо" :
                "Ошибка датчика"
            }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Ошибки",
                Context.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.NoErrors ? "Ошибок нет" :
                Context.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.TransporterDriveUnit ? "Авария привода конвейера" :
                Context.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.SensorOfInitialState ? "Авария датчика исходного состояния" :
                "Неизвестная ошибка"
            }));
            txbTestRDPBCoolingBlocksStatus.Text = Context.RDPBCurrentStatus.CoolingBlocksQuantity.ToString();

        }

    }
}
