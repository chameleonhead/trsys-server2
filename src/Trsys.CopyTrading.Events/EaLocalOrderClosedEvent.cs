using System;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Events
{
    public class EaLocalOrderClosedEvent : IEvent
    {
        public EaLocalOrderClosedEvent()
        {
        }

        public EaLocalOrderClosedEvent(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, long localTicketNo, string symbol, string orderType)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            EaTimestamp = timestamp;
            Key = key;
            KeyType = keyType;
            ServerTicketNo = serverTicketNo;
            LocalTicketNo = localTicketNo;
            Symbol = symbol;
            OrderType = orderType;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLocalOrderOpened";
        public DateTimeOffset EaTimestamp { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Token { get; set; }
        public long ServerTicketNo { get; set; }
        public long LocalTicketNo { get; set; }
        public string Symbol { get; set; }
        public string OrderType { get; set; }
    }
}
