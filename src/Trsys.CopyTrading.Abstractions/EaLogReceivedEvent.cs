using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogReceivedEvent : IEvent
    {
        public EaLogReceivedEvent()
        {
        }

        public EaLogReceivedEvent(DateTimeOffset serverTimestamp, string key, string keyType, string version, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            ServerTimestamp = serverTimestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Text = text;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogReceived";
        public DateTimeOffset ServerTimestamp { get; set; }
        public string SecretKeyId { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string EaSessionId { get; set; }
        public string Token { get; set; }
        public string Version { get; set; }
        public string Text { get; set; }
    }
}
