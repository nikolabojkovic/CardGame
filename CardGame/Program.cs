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
                // register event to respond to game actions
                DomainEvents.Register<GameActionEvent>(action => writer.WriteLine(action));
                var numberOfCards = 40;

                Game.Create(new List<Player> 
                        {
                            Player.Create("Player 1"),
                            Player.Create("Player 2")
                        },
                        Deck.Create(numberOfCards, new RandomNumberGenerator()))
                    .Play();
                
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
