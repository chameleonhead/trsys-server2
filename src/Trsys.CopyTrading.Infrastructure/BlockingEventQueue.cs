using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Application;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class BlockingEventQueue : IEventQueue, IDisposable
    {
        private readonly BlockingCollection<IEvent> events = new();
        private int lockValue = 0;
        private Task task = Task.CompletedTask;
        private readonly CopyTradingEventHandler handler;
        private CancellationTokenSource source = new();

        public BlockingEventQueue(CopyTradingEventHandler handler)
        {
            this.handler = handler;
        }

        public void Enqueue(IEvent @event)
        {
            events.Add(@event);
            if (Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
            {
                task = Task.Run(() => StartProcess(source.Token));
            }
        }

        public async Task StartProcess(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested && events.TryTake(out var item))
                {
                    await handler.Handle(item, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                Interlocked.Exchange(ref lockValue, 0);
            }
            if (events.Any())
            {
                if (Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                {
                    task = Task.Run(() => StartProcess(source.Token));
                }
            }
        }
        public void Dispose()
        {
            source.Cancel();
            task.Wait();
            GC.SuppressFinalize(this);
        }
    }
}
