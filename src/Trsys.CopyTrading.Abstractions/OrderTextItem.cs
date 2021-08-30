using System.Text.RegularExpressions;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderTextItem
    {
        private OrderTextItem(string text, int ticketNo, string symbol, OrderType orderType, decimal price, decimal lots, long time)
        {
            Text = text;
            TicketNo = ticketNo;
            Symbol = symbol;
            OrderType = orderType;
            Price = price.Normalize();
            Lots = lots.Normalize();
            Time = time;
        }

        public string Text { get; }
        public int TicketNo { get; }
        public string Symbol { get; }
        public OrderType OrderType { get; }
        public decimal Price { get; }
        public decimal Lots { get; }
        public long Time { get; set; }

        public static OrderTextItem Parse(string text)
        {
            if (!Regex.IsMatch(text, @"^\d+:[A-Z]+:[01]:\d+(\.\d+)?:\d+(\.\d+)?:\d+"))
            {
                throw new OrderTextFormatException();
            }
            var splitted = text.Split(":");
            var ticketNo = splitted[0];
            var symbol = splitted[1];
            var orderType = (OrderType)int.Parse(splitted[2]);
            var price = splitted[3];
            var lots = splitted[4];
            var time = splitted[5];

            return new OrderTextItem(text, int.Parse(ticketNo), symbol, orderType, decimal.Parse(price), decimal.Parse(lots), long.Parse(time));
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
