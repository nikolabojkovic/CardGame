using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Domain
{
    public class Game 
    {
        public Deck DeckOfCards { get; private set; }
        public IEnumerable<Player> Players { get; private set; }

        protected Game() {  }

        public static Game Create(IEnumerable<Player> players, Deck deckOfCards) {
            return new Game {
                Players = players,
                DeckOfCards = deckOfCards
            };
        }

        public void Play()
        {
            DeckOfCards.Shuffle(DeckOfCards.DrawPile);
            DrawCards(DeckOfCards.DrawPile.Count / Players.Count());

            while (Players.Count(p => !p.HasLostTheGame()) > 1)
            {
                var playersStillInGame = Players.Where(p => !p.HasLostTheGame()).ToList();
                var winningCard = PlayRoundFor(playersStillInGame);

                if (winningCard != null)
                    DomainEvents.Raise<GameActionEvent>(new GameActionEvent($"{winningCard.Player.Name} wins this round"));
                else
                    DomainEvents.Raise<GameActionEvent>(new GameActionEvent($"No winner in this round"));

                DomainEvents.Raise<GameActionEvent>(new GameActionEvent($""));
            }

            var winner = Players.Single(p => !p.HasLostTheGame());
            DomainEvents.Raise<GameActionEvent>(new GameActionEvent($"{winner.Name} wins the game!"));
        }

        public void DrawCards(int cardsPerPlayer)
        {
            if (DeckOfCards.DrawPile.Count < Players.Count())
                throw new Exception("No enough cards in the deck!");
            
            foreach (var player in Players)
            {
                for (int i = 0; i < cardsPerPlayer; i++) 
                {
                    var card = DeckOfCards.DrawPile.Pop();
                    card.Player = player;
                    player.DeckOfCards.DrawPile.Push(card);
                }
            }
        }

        public Card PlayRoundFor(IEnumerable<Player> players) 
        {         
            var cardsOfThisRound = new List<Card>();

            // collect cards
            foreach (var player in players)          
               cardsOfThisRound.Add(player.PlayCard()); 

            // temporary store cards
            foreach (var card in cardsOfThisRound)
                DeckOfCards.DiscardedPile.Push(card);

            var winningCard = FindWinningCardFrom(cardsOfThisRound);           

            if (winningCard != null)
                AssignPlayedCardsTo(winningCard.Player);

            return winningCard;            
        }

        public Card FindWinningCardFrom(IEnumerable<Card> cards) {
            var maxCardNumber = cards.Max(c => c.Face);
            var winningCards = cards.Where(c => c.Face == maxCardNumber).ToList();
            
            if (winningCards.Count == 1)
                return winningCards[0];

            return null;
        }

        public void AssignPlayedCardsTo(Player roundWinner) 
        {
            while (DeckOfCards.DiscardedPile.Count > 0) 
            {
                var card = DeckOfCards.DiscardedPile.Pop();
                card.Player = roundWinner;
                roundWinner.DeckOfCards.DiscardedPile.Push(card);
            }
        }
    }
}