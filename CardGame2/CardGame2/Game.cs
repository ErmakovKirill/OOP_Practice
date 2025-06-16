using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 
using System.Text;

namespace CardGame2
{
    internal class Game
    {
        private Table table;
        private Options options;

        private List<Card> deck, on_table;
        private List<Card_player> players;

        private int main_mast, hod_player_id, num_cards;
        private ushort defsec, defmin;
        private Random rnd = new Random();

        private const ushort delay = 700;
        private const string file = "players_results.txt";

        public Game()
        {
            Start();
        }

        private void Restart()
        {
            table.Hide();
            _ = StartGame();
            _ = NextStep();
            _ = Defend();
            _ = Podkid();
            _ = Tableclear();
            Start();
        }
        private void Start()
        {            
            deck = new List<Card>();
            on_table = new List<Card>();
            players = new List<Card_player>();
            options = new Options(file);
            table = new Table(players);

            if (options.ShowDialog() == DialogResult.OK) { };
            if (options.GetDemo)
            {
                if (options.GetS == 0)
                    players.Add(new EasyBot(options.GetName));
                if (options.GetS == 1)
                    players.Add(new MediumBot(options.GetName));
            }
            else
            {
                players.Add(new Player(options.GetName));
            }
            GameOptionsEnter(options.GetK, options.GetPl, options.GetS);
            defsec = options.GetSec;
            defmin = options.GetMin;
            table = new Table(players);
            table.FormClosed += Table_FormClosed;
            table.Show();
            table.Draw(on_table, deck);
            table.Shown += async (s, e) => await StartGame();
        }
        private void GameOptionsEnter(int numCards, int numPlayers, int Hard)
        {
            for (int i = 1; i < numPlayers; i++)
            {
                if (Hard == 0)
                    players.Add(new EasyBot("Бот " + Convert.ToString(i)));
                if (Hard ==1)
                    players.Add(new MediumBot("Бот " + Convert.ToString(i)));
            }

            if (numCards == 36)
                for (int i = 6; i <= 14; i++)
                    for (int j = 1; j <= 4; j++)
                        deck.Add(new Card(i, j));
            else
                for (int i = 2; i <= 14; i++)
                    for (int j = 1; j <= 4; j++)
                        deck.Add(new Card(i, j));

            deck = deck.OrderBy(x => rnd.Next()).ToList();
            main_mast = deck[0].GetM;
            num_cards = numCards;

            for (int i = 0; i < numPlayers; i++)
                for (int j = 0; j < 6; j++)
                    if (deck.Count > 0)
                    {
                        players[i].Add(deck[num_cards - 1]);
                        deck.RemoveAt(num_cards - 1);
                        num_cards--;
                    }
            hod_player_id = rnd.Next(0, players.Count - 1);
        }

        private async Task StartGame()
        {
            await NextStep();
        }

        private void PNext()
        {
            hod_player_id++;
            if (hod_player_id == players.Count) 
                hod_player_id = 0;
        }

        private void PBefore()
        {
            hod_player_id--;
            if (hod_player_id < 0) 
                hod_player_id = players.Count - 1;
        }

        private async Task Tableclear()
        {
            on_table = new List<Card>();
            if (players[hod_player_id].GetCount() < 6 && deck.Count > 0)
                while (players[hod_player_id].GetCount() < 6 && deck.Count > 0)
                {
                    players[hod_player_id].Add(deck[num_cards - 1]);
                    deck.RemoveAt(num_cards - 1);
                    num_cards--;
                }
            PNext();
            if (players[hod_player_id].GetCount() < 6 && deck.Count > 0)
                while (players[hod_player_id].GetCount() < 6 && deck.Count > 0)
                {
                    players[hod_player_id].Add(deck[num_cards - 1]);
                    deck.RemoveAt(num_cards - 1);
                    num_cards--;
                }
            CheckWin();
            table.Draw(on_table, deck);
            await NextStep();
        }

        private async Task Podkid()
        {
            if (on_table.Count < 12 && players[hod_player_id].GetCount() > 0)
            {
                PBefore();
                if (players[hod_player_id] is Bot p)
                {
                    await Task.Delay(delay);
                    if (p.More(on_table, main_mast) is Card f)
                    {
                        f.F = true;
                        on_table.Add(f);
                        table.Draw(on_table, deck);
                        await Defend();
                    }
                    else
                    {
                        table.Setbita = true;
                        await Tableclear();
                    }
                }
                else if (players[hod_player_id] is Player b)
                {
                    table.PlayerTimerStart(defsec,defmin);
                    MouseEventArgs click = await WaitForPlayerClick();
                    if (click.Button == MouseButtons.Left)
                    {
                        Card f = b.TestCard(click.X, click.Y);
                        if (f != null && on_table.Any(c => c.GetN == f.GetN))
                        {
                            f = b.UseCard(click.X, click.Y);
                            f.F = true;
                            on_table.Add(f);
                            table.PlayerTimerStop();
                            table.Draw(on_table, deck);
                            await Defend();
                        }
                        else
                        {
                            MessageBox.Show("Нельзя так!");
                            await Podkid();
                        }
                    }
                    else if (click.Button == MouseButtons.Right)
                    {
                        table.PlayerTimerStop();
                        table.Setbita = true;
                        await Tableclear();
                    }
                }
            }
            else
                await Tableclear();
        }

