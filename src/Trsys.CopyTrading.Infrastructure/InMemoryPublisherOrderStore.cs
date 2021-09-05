using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryPublisherOrderStore : IPublisherOrderStore
    {
        private readonly InMemoryCopyTradingContext context;

        public InMemoryPublisherOrderStore(InMemoryCopyTradingContext context)
        {
            this.context = context;
        }

        public Task<OrderDifference<PublisherOrder>> SetOrderTextAsync(string publisherKey, string text)
        {
            var orders = OrderText.Parse(text).Orders;
            var current = context.PublisherOrderStore.TryGetValue(publisherKey, out var publisherOrders) ? publisherOrders : Array.Empty<PublisherOrder>() as IEnumerable<PublisherOrder>;
            var diff = OrderDifference<PublisherOrder>.CalculateDifference(current, orders, (po, o) => po.TicketNo - o.TicketNo, lu => new PublisherOrder()
            {
                PublisherKey = publisherKey,
                Text = lu.Text,
                TicketNo = lu.TicketNo,
                Symbol = lu.Symbol,
                OrderType = lu.OrderType,
                Price = lu.Price,
                Lots = lu.Lots,
                Time = lu.Time,
            });
            if (diff.HasDifference)
            {
                var newPublishedOrders = publisherOrders?.ToList() ?? new List<PublisherOrder>();
                foreach (var order in diff.Opened)
                {
                    newPublishedOrders.Add(order);
                }
                foreach (var order in diff.Closed)
                {
                    newPublishedOrders.Remove(order);
                }
                context.PublisherOrderStore[publisherKey] = newPublishedOrders;
            }
            return Task.FromResult(diff);
        }
    }
}
