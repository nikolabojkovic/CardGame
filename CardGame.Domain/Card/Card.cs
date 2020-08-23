using System;

namespace CardGame.Domain
{
    public class Card
    {
        protected Card() { }
        public Suit Suit { get; private set; }
        public int Face { get; private set; }

        public Player Player{ get; set; }

        public static Card Create(Suit suit, int face)
        { 
            return new Card {
                Suit = suit,
                Face = face
            };
        }

        public override string ToString() {
            return $"({Face} {Suit})";
        }
    }
}
