namespace CardGame.Domain
{
    public class Player 
    {
        public Deck DeckOfCards { get; private set; }
        public string Name { get; private set; }
        
        protected Player() { }

        public static Player CreatePlayer(string name)
        {
            return new Player
            {
                 Name = name,
                 DeckOfCards = Deck.CreateDeck(0, new RandomNumberGenerator())
            };
        }

        public void AssignDeckOfCards(Deck deckOfCards)
        {
            DeckOfCards = deckOfCards;
        }

        public Card PlayCard()
        {
            return DeckOfCards.DrawCard();
        }

        public bool HasLostTheGame()
        {
            return DeckOfCards.DrawPile.Count == 0 && DeckOfCards.DiscardedPile.Count == 0;
        }
    }
}