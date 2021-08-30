using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderTextPublishedEvent : IEvent
    {
        public OrderTextPublishedEvent()
        {
        }

        public OrderTextPublishedEvent(string publisherKey, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            PublisherKey = publisherKey;
            Text = text;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "OrderTextPublished";
        public string PublisherKey { get; set; }
        public string Text { get; set; }
    }
}
