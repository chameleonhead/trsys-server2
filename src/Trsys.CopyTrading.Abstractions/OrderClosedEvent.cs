using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderClosedEvent
    {
        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string PublishedOrderId { get; set; }
    }
}
