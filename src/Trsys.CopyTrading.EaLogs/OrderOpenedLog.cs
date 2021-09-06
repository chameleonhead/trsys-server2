using System;

namespace Trsys.CopyTrading.EaLogs
{
    public class OrderOpenedLog : LogBase
    {
        public OrderOpenedLog()
        {
        }

        public OrderOpenedLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, string serverSymbol, OrderType serverOrderType, long localTicketNo, long orderTicket, string symbol, OrderType orderType, decimal price, decimal volume, long time) : base(timestamp, key, keyType, version, token)
        {
            ServerTicketNo = serverTicketNo;
            ServerSymbol = serverSymbol;
            ServerOrderType = serverOrderType;
            LocalTicketNo = localTicketNo;
            OrderTicket = orderTicket;
            Symbol = symbol;
            OrderType = orderType;
            Price = price;
            Volume = volume;
            Time = time;
        }

        public long ServerTicketNo { get; set; }
        public string ServerSymbol { get; set; }
        public OrderType ServerOrderType { get; set; }
        public long LocalTicketNo { get; set; }
        public long OrderTicket { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public long Time { get; set; }
    }
}
