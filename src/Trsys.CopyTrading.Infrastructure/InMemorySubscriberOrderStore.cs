using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemorySubscriberOrderStore : ISubscriberOrderStore
    {
        private readonly Dictionary<string, List<SubscriberOrder>> _store = new();

        public Task<OrderDifference<SubscriberOrder>> SetOrderTextAsync(string subscriberKey, ActiveOrder activeOrder)
        {
            var orders = activeOrder.Orders;
            var current = _store.TryGetValue(subscriberKey, out var subscriberOrders) ? subscriberOrders : Array.Empty<SubscriberOrder>() as IEnumerable<SubscriberOrder>;
            var diff = OrderDifference<SubscriberOrder>.CalculateDifference(current, orders, (po, o) =>
            {
                var result = po.PublisherKey.CompareTo(o.PublisherKey);
                if (result != 0)
                {
                    return result;
                }
                return po.TicketNo.CompareTo(o.TicketNo);
            }, lu => new SubscriberOrder()
            {
                SubscriberKey = subscriberKey,
                PublisherKey = lu.PublisherKey,
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
                var newPublishedOrders = subscriberOrders?.ToList() ?? new List<SubscriberOrder>();
                foreach (var order in diff.Opened)
                {
                    newPublishedOrders.Add(order);
                }
                foreach (var order in diff.Closed)
                {
                    newPublishedOrders.Remove(order);
                }
                _store[subscriberKey] = newPublishedOrders;
            }
            return Task.FromResult(diff);
        }
    }
}
