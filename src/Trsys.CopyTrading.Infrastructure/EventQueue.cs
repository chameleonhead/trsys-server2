using System;
using System.Text.Json;
using Trsys.CopyTrading.Abstractions;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class EventQueue : IEventQueue
    {
        public void Enqueue(IEvent @event)
        {
            Console.WriteLine(JsonSerializer.Serialize(@event));
        }
    }
}
