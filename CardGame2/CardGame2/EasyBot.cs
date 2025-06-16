using System.Collections.Generic;

namespace CardGame2
{
    internal class EasyBot : Bot
    {
        public EasyBot(string playerName) : base(playerName) { }

        public override Card MinCard(int mm)
        {
            Card min_card = new Card(15, mm);
            foreach (Card card in cards)
                if (card.GetN < min_card.GetN)
                {
                    min_card = card;
                    if (card.GetM != mm) 
                        min_card = card;
                }
            cards.Remove(min_card);
            return min_card;
        }
        public override bool Thinking(Card table_card, int mc)
        {
            foreach (Card card in cards)
            {
                if (card.GetM == table_card.GetM && card.GetN > table_card.GetN)
                {
                    useCard = card;
                    return true;
                }
                if (table_card.GetM != mc && card.GetM == mc)
                {
                    useCard = card;
                    return true;
                }
            }
            return false;
        }

        public override Card More(List<Card> card_on_table, int mk)
        {
            return null;
        }
    }
}
