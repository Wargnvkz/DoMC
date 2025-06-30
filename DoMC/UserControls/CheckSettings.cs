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
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMC.Tools;
using DoMCLib.Tools;
using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes;

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
        public CheckSettings(IMainController Controller, ILogger logger, DoMC.Classes.IDoMCSettingsUpdatedProvider settingsUpdateProvider)
        {
            InitializeComponent();
            MainController = Controller;
            WorkingLog = logger;
            SettingsUpdateProvider = settingsUpdateProvider;
            SettingsUpdateProvider.SettingsUpdated += SettingsUpdateProvider_SettingsUpdated;
            Disposed += OnDispose;
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
