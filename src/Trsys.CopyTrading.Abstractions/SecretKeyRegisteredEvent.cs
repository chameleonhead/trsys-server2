using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class SecretKeyRegisteredEvent
    {
        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
    }
}
