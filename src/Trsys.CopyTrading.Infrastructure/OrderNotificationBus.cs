using System;
using System.Collections.Generic;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class OrderNotificationBus : IOrderNotificationBus
    {
        private readonly BlockingTaskQueue queue = new();
        public event EventHandler<OrderEventArgs> OrderOpenPublished;
        public event EventHandler<OrderEventArgs> OrderClosePublished;
        private List<Action<OrderText>> handlers = new();

        public void PublishOpen(PublisherOrder order)
        {
            queue.Enqueue(() =>
            {
                OrderOpenPublished?.Invoke(this, new OrderEventArgs(order));
            });
        }

        public void PublishClose(PublisherOrder order)
        {
            queue.Enqueue(() =>
            {
                OrderClosePublished?.Invoke(this, new OrderEventArgs(order));
            });
        }

        public void AddOrderTextUpdatedHandler(Action<OrderText> handler)
        {
            handlers.Add(handler);
        }

        public void RemoveOrderTextUpdatedHandler(Action<OrderText> handler)
        {
            handlers.Remove(handler);
        }

        public void UpdateOrderText(OrderText orderText)
        {
            queue.Enqueue(() =>
            {
                foreach (var handler in handlers.ToArray())
                {
                    handler.Invoke(orderText);
                }
            });
        }
    }
}
