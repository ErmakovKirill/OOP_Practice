using System;
using System.Windows.Forms;

namespace CardGame2
{
    public partial class Form1 : Form
    {
        private Game game;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            game = new Game();
        }
    }
}
