using System;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IOrderNotificationBus
    {
        event EventHandler<OrderEventArgs> OrderOpenPublished;
        event EventHandler<OrderEventArgs> OrderClosePublished;

        void PublishOpen(string publisherKey, OrderTextItem orderTextItem);

        void PublishClose(string publisherKey, OrderTextItem orderTextItem);

        void UpdateSubscriberOrder(string subscriberKey, OrderText text);

        void AddSubscriberOrderUpdateHandler(Action<string, OrderText> handler);

        void RemoveSubscriberOrderUpdateHandler(Action<string, OrderText> handler);
    }
}
