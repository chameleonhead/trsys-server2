using System;
using System.Text.Json;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class ConsoleEventPublisher : IEventPublisher
    {
        public void Publish<T>(T e) where T : IEvent
        {
            Console.WriteLine(JsonSerializer.Serialize(e));
        }
    }
}
