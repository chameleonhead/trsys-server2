using System;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryEaStore : IEaStore
    {
        private readonly InMemoryCopyTradingContext context;
        private readonly IEventQueue events;
        private readonly IOrderNotificationBus orderBus;

        public InMemoryEaStore(InMemoryCopyTradingContext context, IEventQueue events, IOrderNotificationBus orderBus)
        {
            this.context = context;
            this.events = events;
            this.orderBus = orderBus;
        }

        public void Add(string key, string keyType)
        {
            switch (keyType)
            {
                case "Publisher":
                    context.EaStore.TryAdd(new InMemoryKeys.SecretKey(key, keyType), new Publisher(key, events, orderBus));
                    break;
                case "Subscriber":
                    context.EaStore.TryAdd(new InMemoryKeys.SecretKey(key, keyType), new Subscriber(key, events));
                    break;
                default:
                    throw new ArgumentException("Invalid key type", nameof(keyType));
            }
        }

        public EaBase Find(string key, string keyType)
        {
            return context.EaStore.TryGetValue(new InMemoryKeys.SecretKey(key, keyType), out var value) ? value : null;
        }

        public void Remove(string key, string keyType)
        {
            if (context.EaStore.TryRemove(new InMemoryKeys.SecretKey(key, keyType), out var value))
            {
                value.Dispose();
            }
        }
    }
}
