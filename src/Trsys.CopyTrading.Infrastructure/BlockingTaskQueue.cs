using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.CopyTrading.Infrastructure
{
    public class BlockingTaskQueue : IDisposable
    {
        private readonly BlockingCollection<Func<CancellationToken, Task>> events = new();
        private int lockValue = 0;
        private Task task = Task.CompletedTask;
        private CancellationTokenSource source = new();

        public void Enqueue(Func<CancellationToken, Task> func)
        {
            events.Add(func);
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
                    await item.Invoke(cancellationToken).ConfigureAwait(false);
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
