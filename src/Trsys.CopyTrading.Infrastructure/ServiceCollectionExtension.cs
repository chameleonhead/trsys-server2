using Microsoft.Extensions.DependencyInjection;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.EaLogs;

namespace Trsys.CopyTrading.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryEaServiceInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IEventQueue, BlockingEventQueue>();
            services.AddSingleton<InMemoryCopyTradingContext>();
            services.AddSingleton<OrderNotificationBus>();
            services.AddSingleton<IEaStore, InMemoryEaStore>();
            services.AddSingleton<IEaLogAnalyzer, BlockingQueuedEaLogAnalyzer>();
            return services;
        }
    }
}
