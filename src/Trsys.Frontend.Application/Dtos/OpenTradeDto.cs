using System;

namespace Trsys.Frontend.Application.Dtos
{
    public class OpenTradeDto
    {
        public string SecretKeyId { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public string CopyTradeId { get; set; }
        public string PublisherTicketNo { get; set; }
        public string Symbol { get; set; }
        public string OrderType { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
    }
}
