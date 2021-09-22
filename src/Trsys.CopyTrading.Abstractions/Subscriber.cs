using System.Collections.Generic;
using System.Linq;
using Trsys.CopyTrading.Events;

namespace Trsys.CopyTrading.Abstractions
{
    public class Subscriber : EaBase
    {
        private IOrderNotificationBus orderBus;
        private HashSet<PublisherOrderKey> currentOrders = new();
        private OrderText currentOrderText = OrderText.Empty;
        private HashSet<PublisherOrderKey> publishedOrders = new();
        private OrderText publishedOrderText = null;

        public Subscriber(string key, IEventQueue events, IOrderNotificationBus orderBus) : base(key, "Subscriber", events)
        {
            this.orderBus = orderBus;
        }

        private void OnOrderOpenPublished(object sender, OrderEventArgs e)
        {
            if (currentOrderText != OrderText.Empty)
            {
                return;
            }
            currentOrders.Add(new PublisherOrderKey(e.Order.PublisherKey, e.Order.TicketNo));
            currentOrderText = OrderText.Parse(e.Order.Text);
            orderBus.UpdateSubscriberOrder(Key, currentOrderText);
        }

        private void OnOrderClosePublished(object sender, OrderEventArgs e)
        {
            if (currentOrderText == OrderText.Empty)
            {
                return;
            }
            var key = new PublisherOrderKey(e.Order.PublisherKey, e.Order.TicketNo);
            if (currentOrders.Contains(key))
            {
                currentOrders.Remove(key);
                currentOrderText = OrderText.Empty;
                orderBus.UpdateSubscriberOrder(Key, currentOrderText);
            }
        }

        public OrderText GetOrderText()
        {
            if (publishedOrderText != currentOrderText)
            {
                var diff = OrderDifference.CalculateDifference(publishedOrderText ?? OrderText.Empty, currentOrderText);
                if (diff.HasDifference)
                {
                    foreach (var item in diff.Opened)
                    {
                        var publishingOrder = currentOrders.FirstOrDefault(e => e.TicketNo == item.TicketNo);
                        if (publishingOrder != null)
                        {
                            publishedOrders.Add(publishingOrder);
                            Events.Enqueue(new SubscriberOrderOpenDeliveredEvent(
                                Key,
                                publishingOrder.PublisherKey,
                                item.Text,
                                item.TicketNo,
                                item.Symbol,
                                item.OrderType.ToString(),
                                item.Price,
                                item.Lots,
                                item.Time));
                        }
                    }
                    foreach (var item in diff.Closed)
                    {
                        var publishedOrder = publishedOrders.FirstOrDefault(e => e.TicketNo == item.TicketNo);
                        if (publishedOrder != null)
                        {
                            publishedOrders.Remove(publishedOrder);
                            Events.Enqueue(new SubscriberOrderCloseDeliveredEvent(
                                Key,
                                publishedOrder.PublisherKey,
                                item.Text,
                                item.TicketNo,
                                item.Symbol,
                                item.OrderType.ToString(),
                                item.Price,
                                item.Lots,
                                item.Time));
                        }
                    }
                }
                publishedOrderText = currentOrderText;
            }
            return currentOrderText;
        }

        public override void Dispose()
        {
            orderBus.OrderOpenPublished -= OnOrderOpenPublished;
            orderBus.OrderClosePublished -= OnOrderClosePublished;
            base.Dispose();
        }
        record PublisherOrderKey(string PublisherKey, int TicketNo);
    }
}
