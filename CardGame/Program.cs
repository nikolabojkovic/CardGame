using System;
using System.Collections.Generic;
using CardGame.Domain;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try 
            {
                IWriter writer = new ConsoleWriter();
                DomainEvents.Register<GameAction>(ev => writer.WriteLine(ev.Description));

                var players = new List<Player> 
                {
                    Player.CreatePlayer("Player 1"),
                    Player.CreatePlayer("Player 2")
                };

                var deck = Deck.CreateDeck(40, new RandomNumberGenerator());

                Game.CreateGame(players, deck)
                    .StartGame();
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Whoops, something went wrong. {ex.Message}");
            }
            finally
            {
               // Console.WriteLine("Press any key to close...");
               // Console.ReadKey();
            }
        }
    }
}
