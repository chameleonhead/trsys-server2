using System;

namespace Trsys.Backoffice.Abstractions
{
    public interface IEvent
    {
        string Id { get; }
        DateTimeOffset Timestamp { get; }
        string Type { get; }
    }
}