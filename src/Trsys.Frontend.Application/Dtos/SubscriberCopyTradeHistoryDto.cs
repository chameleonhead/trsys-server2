using System;

namespace Trsys.Frontend.Application.Dtos
{
    public class SubscriberCopyTradeHistoryDto
    {
        public string CopyTradeId { get; set; }
        public string Symbol { get; set; }
        public string OrderType { get; set; }
        public bool IsOpen { get; set; }
        public decimal ProfitLoss { get; set; }
        public DateTimeOffset OpenTimestamp { get; set; }
        public DateTimeOffset? CloseTimestamp { get; set; }
    }
}
