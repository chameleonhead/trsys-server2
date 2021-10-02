using Akkatecture.Aggregates;
using Akkatecture.Commands;

namespace Trsys.CopyTrading.Domain
{
    public class CopyTradeAggregateManager : AggregateManager<CopyTradeAggregate, CopyTradeId, Command<CopyTradeAggregate, CopyTradeId>>
    {
    }
}
