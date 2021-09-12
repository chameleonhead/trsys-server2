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
        private List<Action<string, OrderText>> handlers = new();

        public void PublishOpen(string publisherKey, OrderTextItem orderTextItem)
        {
            queue.Enqueue(() =>
            {
                OrderOpenPublished?.Invoke(this, new OrderEventArgs(publisherKey, orderTextItem));
            });
        }

        public void PublishClose(string publisherKey, OrderTextItem orderTextItem)
        {
            queue.Enqueue(() =>
            {
                OrderClosePublished?.Invoke(this, new OrderEventArgs(publisherKey, orderTextItem));
            });
        }

        public void UpdateSubscriberOrder(string subscriberKey, OrderText text)
        {
            queue.Enqueue(() =>
            {
                foreach (var handler in handlers.ToArray())
                {
                    handler.Invoke(subscriberKey, text);
                }
            });
        }

        public void AddSubscriberOrderUpdateHandler(Action<string, OrderText> handler)
        {
            handlers.Add(handler);
        }

        public void RemoveSubscriberOrderUpdateHandler(Action<string, OrderText> handler)
        {
            handlers.Remove(handler);
        }
    }
}
