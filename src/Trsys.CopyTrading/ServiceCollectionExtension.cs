using System;
using System.Net.Http;
using Grpc.Core;
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
                services.AddSingleton<CopyTradingEventHandler>();
                services.AddInMemoryEaServiceInfrastructure();
            }
            else
            {
                var httpHandler = new HttpClientHandler();
                // Return `true` to allow certificates that are untrusted/invalid
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                var channel = GrpcChannel.ForAddress(options.ServiceEndpoint, new GrpcChannelOptions { HttpHandler = httpHandler });
                services.AddSingleton<IEaService>(new GrpcEaService(channel));
            }
            return services;
        }
    }
}
