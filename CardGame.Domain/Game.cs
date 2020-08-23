using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Domain
{
    public class Game 
    {
        private IWriter _writer;

        public Deck DeckOfCards { get; private set; }
        public IEnumerable<Player> Players { get; private set; }

        protected Game() {  }

        public static Game CreateGame(IEnumerable<Player> players, Deck deckOfCards, IWriter writer) {
            return new Game {
                Players = players,
                DeckOfCards = deckOfCards,
                _writer = writer
            };
        }

        public void StartGame()
        {
            DeckOfCards.Shuffle(DeckOfCards.DrawPile);
            DrawCards();

            while (Players.Count(p => !p.HasLostTheGame()) > 1)
            {
                var playersStillInGame = Players.Where(p => !p.HasLostTheGame());
                var winningCard = PlayRound(playersStillInGame);

                // reset players played cards
                foreach (var player in playersStillInGame)
                    player.DeckOfCards.PlayedCard = null;

                if (winningCard != null)
                    Console.WriteLine($"{winningCard.Player.Name} wins this round");
                else
                    Console.WriteLine("No winner in this round");

                _writer.WriteLine();
            }

            var winner = Players.Single(p => !p.HasLostTheGame());
            _writer.WriteLine($"{winner.Name} wins the game!");
        }

        public void DrawCards()
        {
            var cardsPerPlayer = DeckOfCards.DrawPile.Count / Players.Count();
            
            foreach (var player in Players)
            {
                for (int i = 0; i < cardsPerPlayer; i++) 
                {
                    var card = DeckOfCards.DrawPile.Pop();
                    card.AssignToPlayer(player);
                    player.DeckOfCards.DrawPile.Push(card);
                }
            }
        }

        public Card PlayRound(IEnumerable<Player> players) 
        {         
            // play cards
            foreach (var player in players)
            {                
                player.PlayCard();   
                _writer.WriteLine($"{player.Name} ({player.DeckOfCards.Count()} cards) {player.DeckOfCards.PlayedCard.Face}");             
            }

            var cards = CollectRoundPlayedCards(players);

            // push to discarded(temp) pile
            foreach (var card in cards)
                DeckOfCards.DiscardedPile.Push(card);

            var winningCard = FindWinningCard(cards);           

            // assign all played cards to winned if one found
            if (winningCard != null)
            {
                AssignCardsToWinner(winningCard.Player);
            }

            return winningCard;            
        }

        public Card FindWinningCard(IEnumerable<Card> cards) {
            var maxCardNumber = cards.Max(c => c.Face);
            var winningCards = cards.Where(c => c.Face == maxCardNumber).ToList();
            
            if (winningCards.Count == 1)
                return winningCards[0];

            return null;
        }

        public IEnumerable<Card> CollectRoundPlayedCards(IEnumerable<Player> players) {
            var cards = new List<Card>();

            foreach(var player in players)
                cards.Add(player.DeckOfCards.PlayedCard);

            return cards;
        }

        public void AssignCardsToWinner(Player player) 
        {
            var roundWinner = Players.Single(x => x == player);

            while (DeckOfCards.DiscardedPile.Count > 0) 
            {
                var card = DeckOfCards.DiscardedPile.Pop();
                card.AssignToPlayer(player);
                roundWinner.DeckOfCards.DiscardedPile.Push(card);
            }
        }
    }
}