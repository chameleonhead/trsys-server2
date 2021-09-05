using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogReceivedV2Event : IEvent
    {
        public EaLogReceivedV2Event()
        {
        }
        public EaLogReceivedV2Event(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            ServerTimestamp = serverTimestamp;
            EaTimestamp = eaTimestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Text = text;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogReceivedV2";

        public DateTimeOffset ServerTimestamp { get; set; }
        public long EaTimestamp { get; set; }

        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Token { get; set; }
        public string Version { get; set; }
        public string Text { get; set; }
    }
}
