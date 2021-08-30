using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderOpenedEvent
    {
        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string PublishedOrderId { get; set; }
        public int MagicNumber { get; set; }
        public string PublisherKey { get; set; }
        public int TicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Lots { get; set; }
        public long Time { get; set; }
    }
}
