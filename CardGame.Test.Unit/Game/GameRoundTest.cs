using System.Collections.Generic;
using System.Linq;
using CardGame.Domain;
using FluentAssertions;
using Xunit;

namespace CardGame.UnitTests
{
    public class GameRoundTest
    {
        [Fact]
        public void CollectPlayedCards_ShouldCollectAllPlayedCards()
        {
            // Arrange
            var deckOfCards1 = Deck.CreateDeck(0, new RandomNumberGenerator());
            deckOfCards1.DrawPile.Push(Card.CreateCard(Suit.Clubs, 7));
            deckOfCards1.DrawPile.Push(Card.CreateCard(Suit.Clubs, 4));
            var deckOfCards2 = Deck.CreateDeck(0, new RandomNumberGenerator());
            deckOfCards2.DrawPile.Push(Card.CreateCard(Suit.Clubs, 2));
            deckOfCards2.DrawPile.Push(Card.CreateCard(Suit.Clubs, 5));
            var deckOfCards3 = Deck.CreateDeck(0, new RandomNumberGenerator());
            deckOfCards3.DrawPile.Push(Card.CreateCard(Suit.Clubs, 6));
            deckOfCards3.DrawPile.Push(Card.CreateCard(Suit.Clubs, 8));

            var players = new List<Player>
            {
                Player.CreatePlayer("Player 1"),
                Player.CreatePlayer("Player 2"),
                Player.CreatePlayer("Player 3")
            };

            players[0].AssignDeckOfCards(deckOfCards1);
            players[0].PlayCard();
            players[1].AssignDeckOfCards(deckOfCards2);
            players[1].PlayCard();
            players[2].AssignDeckOfCards(deckOfCards3);
            players[2].PlayCard();

            var game = Game.CreateGame(players, null, new FakeWriter());

            // Act
            var cards = game.CollectRoundPlayedCards(players).ToList();

            // Assert
            cards.Count().Should().Be(3);
            cards[0].Face.Should().Be(4);
            cards[1].Face.Should().Be(5);
            cards[2].Face.Should().Be(8);
        }

        [Fact]
        public void AssignCardsToWinner_OneRound_ShouldTransfereAllPlayedCardsToWinner()
        {
            // Arrange
            var player = Player.CreatePlayer("Player 1");
            var deckOfCards = Deck.CreateDeck(10, new RandomNumberGenerator());
            deckOfCards.DiscardedPile.Push(Card.CreateCard(Suit.Clubs, 4));
            deckOfCards.DiscardedPile.Push(Card.CreateCard(Suit.Clubs, 3));
            deckOfCards.DiscardedPile.Push(Card.CreateCard(Suit.Clubs, 8));
            var game = Game.CreateGame(new List<Player> {player}, deckOfCards, new FakeWriter());

            // Act
            game.AssignCardsToWinner(player);

            // Assert
            var gamePlayer = game.Players.ToList()[0];
            gamePlayer.DeckOfCards.DiscardedPile.Count.Should().Be(3);
            gamePlayer.DeckOfCards.DiscardedPile.Pop().Face.Should().Be(4);
            gamePlayer.DeckOfCards.DiscardedPile.Pop().Face.Should().Be(3);
            gamePlayer.DeckOfCards.DiscardedPile.Pop().Face.Should().Be(8);
        }
    
        [Fact]
        public void PlayRound_ShouldAssignCardsToWinnder()
        {
            // Arrange
            var players = new List<Player> 
            {
                Player.CreatePlayer("Player 1"),
                Player.CreatePlayer("Player 2")
            };

            var deck = Deck.CreateDeck(0, new RandomNumberGenerator());
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 4));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 6));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 7));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 1));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 5));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 2));

            var game = Game.CreateGame(players, deck, new FakeWriter());
            game.DrawCards();

            // Act
            game.PlayRound(players);

            // Assert
            deck.DrawPile.Count.Should().Be(0);
            players[0].DeckOfCards.DiscardedPile.Count.Should().Be(0);
            players[0].DeckOfCards.DrawPile.Count.Should().Be(2);
            players[1].DeckOfCards.DiscardedPile.Count.Should().Be(2);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(2);
        }

         [Fact]
        public void PlayRound_SecondRound_ShouldAssignCardsToWinnder()
        {
            // Arrange
            var players = new List<Player> 
            {
                Player.CreatePlayer("Player 1"),
                Player.CreatePlayer("Player 2")
            };

            var deck = Deck.CreateDeck(0, new RandomNumberGenerator());
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 4));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 6));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 7));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 4));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 5));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 2));

            var game = Game.CreateGame(players, deck, new FakeWriter());
            game.DrawCards();

            // Act
            game.PlayRound(players);
            game.PlayRound(players);

            // Assert
            deck.DrawPile.Count.Should().Be(0);
            players[0].DeckOfCards.DiscardedPile.Count.Should().Be(0);
            players[0].DeckOfCards.DrawPile.Count.Should().Be(1);
            players[1].DeckOfCards.DiscardedPile.Count.Should().Be(4);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(1);
        }
    }
}