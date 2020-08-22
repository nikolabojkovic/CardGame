using System;

namespace CardGame.Domain 
{
    public class RandomNumberGenerator : Random, IRandomNumberGenerator
    {
        public override int Next(int maxValue) 
        {
            return base.Next(maxValue);
        }
    }
}