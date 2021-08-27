using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryOrderStore : IOrderStore
    {
        private Dictionary<string, PublishedOrders> _store = new();

        public Task SetTextAsync(string key, string text)
        {
            _store[key] = PublishedOrders.Parse(text);
            return Task.CompletedTask;
        }
    }
}
