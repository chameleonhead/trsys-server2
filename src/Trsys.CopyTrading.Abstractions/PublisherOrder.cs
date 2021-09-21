namespace Trsys.CopyTrading.Abstractions
{
    public class PublisherOrder
    {
        public PublisherOrder(string publisherKey, int ticketNo, string symbol, OrderType orderType, decimal price, decimal lots, long time, string text)
        {
            PublisherKey = publisherKey;
            TicketNo = ticketNo;
            Symbol = symbol;
            OrderType = orderType;
            Price = price;
            Lots = lots;
            Time = time;
            Text = text;
        }

        public string PublisherKey { get; }
        public int TicketNo { get; }
        public string Symbol { get; }
        public OrderType OrderType { get; }
        public decimal Price { get; }
        public decimal Lots { get; }
        public long Time { get; set; }
        public string Text { get; }
    }
}
