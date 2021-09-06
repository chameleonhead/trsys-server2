using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.Events.Abstractions;

namespace Trsys.Events.InMemory
{
    public class InMemoryEventDispatcher : IEventPublisher, IEventSubscriber
    {
        private readonly ILogger<InMemoryEventDispatcher> logger;
        private readonly List<IEventHandler> handlers = new();

        public InMemoryEventDispatcher(ILogger<InMemoryEventDispatcher> logger)
        {
            this.logger = logger;
        }

        public async void Publish<T>(T e) where T : IEvent
        {
            await Task.WhenAll(handlers.Select(handler => RaiseEvent(handler, e)).ToArray());
        }

        private async Task RaiseEvent(IEventHandler handler, IEvent e)
        {
            try
            {
                await handler.Handle(e);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Raise event error");
            }
        }

        public void Subscribe(IEventHandler handler)
        {
            handlers.Add(handler);
        }

        public void Subscribe(IEnumerable<IEventHandler> handlers)
        {
            this.handlers.AddRange(handlers);
        }
    }
}
