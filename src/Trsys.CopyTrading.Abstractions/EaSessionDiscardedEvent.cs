using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaSessionDiscardedEvent : IEvent
    {
        public EaSessionDiscardedEvent()
        {
        }

        public EaSessionDiscardedEvent(EaSession session)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            Key = session.Key;
            KeyType = session.KeyType;
            Token = session.Token;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaSessionDiscarded";
        public string EaSessionId { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Token { get; set; }
    }
}
