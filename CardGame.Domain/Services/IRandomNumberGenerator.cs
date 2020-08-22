namespace CardGame.Domain 
{
    public interface IRandomNumberGenerator 
    {
        int Next(int maxValue);
    }
}