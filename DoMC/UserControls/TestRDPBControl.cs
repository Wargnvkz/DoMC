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
using DoMCLib.Classes.Module.RDPB;

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
        private async Task Observer_NotificationReceivers(string EventName, object? data)
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

        private async Task SettingsUpdateProvider_SettingsUpdated(object? sender)
        {
            var context = SettingsUpdateProvider.GetContext();
            if (TestRDPBConnected)
                await TestRDPBStop();
            await ApplyNewContext(context);
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
        private async Task ApplyNewContext(DoMCLib.Classes.DoMCApplicationContext context)
        {
            CurrentContext = context;
            await SetRDPBParameters();
            FillPage();
        }
        private void FillPage()
        {

        }
        /*private async Task SendCommands( Command, object data = null)
        {
            var cmd = MainController.CreateCommandInstance(Command);

            var res = cmd.Wait(data, CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, out CurrentStatus);
            if (!res)
            {

            }

        }*/
        private async Task TestRDPBStop()
        {
            try
            {
                await new DoMCLib.Classes.Module.RDPB.Commands.RDPBStopCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
                TestRDPBConnected = false;
                btnRDPBTestConnect.BackColor = SystemColors.Control;
                btnRDPBTestConnect.Text = "Подключить";
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при отлючении", ex);
                DisplayMessage.Show("Ошибка при отлючении", "Ошибка");

            }


        }
        private async Task TestRDPBStart()
        {
            try
            {
                await new DoMCLib.Classes.Module.RDPB.Commands.RDPBStartCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
                TestRDPBConnected = true;
                btnRDPBTestConnect.BackColor = Color.Green;
                btnRDPBTestConnect.Text = "Отключить";
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось подключиться к бракеру", ex);
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                DisplayMessage.Show("Не удалось подключиться к бракеру", "Ошибка");
            }

        }

        private async void btnRDPBTestConnect_Click(object sender, EventArgs e)
        {
            if (TestRDPBConnected)
            {
                await TestRDPBStop();
            }
            else
            {
                await TestRDPBStart();
                TestRDPBStatusFill();
            }
        }

        private async Task SetRDPBParameters()
        {
            try
            {
                await new DoMCLib.Classes.Module.RDPB.Commands.SendConfigurationToModuleCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync(CurrentContext.Configuration);
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось оправить команду", ex);
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                await TestRDPBStop();
            }
            finally
            {
                TestRDPBStatusFill();
            }

        }

        private async void btnTestRDPBN80_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            try
            {
                CurrentStatus = await new DoMCLib.Classes.Module.RDPB.Commands.SendSetIsOkCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось оправить команду", ex);
                MessageBox.Show("Не удалось оправить команду");
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                await TestRDPBStop();
            }
            finally
            {
                TestRDPBStatusFill();
            }
        }

        private async void btnTestRDPBN81_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            try
            {
                CurrentStatus = await new DoMCLib.Classes.Module.RDPB.Commands.SendSetIsBadCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось оправить команду", ex);
                MessageBox.Show("Не удалось оправить команду");
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                await TestRDPBStop();
            }
            finally
            {
                TestRDPBStatusFill();
            }

        }

        private async void btnTestRDPBN82_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            try
            {
                CurrentStatus = await new DoMCLib.Classes.Module.RDPB.Commands.TurnOnCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось оправить команду", ex);
                MessageBox.Show("Не удалось оправить команду");
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                await TestRDPBStop();
            }
            finally
            {
                TestRDPBStatusFill();
            }

        }

        private async void btnTestRDPBN83_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            try
            {
                CurrentStatus = await new DoMCLib.Classes.Module.RDPB.Commands.TurnOffCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось оправить команду", ex);
                MessageBox.Show("Не удалось оправить команду");
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                await TestRDPBStop();
            }
            finally
            {
                TestRDPBStatusFill();
            }

        }

        private async void btnTestRDPBN90_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            try
            {
                CurrentStatus = await new DoMCLib.Classes.Module.RDPB.Commands.GetParametersCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось оправить команду", ex);
                MessageBox.Show("Не удалось оправить команду");
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                await TestRDPBStop();
            }
            finally
            {
                TestRDPBStatusFill();
            }

        }

        private async void btnTestRDPBSendCommand_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            try
            {
                await new DoMCLib.Classes.Module.RDPB.Commands.SendManualCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync(txbTestRDPBManualCommand.Text);
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось оправить команду", ex);
                MessageBox.Show("Не удалось оправить команду");
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                await TestRDPBStop();
            }
            finally
            {
                TestRDPBStatusFill();
            }

        }

        private async void cbTestRDPBCoolingBlocksQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            var CoolingBlocksQuantityToSet = int.Parse(cbTestRDPBCoolingBlocksQuantity.SelectedItem?.ToString() ?? "0");
            try
            {
                CurrentStatus = await new DoMCLib.Classes.Module.RDPB.Commands.SetCoolingBlockQuantityCommand(MainController, MainController.GetModule(typeof(RDPBModule))).ExecuteCommandAsync(CoolingBlocksQuantityToSet);
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Не удалось оправить команду", ex);
                MessageBox.Show("Не удалось оправить команду");
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                await TestRDPBStop();
            }
            finally
            {
                TestRDPBStatusFill();
            }

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
