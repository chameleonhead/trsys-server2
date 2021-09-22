using System.Collections.Concurrent;
using System.Collections.Generic;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryCopyTradingContext
    {
        public ConcurrentDictionary<InMemoryKeys.SecretKey, EaBase> EaStore { get; } = new();

        public OrderText CurrentOrderText { get; set; } = OrderText.Empty;
        public HashSet<InMemoryKeys.PublisherOrderKey> CurrentOrders { get; } = new();
    }
}
