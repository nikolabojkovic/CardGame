using CardGame.Domain;
using Xunit;
using FluentAssertions;

namespace CardGame.UnitTests
{
    public partial class DeckTest
    {
        [Fact]
        public void SwapCards_InDeck_ShouldSwap2Cards()
        {
            // Arrange
            Deck deck = Deck.Create(40, new RandomNumberGenerator());
            Card[] cards = new Card[6] 
            {
                Card.Create(Suit.Clubs, 1),
                Card.Create(Suit.Clubs, 2),
                Card.Create(Suit.Clubs, 3),
                Card.Create(Suit.Clubs, 4),
                Card.Create(Suit.Clubs, 5),
                Card.Create(Suit.Clubs, 6)
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
            Deck deck = Deck.Create(6, new FakeRandomNumberGenerator());            
            var beforeShuffle = deck.DrawPile.ToArray();

            // Act
            deck.Shuffle(deck.DrawPile);

            // Assert            
            deck.DrawPile.Count.Should().Be(6);
            var afterShuffle = deck.DrawPile.ToArray();
            var deckHasBeenShuffled = false;
            for(int i = 0; i < afterShuffle.Length; i++) 
            {
                if (beforeShuffle[i] != afterShuffle[i])
                    deckHasBeenShuffled = true;
            }

            deckHasBeenShuffled.Should().BeTrue();
        }

        [Fact]
        public void ShuffleDeck_EmptyDeck_BorderlineCase_ShouldShuffleDrawPile()
        {
            // Arrange        
            Deck deck = Deck.Create(0, new FakeRandomNumberGenerator());            
            var beforeShuffle = deck.DrawPile.ToArray();

            // Act
            deck.Shuffle(deck.DrawPile);

            // Assert            
            deck.DrawPile.Count.Should().Be(0);
        }

          [Fact]
        public void ShuffleDeck_WithOneCard_BorderLineCase_ShouldShuffleDrawPile()
        {
            // Arrange        
            Deck deck = Deck.Create(1, new FakeRandomNumberGenerator());            
            var beforeShuffle = deck.DrawPile.ToArray();

            // Act
            deck.Shuffle(deck.DrawPile);

            // Assert            
            deck.DrawPile.Count.Should().Be(1);
            deck.DrawPile.Peek().Face.Should().Be(1);
        }
    }
}
