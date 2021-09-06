using System;

namespace Trsys.CopyTrading.EaLogs
{
    public class ServerOrderClosedLog : LogBase
    {
        public ServerOrderClosedLog()
        {
        }

        public ServerOrderClosedLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, string symbol, OrderType orderType) : base(timestamp, key, keyType, version, token)
        {
            ServerTicketNo = serverTicketNo;
            Symbol = symbol;
            OrderType = orderType;
        }

        public long ServerTicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
    }
}
