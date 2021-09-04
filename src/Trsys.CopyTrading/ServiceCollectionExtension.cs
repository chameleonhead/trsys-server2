using Microsoft.Extensions.DependencyInjection;
using Trsys.CopyTrading.Application;
using Trsys.CopyTrading.Infrastructure;

namespace Trsys.CopyTrading
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryEaService(this IServiceCollection services)
        {
            services.AddSingleton<IEaService, EaService>();
            services.AddInMemoryEaServiceInfrastructure();
            return services;
        }
    }
}
