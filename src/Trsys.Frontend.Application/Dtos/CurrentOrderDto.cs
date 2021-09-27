using System;

namespace Trsys.Frontend.Application.Dtos
{
    public class CurrentOrderDto
    {
        public string CopyTradeId { get; set; }
        public int TicketNo { get; set; }
        public string Symbol { get; set; }
        public string OrderType { get; set; }
        public DateTimeOffset OpenTimestamp { get; set; }
    }
}
