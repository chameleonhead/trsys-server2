using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class SubscriberOrderOpenDeliveredEvent : IEvent
    {
        public SubscriberOrderOpenDeliveredEvent()
        {
        }

        public SubscriberOrderOpenDeliveredEvent(string subscriberKey, string publisherKey, string text, int ticketNo, string symbol, string orderType, decimal price, decimal lots, long time)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            SubscriberKey = subscriberKey;
            PublisherKey = publisherKey;
            Text = text;
            TicketNo = ticketNo;
            Symbol = symbol;
            OrderType = orderType;
            Price = price;
            Lots = lots;
            Time = time;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "SubscriberOrderOpenDelivered";
        public string SubscriberKey { get; set; }
        public string PublisherKey { get; set; }
        public string Text { get; set; }
        public int TicketNo { get; set; }
        public string Symbol { get; set; }
        public string OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Lots { get; set; }
        public long Time { get; set; }
    }
}