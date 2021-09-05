using System.Collections.Concurrent;
using System.Collections.Generic;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryCopyTradingContext
    {
        public ActiveOrder ActiveOrder { get; set; } = ActiveOrder.Empty;

        public ConcurrentDictionary<string, EaSession> EaSessionStoreByToken { get; } = new();
        public ConcurrentDictionary<InMemoryKeys.SecretKey, EaSession> EaSessionStoreByKey { get; } = new();

        public ConcurrentDictionary<string, List<PublisherOrder>> PublisherOrderStore { get; } = new();

        public ConcurrentDictionary<InMemoryKeys.SecretKey, SecretKey> SecretKeyStore { get; } = new();

        public ConcurrentDictionary<string, List<SubscriberOrder>> SubscriberOrderStore { get; } = new();
    }
}
