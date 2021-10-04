using Akkatecture.Commands;

namespace Trsys.CopyTrading.Domain
{
    public class OpenCopyTradeCommand : Command<CopyTradeAggregate, CopyTradeId>
    {
        public OpenCopyTradeCommand(CopyTradeId aggregateId, string symbol, string orderType) : base(aggregateId)
        {
            Symbol = symbol;
            OrderType = orderType;
        }

        public string Symbol { get; }
        public string OrderType { get; }
    }
}