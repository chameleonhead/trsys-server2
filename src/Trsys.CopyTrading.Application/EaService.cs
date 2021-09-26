using System;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.EaLogs;

namespace Trsys.CopyTrading.Application
{
    public class EaService : IEaService
    {
        private readonly IEaSessionTokenProvider tokenProvider;
        private readonly IEaStore eaStore;
        private readonly IEaLogAnalyzer eaLogAnalyzer;
        private readonly IOrderNotificationBus orderBus;
        private readonly IOrderStore orderStore;

        public EaService(IEaStore eaStore, IEaSessionTokenProvider tokenProvider, IEaLogAnalyzer eaLogAnalyzer, IOrderNotificationBus orderBus, IOrderStore orderStore)
        {
            this.tokenProvider = tokenProvider;
            this.eaStore = eaStore;
            this.eaLogAnalyzer = eaLogAnalyzer;
            this.orderBus = orderBus;
            this.orderStore = orderStore;
        }

        public Task AddSecretKeyAsync(string key, string keyType)
        {
            eaStore.Add(key, keyType);
            return Task.CompletedTask;
        }

        public Task RemvoeSecretKeyAsync(string key, string keyType)
        {
            eaStore.Remove(key, keyType);
            return Task.CompletedTask;
        }

        public Task<EaSession> GenerateSessionTokenAsync(string key, string keyType)
        {
            var ea = eaStore.Find(key, keyType);
            if (ea == null)
            {
                return Task.FromResult(default(EaSession));
            }
            if (ea.Session != null)
            {
                throw new EaSessionAlreadyExistsException();
            }
            return Task.FromResult(ea.GenerateSession(tokenProvider.GenerateToken(key, keyType)));
        }

        public Task DiscardSessionTokenAsync(string token, string key, string keyType)
        {
            var ea = eaStore.Find(key, keyType);
            if (ea == null)
            {
                throw new EaSessionTokenNotFoundException();
            }
            ea.DiscardSession(token);
            return Task.CompletedTask;
        }

        public Task ValidateSessionTokenAsync(string token, string key, string keyType)
        {
            var ea = eaStore.Find(key, keyType);
            if (ea == null)
            {
                throw new EaSessionTokenNotFoundException();
            }
            ea.ValidateSession(token);
            return Task.CompletedTask;
        }

        public Task PublishOrderTextAsync(string key, string text)
        {
            var publisher = eaStore.Find(key, "Publisher") as Publisher;
            if (publisher == null)
            {
                throw new EaSessionTokenNotFoundException();
            }
            var orderText = OrderText.Parse(text);
            publisher.UpdateOrderText(orderText);
            return Task.CompletedTask;
        }

        public Task<OrderText> GetCurrentOrderTextAsync()
        {
            return Task.FromResult(orderStore.GetOrderText());
        }

        public Task SubscribeOrderTextAsync(string key, string text)
        {
            var subscriber = eaStore.Find(key, "Subscriber") as Subscriber;
            if (subscriber == null)
            {
                throw new EaSessionTokenNotFoundException();
            }
            var orderText = OrderText.Parse(text);
            subscriber.SubscribeOrderText(orderText);
            return Task.CompletedTask;
        }
        public Task ReceiveLogAsync(DateTimeOffset timestamp, string key, string keyType, string version, string token, string text)
        {
            eaLogAnalyzer.AnalyzeLog(timestamp, key, keyType, version, token, text);
            return Task.CompletedTask;
        }

        public Task ReceiveLogAsync(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text)
        {
            eaLogAnalyzer.AnalyzeLog(serverTimestamp, eaTimestamp, key, keyType, version, token, text);
            return Task.CompletedTask;
        }

        public void SubscribeOrderTextUpdated(Action<OrderText> handler)
        {
            orderBus.AddOrderTextUpdatedHandler(handler);
        }

        public void UnsubscribeOrderTextUpdated(Action<OrderText> handler)
        {
            orderBus.RemoveOrderTextUpdatedHandler(handler);
        }
    }
}
