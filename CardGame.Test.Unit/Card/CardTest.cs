using System;
using CardGame.Domain;
using Xunit;
using FluentAssertions;

namespace CardGame.UnitTests
{
    public class CardTest
    {
        [Theory]
        [InlineData(Suit.Clubs, 5)]
        public void CreateNewCard_WithSuitAndFace_ShouldCreateNewCardObject(Suit expectedSuit, int expectedFace)
        {
            // Arrange
            var expectedCard = $"({expectedFace} {expectedSuit})";

            // Act
            Card card = Card.Create(expectedSuit, expectedFace);

            // Assert
            card.Suit.Should().Be(expectedSuit);
            card.Face.Should().Be(expectedFace);
            card.ToString().Should().Be(expectedCard);
        }
    }
}
