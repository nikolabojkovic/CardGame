using CardGame.Domain;

namespace CardGame.UnitTests
{
    public class FakeRandomNumberGenerator : IRandomNumberGenerator
    {
        public int Next(int maxValue)
        {
            return maxValue / 2;
        }
    }
}