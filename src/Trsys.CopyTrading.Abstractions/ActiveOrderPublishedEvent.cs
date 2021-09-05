using System;
using System.Collections.Generic;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Abstractions
{
    public class ActiveOrderPublishedEvent : IEvent
    {
        public ActiveOrderPublishedEvent()
        {
        }

        public ActiveOrderPublishedEvent(string subscriberKey, ActiveOrder activeOrder)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            SubscriberKey = subscriberKey;
            Text = activeOrder.OrderText.Text;
            Orders = activeOrder.Orders;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "ActiveOrderPublished";
        public string SubscriberKey { get; set; }
        public string Text { get; set; }
        public List<PublisherOrder> Orders { get; set; }
    }
}
