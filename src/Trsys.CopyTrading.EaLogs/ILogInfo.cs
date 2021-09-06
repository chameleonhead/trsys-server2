using System;

namespace Trsys.CopyTrading.EaLogs
{
    public interface ILogInfo
    {
        public string Id { get; }
        public DateTimeOffset Timestamp { get; }
        public string Type { get; }
    }
}
