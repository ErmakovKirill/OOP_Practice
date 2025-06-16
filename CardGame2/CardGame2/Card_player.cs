using System.Collections.Generic;

namespace CardGame2
{
    internal abstract class Card_player
    {
        protected internal List<Card> cards = new List<Card>();
        protected internal string playerName;
        public Card_player(string playerName)
        {
            this.playerName = playerName;
        }

        public virtual void Add(Card card)
        {
            this.cards.Add(card);
        }

        public virtual int GetCount() { return cards.Count; }
        public virtual string GetPlayerName() { return playerName; }
    }
}
