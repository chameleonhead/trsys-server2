using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogServerOrderClosedEvent : IEvent
    {
        public EaLogServerOrderClosedEvent()
        {
        }

        public EaLogServerOrderClosedEvent(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, string symbol, OrderType orderType)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = timestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Token = token;
            ServerTicketNo = serverTicketNo;
            Symbol = symbol;
            OrderType = orderType;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogServerOrderClosed";
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
        public long ServerTicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
    }
}
