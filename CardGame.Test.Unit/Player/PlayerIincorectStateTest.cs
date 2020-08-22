using System;
using CardGame.Domain;
using FluentAssertions;
using Xunit;

namespace CardGame.UnitTests
{
    public class PlayerIncorectStateTest 
    {
        [Fact]
        public void PlayCard_WithNoCardsInDeck_ShouldThrowException()
        {
            // Arrange
            string expectedPlayerName = "Player 1";
            // Act
            var player = Player.CreatePlayer(expectedPlayerName);
            Action act = () =>  player.PlayCard();
            
            // Assert
            act.Should().Throw<Exception>().WithMessage("Deck is Empty!");
        }
    }

}