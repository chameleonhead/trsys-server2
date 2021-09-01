using System;

namespace Trsys.Backoffice.Abstractions
{
    public interface IEvent
    {
        public string Id { get; }
        public DateTimeOffset Timestamp { get; }
        public string Type { get; }
    }
}