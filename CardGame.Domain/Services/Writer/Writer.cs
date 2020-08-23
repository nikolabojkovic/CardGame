using System;

namespace CardGame.Domain 
{
    public class ConsoleWriter: IWriter
    {
        public void WriteLine(string content) 
        {
            Console.WriteLine(content);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }
    }
}