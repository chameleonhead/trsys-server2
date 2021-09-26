using Grpc.Net.Client;
using System;
using System.Net.Http;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Trsys.CopyTrading.Service;

namespace Trsys.CopyTrading
{
    public class EaServicePool : IDisposable
    {
        private readonly GrpcChannel channelForBackgroundWorker;
        private readonly GrpcChannel[] channels;
        private int index = 0;

        public EaServicePool(string serviceEndpoint, int poolSize)
        {
            channels = new GrpcChannel[poolSize];
            for (var i = 0; i <= channels.Length; i++)
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
                var channel = GrpcChannel.ForAddress(serviceEndpoint, new GrpcChannelOptions { HttpHandler = httpHandler });
                if (i == channels.Length)
                {
                    channelForBackgroundWorker = channel;
                }
                else
                {
                    channels[i] = channel;
                }
            }
        }

        public Ea.EaClient ServiceForBackgroundWorker => new Ea.EaClient(channelForBackgroundWorker);

        public Task UseClientAsync(Func<Ea.EaClient, Task> task)
        {
            var tcs = new TaskCompletionSource();
            Task.Run(async () =>
            {
                var nextIndex = Interlocked.Increment(ref index);
                var client = new Ea.EaClient(channels[nextIndex % channels.Length]);
                await task?.Invoke(client);
                tcs.SetResult();
            });
            return tcs.Task;
        }

        public Task<T> UseClientAsync<T>(Func<Ea.EaClient, Task<T>> task)
        {
            var tcs = new TaskCompletionSource<T>();
            Task.Run(async () =>
            {
                var nextIndex = Interlocked.Increment(ref index);
                var client = new Ea.EaClient(channels[nextIndex % channels.Length]);
                tcs.SetResult(await task?.Invoke(client));
            });
            return tcs.Task;
        }

        public void Dispose()
        {
            channelForBackgroundWorker.Dispose();
            foreach (var channel in channels)
            {
                channel.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
