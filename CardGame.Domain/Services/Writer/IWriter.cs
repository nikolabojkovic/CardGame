namespace CardGame.Domain 
{
    public interface IWriter 
    {
        void WriteLine(string content);
        void WriteLine();
    }
}