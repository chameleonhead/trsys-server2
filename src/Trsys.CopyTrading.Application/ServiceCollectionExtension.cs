using Microsoft.Extensions.DependencyInjection;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Infrastructure;

namespace Trsys.CopyTrading.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEaServiceInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IEaService, EaService>();
            services.AddSingleton<IEaSessionStore, InMemoryEaSessionStore>();
            services.AddSingleton<IValidSecretKeyStore, InMemoryValidSecretKeyStore>();
            return services;
        }
    }
}
