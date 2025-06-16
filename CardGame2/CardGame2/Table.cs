using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CardGame2
{
    public partial class Table : Form
    {
        private ushort sec, min;
        private long time;

        private List<Card> on_table, deck;
        private List<Card_player> players;

        public event MouseEventHandler TableMouseDown;

        private bool loose = false, bita = false;

        private Card bitacard = new Card(1, 1);

        internal Table(List<Card_player> players)
        {
            this.players = players;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sec--;
            if (sec == 0 && min > 0)
            {
                sec = 60;
                min--;
            }
            label1.Text = min.ToString() + ":" + (sec < 10 ? "0" + sec.ToString() : sec.ToString());
            if (min == 0 && sec == 0)
            {
                timer1.Stop();
                loose = true;
                this.Close();
            }
        }

        private void Table_Paint(object sender, PaintEventArgs e)
        {
            if (players != null)
            {

                int num_cards = deck.Count();
                int deckX = ClientRectangle.Width - 100;
                if (num_cards > 0)
                    deck[0].DrawFrontHorizontal(e.Graphics, deckX - 120, 40);
                if (num_cards > 1)
                    deck[1].DrawBack(e.Graphics, deckX, 20);

                int cardsOnTableX = deckX - 200;
                int cardsOnTableY = 140;
                for (int i = 0; i < on_table.Count; i++)
                    on_table[i].DrawFront(e.Graphics, cardsOnTableX + (i % 2) * 80, cardsOnTableY + (i / 2) * 120);
                int playerY = 20; 
                int playerX = 20;

                for (int i = 0; i < players.Count; i++)
                {
                    e.Graphics.DrawString(players[i].GetPlayerName(), new Font("Arial", 12), Brushes.Black, playerX, playerY);
                    for (int j = 0; j < players[i].cards.Count; j++)
                    {
                        int cardX = playerX + j * 80;
                        int cardY = playerY + 20;
                        if (players[i] is Player)
                            players[i].cards[j].DrawFront(e.Graphics, cardX, cardY);
                        else
                            players[i].cards[j].DrawBack(e.Graphics, cardX, cardY);
                    }
                    playerY += 140; 
                }
                if (bita)
                {
                    bitacard.DrawBack(e.Graphics, ClientRectangle.Width - 90, ClientRectangle.Height - 130);
                }
                if (!timer2.Enabled)
                {
                    timer2.Enabled = true;
                    timer2.Start();
                }
            }
        }

        private void Table_MouseDown(object sender, MouseEventArgs e)
        {
            TableMouseDown?.Invoke(this, e);
        }

        internal void Draw(List<Card> on_table, List<Card> deck)
        {
            this.deck = deck;
            this.on_table = on_table;
            Invalidate();
        }

        internal void PlayerTimerStart(ushort defsec, ushort defmin)
        {
            if (timer1.Enabled == false)
            {
                timer1.Enabled = true;
                sec = defsec;
                min = defmin;
                timer1.Start();
            }
        }

        private void Table_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            time++;
        }

        internal void PlayerTimerStop()
        {
            timer1.Stop();
            timer1.Enabled=false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rules r = new Rules();
            r.Show();
        }

        public bool GetLoose { get => loose; set => loose = value; }
        public bool Setbita { set => bita = value; }
        public long Time { get => time; set =>time = value; }
    }
}