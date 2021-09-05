using System;

namespace Trsys.Events.Abstractions
{
    public interface IEvent
    {
        public string Id { get; }
        public DateTimeOffset Timestamp { get; }
        public string Type { get; }
    }
}