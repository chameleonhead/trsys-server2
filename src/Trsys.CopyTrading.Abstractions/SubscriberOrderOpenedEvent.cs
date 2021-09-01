using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class SubscriberOrderOpenedEvent : IEvent
    {
        public SubscriberOrderOpenedEvent()
        {
        }

        public SubscriberOrderOpenedEvent(SubscriberOrder subscriberOrder)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            SubscriberOrderId = subscriberOrder.Id;
            SubscriberKey = subscriberOrder.SubscriberKey;
            PublisherOrderId = subscriberOrder.PublisherOrderId;
            Text = subscriberOrder.Text;
            TicketNo = subscriberOrder.TicketNo;
            Symbol = subscriberOrder.Symbol;
            OrderType = subscriberOrder.OrderType;
            Price = subscriberOrder.Price;
            Lots = subscriberOrder.Lots;
            Time = subscriberOrder.Time;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "SubscriberOrderOpened";
        public string SubscriberOrderId { get; set; }
        public string SubscriberKey { get; set; }
        public string PublisherOrderId { get; set; }
        public string Text { get; set; }
        public int TicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Lots { get; set; }
        public long Time { get; set; }
    }
}