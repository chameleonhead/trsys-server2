using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaSessionGeneratedEvent
    {
        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string EaSessionId { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Token { get; set; }
    }
}
