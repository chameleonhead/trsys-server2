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
            var diff = OrderDifference<SubscriberOrder>.CalculateDifference(current, orders, (po, o) => po.PublisherOrderId.CompareTo(o.Id), lu => new SubscriberOrder()
            {
                Id = Guid.NewGuid().ToString(),
                SubscriberKey = subscriberKey,
                PublisherOrderId = lu.Id,
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
