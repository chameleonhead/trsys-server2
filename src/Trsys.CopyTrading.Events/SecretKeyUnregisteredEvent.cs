using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class SecretKeyUnregisteredEvent : IEvent
    {
        public SecretKeyUnregisteredEvent()
        {
        }

        public SecretKeyUnregisteredEvent(string key, string keyType)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            Key = key;
            KeyType = keyType;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "SecretKeyUnregistered";
        public string Key { get; set; }
        public string KeyType { get; set; }
    }
}
