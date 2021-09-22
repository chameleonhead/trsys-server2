using System;
using Microsoft.Extensions.DependencyInjection;
using Trsys.CopyTrading.Application;
using Trsys.CopyTrading.Infrastructure;

namespace Trsys.CopyTrading
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEaService(this IServiceCollection services, Action<CopyTradingOptions> configureOptions)
        {
            var options = new CopyTradingOptions();
            configureOptions.Invoke(options);
            if (options.ServiceEndpoint == "InMemory")
            {
                services.AddSingleton<IEaService, EaService>();
                services.AddSingleton<CopyTradingEventHandler>();
                services.AddInMemoryEaServiceInfrastructure();
            }
            else
            {
                services.AddSingleton<EaServicePool>(new EaServicePool(options.ServiceEndpoint, 4));
                services.AddTransient<IEaService, GrpcEaService>();
            }
            return services;
        }
    }
}
