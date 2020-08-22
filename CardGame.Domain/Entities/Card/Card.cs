using System;

namespace CardGame.Domain
{
    public class Card
    {
        protected Card() { }
        public Suit Suit { get; private set; }
        public int Face { get; private set; }

        public Player Player{ get; private set; }

        public static Card CreateCard(Suit suit, int face)
        { 
            return new Card {
                Suit = suit,
                Face = face
            };
        }

        public void AssignToPlayer(Player player)
        {
            Player = player;
        }

        public override string ToString() {
            return $"({Face} {Suit})";
        }
    }
}
