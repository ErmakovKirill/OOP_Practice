using System.Collections.Generic;

namespace CardGame2
{
    internal abstract class Bot : Card_player
    {
        protected internal Card useCard;
        public Bot(string playerName) : base(playerName) { }
        public abstract Card MinCard(int mm);
        public virtual Card GetCard()
        {
            cards.Remove(useCard);
            return useCard;
        }
        public abstract bool Thinking(Card table_card, int mc);

        public abstract Card More(List<Card> card_on_table, int mk);

    }
}
