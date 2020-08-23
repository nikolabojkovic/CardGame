namespace CardGame.Domain
{
    public class Player 
    {
        public Deck DeckOfCards { get; set; }
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

        public Card PlayCard()
        {
            var card = DeckOfCards.DrawCard();
            DomainEvents.Raise<GameAction>(new GameAction($"{Name} ({DeckOfCards.Count()} cards) {DeckOfCards.PlayedCard.Face}"));

            return card;
        }

        public bool HasLostTheGame()
        {
            return DeckOfCards.DrawPile.Count == 0 && DeckOfCards.DiscardedPile.Count == 0;
        }
    }
}