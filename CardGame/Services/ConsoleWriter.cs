using System;

namespace CardGame.Domain 
{
    public class ConsoleWriter : IWriter
    {
        public void WriteLine(GameActionEvent action) 
        {
            Console.WriteLine(action.Description);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }
    }
}