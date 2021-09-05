using Microsoft.Extensions.DependencyInjection;
using Trsys.Events.Abstractions;
using Trsys.Events.Infrastructure;

namespace Trsys.Events
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEvents(this IServiceCollection services)
        {
            services.AddSingleton<IEventPublisher, ConsoleEventPublisher>();
            return services;
        }
    }
}
