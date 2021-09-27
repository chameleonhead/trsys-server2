using NodaTime;
using System.Collections.Generic;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Application.Admin.ClientDetails
{
    public class SubscriberCopyTradeHistorySearchResult
    {
        public YearMonth CurrentYearMonth { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalProfitLoss { get; set; }
        public List<SubscriberCopyTradeHistoryDto> Items { get; set; }
    }
}
