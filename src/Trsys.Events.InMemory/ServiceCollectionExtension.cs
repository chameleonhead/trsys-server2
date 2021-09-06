using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Trsys.Events.Abstractions;

namespace Trsys.Events.InMemory
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventsWithInMemory(this IServiceCollection services, IEnumerable<IEventHandler> handlers)
        {
            services.AddSingleton(sp =>
            {
                var dispatcher = new InMemoryEventDispatcher(sp.GetRequiredService<ILogger<InMemoryEventDispatcher>>());
                dispatcher.Subscribe(handlers);
                return dispatcher;
            });
            services.AddSingleton<IEventPublisher, InMemoryEventDispatcher>();
            services.AddSingleton<IEventPublisher, InMemoryEventDispatcher>();
            return services;
        }
    }
}
