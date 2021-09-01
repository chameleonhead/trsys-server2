using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class PublisherOrderClosePublishedEvent : IEvent
    {
        public PublisherOrderClosePublishedEvent()
        {
        }

        public PublisherOrderClosePublishedEvent(PublisherOrder publisherOrder)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            PublisherOrderId = publisherOrder.Id;
            PublisherKey = publisherOrder.PublisherKey;
            Text = publisherOrder.Text;
            TicketNo = publisherOrder.TicketNo;
            Symbol = publisherOrder.Symbol;
            OrderType = publisherOrder.OrderType;
            Price = publisherOrder.Price;
            Lots = publisherOrder.Lots;
            Time = publisherOrder.Time;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "PublisherOrderClosePublished";
        public string PublisherOrderId { get; set; }
        public string PublisherKey { get; set; }
        public string Text { get; set; }
        public int TicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Lots { get; set; }
        public long Time { get; set; }
    }
}