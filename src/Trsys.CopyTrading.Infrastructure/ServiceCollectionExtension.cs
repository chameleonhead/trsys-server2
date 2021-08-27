using Microsoft.Extensions.DependencyInjection;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEaServiceInfrastructure(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEaSessionStore, InMemoryEaSessionStore>()
                .AddSingleton<IValidSecretKeyStore, InMemoryValidSecretKeyStore>();
        }
    }
}
