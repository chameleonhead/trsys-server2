using System;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IOrderNotificationBus
    {
        event EventHandler<OrderEventArgs> OrderOpenPublished;
        event EventHandler<OrderEventArgs> OrderClosePublished;

        void PublishOpen(PublisherOrder order);

        void PublishClose(PublisherOrder order);

        void UpdateSubscriberOrder(string subscriberKey, OrderText text);

        void AddSubscriberOrderUpdateHandler(Action<string, OrderText> handler);

        void RemoveSubscriberOrderUpdateHandler(Action<string, OrderText> handler);
    }
}
