namespace CardGame.Domain
{
    public class GameAction : IDomainEvent
    {
        public GameAction(string s)
        {
            Description = s;
        }

        public string Description { get; }
    }
}