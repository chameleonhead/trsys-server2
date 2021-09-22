using System;
using System.Net.Http;
using System.Net.Security;
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
                var httpHandler = new SocketsHttpHandler();
                httpHandler.SslOptions = new SslClientAuthenticationOptions()
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                };
                httpHandler.EnableMultipleHttp2Connections = true;

                var channel = GrpcChannel.ForAddress(options.ServiceEndpoint, new GrpcChannelOptions { HttpHandler = httpHandler });
                services.AddTransient<IEaService>(service => new GrpcEaService(channel));
            }
            return services;
        }
    }
}
