using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogReceivedEvent : IEvent
    {
        public EaLogReceivedEvent()
        {
        }
        public EaLogReceivedEvent(string key, string keyType, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            Key = key;
            KeyType = keyType;
            Text = text;
        }

        public EaLogReceivedEvent(SecretKey secretKey, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            SecretKeyId = secretKey.Id;
            Key = secretKey.Key;
            KeyType = secretKey.KeyType;
            Text = text;
        }

        public EaLogReceivedEvent(EaSession session, string text)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            SecretKeyId = session.SecretKeyId;
            EaSessionId = session.Id;
            Key = session.Key;
            KeyType = session.KeyType;
            Token = session.Token;
            Text = text;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogReceived";
        public string SecretKeyId { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string EaSessionId { get; set; }
        public string Token { get; set; }
        public string Text { get; set; }
    }
}
