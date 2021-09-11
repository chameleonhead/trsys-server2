using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class EaSessionGeneratedEvent : IEvent
    {
        public EaSessionGeneratedEvent()
        {
        }

        public EaSessionGeneratedEvent(string key, string keyType, string token)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            Key = key;
            KeyType = keyType;
            Token = token;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaSessionGenerated";
        public string EaSessionId { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Token { get; set; }
    }
}
