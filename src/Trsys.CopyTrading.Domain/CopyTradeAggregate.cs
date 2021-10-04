using Akkatecture.Aggregates;
using System;

namespace Trsys.CopyTrading.Domain
{
    public class CopyTradeAggregate : AggregateRoot<CopyTradeAggregate, CopyTradeId, CopyTradeState>,
        IExecute<OpenCopyTradeCommand>,
        IExecute<CloseCopyTradeCommand>
    {
        public CopyTradeAggregate(CopyTradeId id) : base(id)
        {
        }

        public bool Execute(OpenCopyTradeCommand command)
        {
            if (!IsNew)
            {
                throw new InvalidOperationException();
            }
            Emit(new CopyTradeOpenedEvent(command.Symbol, command.OrderType));
            return true;
        }

        public bool Execute(CloseCopyTradeCommand command)
        {
            if (State.IsOpen)
            {
                Emit(new CopyTradeClosedEvent());
            }
            return true;
        }
    }
}
