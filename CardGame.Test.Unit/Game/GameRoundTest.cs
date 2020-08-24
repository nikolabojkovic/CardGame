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
        public void PlayCard_ShouldReturnPlayedCard()
        {
            // Arrange
            var deckOfCards1 = Deck.Create(0, new RandomNumberGenerator());
            deckOfCards1.DrawPile.Push(Card.Create(Suit.Clubs, 7));
            deckOfCards1.DrawPile.Push(Card.Create(Suit.Clubs, 4));
            var deckOfCards2 = Deck.Create(0, new RandomNumberGenerator());
            deckOfCards2.DrawPile.Push(Card.Create(Suit.Clubs, 2));
            deckOfCards2.DrawPile.Push(Card.Create(Suit.Clubs, 5));
            var deckOfCards3 = Deck.Create(0, new RandomNumberGenerator());
            deckOfCards3.DrawPile.Push(Card.Create(Suit.Clubs, 6));
            deckOfCards3.DrawPile.Push(Card.Create(Suit.Clubs, 8));

            var players = new List<Player>
            {
                Player.Create("Player 1"),
                Player.Create("Player 2"),
                Player.Create("Player 3")
            }; 

            players[0].DeckOfCards = deckOfCards1;
            players[1].DeckOfCards = deckOfCards2;
            players[2].DeckOfCards = deckOfCards3;

            var cards = new List<Card>();

            // Act
            cards.Add(players[0].PlayCard());
            cards.Add(players[1].PlayCard());
            cards.Add(players[2].PlayCard());

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
            var player = Player.Create("Player 1");
            var deckOfCards = Deck.Create(10, new RandomNumberGenerator());
            deckOfCards.DiscardedPile.Push(Card.Create(Suit.Clubs, 4));
            deckOfCards.DiscardedPile.Push(Card.Create(Suit.Clubs, 3));
            deckOfCards.DiscardedPile.Push(Card.Create(Suit.Clubs, 8));
            var game = Game.Create(new List<Player> {player}, deckOfCards);

            // Act
            game.AssignPlayedCardsTo(player);

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
                Player.Create("Player 1"),
                Player.Create("Player 2")
            };

            var deck = Deck.Create(0, new RandomNumberGenerator());
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 4));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 6));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 7));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 1));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 5));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 2));

            var game = Game.Create(players, deck);
            game.DrawCards(game.DeckOfCards.DrawPile.Count / game.Players.Count());

            // Act
            game.PlayRoundFor(players);

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
                Player.Create("Player 1"),
                Player.Create("Player 2")
            };

            var deck = Deck.Create(0, new RandomNumberGenerator());
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 4));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 6));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 7));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 4));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 5));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 2));

            var game = Game.Create(players, deck);
            game.DrawCards(game.DeckOfCards.DrawPile.Count / game.Players.Count());

            // Act
            game.PlayRoundFor(players);
            players[0].DeckOfCards.DiscardedPile.Count.Should().Be(0);
            players[1].DeckOfCards.DiscardedPile.Count.Should().Be(0);

            game.PlayRoundFor(players);

            // Assert
            deck.DrawPile.Count.Should().Be(0);
            players[0].DeckOfCards.DiscardedPile.Count.Should().Be(0);
            players[0].DeckOfCards.DrawPile.Count.Should().Be(1);
            players[1].DeckOfCards.DiscardedPile.Count.Should().Be(4);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(1);
        }
    }
}