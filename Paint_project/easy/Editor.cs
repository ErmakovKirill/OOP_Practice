using System;
using System.Drawing;
using System.Windows.Forms;

namespace easy
{
    internal partial class Editor : Form
    {
        private Tractor tr = null;
        private Share share;
        private int w, h;
        private Game g;
        private bool f;
        public Editor(Game g)
        {
            this.g = g;
            InitializeComponent();
            if (g.LastEditorLocation != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = g.LastEditorLocation;
            }
            w = pictureBox1.ClientRectangle.Width / 2;
            h = pictureBox1.ClientRectangle.Height / 2;
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            tr.WheelChange();
            pictureBox1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CvetChange(share);
        }
        private void CvetChange(Share obj)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;
            MyDialog.ShowHelp = false; 
            MyDialog.Color = obj.C;
            if (MyDialog.ShowDialog(this) == DialogResult.OK)
                obj.C = MyDialog.Color;
            button3.BackColor = MyDialog.Color;
            button3.ForeColor = Color.FromArgb(255 - share.C.R, 255 - share.C.G, 255 - share.C.B);
            pictureBox1.Invalidate();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (tr.Ride)
                tr.Ride = false;
            else 
                tr.Ride = true;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (share != null)
                share.Draw(e.Graphics, w - share.W / 2, h - share.H / 2);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (f)
            {
                share.W = (int)numericUpDown1.Value;
                if (tr != null)
                    numericUpDown2.Maximum = numericUpDown1.Value / 2;
   
            }
            pictureBox1.Invalidate();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (f)
            {
                share.H = (int)numericUpDown2.Value;
                if (tr != null)
                    numericUpDown1.Minimum = numericUpDown2.Value * 2;
            }
            pictureBox1.Invalidate();
        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            g.LastEditorLocation = this.Location;
            g.EditorUpdate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                if (tr != null)
                {
                    Share part = tr.TouchinEditor(e.X, e.Y, w, h);
                    if (part != null)
                        CvetChange(part);
                }
        }

        public Share Share 
        { 
            get => share;
            set
            {
                f = false;
                share = value;
                this.tr = null;
                if (share is Tractor tr)
                {
                    this.tr = tr;
                    button2.Visible = true;
                    button1.Visible = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                }
                numericUpDown1.Minimum = 15;
                numericUpDown1.Maximum = 200;
                numericUpDown2.Minimum = 15;
                numericUpDown2.Maximum = 200;
                numericUpDown1.Value = share.W;
                numericUpDown2.Value = share.H;
                button3.BackColor = share.C;
                button3.ForeColor = Color.FromArgb(255 - button3.BackColor.R, 255 - button3.BackColor.G, 255 - button3.BackColor.B);
                f = true;
            }
        }
    }
}
