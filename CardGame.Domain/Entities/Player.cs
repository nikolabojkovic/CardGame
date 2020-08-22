namespace CardGame.Domain
{
    public class Player 
    {
        public Deck DeckOfCards { get; private set; }
        public string Name { get; private set; }
        
        protected Player() { }

        public static Player CreatePlayer(string name)
        {
            return new Player { Name = name };
        }

        public Card PlayCard()
        {
            return Card.CreateCard(Suit.Clubs, 10);
        }

        public bool HasLostTheGame()
        {
            return true;
        }
    }
}