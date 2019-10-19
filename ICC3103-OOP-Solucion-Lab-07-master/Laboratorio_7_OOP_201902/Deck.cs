using Laboratorio_7_OOP_201902.Cards;
using Laboratorio_7_OOP_201902.Enums;
using Laboratorio_7_OOP_201902.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laboratorio_7_OOP_201902
{
    [Serializable]
    public class Deck : ICharacteristics
    {

        private List<Card> cards;

        public Deck()
        {
        
        }

        public List<Card> Cards { get => cards; set => cards = value; }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
        public void DestroyCard(int cardId)
        {
            cards.RemoveAt(cardId);
        }

        

        public void Shuffle()
        {
            Random random = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public List<string> GetCharacteristics()
        {
            List<string> list;

            int totalCards = (from cards in Cards select cards).Count();
            int meleeCards = (from cards in Cards where cards.Type == EnumType.melee select cards).Count();
            int rangeCards = (from cards in Cards where cards.Type == EnumType.range select cards).Count();
            int longRangeCards = (from cards in Cards where cards.Type == EnumType.longRange select cards).Count();
            int buffCards = (from cards in Cards where cards.Type == EnumType.buff select cards).Count();
            int weatherCards = (from cards in Cards where cards.Type == EnumType.weather select cards).Count();

            int meleeAP = (from CombatCard card in (from cards in Cards where cards.Type == EnumType.melee select cards) select card.AttackPoints).Sum();
            int rangeAP = (from CombatCard card in (from cards in Cards where cards.Type == EnumType.range select cards) select card.AttackPoints).Sum();
            int longRangeAP = (from CombatCard card in (from cards in Cards where cards.Type == EnumType.longRange select cards) select card.AttackPoints).Sum();
            int totalAP = meleeAP + rangeAP + longRangeAP;

            list = new List<string>()
            {
                $"Total cards: {totalCards.ToString()}",
                $"Total melee cards: {meleeCards.ToString()}",
                $"Total range cards: {rangeCards.ToString()}",
                $"Total longRange cards: {longRangeCards.ToString()}",
                $"Total buff cards: {buffCards.ToString()}",
                $"Total weather cards: {weatherCards.ToString()}",
                $"Total melee cards attackPoints: {meleeAP.ToString()}",
                $"Total range cards attackPoints: {rangeAP.ToString()}",
                $"Total longRange cards attackPoints: {longRangeAP.ToString()}",
                $"Total cards attackPoints: {totalAP.ToString()}"
            };

            return list;

        }

    }
}
