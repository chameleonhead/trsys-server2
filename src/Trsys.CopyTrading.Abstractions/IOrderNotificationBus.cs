using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderNotificationBus
    {
        public event EventHandler<OrderEventArgs> OrderOpenPublished;
        public event EventHandler<OrderEventArgs> OrderClosePublished;

        public void PublishOpen(string publisherKey, OrderTextItem orderTextItem)
        {
            OrderOpenPublished?.Invoke(this, new OrderEventArgs(publisherKey, orderTextItem));
        }

        public void PublishClose(string publisherKey, OrderTextItem orderTextItem)
        {
            OrderClosePublished?.Invoke(this, new OrderEventArgs(publisherKey, orderTextItem));
        }
    }
}
