using Akkatecture.Commands;

namespace Trsys.CopyTrading.Domain
{
    public class CloseCopyTradeCommand : Command<CopyTradeAggregate, CopyTradeId>
    {
        public CloseCopyTradeCommand(CopyTradeId aggregateId) : base(aggregateId)
        {
        }
    }
}