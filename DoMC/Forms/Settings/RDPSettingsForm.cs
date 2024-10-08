using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Forms
{
    public partial class RDPSettingsForm : Form
    {
        public IPAddress IPAddress
        {
            get { return IPAddress.Parse(txbRDPIP.Text); }
            set { txbRDPIP.Text = value.ToString(); }
        }
        public int IPPort
        {
            get { return int.Parse(txbIPPort.Text); }
            set { txbIPPort.Text = value.ToString(); }
        }
        public int CoolingBlocks
        {
            get { return cbCoolingBlock.SelectedItem!=null?int.Parse(cbCoolingBlock.SelectedItem.ToString()):3; }
            set
            {
                var ind = Array.IndexOf(CoolingBlocksQuantity, value);
                cbCoolingBlock.SelectedIndex = ind;
            }
        }

        public int MachineNumber
        {
            get
            {
                int.TryParse(txbMachineNumber.Text, out int machineNumber);
                return machineNumber;
            }
            set
            {
                txbMachineNumber.Text = value.ToString();
            }
        }
        public bool SendBadCycleToRDPB
        {
            get { return cbRDPBSendBadCycle.Checked; }
            set { cbRDPBSendBadCycle.Checked = value; }
        }
        int[] CoolingBlocksQuantity = new int[] { 3, 4 };
        public RDPSettingsForm()
        {
            InitializeComponent();
            cbCoolingBlock.Items.Clear();
            foreach (var cb in CoolingBlocksQuantity)
            {
                cbCoolingBlock.Items.Add(cb);
            }
        }

        private void txbRDPIP_Validating(object sender, CancelEventArgs e)
        {
            if (!IPAddress.TryParse(txbRDPIP.Text, out IPAddress _))
            {
                epError.SetError(txbRDPIP, "Должен быть IP адрес");
                e.Cancel = true;
            }
            else
            {
                epError.Clear();
                e.Cancel = false;
            }
        }

        private void txbIPPort_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(txbIPPort.Text, out int v) || v < 1 || v > 65535)
            {
                epError.SetError(txbIPPort, "Должно быть значение от 1 до 65535");
                e.Cancel = true;
            }
            else
            {
                epError.Clear();
                e.Cancel = false;
            }

        }

        private void txbMachineNumber_DoubleClick(object sender, EventArgs e)
        {
            int.TryParse(txbMachineNumber.Text, out int machineNumber);
            var value = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog("Введите номер линии", false, machineNumber);
            if (value >= 0)
            {
                txbMachineNumber.Text = value.ToString();
            }
        }

        private void cbCoolingBlock_Validating(object sender, CancelEventArgs e)
        {
            if (cbCoolingBlock.SelectedItem == null) e.Cancel = true;
        }

        private void RDPSettingsForm_Validating(object sender, CancelEventArgs e)
        {
            if (cbCoolingBlock.SelectedItem == null) e.Cancel = true;
        }
    }
}
