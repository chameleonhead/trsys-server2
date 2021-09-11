using System;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Application;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class BlockingEventQueue : IEventQueue, IDisposable
    {
        private readonly BlockingTaskQueue queue = new();
        private readonly CopyTradingEventHandler handler;

        public BlockingEventQueue(CopyTradingEventHandler handler)
        {
            this.handler = handler;
        }

        public void Enqueue(IEvent e)
        {
            queue.Enqueue(token => handler.Handle(e, token));
        }

        public void Dispose()
        {
            queue.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
