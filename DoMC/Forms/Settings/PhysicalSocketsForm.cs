using DoMCLib.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Forms
{
    public partial class PhysicalSocketsForm : Form
    {
        List<DisplaySockets2PhysicalSockets.Cards> _Sockets;
        public DisplaySockets2PhysicalSockets DisplaySockets2PhysicalSockets
        {
            get
            {
                DisplaySockets2PhysicalSockets ds2ps = new DisplaySockets2PhysicalSockets();
                ds2ps.SetSocketsByCards(_Sockets);
                return ds2ps;
            }
            set
            {
                _Sockets = value.GetSocketsByCards();
                dgvSockets.DataSource = _Sockets;
                for (int i=1;i< dgvSockets.Columns.Count; i++)
                {
                    dgvSockets.Columns[i].HeaderText= $"Плата {i}";
                    dgvSockets.Columns[i].Width = 80;
                }
                var socketColumn = dgvSockets.Columns["Socket"];
                socketColumn.HeaderText = "Гнездо";
                socketColumn.ReadOnly = true;
                socketColumn.Width = 70;
                dgvSockets.RowHeadersVisible = false;
                
            }
        }
        public PhysicalSocketsForm()
        {
            InitializeComponent();
        }

        private void dgvSockets_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.CellStyle.BackColor = Color.LightBlue;
            }
        }

        private bool CheckSockets()
        {
            HashSet<int> sockets = new HashSet<int>();
            for(int i = 0; i < _Sockets.Count; i++)
            {
                var socketsInCards = _Sockets[i].Array;
                for(int card = 0; card < socketsInCards.Length; card++)
                {
                    var displaySocket = socketsInCards[card];
                    if (!sockets.Contains(displaySocket))
                    {
                        sockets.Add(displaySocket);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void dgvSockets_Validating(object sender, CancelEventArgs e)
        {
            var result=CheckSockets();
            e.Cancel = !result;
            epSockets.Clear();
            if (!result)
                epSockets.SetError(btnOK, "Не должно быть повторяющихся гнезд");
        }
    }
}
