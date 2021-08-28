using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryOrderStore : IOrderStore
    {
        private Dictionary<string, PublishedOrders> _store = new();
        private PublishedOrders currentOrderText = PublishedOrders.Empty;

        public Task SetTextAsync(string publisher, string text)
        {
            var orders = PublishedOrders.Parse(publisher, text);
            _store[publisher] = orders;
            if (currentOrderText == PublishedOrders.Empty)
            {
                if (orders == PublishedOrders.Empty)
                {
                    return Task.CompletedTask;
                }
                currentOrderText = PublishedOrders.FromOrder(publisher, orders.Orders.First());
                return Task.CompletedTask;
            }
            else if (currentOrderText.Publisher == publisher)
            {
                var targetOrder = currentOrderText.Orders.Single();
                if (orders.Orders.Any(o => o.TicketNo == targetOrder.TicketNo))
                {
                    return Task.CompletedTask;
                }
                currentOrderText = PublishedOrders.Empty;
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }

        public Task<PublishedOrders> GetTextAsync(string subscriber)
        {
            return Task.FromResult(currentOrderText);
        }
    }
}
