using LoadTesting.Extensions;
using NBomber.Contracts;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTesting.Client
{
    public abstract class TokenClientBase
    {
        protected readonly static ActivitySource source = new("Trsys.Server.Client");
        protected HttpClient Client { get; }
        protected string SecretKey { get; }
        protected string KeyType { get; }
        protected string Token { get; private set; }

        private bool isInit = false;
        private readonly SemaphoreSlim lockObject = new SemaphoreSlim(1);

        public TokenClientBase(HttpClient client, string secretKey, string keyType)
        {
            SecretKey = secretKey;
            KeyType = keyType;
            Client = client;
        }

        public async Task InitializeAsync()
        {
            using var activity = source.StartActivity("GenerateToken", ActivityKind.Client);
            Token = await Client.GenerateTokenAsync(SecretKey, KeyType);
            isInit = true;
        }

        public async Task FinalizeAsync()
        {
            using var activity = source.StartActivity("InvalidateToken", ActivityKind.Client);
            await Client.InvalidateTokenAsync(SecretKey, KeyType, Token);
            Token = null;
            isInit = false;
        }

        public async Task<Response> ExecuteAsync()
        {
            EnsureInited();

            await lockObject.WaitAsync();
            try
            {
                return await OnExecuteAsync();
            }
            finally
            {
                lockObject.Release();
            }
        }

        private void EnsureInited()
        {
            if (!isInit) throw new InvalidOperationException("not initialized");
        }

        protected abstract Task<Response> OnExecuteAsync();
    }
}
