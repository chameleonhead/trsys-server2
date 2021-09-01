using System;

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

        public EaLogReceivedV2Event(DateTimeOffset serverTimestamp, long eaTimestamp, SecretKey secretKey, string version, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            ServerTimestamp = serverTimestamp;
            EaTimestamp = eaTimestamp;
            SecretKeyId = secretKey.Id;
            Key = secretKey.Key;
            KeyType = secretKey.KeyType;
            Version = version;
            Text = text;
        }

        public EaLogReceivedV2Event(DateTimeOffset serverTimestamp, long eaTimestamp, EaSession session, string version, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            ServerTimestamp = serverTimestamp;
            EaTimestamp = eaTimestamp;
            SecretKeyId = session.SecretKeyId;
            EaSessionId = session.Id;
            Key = session.Key;
            KeyType = session.KeyType;
            Token = session.Token;
            Version = version;
            Text = text;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogReceivedV2";

        public DateTimeOffset ServerTimestamp { get; set; }
        public long EaTimestamp { get; set; }

        public string SecretKeyId { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string EaSessionId { get; set; }
        public string Token { get; set; }
        public string Version { get; set; }
        public string Text { get; set; }
    }
}
