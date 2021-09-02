using System;

namespace Trsys.Analytics.EaLogs
{
    public class ServerOrderOpenedLog : LogBase
    {
        public ServerOrderOpenedLog()
        {
        }

        public ServerOrderOpenedLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, string symbol, OrderType orderType) : base(timestamp, key, keyType, version, token)
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
