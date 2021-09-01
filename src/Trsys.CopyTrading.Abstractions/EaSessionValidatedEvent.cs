using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaSessionValidatedEvent : IEvent
    {
        public EaSessionValidatedEvent()
        {
        }

        public EaSessionValidatedEvent(EaSession session)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            Key = session.Key;
            KeyType = session.KeyType;
            Token = session.Token;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaSessionValidated";
        public string EaSessionId { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Token { get; set; }
    }
}
