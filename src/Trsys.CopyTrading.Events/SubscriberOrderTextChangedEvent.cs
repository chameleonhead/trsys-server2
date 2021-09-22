using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class SubscriberOrderTextChangedEvent : IEvent
    {
        public SubscriberOrderTextChangedEvent()
        {
        }

        public SubscriberOrderTextChangedEvent(string subscriberKey, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            SubscriberKey = subscriberKey;
            Text = text;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "SubscriberOrderTextChanged";
        public string SubscriberKey { get; set; }
        public string Text { get; set; }
    }
}