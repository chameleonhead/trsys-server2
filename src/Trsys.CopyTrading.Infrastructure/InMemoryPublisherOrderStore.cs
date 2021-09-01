using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryPublisherOrderStore : IPublisherOrderStore
    {
        private readonly Dictionary<string, Dictionary<int, PublisherOrder>> _store = new();

        public Task<OrderDifference<PublisherOrder>> SetOrderTextAsync(string publisherKey, string text)
        {
            var orders = OrderText.Parse(text).Orders;
            if (_store.TryGetValue(publisherKey, out var publishedOrdersDictionary) && publishedOrdersDictionary.Any())
            {
                if (orders.Any())
                {
                    var prevTicketNos = publishedOrdersDictionary.Values.Select(o => o.TicketNo);
                    var nextTicketNos = orders.Select(o => o.TicketNo);
                    var closed = prevTicketNos.Except(nextTicketNos);
                    var notChanged = prevTicketNos.Intersect(nextTicketNos);
                    var opened = nextTicketNos.Except(prevTicketNos);
                    var notChangedOrders = publishedOrdersDictionary.Values.Where(kv => notChanged.Contains(kv.TicketNo));
                    var openedOrders = orders.Where(o => opened.Contains(o.TicketNo)).Select(lu => new PublisherOrder()
                    {
                        Id = Guid.NewGuid().ToString(),
                        PublisherKey = publisherKey,
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
                        var newPublishedOrdersDictionary = new Dictionary<int, PublisherOrder>();
                        foreach (var order in notChangedOrders)
                        {
                            newPublishedOrdersDictionary.Add(order.TicketNo, order);
                        }
                        foreach (var order in openedOrders)
                        {
                            newPublishedOrdersDictionary.Add(order.TicketNo, order);
                        }
                        _store[publisherKey] = newPublishedOrdersDictionary;
                        return Task.FromResult(new OrderDifference<PublisherOrder>(openedOrders, closed.Select(o => publishedOrdersDictionary[o])));
                    }
                    else
                    {
                        return Task.FromResult(OrderDifference<PublisherOrder>.NoDifference);
                    }
                }
                else
                {
                    _store.Remove(publisherKey);
                    return Task.FromResult(new OrderDifference<PublisherOrder>(Array.Empty<PublisherOrder>(), publishedOrdersDictionary.Values.ToArray()));
                }
            }
            else
            {
                if (orders.Any())
                {
                    var openedOrders = orders.Select(lu => new PublisherOrder()
                    {
                        Id = Guid.NewGuid().ToString(),
                        PublisherKey = publisherKey,
                        Text = lu.Text,
                        TicketNo = lu.TicketNo,
                        Symbol = lu.Symbol,
                        OrderType = lu.OrderType,
                        Price = lu.Price,
                        Lots = lu.Lots,
                        Time = lu.Time,
                    });
                    var newPublishedOrdersDictionary = new Dictionary<int, PublisherOrder>();
                    foreach (var order in openedOrders)
                    {
                        newPublishedOrdersDictionary.Add(order.TicketNo, order);
                    }
                    _store[publisherKey] = newPublishedOrdersDictionary;
                    return Task.FromResult(new OrderDifference<PublisherOrder>(openedOrders.ToArray(), Array.Empty<PublisherOrder>()));
                }
                else
                {
                    return Task.FromResult(OrderDifference<PublisherOrder>.NoDifference);
                }
            }
        }
    }
}
