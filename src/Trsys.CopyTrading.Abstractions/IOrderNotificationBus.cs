using System;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IOrderNotificationBus
    {
        event EventHandler<OrderEventArgs> OrderOpenPublished;
        event EventHandler<OrderEventArgs> OrderClosePublished;

        void PublishOpen(PublisherOrder order);

        void PublishClose(PublisherOrder order);

        void AddOrderTextUpdatedHandler(Action<OrderText> handler);

        void RemoveOrderTextUpdatedHandler(Action<OrderText> handler);

        void UpdateOrderText(OrderText currentOrderText);
    }
}
