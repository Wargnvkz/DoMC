using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Dialogs
{
    public class DigitalInput
    {

        public static int ShowIntegerDialog(string caption, bool IsPinCode, int value=-1)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 600,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            int WIndent = 10;
            int HIndent = 5;
            int TextHeight = 80;
            bool ButtonsPressed = false;
            TextBox textBox = new TextBox() { Left = WIndent, Top = 5, Width = prompt.Width - WIndent * 2, Height = TextHeight - HIndent * 2, Font = new Font(prompt.Font.FontFamily, 40),Text=value>=0?value.ToString():"",MaxLength=9,TextAlign=HorizontalAlignment.Center };
            if (IsPinCode)
            {
                textBox.PasswordChar = '*';
            }
            prompt.Controls.Add(textBox);
            textBox.KeyPress += (sender, e)=> { 
                if (e.KeyChar == 0x0d)
                {
                    e.Handled = true;
                    prompt.DialogResult = DialogResult.OK;
                    prompt.Close();
                }
                if (e.KeyChar == 0x1b)
                {
                    e.Handled = true;
                    prompt.DialogResult = DialogResult.Cancel;
                    prompt.Close();
                }
            };

            ButtonType[] ButtonTexts = new ButtonType[] {
                new ButtonType("1",Color.Black,DialogResult.None),
                new ButtonType("2",Color.Black,DialogResult.None),
                new ButtonType("3",Color.Black,DialogResult.None),
                new ButtonType("4",Color.Black,DialogResult.None),
                new ButtonType("5",Color.Black,DialogResult.None),
                new ButtonType("6",Color.Black,DialogResult.None),
                new ButtonType("7",Color.Black,DialogResult.None),
                new ButtonType("8",Color.Black,DialogResult.None),
                new ButtonType("9",Color.Black,DialogResult.None),
                new ButtonType("\u232B",Color.Yellow,DialogResult.None,
                (sender,e)=>
                { if (textBox.Text.Length>0)
                    textBox.Text=textBox.Text.Remove(textBox.Text.Length-1); }
                ),
                new ButtonType("0",Color.Black,DialogResult.None),
                new ButtonType("\u2713",Color.Green,DialogResult.OK, (sender,e)=>{ }) };
            var btnW = (prompt.ClientSize.Width - WIndent * 2) / 3;
            var btnH = (prompt.ClientSize.Height - HIndent * 2 - TextHeight) / 4;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var n = y * 3 + x;
                    var btn = new Button() { Text = ButtonTexts[n].Text, Left = 10 + btnW * x, Top = TextHeight + HIndent * 2 + btnH * y, Width = btnW, Height = btnH, Font = new Font(prompt.Font.FontFamily, 60), ForeColor = ButtonTexts[n].Color, DialogResult = ButtonTexts[n].DialogResult };
                    if (ButtonTexts[n].OnClick == null)
                    {
                        btn.Click += (sender, e) =>
                        {
                            if (!ButtonsPressed)
                            {
                                textBox.Text = "";
                                ButtonsPressed = true;
                            }
                            textBox.Text += ButtonTexts[n].Text;
                        };
                    }
                    else
                    {
                        btn.Click += ButtonTexts[n].OnClick;
                    }
                    prompt.Controls.Add(btn);
                }
            }
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                if(int.TryParse(textBox.Text, out int result))
                {
                    return result;
                }
                return -1;
            }
            else
            {
                return -1;
            }
        }
                

        private class ButtonType
        {
            public ButtonType(string text, Color color, DialogResult dialogResult, EventHandler onClick = null)
            {
                Text = text;
                Color = color;
                DialogResult = dialogResult;
                OnClick = onClick;
            }
            public string Text;
            public Color Color;
            public DialogResult DialogResult;
            public EventHandler OnClick;
        }
    }
}
