﻿namespace Trsys.CopyTrading.Abstractions
{
    public class SubscriberOrder
    {
        public string SubscriberKey { get; set; }
        public string PublisherKey { get; set; }
        public string Text { get; set; }
        public int TicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Lots { get; set; }
        public long Time { get; set; }
    }
}
