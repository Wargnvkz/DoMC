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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            /*var EventNameParts = EventName.Split('.');
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
            }*/
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
            SetRDPBParameters();
            FillPage();
        }
        private void FillPage()
        {

        }
        private void SendCommands(Type Command, object data = null)
        {
            var cmd = MainController.CreateCommandInstance(Command);
            var res = cmd.Wait(data, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, out CurrentStatus);
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();

        }
        private void TestRDPBStop()
        {
            var cmd = MainController.CreateCommandInstance(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.StopCommand));
            var res = cmd.Wait(null, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, out object? _);
            if (res)
            {
                TestRDPBConnected = false;
                btnRDPBTestConnect.BackColor = SystemColors.Control;
                btnRDPBTestConnect.Text = "Подключить";
            }
            else
            {
                DisplayMessage.Show("Ошибка при отлючении", "Ошибка");
            }
        }
        private void TestRDPBStart()
        {
            var cmd = MainController.CreateCommandInstance(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.StartCommand));
            var res = cmd.Wait(null, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, out object? _);
            if (res)
            {
                TestRDPBConnected = true;
                btnRDPBTestConnect.BackColor = Color.Green;
                btnRDPBTestConnect.Text = "Отключить";
            }
            else
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                DisplayMessage.Show("Не удалось подключиться к бракеру", "Ошибка");
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

        private void SetRDPBParameters()
        {
            var cmd = MainController.CreateCommandInstance(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.LoadConfigurationToModuleCommand));
            var res = cmd.Wait(CurrentContext.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, out bool result);
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
        }

        private void btnTestRDPBN80_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            SendCommands(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.SendSetIsOkCommand));
        }

        private void btnTestRDPBN81_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            SendCommands(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.SendSetIsBadCommand));

        }

        private void btnTestRDPBN82_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            SendCommands(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.TurnOnCommand));

        }

        private void btnTestRDPBN83_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            SendCommands(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.TurnOffCommand));

        }

        private void btnTestRDPBN90_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            SendCommands(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.GetParametersCommand));

        }

        private void btnTestRDPBSendCommand_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            SendCommands(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.SendManualCommand), txbTestRDPBManualCommand.Text);

        }

        private void cbTestRDPBCoolingBlocksQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            var CoolingBlocksQuantityToSet = int.Parse(cbTestRDPBCoolingBlocksQuantity.SelectedItem?.ToString() ?? "0");
            SendCommands(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.SetCoolingBlockQuantityCommand), CoolingBlocksQuantityToSet);

        }

        private void TestRDPBStatusFill()
        {
            lvTestRDPBStatuses.Items.Clear();
            if (CurrentStatus == null) return;
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Короб",
                CurrentStatus.BoxDirection == BoxDirectionType.Left ? "Левый" :
                CurrentStatus.BoxDirection == BoxDirectionType.Right ? "Правый" :
                "Неизвестно"
            }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Номер короба", CurrentStatus.BoxNumber.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Бракер ", (CurrentStatus.BlockIsOn ? "Включен" : "Выключен") }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Охлаждающих блоков", CurrentStatus.CoolingBlocksQuantity.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Номер цикла", CurrentStatus.CycleNumber.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Забракованных съемов", CurrentStatus.BadSetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Хороших съемов", CurrentStatus.GoodSetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Съемов в коробе", CurrentStatus.SetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Направление ленты",
                CurrentStatus.TransporterSide == RDPBTransporterSide.Stoped ? "Стоит" :
                CurrentStatus.TransporterSide == RDPBTransporterSide.Left ? "Влево" :
                CurrentStatus.TransporterSide == RDPBTransporterSide.Right ? "Вправо" :
                "Ошибка датчика"
            }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Ошибки",
                CurrentStatus.Errors == RDPBErrors.NoErrors ? "Ошибок нет" :
                CurrentStatus.Errors == RDPBErrors.TransporterDriveUnit ? "Авария привода конвейера" :
                CurrentStatus.Errors == RDPBErrors.SensorOfInitialState ? "Авария датчика исходного состояния" :
                "Неизвестная ошибка"
            }));
            txbTestRDPBCoolingBlocksStatus.Text = CurrentStatus.CoolingBlocksQuantity.ToString();

        }

    }
}
