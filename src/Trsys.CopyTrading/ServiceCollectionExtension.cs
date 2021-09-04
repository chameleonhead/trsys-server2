using System;
using Grpc.Net.Client;
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
                services.AddInMemoryEaServiceInfrastructure();
            }
            else
            {
                services.AddSingleton<IEaService>(new GrpcEaService(GrpcChannel.ForAddress(options.ServiceEndpoint)));
            }
            return services;
        }
    }
}
