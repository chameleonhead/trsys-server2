using System.Collections.Concurrent;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryCopyTradingContext
    {
        public ConcurrentDictionary<InMemoryKeys.SecretKey, EaBase> EaStore { get; } = new();
    }
}
