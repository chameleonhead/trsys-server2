using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemorySubscriberOrderStore : ISubscriberOrderStore
    {
        private readonly Dictionary<string, Dictionary<int, SubscriberOrder>> _store = new();

        public Task<OrderDifference<SubscriberOrder>> SetOrderTextAsync(string subscriberKey, ActiveOrder activeOrder)
        {
            var orders = activeOrder.Orders;
            if (_store.TryGetValue(subscriberKey, out var subscriberOrdersDictionary) && subscriberOrdersDictionary.Any())
            {
                if (orders.Any())
                {
                    var prevTicketNos = subscriberOrdersDictionary.Values.Select(o => o.TicketNo);
                    var nextTicketNos = orders.Select(o => o.TicketNo);
                    var closed = prevTicketNos.Except(nextTicketNos);
                    var notChanged = prevTicketNos.Intersect(nextTicketNos);
                    var opened = nextTicketNos.Except(prevTicketNos);
                    var notChangedOrders = subscriberOrdersDictionary.Values.Where(kv => notChanged.Contains(kv.TicketNo));
                    var openedOrders = orders.Where(o => opened.Contains(o.TicketNo)).Select(lu => new SubscriberOrder()
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
                    if (opened.Any() || closed.Any())
                    {
                        var newPublishedOrdersDictionary = new Dictionary<int, SubscriberOrder>();
                        foreach (var order in notChangedOrders)
                        {
                            newPublishedOrdersDictionary.Add(order.TicketNo, order);
                        }
                        foreach (var order in openedOrders)
                        {
                            newPublishedOrdersDictionary.Add(order.TicketNo, order);
                        }
                        _store[subscriberKey] = newPublishedOrdersDictionary;
                        return Task.FromResult(new OrderDifference<SubscriberOrder>(openedOrders, closed.Select(o => subscriberOrdersDictionary[o])));
                    }
                    else
                    {
                        return Task.FromResult(OrderDifference<SubscriberOrder>.NoDifference);
                    }
                }
                else
                {
                    _store.Remove(subscriberKey);
                    return Task.FromResult(new OrderDifference<SubscriberOrder>(Array.Empty<SubscriberOrder>(), subscriberOrdersDictionary.Values.ToArray()));
                }
            }
            else
            {
                if (orders.Any())
                {
                    var openedOrders = orders.Select(lu => new SubscriberOrder()
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
                    var newPublishedOrdersDictionary = new Dictionary<int, SubscriberOrder>();
                    foreach (var order in openedOrders)
                    {
                        newPublishedOrdersDictionary.Add(order.TicketNo, order);
                    }
                    _store[subscriberKey] = newPublishedOrdersDictionary;
                    return Task.FromResult(new OrderDifference<SubscriberOrder>(openedOrders.ToArray(), Array.Empty<SubscriberOrder>()));
                }
                else
                {
                    return Task.FromResult(OrderDifference<SubscriberOrder>.NoDifference);
                }
            }
        }
    }
}
