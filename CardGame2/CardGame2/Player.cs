using System.Collections.Generic;

namespace CardGame2
{
    internal class Player : Card_player
    {
        public Player(string playerName) : base(playerName) { }

        public Card UseCard(int x, int y)
        {
            foreach (Card card in cards) if (card.InTouch(x, y))
                {
                    cards.Remove(card);
                    return card;
                }
            return null;
        }

        public Card TestCard(int x, int y)
        {
            foreach (Card card in cards) if (card.InTouch(x, y))
                    return card;
            return null;
        }
    }
}
