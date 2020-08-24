using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Domain;
using FluentAssertions;
using Xunit;

namespace CardGame.UnitTests
{
    public class GameTest
    {

        [Theory]
        [InlineData(60)]
        [InlineData(40)]
        public void DrowCards_ShouldDrawEqualNumberOfCardsTo3Players(int numberOfCards)
        {
            // Arrange
            var deckOfCards = Deck.Create(numberOfCards, new RandomNumberGenerator());

            var players = new List<Player>
            {
                Player.Create("Player 1"),
                Player.Create("Player 2"),
                Player.Create("Player 3")
            };

            var game = Game.Create(players, deckOfCards);

            // Act
            game.DrawCards(game.DeckOfCards.DrawPile.Count / game.Players.Count());

            // Assert
            players[0].DeckOfCards.DrawPile.Count.Should().Be(numberOfCards / 3);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(numberOfCards / 3);
            players[2].DeckOfCards.DrawPile.Count.Should().Be(numberOfCards / 3);
        }

        [Theory]
        [InlineData(66)]
        [InlineData(40)]
        public void DrowCards_ShouldDrawEqualNumberOfCardsTo2Players(int numberOfCards)
        {
            // Arrange
            var deckOfCards = Deck.Create(numberOfCards, new RandomNumberGenerator());

            var players = new List<Player>
            {
                Player.Create("Player 1"),
                Player.Create("Player 2")
            };

            var game = Game.Create(players, deckOfCards);

            // Act
            game.DrawCards(game.DeckOfCards.DrawPile.Count / game.Players.Count());

            // Assert
            players[0].DeckOfCards.DrawPile.Count.Should().Be(numberOfCards / 2);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(numberOfCards / 2);
        }

        [Theory]
        [InlineData(40)]
        public void DrowCards_Draw2Hands_ShouldDrawTo2Players(int numberOfCards)
        {
            // Arrange
            var deckOfCards = Deck.Create(numberOfCards, new RandomNumberGenerator());

            var players = new List<Player>
            {
                Player.Create("Player 1"),
                Player.Create("Player 2")
            };

            var game = Game.Create(players, deckOfCards);

            // Act
            game.DrawCards(10);

            // Assert
            players[0].DeckOfCards.DrawPile.Count.Should().Be(10);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(10);

            game.DrawCards(10);
            players[0].DeckOfCards.DrawPile.Count.Should().Be(20);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(20);
        }

        [Theory]
        [InlineData(40)]
        public void DrowCards_Draw3Hands_ShouldThrowException(int numberOfCards)
        {
            // Arrange
            var deckOfCards = Deck.Create(numberOfCards, new RandomNumberGenerator());

            var players = new List<Player>
            {
                Player.Create("Player 1"),
                Player.Create("Player 2")
            };

            var game = Game.Create(players, deckOfCards);

            // Act
            game.DrawCards(10);
            game.DrawCards(10);
            Action act= () => game.DrawCards(10);

            // Assert
            act.Should().Throw<Exception>().WithMessage("No enough cards in the deck!");
        }

        [Fact]
        public void FindWinningCard_OnlyOneWinningCardExists_ShouldFindHighestCard()
        {
            // Arrange
            var game = Game.Create(null, null);
            List<Card> cards = new List<Card> 
            {
                Card.Create(Suit.Clubs, 5),
                Card.Create(Suit.Clubs, 8),
                Card.Create(Suit.Clubs, 2)
            };

            // Act
            var winningCard = game.FindWinningCardFrom(cards);

            // Assert
            winningCard.Should().NotBeNull();
            winningCard.Face.Should().Be(8);
        }

        [Fact]
        public void FindWinningCard_MoreThenOneWinningCardsExist_ShouldNotFindWinningCard()
        {
            // Arrange
            var game = Game.Create(null, null);
            List<Card> cards = new List<Card> 
            {
                Card.Create(Suit.Clubs, 8),
                Card.Create(Suit.Clubs, 8),
                Card.Create(Suit.Clubs, 2)
            };

            // Act
            var winningCard = game.FindWinningCardFrom(cards);

            // Assert
            winningCard.Should().BeNull();
        }
    
        [Fact]
        public void StartGame_ShouldPlay2Rounds()
        {
             // Arrange
            var players = new List<Player> 
            {
                Player.Create("Player 1"),
                Player.Create("Player 2")
            };

            var deck = Deck.Create(0, new FakeRandomNumberGenerator());
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 4));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 6));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 7));
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 4));

            var game = Game.Create(players, deck);

            // Act
            game.Play();

            // Assert
            deck.DrawPile.Count.Should().Be(0);
            players[0].DeckOfCards.DiscardedPile.Count.Should().Be(4);
            players[0].DeckOfCards.DrawPile.Count.Should().Be(0);
            players[1].DeckOfCards.DiscardedPile.Count.Should().Be(0);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(0);
        }

         [Fact]
        public void StartGame_BorderlineCase_NotEnoughCards_ShouldPThrowException()
        {
             // Arrange
            var players = new List<Player> 
            {
                Player.Create("Player 1"),
                Player.Create("Player 2")
            };

            var deck = Deck.Create(0, new FakeRandomNumberGenerator());
            deck.DrawPile.Push(Card.Create(Suit.Clubs, 4));

            var game = Game.Create(players, deck);

            // Act
            Action act = () =>  game.Play();
            
            // Assert
            act.Should().Throw<Exception>().WithMessage("No enough cards in the deck!");
        }
    }
}