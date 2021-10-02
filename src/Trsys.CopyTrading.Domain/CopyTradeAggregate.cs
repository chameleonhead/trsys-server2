using Akkatecture.Aggregates;

namespace Trsys.CopyTrading.Domain
{
    public class CopyTradeAggregate : AggregateRoot<CopyTradeAggregate, CopyTradeId, CopyTradeState>
    {
        public CopyTradeAggregate(CopyTradeId id) : base(id)
        {
        }
    }
}
