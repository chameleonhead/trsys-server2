using Akkatecture.Aggregates;

namespace Trsys.CopyTrading.Domain
{
    public class CopyTradeState : AggregateState<CopyTradeAggregate, CopyTradeId>,
        IApply<CopyTradeOpenedEvent>,
        IApply<CopyTradeClosedEvent>
    {
        public string Symbol { get; set; }
        public string OrderType { get; set; }
        public bool IsOpen { get; set; }

        public void Apply(CopyTradeOpenedEvent e)
        {
            Symbol = e.Symbol;
            OrderType = e.OrderType;
            IsOpen = true;
        }

        public void Apply(CopyTradeClosedEvent e)
        {
            IsOpen = false;
        }
    }
}