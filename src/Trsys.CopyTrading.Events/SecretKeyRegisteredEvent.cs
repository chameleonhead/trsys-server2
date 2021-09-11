using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class SecretKeyRegisteredEvent : IEvent
    {
        public SecretKeyRegisteredEvent()
        {
        }

        public SecretKeyRegisteredEvent(string key, string keyType)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            Key = key;
            KeyType = keyType;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "SecretKeyRegistered";
        public string Key { get; set; }
        public string KeyType { get; set; }
    }
}
