using NodaTime;
using System.Collections.Generic;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Application.Admin.ClientDetails
{
    public class ClientDetailsResponse
    {
        public SecretKeyDto SecretKey { get; set; }
        public SubscriberCopyTradeHistorySearchResult TradeHistorySearchResult { get; set; } 
        public List<YearMonth> YearMonthSelection { get; set; }
    }
}