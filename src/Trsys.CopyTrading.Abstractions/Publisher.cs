using System;
using System.Collections.Generic;
using Trsys.CopyTrading.Events;

namespace Trsys.CopyTrading.Abstractions
{
    public class Publisher : EaBase
    {
        private OrderText CurrentOrder = OrderText.Empty;
        private Dictionary<int, PublisherOrder> publisherOrders = new();
        private IOrderNotificationBus orderBus;

        public Publisher(string key, IEventQueue events, IOrderNotificationBus orderBus) : base(key, "Publisher", events)
        {
            this.orderBus = orderBus;
        }

        public void UpdateOrderText(DateTimeOffset timestamp, OrderText orderText)
        {
            var diff = OrderDifference.CalculateDifference(CurrentOrder, orderText);
            if (diff.HasDifference)
            {
                CurrentOrder = orderText;
                Events.Enqueue(new PublisherOrderTextChangedEvent(timestamp, Key, orderText.Text));
                foreach (var item in diff.Opened)
                {
                    var order = new PublisherOrder(
                        Key,
                        item.TicketNo,
                        item.Symbol,
                        item.OrderType,
                        item.Price,
                        item.Lots,
                        item.Time,
                        item.Text);
                    publisherOrders.Add(order.TicketNo, order);
                    orderBus.PublishOpen(order);
                    Events.Enqueue(new PublisherOrderOpenPublishedEvent(
                        Key,
                        item.Text,
                        item.TicketNo,
                        item.Symbol,
                        item.OrderType.ToString(),
                        item.Price,
                        item.Lots,
                        item.Time));
                }
                foreach (var item in diff.Closed)
                {
                    var order = publisherOrders[item.TicketNo];
                    publisherOrders.Remove(item.TicketNo);
                    orderBus.PublishClose(order);
                    Events.Enqueue(new PublisherOrderClosePublishedEvent(
                        Key,
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
    }
}
