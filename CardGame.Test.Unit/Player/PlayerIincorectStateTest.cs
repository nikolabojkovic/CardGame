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
            var player = Player.CreatePlayer(expectedPlayerName);
            player.DeckOfCards = Deck.CreateDeck(0, new RandomNumberGenerator());

            // Act            
            Action act = () =>  player.PlayCard();
            
            // Assert
            act.Should().Throw<Exception>().WithMessage("Deck is Empty!");
        }
    }

}