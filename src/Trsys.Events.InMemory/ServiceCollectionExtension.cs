using Microsoft.Extensions.DependencyInjection;
using Trsys.Events.Abstractions;
using Trsys.Events.InMemory;

namespace Trsys.Events
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryEventInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IEventPublisher, InMemoryEventDispatcher>();
            services.AddSingleton<IEventSubscriber, InMemoryEventDispatcher>();
            return services;
        }
    }
}
