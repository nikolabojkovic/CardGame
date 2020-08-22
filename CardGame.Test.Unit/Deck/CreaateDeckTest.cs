using System;
using CardGame.Domain;
using Xunit;
using FluentAssertions;

namespace CardGame.UnitTests
{
    public partial class DeckTest
    {
        [Theory]
        [InlineData(5)]
        [InlineData(40)]
        [InlineData(39)]
        public void CreateNewDeck_WithSomeNumberOfCards_ShouldCreateDeckOfExpectedNumberOfCards(int expectedNumberOfCards)
        {
            // Arrange

            // Act
            Deck deck = Deck.CreateDeck(expectedNumberOfCards, new RandomNumberGenerator());

            // Assert
            deck.DrawPile.Count.Should().Be(expectedNumberOfCards);
        }

        [Fact]
        public void CreateNewDeck_With40Cards_ShouldCreateDeckOf4Times1To10Card()
        {
            // Arrange

            // Act
            Deck deck = Deck.CreateDeck(40, new RandomNumberGenerator());

            // Assert
            deck.DrawPile.Count.Should().Be(40);

            for(int i = 0; i < 4; i++)
            {
                for (int j = 10; j > 0; j--) 
                {
                    var card = deck.DrawPile.Pop();
                    card.Face.Should().Be(j);
                }
            }
        }

        [Fact]
        public void CreateNewDeck_With47Cards_ShouldCreateDeckOf4Times1To10AndOnce1To7Card()
        {
            // Arrange

            // Act
            Deck deck = Deck.CreateDeck(47, new RandomNumberGenerator());

            // Assert
            deck.DrawPile.Count.Should().Be(47);

            for(int i = 7; i > 0; i--)
            {
                var card = deck.DrawPile.Pop();
                card.Face.Should().Be(i);
            }

            for(int i = 0; i < 4; i++)
            {
                for (int j = 10; j > 0; j--) 
                {
                    var card = deck.DrawPile.Pop();
                    card.Face.Should().Be(j);
                }
            }
        }

        [Fact]
        public void TotalCardsCount_WithNoPlayedCard_ShouldReturn10Cards()
        {
            // Arrange

            // Act
            Deck deck = Deck.CreateDeck(10, new RandomNumberGenerator());

            // Assert
            deck.TotalCardsCount().Should().Be(10);
            deck.DrawPile.Count.Should().Be(10);
            deck.DiscaredPile.Count.Should().Be(0);
            deck.PlayedCard.Should().BeNull();
        }

        [Fact]
        public void TotalCardsCount_WithPlayedCard_ShouldReturn10Cards()
        {
            // Arrange

            // Act
            Deck deck = Deck.CreateDeck(10, new RandomNumberGenerator());
            deck.DrawCard();

            // Assert
            deck.TotalCardsCount().Should().Be(10);
            deck.DrawPile.Count.Should().Be(9);
            deck.DiscaredPile.Count.Should().Be(0);
            deck.PlayedCard.Should().NotBeNull();
        }
    }
}
