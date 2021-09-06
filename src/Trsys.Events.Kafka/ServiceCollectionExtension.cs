using Microsoft.Extensions.DependencyInjection;
using Trsys.Events.Abstractions;
using Trsys.Events.Infrastructure;

namespace Trsys.Events.Kafka
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventsWithKafka(this IServiceCollection services)
        {
            services.AddSingleton<IEventPublisher, ConsoleEventPublisher>();
            return services;
        }
    }
}
