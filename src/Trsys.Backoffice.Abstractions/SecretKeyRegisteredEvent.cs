using System;

namespace Trsys.Backoffice.Abstractions
{
    public class SecretKeyRegisteredEvent
    {
        public SecretKeyRegisteredEvent()
        {
        }

        public SecretKeyRegisteredEvent(string secretKeyId, string key, string keyType)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            SecretKeyId = secretKeyId;
            Key = key;
            KeyType = keyType;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "SecretKeyCreated";

        public string SecretKeyId { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
    }
}
