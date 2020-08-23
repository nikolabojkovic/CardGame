using CardGame.Domain;
using FluentAssertions;
using Xunit;

namespace CardGame.UnitTests
{
    public class PlayerTest 
    {
        [Fact]
        public void CreatePlayer_WithName_ShouldCreatePlayerObjectWithName()
        {
            // Arrange
            string expectedPlayerName = "Player 1";
            // Act
            var player = Player.CreatePlayer(expectedPlayerName);

            // Assert
            player.Name.Should().Be(expectedPlayerName);
        }

        [Fact]
        public void AssignDeckOfCards_ToPlayer_ShouldAssignDeckOfCardsToPlayer()
        {
            // Arrange
            string expectedPlayerName = "Player 1";
            var player = Player.CreatePlayer(expectedPlayerName);

            // Act
            player.AssignDeckOfCards(Deck.CreateDeck(10, new RandomNumberGenerator()));

            // Assert
            player.DeckOfCards.Count().Should().Be(10);
            player.HasLostTheGame().Should().BeFalse();
        }

        [Fact]
        public void PlayOneCard_FromDeck_ShouldReturnPlayedCard()
        {
            // Arrange
            string expectedPlayerName = "Player 1";
            var player = Player.CreatePlayer(expectedPlayerName);
            player.AssignDeckOfCards(Deck.CreateDeck(10, new RandomNumberGenerator()));

            // Act
            var card = player.PlayCard();

            // Assert
            player.HasLostTheGame().Should().BeFalse();
            card.Face.Should().Be(10);
            player.DeckOfCards.DrawPile.Count.Should().Be(9);
            player.DeckOfCards.Count().Should().Be(10);
            player.DeckOfCards.PlayedCard.Should().NotBeNull();
        }

        [Fact]
        public void PlayOneCard_DrawPileIsEmpty_ShouldShuffleDiscardedPileAndAssignToDrawPileAndReturnPlayedCard()
        {
            // Arrange
            string expectedPlayerName = "Player 1";
            var player = Player.CreatePlayer(expectedPlayerName);
            player.AssignDeckOfCards(Deck.CreateDeck(2, new FakeRandomNumberGenerator()));
            player.DeckOfCards.DiscardedPile.Push(Card.CreateCard(Suit.Clubs, 9));
            player.DeckOfCards.DiscardedPile.Push(Card.CreateCard(Suit.Clubs, 7));
            player.DeckOfCards.DiscardedPile.Push(Card.CreateCard(Suit.Clubs, 8));

            // Act
            player.PlayCard();
            player.PlayCard();
            var card = player.PlayCard();

            // Assert
            player.HasLostTheGame().Should().BeFalse();
            card.Face.Should().Be(8);
            player.DeckOfCards.DrawPile.Count.Should().Be(2);
            player.DeckOfCards.DiscardedPile.Count.Should().Be(0);
            player.DeckOfCards.Count().Should().Be(3);
            player.DeckOfCards.PlayedCard.Should().NotBeNull();
        }


        [Fact]
        public void Player_PlayedAllCards_ShouldLooseTheGame()
        {
            // Arrange
            string expectedPlayerName = "Player 1";
            var player = Player.CreatePlayer(expectedPlayerName);
            player.AssignDeckOfCards(Deck.CreateDeck(2, new FakeRandomNumberGenerator()));

            // Act
            player.PlayCard();
            var card = player.PlayCard();
            player.DeckOfCards.PlayedCard = null;

            // Assert
            player.HasLostTheGame().Should().BeTrue();
            player.DeckOfCards.DrawPile.Count.Should().Be(0);
            player.DeckOfCards.DiscardedPile.Count.Should().Be(0);
            player.DeckOfCards.Count().Should().Be(0);
            player.DeckOfCards.PlayedCard.Should().BeNull();
        }
    }
}