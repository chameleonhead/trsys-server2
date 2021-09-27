using NodaTime;
using System.Collections.Generic;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Application.Admin.ClientDetails
{
    public class ClientDetailsSubscriberTradeHistorySearchResponse
    {
        public SubscriberCopyTradeHistorySearchResult TradeHistorySearchResult { get; set; } 
        public List<YearMonth> YearMonthSelection { get; set; }
    }
}