using System;
using System.Collections.Generic;
using System.Linq;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class ActiveOrderPublishedEvent : IEvent
    {
        public class PublisherOrderDto
        {
            public string PublisherKey { get; set; }
            public string Text { get; set; }
            public int TicketNo { get; set; }
            public string Symbol { get; set; }
            public string OrderType { get; set; }
            public decimal Price { get; set; }
            public decimal Lots { get; set; }
            public long Time { get; set; }
        }

        public ActiveOrderPublishedEvent()
        {
        }

        public ActiveOrderPublishedEvent(string subscriberKey, string text, IEnumerable<PublisherOrderDto> orders)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            SubscriberKey = subscriberKey;
            Text = text;
            Orders = orders.ToList();
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "ActiveOrderPublished";
        public string SubscriberKey { get; set; }
        public string Text { get; set; }
        public IEnumerable<PublisherOrderDto> Orders { get; set; }
    }
}
