using System;
using System.Net.Http;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Trsys.CopyTrading.Service;

namespace Trsys.CopyTrading
{
    public class EaServicePool
    {
        private readonly GrpcChannel[] channels;
        private readonly Random random;

        public EaServicePool(string serviceEndpoint, int poolSize)
        {
            channels = new GrpcChannel[poolSize];
            for (var i = 0; i < channels.Length; i++)
            {
                var httpHandler = new SocketsHttpHandler();
                httpHandler.SslOptions = new SslClientAuthenticationOptions()
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                };
                httpHandler.PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan;
                httpHandler.KeepAlivePingDelay = TimeSpan.FromSeconds(60);
                httpHandler.KeepAlivePingTimeout = TimeSpan.FromSeconds(30);
                httpHandler.EnableMultipleHttp2Connections = true;
                channels[i] = GrpcChannel.ForAddress(serviceEndpoint, new GrpcChannelOptions { HttpHandler = httpHandler });
            }
            random = new Random();
        }

        public Task UseClientAsync(Func<Ea.EaClient, Task> callback)
        {
            var channel = channels[random.Next(0, channels.Length - 1)];
            return callback.Invoke(new Ea.EaClient(channel));
        }

        public Task<T> UseClientAsync<T>(Func<Ea.EaClient, Task<T>> callback)
        {
            var channel = channels[random.Next(0, channels.Length - 1)];
            return callback.Invoke(new Ea.EaClient(channel));
        }
    }
}
