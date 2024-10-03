using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Dialogs
{
    public class Prompt
    {
        public static string ShowDialog(string text, string caption, bool password)
        {
            Form prompt = new Form()
            {
                Width = 420,
                Height = 120,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 10, Top = 10, Text = text, Width = 400 };
            TextBox textBox = new TextBox() { Left = 10, Top = 25, Width = 400 };
            if (password) textBox.PasswordChar = '*';
            Button confirmation = new Button() { Text = "ОК", Left = 170, Width = 100, Top = 55, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
