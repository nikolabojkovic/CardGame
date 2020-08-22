using System;
using CardGame.Domain;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var card = Card.NewCard(Suit.Clubs, 5);
            Console.WriteLine(card);
        }
    }
}
