using System;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.EaLogs;

namespace Trsys.CopyTrading.Infrastructure
{
    public class BlockingQueuedEaLogAnalyzer : IEaLogAnalyzer
    {
        private readonly BlockingTaskQueue queue = new();
        private readonly IEventQueue events;

        public BlockingQueuedEaLogAnalyzer(IEventQueue events)
        {
            this.events = events;
        }

        public void AnalyzeLog(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text)
        {
            queue.Enqueue(cancellationToken =>
            {
                var logs = EaLogParser.Parse(serverTimestamp, key, keyType, token, version, text);
                foreach (var log in logs)
                {
                    switch (log)
                    {
                    }
                }
                return Task.CompletedTask;
            });
        }

        public void AnalyzeLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, string text)
        {
            queue.Enqueue(cancellationToken =>
            {
                var logs = EaLogParser.Parse(timestamp, key, keyType, token, version, text);
                foreach (var log in logs)
                {
                    switch (log)
                    {
                    }
                }
                return Task.CompletedTask;
            });
        }
    }
}