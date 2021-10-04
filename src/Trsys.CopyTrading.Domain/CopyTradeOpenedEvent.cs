using Akkatecture.Aggregates;

namespace Trsys.CopyTrading.Domain
{
    public class CopyTradeOpenedEvent : AggregateEvent<CopyTradeAggregate, CopyTradeId>
    {
        public CopyTradeOpenedEvent(string symbol, string orderType)
        {
            Symbol = symbol;
            OrderType = orderType;
        }

        public string Symbol { get; set; }
        public string OrderType { get; internal set; }
    }
}