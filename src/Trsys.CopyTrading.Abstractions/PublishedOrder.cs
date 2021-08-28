using System.Text.RegularExpressions;

namespace Trsys.CopyTrading.Abstractions
{
    public class PublishedOrder
    {
        public int TicketNo { get; set; }
        public string Symbol { get; set; }
        public OrderType OrderType { get; set; }
        private decimal _Price;
        public decimal Price
        {
            get { return _Price; }
            set { _Price = value.Normalize(); }
        }
        private decimal _Lots;
        public decimal Lots
        {
            get { return _Lots; }
            set { _Lots = value.Normalize(); }
        }
        public long Time { get; set; }

        public static PublishedOrder Parse(string text)
        {
            if (!Regex.IsMatch(text, @"^\d+:[A-Z]+:[01]:\d+(\.\d+)?:\d+(\.\d+)?:\d+"))
            {
                throw new PublishOrderFormatException();
            }
            var splitted = text.Split(":");
            var ticketNo = splitted[0];
            var symbol = splitted[1];
            var orderType = (OrderType)int.Parse(splitted[2]);
            var price = splitted[3];
            var lots = splitted[4];
            var time = splitted[5];

            return new PublishedOrder()
            {
                TicketNo = int.Parse(ticketNo),
                Symbol = symbol,
                OrderType = orderType,
                Price = decimal.Parse(price),
                Lots = decimal.Parse(lots),
                Time = long.Parse(time),
            };
        }

        public override string ToString()
        {
            return $"{TicketNo}:{Symbol}:{(int)OrderType}:{Price}:{Lots}:{Time}";
        }
    }

    static class DecimalExtension
    {
        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }
    }
}
