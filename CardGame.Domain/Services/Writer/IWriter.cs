namespace CardGame.Domain 
{
    public interface IWriter 
    {
        void WriteLine(GameActionEvent content);
        void WriteLine();
    }
}