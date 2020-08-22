using System.Collections.Generic;

namespace CardGame.Domain
{
    public class Game 
    {
        public Deck DeckOfCards { get; private set; }
        public IEnumerable<Player> Players { get; private set; }

        protected Game() {  }

        public static Game CreateGame(IEnumerable<Player> players, Deck deckOfCards) {
            return new Game {
                Players = players,
                DeckOfCards = deckOfCards
            };
        }

        public void StartGame()
        {

        }

        private void PlayRound() {

        }

        private Player CompareCards() {
            return Player.CreatePlayer("Player 1");
        }

        private IEnumerable<Card> CollectPlayedCards() {
            return new List<Card> { };
        }

        private void AssignCardsToWiner() 
        {

        }
    }
}