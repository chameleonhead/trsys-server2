using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class PublisherOrderIgnoredEvent : IEvent
    {
        public PublisherOrderIgnoredEvent()
        {
        }

        public PublisherOrderIgnoredEvent(string publisherKey, string text, int ticketNo, string symbol, string orderType, decimal price, decimal lots, long time)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
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
        public string Type => "PublisherOrderIgnored";
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