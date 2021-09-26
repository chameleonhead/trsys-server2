using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTesting.Server
{
    public class HttpClientPool
    {
        private readonly List<HttpClient> clients;
        private readonly Semaphore semaphore;
        private int index = 0;

        public HttpClientPool(Func<HttpClient> clientFactory, int poolCount)
        {
            var clients = new List<HttpClient>();
            for (int i = 0; i < poolCount; i++)
            {
                clients.Add(clientFactory.Invoke());
            }
            this.clients = clients;
            this.semaphore = new Semaphore(clients.Count, clients.Count);
        }

        public Task UseClientAsync(Func<HttpClient, Task> task)
        {
            var tcs = new TaskCompletionSource();
            Task.Run(async () =>
            {
                semaphore.WaitOne();
                try
                {
                    var nextIndex = Interlocked.Increment(ref index);
                    var client = clients[nextIndex % clients.Count];
                    await task?.Invoke(client);
                    tcs.SetResult();
                }
                finally
                {
                    semaphore.Release();
                }
            });
            return tcs.Task;
        }

        public Task<T> UseClientAsync<T>(Func<HttpClient, Task<T>> task)
        {
            var tcs = new TaskCompletionSource<T>();
            Task.Run(async () =>
            {
                semaphore.WaitOne();
                try
                {
                    var nextIndex = Interlocked.Increment(ref index);
                    var client = clients[nextIndex % clients.Count];
                    tcs.SetResult(await task?.Invoke(client));
                }
                finally
                {
                    semaphore.Release();
                }
            });
            return tcs.Task;
        }
    }
}
