using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryActiveOrderStore : IActiveOrderStore
    {
        private ActiveOrder activeOrder = ActiveOrder.Empty;

        public Task<ActiveOrderSetResult> ApplyChangesAsync(OrderDifference<PublisherOrder> diff)
        {
            var opened = new List<PublisherOrder>();
            var ignored = new List<PublisherOrder>();
            var closed = new List<PublisherOrder>();
            var current = new List<PublisherOrder>();
            if (activeOrder.Orders.Any())
            {
                var currentOrders = activeOrder.Orders.ToList();
                if (diff.Closed.Any())
                {
                    foreach (var order in diff.Closed)
                    {
                        var o = currentOrders.FirstOrDefault(o => o.PublisherKey == order.PublisherKey && o.TicketNo == order.TicketNo);
                        if (o != null)
                        {
                            currentOrders.Remove(o);
                            closed.Add(o);
                        }
                    }
                }
                else
                {
                    current.AddRange(currentOrders);
                }
                ignored.AddRange(diff.Opened);
            }
            else
            {
                if (diff.Opened.Any())
                {
                    opened.AddRange(diff.Opened.Take(1));
                    ignored.AddRange(diff.Opened.Skip(1));
                    current.AddRange(opened);
                }
            }
            activeOrder = new ActiveOrder(OrderText.Parse(string.Join("@", current.Select(c => c.Text))), current);
            return Task.FromResult(new ActiveOrderSetResult(ignored, opened, closed, activeOrder));
        }

        public Task<ActiveOrder> GetActiveOrderAsync()
        {
            return Task.FromResult(activeOrder);
        }
    }
}
