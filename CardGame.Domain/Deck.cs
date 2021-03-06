using System;
using System.Collections.Generic;

namespace CardGame.Domain
{
    public class Deck 
    {
        private IRandomNumberGenerator _randomNumberGenerator;

        public Stack<Card> DrawPile { get; private set; }
        public Stack<Card> DiscardedPile { get; private set; }

        public Card PlayedCard { get; set; }

        protected Deck() {  }

        public static Deck Create(int numberOfCards, IRandomNumberGenerator random) { 
            var deck = new Deck {
                _randomNumberGenerator = random,
                DrawPile = new Stack<Card>(),
                DiscardedPile = new Stack<Card>()
            };

            for(int i = 1; i <= numberOfCards; i++) {
                var cardNumber = (i % 10);
                deck.DrawPile.Push(Card.Create(Suit.Clubs, cardNumber == 0 ? 10 : cardNumber));
            }           

            return deck;
        }

        public void Shuffle(Stack<Card> pile) 
        {
            var cards = pile.ToArray();
            pile.Clear();

            for (int i = cards.Length - 1; i > 0 ; i--)
            {
                var randomIndex = _randomNumberGenerator.Next(i + 1);
                Swap(cards, i, randomIndex);
            }

            for(int i = 0; i < cards.Length; i++)
            {
                pile.Push(cards[i]);
            }
        }

        public Card DrawCard()
        {
            if (DrawPile.Count == 0 && DiscardedPile.Count == 0)
                throw new Exception("Deck is empty!");

            if (DrawPile.Count == 0)
                MoveCardsFromDiscardedToDrawPile();

            PlayedCard = DrawPile.Pop();
            return PlayedCard;
        }
        
        public int Count()
        {
            return DrawPile.Count + DiscardedPile.Count + (PlayedCard != null ? 1 : 0);
        }

        public void Swap(Card[] cards, int index1, int index2)
        {
            var temp = cards[index1];
            cards[index1] = cards[index2];
            cards[index2] = temp;
        }

        private void MoveCardsFromDiscardedToDrawPile()
        {
            Shuffle(DiscardedPile);

            while(DiscardedPile.Count > 0)
                DrawPile.Push(DiscardedPile.Pop());
        }
    }
}