using System;

namespace Trsys.CopyTrading.EaLogs
{
    public class LocalOrderClosedLog : LogBase
    {
        public LocalOrderClosedLog()
        {
        }

        public LocalOrderClosedLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, long localTicketNo, string symbol, OrderType orderType) : base(timestamp, key, keyType, version, token)
        {
            ServerTicketNo = serverTicketNo;
            LocalTicketNo = localTicketNo;
            Symbol = symbol;
            OrderType = orderType;
        }

        public long ServerTicketNo { get; set; }
        public long LocalTicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
    }
}
