using System;

namespace Trsys.Analytics.EaLogs
{
    public class OrderSendExecutingLog : LogBase
    {
        public OrderSendExecutingLog()
        {
        }

        public OrderSendExecutingLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, string symbol, OrderType orderType, decimal orderingVolume) : base(timestamp, key, keyType, version, token)
        {
            ServerTicketNo = serverTicketNo;
            Symbol = symbol;
            OrderType = orderType;
            OrderingVolume = orderingVolume;
        }

        public long ServerTicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
        public decimal OrderingVolume { get; set; }
    }
}