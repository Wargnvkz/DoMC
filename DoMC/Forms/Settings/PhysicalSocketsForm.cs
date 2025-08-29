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
        SocketCards[] AllCards = new SocketCards[8];
        public int[] CardSocket2EquipmentSocket
        {
            get
            {
                return CardsToArray(AllCards);
            }
            set
            {
                AllCards = ArrayToCards(value);
                dgvSockets.DataSource = AllCards;
                for (int i = 1; i < dgvSockets.Columns.Count; i++)
                {
                    dgvSockets.Columns[i].HeaderText = $"Плата {i}";
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
            /*
            HashSet<int> sockets = new HashSet<int>();
            for(int card = 0; card < _Sockets.Count; card++)
            {
                var socketsInCards = _Sockets[card].Array;
                for(int cards = 0; cards < socketsInCards.Length; cards++)
                {
                    var displaySocket = socketsInCards[cards];
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
            */
            return true;
        }

        private void dgvSockets_Validating(object sender, CancelEventArgs e)
        {
            var result = CheckSockets();
            e.Cancel = !result;
            epSockets.Clear();
            if (!result)
                epSockets.SetError(btnOK, "Не должно быть повторяющихся гнезд");
        }

        private SocketCards[] ArrayToCards(int[] CardSocket2EquipmentSocket)
        {
            SocketCards[] cards = new SocketCards[8];
            for (int socket = 0; socket < 8; socket++)
            {
                int[] socketForCards = new int[12];
                for (int card = 0; card < socketForCards.Length; card++)
                {
                    var cs = card * 8 + socket;
                    socketForCards[card] = CardSocket2EquipmentSocket[cs];
                }
                var SocketForCards = new SocketCards();
                SocketForCards.Array = socketForCards;
                SocketForCards.Socket = socket + 1;
                cards[socket] = SocketForCards;
            }
            return cards;
        }
        private int[] CardsToArray(SocketCards[] socketCards)
        {
            int[] CardsSocket2EquipmentSocket = new int[96];
            for (int socket = 0; socket < 8; socket++)
            {
                int[] socketForCards = socketCards[socket].Array;
                for (int card = 0; card < socketForCards.Length; card++)
                {
                    var cs = card * 8 + socket;
                    CardsSocket2EquipmentSocket[cs] = socketForCards[card];
                }
            }
            return CardsSocket2EquipmentSocket;
        }

        public class SocketCards
        {
            public int Socket { get; set; }
            public int Card1 { get; set; }
            public int Card2 { get; set; }
            public int Card3 { get; set; }
            public int Card4 { get; set; }
            public int Card5 { get; set; }
            public int Card6 { get; set; }
            public int Card7 { get; set; }
            public int Card8 { get; set; }
            public int Card9 { get; set; }
            public int Card10 { get; set; }
            public int Card11 { get; set; }
            public int Card12 { get; set; }

            public int[] Array
            {
                get
                {
                    return new int[] { Card1, Card2, Card3, Card4, Card5, Card6, Card7, Card8, Card9, Card10, Card11, Card12 };
                }
                set
                {
                    Card1 = value[0];
                    Card2 = value[1];
                    Card3 = value[2];
                    Card4 = value[3];
                    Card5 = value[4];
                    Card6 = value[5];
                    Card7 = value[6];
                    Card8 = value[7];
                    Card9 = value[8];
                    Card10 = value[9];
                    Card11 = value[10];
                    Card12 = value[11];
                }
            }
        }

        private void dgvSockets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // проверяем, чтобы кликнули по строке, а не по заголовку
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = dgvSockets.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // проверяем, что это числовая колонка (можешь уточнить по типу или имени колонки)
                if (cell.OwningColumn.ValueType == typeof(int) || cell.OwningColumn.ValueType == typeof(decimal))
                {
                    cell.Value = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog($"Ввод значения", false, (int)cell.Value);
                    /*using (var form = new PinPadForm()) // твое окно для ввода цифр
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            cell.Value = form.Result; // результат из твоей формы
                        }
                    }*/
                }
            }
        }
    }
}
