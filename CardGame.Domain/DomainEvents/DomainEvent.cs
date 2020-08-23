using System;
using System.Collections.Generic;

namespace CardGame.Domain
{
    public static class DomainEvents
    {
        private static Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();

        public static void Register<T>(Action<T> eventHandler)
            where T : IDomainEvent
        {
            if (!_handlers.ContainsKey(typeof(T)))
                _handlers.Add(typeof(T), new List<Delegate>());
                
            _handlers[typeof(T)].Add(eventHandler);
        }

        public static void Raise<T>(T domainEvent)
            where T : IDomainEvent
        {
            if (!_handlers.ContainsKey(domainEvent.GetType()))
                return;

            foreach (Delegate handler in _handlers[domainEvent.GetType()])
            {
                var action = (Action<T>)handler;
                action(domainEvent);
            }
        }
    }
}