        private async Task Defend()
        {
            PNext();
            if (players[hod_player_id] is Bot p)
            {
                await Task.Delay(delay);
                if (p.Thinking(on_table[on_table.Count - 1], main_mast))
                {
                    on_table.Add(p.GetCard());
                    table.Draw(on_table, deck);
                    await Podkid();
                }
                else
                {
                    foreach (Card card in on_table)
                        p.Add(card);
                    await Tableclear();
                }
            }
            else if (players[hod_player_id] is Player b)
            {
                table.PlayerTimerStart(defsec,defmin);
                MouseEventArgs click = await WaitForPlayerClick();
                if (click.Button == MouseButtons.Left)
                {
                    Card f = b.TestCard(click.X, click.Y);
                    Card sr = on_table[on_table.Count - 1];
                    if (f != null && f.Comparison(sr.GetM, sr.GetN, main_mast))
                    {
                        f = b.UseCard(click.X, click.Y);
                        on_table.Add(f);
                        table.PlayerTimerStop();
                        table.Draw(on_table, deck);
                        await Podkid();
                    }
                    else
                    {
                        MessageBox.Show("Нельзя так!");
                        PBefore();
                        await Defend();
                    }
                }
                else if (click.Button == MouseButtons.Right)
                {
                    foreach (Card card in on_table)
                        b.Add(card);
                    table.PlayerTimerStop();
                    table.Draw(on_table, deck);
                    await Tableclear();
                }
            }
        }

        private async Task NextStep()
        {
            if (players[hod_player_id] is Bot p)
            {
                await Task.Delay(delay);
                on_table.Add(p.MinCard(main_mast));
                table.Draw(on_table, deck);
                await Defend();
            }
            else if (players[hod_player_id] is Player b)
            {
                table.PlayerTimerStart(defsec, defmin);
                MouseEventArgs click = await WaitForPlayerClick();
                if (click.Button == MouseButtons.Left)
                {
                    Card f = b.UseCard(click.X, click.Y);
                    if (f != null)
                    {
                        on_table.Add(f);
                        table.PlayerTimerStop();
                        table.Draw(on_table, deck);
                        await Defend();
                    }
                    else
                    {
                        MessageBox.Show("Выберите карту!");
                        await NextStep();
                    }
                }
                else
                {
                    MessageBox.Show("Вы должны сыграть карту!");
                    await NextStep();
                }
            }
        }

        private Task<MouseEventArgs> WaitForPlayerClick()
        {
            var tcs = new TaskCompletionSource<MouseEventArgs>();
            MouseEventHandler handler = null;
            handler = (sender, e) =>
            {
                tcs.SetResult(e);
                table.TableMouseDown -= handler;
            };
            table.TableMouseDown += handler;
            return tcs.Task;
        }

        private void CheckWin()
        {
            foreach (Card_player pl in players)
                if (pl.GetCount() == 0)
                {
                    int m = (int)(table.Time / 60), s = (int)(table.Time % 60); 
                    MessageBox.Show("Победил: " + pl.GetPlayerName() + "\n Время игры: " + m + ":" +s);
                    if (pl is Player)
                    {
                        string line = pl.GetPlayerName() + ";" +
                            m + ":" + s + ";" +
                            Convert.ToString(options.GetK) + ";" +
                            Convert.ToString(options.GetPl) + ";" +
                            Convert.ToString(options.GetS) ;
                        File.AppendAllText(file, line);
                    }
                    else 
                        table.GetLoose = true;
                    table.Close();                  
                }
        }

        private void Table_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (table.GetLoose)
            {
                DialogResult guest = MessageBox.Show("Ты проиграл \n Начать заново?", "", MessageBoxButtons.YesNo);
                if (guest == DialogResult.Yes)
                    Restart();
                if (guest == DialogResult.No)
                    Application.Exit();
            }
            else
            {
                DialogResult guest = MessageBox.Show("Начать заново?", "", MessageBoxButtons.YesNo);
                if (guest == DialogResult.Yes)
                    Restart();
                if (guest == DialogResult.No)
                    Application.Exit();
            }
        }

    }
}
