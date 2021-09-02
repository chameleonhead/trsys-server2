using System;

namespace Trsys.Analytics.EaLogs
{
    public class OrderSendExecutingLog : ILogInfo
    {
        public OrderSendExecutingLog()
        {
        }

        public OrderSendExecutingLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, string symbol, OrderType orderType, decimal orderingVolume)
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
            OrderingVolume = orderingVolume;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "OrderSendExecutingLog";
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
        public long ServerTicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
        public decimal OrderingVolume { get; set; }
    }
}