using System.Collections.Generic;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Application.Admin.Dashboard
{
    public class DashboardSearchResponse
    {
        public List<SecretKeyDto> ConnectedKeys { get; set; }
        public OrderTextDto CurrentOrderText { get; set; }
        public List<TradeDto> Trades { get; set; }
        public List<SecretKeyDto> UnapprovedKeys { get; set; } 
    }
}