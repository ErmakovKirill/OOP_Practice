using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CardGame2
{
    public partial class Options : Form
    {
        private int pl = 2, k = 0, s = 0;
        private ushort min = 0, sec = 30;
        private string pname;
        private bool demo = false;
        public Options(string file)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            pname = textBox2.Text;

            try
            {
                string[] lines = File.ReadAllLines(file, Encoding.UTF8);
                char delimiter = ';';
                string k;
                foreach (string line in lines)
                {

                    string[] values = line.Split(delimiter);

                    if (values[4] == "0")
                        k = "Легкие боты";
                    else 
                        k = "Средние боты";
                    string sline = values[0] + ", " +
                        values[1] + ", " +
                        values[2] + ", " +
                        values[3] + ", " + 
                        k;
                    listBox1.Items.Add(sline);
                }
            }
            catch { }
            
        }
        
        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (radioButton1.Checked)
                k = 36;
            else
                k = 52;

        }

        private void comboBox1_SelectedValueChanged(object sender, System.EventArgs e)
        {
            s = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if(comboBox2.SelectedIndex == 0)
            {
                min = 1;
                sec = 30;
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                min = 1; 
                sec = 0;
            }
            else if (comboBox2.SelectedIndex == 2)
            {
                min = 0;
                sec = 30;
            }
            else if (comboBox2.SelectedIndex == 3)
            {
                min = 0;
                sec = 15;
            }
        }

        private void textBox2_TextChanged(object sender, System.EventArgs e)
        {
            if (textBox2.Text.Length < 16 && !(string.IsNullOrEmpty(textBox2.Text)))
                pname = textBox2.Text;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            demo = true;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            int p = 2;
            if (int.TryParse(textBox1.Text, out p) && p > 1 && p < 6)
                pl = p;               
        }

        public ushort GetSec { get => sec; }
        public string GetName { get => pname; }
        public bool GetDemo {  get => demo; }
        public int GetK { get => k; }

        private void button4_Click(object sender, System.EventArgs e)
        {
            if (listBox1.Visible)
            {
                this.Width -= 350;
                listBox1.Visible = false;
            }
            else
            {
                this.Width += 350;
                listBox1.Visible = true;
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Rules r = new Rules();
            r.Show();
        }

        public ushort GetMin { get => min; }
        public int GetS { get => s; }
        public int GetPl { get => pl; }
    }
}
