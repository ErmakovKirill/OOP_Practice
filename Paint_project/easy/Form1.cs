using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace easy
{
    public partial class Form1 : Form
    {
        private Game game;
        private List<CheckBox> ch = new List<CheckBox>();
        private decimal n1 = 0, n2 = 0, n3 = 0, n4 = 0;
        private bool obj_move = false;
        public Form1()
        {
            InitializeComponent();
            game = new Game();
            timer1.Start();
        }
        private void CreateCheckBoxAndHandleChecked(int y)
        {
            CheckBox k = new CheckBox();
            k.Checked = true;
            int x = ch.Count(cb => cb.Tag?.ToString() == y.ToString());
            k.Tag = y;
            k.Location = new Point(105 + 20 * x, 25 * y);
            k.Name = ch.Count.ToString();
            k.CheckState = CheckState.Checked;
            k.Size = new Size(15, 15);
            k.UseVisualStyleBackColor = true;
            k.Visible = true;
            k.CheckedChanged += CheckBox_CheckedChanged;
            this.Controls.Add(k);
            ch.Add(k);
        }
        private void RemoveCheckBoxAndNotifyGame(decimal numUpDnVal, int yValue)
        {
            decimal numToRemove = ch.Count(cb => cb.Tag.ToString() == yValue.ToString()) - numUpDnVal;
            for (decimal i = 0; i < numToRemove; i++)
            {
                int indexToRemove = ch.FindLastIndex(cb => cb.Tag.ToString() == yValue.ToString());

                if (indexToRemove != -1)
                {
                    CheckBox checkBoxToRemove = ch[indexToRemove];
                    this.Controls.Remove(checkBoxToRemove);
                    ch.RemoveAt(indexToRemove);
                    game.DeleteAT(indexToRemove);
                    checkBoxToRemove.Dispose();
                }
            }

            int xPosition = 0;
            foreach (CheckBox cb in ch.FindAll(cb => cb.Tag.ToString() == yValue.ToString()))
            {
                cb.Location = new Point(105 + 20 * xPosition, 25 * yValue);
                xPosition++;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            decimal newValue = numericUpDown1.Value;
            while (newValue > n1)
            {
                decimal diff = newValue - n1;
                for (int i = 0; i < diff; i++)
                {
                    CreateCheckBoxAndHandleChecked(1);
                    game.AddE(this.ClientRectangle.Width, this.ClientRectangle.Height);
                }
                n1 = newValue;
            }
            while (newValue < n1)
            {
                RemoveCheckBoxAndNotifyGame(newValue, 1);
                n1 = newValue;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            decimal newValue = numericUpDown2.Value;
            while (newValue > n2)
            {
                decimal diff = newValue - n2;
                for (int i = 0; i < diff; i++)
                {
                    CreateCheckBoxAndHandleChecked(2);
                    game.AddR(this.ClientRectangle.Width, this.ClientRectangle.Height);
                }
                n2 = newValue;
            }
            while (newValue < n2)
            {
                RemoveCheckBoxAndNotifyGame(newValue, 2);
                n2 = newValue;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (game.Check(e.X, e.Y))
                if (e.Button == MouseButtons.Left)
                    obj_move = true;
                else if (e.Button == MouseButtons.Right)
                    game.NewEd();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (obj_move)
                obj_move = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (obj_move)
                game.MoveOBj(e.X, e.Y);
            game.FaceMove(e.X, e.Y);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            game.DrawUpd(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            decimal newValue = numericUpDown3.Value;
            while (newValue > n3)
            {
                decimal diff = newValue - n3;
                for (int i = 0; i < diff; i++)
                {
                    CreateCheckBoxAndHandleChecked(3);
                    game.AddT(this.ClientRectangle.Width, this.ClientRectangle.Height);
                }
                n3 = newValue;
            }
            while (newValue < n3)
            {
                RemoveCheckBoxAndNotifyGame(newValue, 3);
                n3 = newValue;
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            decimal newValue = numericUpDown4.Value;
            while (newValue > n4)
            {
                decimal diff = newValue - n4;
                for (int i = 0; i < diff; i++)
                {
                    CreateCheckBoxAndHandleChecked(4);
                    game.AddP(this.ClientRectangle.Width, this.ClientRectangle.Height);
                }
                n4 = newValue;
            }
            while (newValue < n4)
            {
                RemoveCheckBoxAndNotifyGame(newValue, 4);
                n4 = newValue;
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            int checkBoxIndex = ch.IndexOf(checkBox);
            if (checkBox.Checked)
                game.Check(checkBoxIndex);
            else
                game.Uncheck(checkBoxIndex);
        }
    }
}
