using System;
using System.Collections.Generic;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderNotificationBus
    {
        public event EventHandler<OrderEventArgs> OrderOpenPublished;
        public event EventHandler<OrderEventArgs> OrderClosePublished;
        private List<Action<string, OrderText>> handlers = new();

        public void PublishOpen(string publisherKey, OrderTextItem orderTextItem)
        {
            OrderOpenPublished?.Invoke(this, new OrderEventArgs(publisherKey, orderTextItem));
        }

        public void PublishClose(string publisherKey, OrderTextItem orderTextItem)
        {
            OrderClosePublished?.Invoke(this, new OrderEventArgs(publisherKey, orderTextItem));
        }

        public void UpdateSubscriberOrder(string subscriberKey, OrderText text)
        {
            foreach (var handler in handlers.ToArray())
            {
                handler.Invoke(subscriberKey, text);
            }
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
