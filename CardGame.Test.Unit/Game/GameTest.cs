using System;
using System.Collections.Generic;
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
            var deckOfCards = Deck.CreateDeck(numberOfCards, new RandomNumberGenerator());

            var players = new List<Player>
            {
                Player.CreatePlayer("Player 1"),
                Player.CreatePlayer("Player 2"),
                Player.CreatePlayer("Player 3")
            };

            var game = Game.CreateGame(players, deckOfCards, new FakeWriter());

            // Act
            game.DrawCards();

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
            var deckOfCards = Deck.CreateDeck(numberOfCards, new RandomNumberGenerator());

            var players = new List<Player>
            {
                Player.CreatePlayer("Player 1"),
                Player.CreatePlayer("Player 2")
            };

            var game = Game.CreateGame(players, deckOfCards, new FakeWriter());

            // Act
            game.DrawCards();

            // Assert
            players[0].DeckOfCards.DrawPile.Count.Should().Be(numberOfCards / 2);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(numberOfCards / 2);
        }

        [Fact]
        public void FindWinningCard_WhereOnlyOneWinningCard_ShouldFindHighestCard()
        {
            // Arrange
            var game = Game.CreateGame(null, null, new FakeWriter());
            List<Card> cards = new List<Card> 
            {
                Card.CreateCard(Suit.Clubs, 5),
                Card.CreateCard(Suit.Clubs, 8),
                Card.CreateCard(Suit.Clubs, 2)
            };

            // Act
            var winningCard = game.FindWinningCard(cards);

            // Assert
            winningCard.Should().NotBeNull();
            winningCard.Face.Should().Be(8);
        }

        [Fact]
        public void FindWinningCard_WhereMoreThenOneWinningCard_ShouldNotFind()
        {
            // Arrange
            var game = Game.CreateGame(null, null, new FakeWriter());
            List<Card> cards = new List<Card> 
            {
                Card.CreateCard(Suit.Clubs, 8),
                Card.CreateCard(Suit.Clubs, 8),
                Card.CreateCard(Suit.Clubs, 2)
            };

            // Act
            var winningCard = game.FindWinningCard(cards);

            // Assert
            winningCard.Should().BeNull();
        }
    
        [Fact]
        public void StartGame_ShouldPlay2Rounds()
        {
             // Arrange
            var players = new List<Player> 
            {
                Player.CreatePlayer("Player 1"),
                Player.CreatePlayer("Player 2")
            };

            var deck = Deck.CreateDeck(0, new FakeRandomNumberGenerator());
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 4));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 6));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 7));
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 4));

            var game = Game.CreateGame(players, deck, new FakeWriter());

            // Act
            game.StartGame();

            // Assert
            deck.DrawPile.Count.Should().Be(0);
            players[0].DeckOfCards.DiscardedPile.Count.Should().Be(0);
            players[0].DeckOfCards.DrawPile.Count.Should().Be(0);
            players[1].DeckOfCards.DiscardedPile.Count.Should().Be(4);
            players[1].DeckOfCards.DrawPile.Count.Should().Be(0);
        }

         [Fact]
        public void StartGame_BorderlineCase_NoEnoughCards_ShouldPThrowException()
        {
             // Arrange
            var players = new List<Player> 
            {
                Player.CreatePlayer("Player 1"),
                Player.CreatePlayer("Player 2")
            };

            var deck = Deck.CreateDeck(0, new FakeRandomNumberGenerator());
            deck.DrawPile.Push(Card.CreateCard(Suit.Clubs, 4));

            var game = Game.CreateGame(players, deck, new FakeWriter());

            // Act
            Action act = () =>  game.StartGame();
            
            // Assert
            act.Should().Throw<Exception>().WithMessage("No enough cards in the deck!");
        }
    }
}