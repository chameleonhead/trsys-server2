using System;
using System.Text.Json;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class ConsoleEventPublisher : IEventPublisher
    {
        public void Publish<T>(T e) where T : IEvent
        {
            Console.WriteLine("{0:yyyy-MM-dd'T'HH:mm:ss.fff} {1}: {2}", e.Timestamp, e.Type, JsonSerializer.Serialize(e));
        }
    }
}
