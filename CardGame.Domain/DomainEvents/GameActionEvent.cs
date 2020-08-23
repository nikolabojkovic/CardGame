namespace CardGame.Domain
{
    public class GameActionEvent : IDomainEvent
    {
        public GameActionEvent(string s)
        {
            Description = s;
        }

        public string Description { get; }
    }
}