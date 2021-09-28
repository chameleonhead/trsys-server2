using System.Collections.Generic;
using System.Linq;

namespace Trsys.Frontend.Application.Dtos
{
    public class SubscriberCopyTradeSummaryDto
    {
        public string SecretKeyId { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public string CopyTradeId { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClosed => Volume > 0 && Volume == CloseTrades.Sum(t => t.Volume);
        public int TicketNo { get; set; }
        public string Symbol { get; set; }
        public string OrderType { get; set; }
        public decimal Volume => OpenTrades.Sum(t => t.Volume);
        public decimal OpenPriceAvg => OpenTrades.Average(t => t.Price);
        public decimal ClosePriceAvg => CloseTrades.Average(t => t.Price);
        public decimal TotalProfitLoss => CloseTrades.Sum(t => t.ProfitLoss);
        public List<OpenTradeDto> OpenTrades { get; set; } = new();
        public List<CloseTradeDto> CloseTrades { get; set; } = new();
    }
}
