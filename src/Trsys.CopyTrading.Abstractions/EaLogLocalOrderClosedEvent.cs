using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogLocalOrderClosedEvent : IEvent
    {
        public EaLogLocalOrderClosedEvent()
        {
        }

        public EaLogLocalOrderClosedEvent(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, long localTicketNo, string symbol, OrderType orderType)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = timestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Token = token;
            ServerTicketNo = serverTicketNo;
            LocalTicketNo = localTicketNo;
            Symbol = symbol;
            OrderType = orderType;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogLocalOrderClosed";
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
        public long ServerTicketNo { get; set; }
        public long LocalTicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
    }
}
