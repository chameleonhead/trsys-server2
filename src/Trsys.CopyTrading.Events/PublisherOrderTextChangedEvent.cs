using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class PublisherOrderTextChangedEvent : IEvent
    {
        public PublisherOrderTextChangedEvent()
        {
        }

        public PublisherOrderTextChangedEvent(string publisherKey, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            PublisherKey = publisherKey;
            Text = text;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "PublisherOrderTextChanged";
        public string PublisherKey { get; set; }
        public string Text { get; set; }
    }
}
