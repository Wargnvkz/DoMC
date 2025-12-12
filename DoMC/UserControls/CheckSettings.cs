using DoMC.Classes;
using DoMC.Tools;
using DoMCLib.Classes;
using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoMC.UserControls
{
    public partial class CheckSettings : UserControl
    {
        IMainController MainController;
        ILogger WorkingLog;
        IDoMCSettingsUpdatedProvider SettingsUpdateProvider;
        DoMCLib.Classes.DoMCApplicationContext CurrentContext;
        bool?[] CardsChecks = new bool?[12];
        int checkTimeout = 5;
        Panel[] SocketsSettingsSocketsPanelList;
        public CheckSettings(IMainController Controller, ILogger logger, DoMC.Classes.IDoMCSettingsUpdatedProvider settingsUpdateProvider)
        {
            InitializeComponent();
            MainController = Controller;
            WorkingLog = logger;
            SettingsUpdateProvider = settingsUpdateProvider;
            SettingsUpdateProvider.SettingsUpdated += SettingsUpdateProvider_SettingsUpdated;
            Disposed += OnDispose;
            SocketsSettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(96, ref pnlSocketConfigurationSet);

            //MainController.GetObserver().NotificationReceivers += GetCCDStandardInterface_NotificationReceivers;
        }
        private async Task SettingsUpdateProvider_SettingsUpdated(object sender)
        {
            var context = SettingsUpdateProvider.GetContext();
            ApplyNewContext(context);
        }

        private void ApplyNewContext(DoMCLib.Classes.DoMCApplicationContext context)
        {
            CurrentContext = context;
            FillPage();
        }
        private void FillPage()
        {
            lvDoMCCards.Items.Clear();
            for (int i = 0; i < 12; i++)
            {
                Color color;
                if (CardsChecks[i] != null)
                {
                    if (CardsChecks[i].Value)
                    {
                        color = Color.LimeGreen;
                    }
                    else
                    {
                        color = Color.FromArgb(255, 255, 128, 128);
                    }
                }
                else
                {
                    color = Color.White;
                }

                var lvi = new ListViewItem(new string[] { (i + 1).ToString(), "🗸", CCDCardTCPClient.GetCardIPAddress(i + 1), String.Join(", ", CurrentContext.Configuration.HardwareSettings.CardSocket2EquipmentSocket.Skip(i * 8).Take(8)) });
                lvi.BackColor = color;
                lvDoMCCards.Items.Add(lvi);
            }
        }

        private async void btnSettingsCheckCardStatus_Click(object sender, EventArgs e)
        {
            var result = await DoMCEquipmentCommands.TestCards(MainController, CurrentContext, WorkingLog);
            //if (result.Item1)
            //{
            for (int i = 0; i < result.Item2.requested.Length; i++)
            {
                if (result.Item2.requested[i])
                {
                    CardsChecks[i] = result.Item2.answered[i];
                }
            }
            /*}
            else
            {
                for (int i = 0; i < result.Item2.requested.Length; i++)
                {
                    CardsChecks[i] = false;
                }

            }*/
            FillPage();

        }

        private void btnCheckSettings_Click(object sender, EventArgs e)
        {
            CheckSettingsDisplay();
        }
        private void CheckSettingsDisplay()
        {
            var NotSetColor = Color.Red;
            var SetColor = Color.LimeGreen;
            var PartiallySetColor = Color.Yellow;
            var NotToCheckColor = Color.Gray;
            var status = CurrentContext.Configuration.GetConfigurationFillStatus();

            SetControlColors(lblLCBParameters, status.IsLCBParametersSet ? SetColor : NotSetColor);

            SetControlColors(lblDBSet, status.IsDBSettingsSet ? SetColor : NotSetColor);

            SetControlColors(lblStandardRecalculationSettingsSet, status.IsStandardRecalculationSettingsSet ? SetColor : NotSetColor);

            SetControlColors(lblRemoveDefectedPreformBlockConfigSet, status.IsRemoveDefectedPreformBlockConfigSet ? SetColor : NotSetColor);

            if (status.SocketParametersStatus != null)
                for (int i = 0; i < 96; i++)
                {
                    if (status.SocketParametersStatus.Length > i)
                    {
                        if (!status.SocketParametersStatus[i].IsInCheck)
                        {
                            SetControlColors(SocketsSettingsSocketsPanelList[i], NotToCheckColor);
                        }
                        else
                        {
                            var socketStat = (status.SocketParametersStatus[i].IsReadingParametersSet ? 1 : 0) +
                            (status.SocketParametersStatus[i].IsWindowsCheckSet ? 1 : 0) +
                            (status.SocketParametersStatus[i].IsMakeDecisionOperationsSet ? 1 : 0);
                            SetControlColors(SocketsSettingsSocketsPanelList[i], socketStat == 3 ? SetColor : (socketStat == 0 ? NotSetColor : PartiallySetColor));
                        }
                    }
                    else
                    {
                        SetControlColors(SocketsSettingsSocketsPanelList[i], NotToCheckColor);
                    }
                }
        }
        private void SetControlColors(Control control, Color backColor)
        {
            control.BackColor = backColor;
            control.ForeColor = GetContrastColor(backColor);
        }
        private Color GetContrastColor(Color backgroundColor)
        {
            // Calculate the luminance (brightness) using a weighted formula
            // A common formula uses these coefficients to match human perception of brightness
            double luminance = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255;

            // Use black text for light backgrounds and white text for dark backgrounds
            if (luminance > 0.5) // The threshold can be adjusted (e.g., 0.5, 0.6)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
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
    }
}
