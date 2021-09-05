using Microsoft.Extensions.DependencyInjection;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryEaServiceInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryCopyTradingContext>();
            services.AddSingleton<IEaSessionStore, InMemoryEaSessionStore>();
            services.AddSingleton<ISecretKeyStore, InMemorySecretKeyStore>();
            services.AddSingleton<IPublisherOrderStore, InMemoryPublisherOrderStore>();
            services.AddSingleton<IActiveOrderStore, InMemoryActiveOrderStore>();
            services.AddSingleton<ISubscriberOrderStore, InMemorySubscriberOrderStore>();
            return services;
        }
    }
}
