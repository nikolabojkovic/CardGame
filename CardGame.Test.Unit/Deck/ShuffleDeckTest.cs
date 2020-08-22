using System;
using CardGame.Domain;
using Xunit;
using FluentAssertions;
using Moq;

namespace CardGame.UnitTests
{
    public partial class DeckTest
    {
        [Fact]
        public void SwapCards_InDeck_ShouldSwap2Cards()
        {
            // Arrange
            Deck deck = Deck.CreateDeck(40, new RandomNumberGenerator());
            Card[] cards = new Card[6] 
            {
                Card.CreateCard(Suit.Clubs, 1),
                Card.CreateCard(Suit.Clubs, 2),
                Card.CreateCard(Suit.Clubs, 3),
                Card.CreateCard(Suit.Clubs, 4),
                Card.CreateCard(Suit.Clubs, 5),
                Card.CreateCard(Suit.Clubs, 6)
            };

            // Act
            deck.Swap(cards, 2, 5);

            // Assert
            cards[2].Face.Should().Be(6);
            cards[5].Face.Should().Be(3);
        }

        [Fact]
        public void Shuffle_Deck_ShouldShuffleDrawPile()
        {
            // Arrange
        
            Deck deck = Deck.CreateDeck(6, new FakeRandomNumberGenerator());            
            var beforeShuffle = deck.DrawPile.ToArray();

            // Act
            deck.Shuffle(deck.DrawPile);

            // Assert            
            deck.DrawPile.Count.Should().Be(6);
            var afterShuffle = deck.DrawPile.ToArray();
            var stateHasChanged = false;
            for(int i = 0; i < afterShuffle.Length; i++) 
            {
                if (beforeShuffle[i] != afterShuffle[i])
                    stateHasChanged = true;
            }

            stateHasChanged.Should().BeTrue();
        }
    }
}
