using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public class EaService : IEaService
    {
        private readonly ISecretKeyStore keyStore;
        private readonly IEaSessionStore sessionStore;
        private readonly IPublisherOrderStore orderStore;
        private readonly IActiveOrderStore activeOrderStore;
        private readonly IEventPublisher publisher;

        public EaService(ISecretKeyStore keyStore, IEaSessionStore sessionStore, IPublisherOrderStore orderStore, IActiveOrderStore activeOrderStore, IEventPublisher publisher)
        {
            this.keyStore = keyStore;
            this.sessionStore = sessionStore;
            this.orderStore = orderStore;
            this.activeOrderStore = activeOrderStore;
            this.publisher = publisher;
        }

        public async Task AddSecretKeyAsync(string key, string keyType)
        {
            var secretKey = await keyStore.AddAsync(key, keyType);
            publisher.Publish(new SecretKeyRegisteredEvent(secretKey));
        }

        public async Task<EaSession> GenerateTokenAsync(string key, string keyType)
        {
            var secretKey = await keyStore.FindAsync(key, keyType);
            if (secretKey is null)
            {
                return null;
            }
            if (secretKey.KeyType != keyType)
            {
                return null;
            }
            var session = await sessionStore.CreateSessionAsync(secretKey);
            publisher.Publish(new EaSessionGeneratedEvent(session));
            return session;
        }

        public async Task<bool> InvalidateSessionAsync(string token, string key, string keyType)
        {
            var session = await sessionStore.FindByTokenAsync(token);
            if (session is null)
            {
                return false;
            }
            if (session.Key != key)
            {
                return false;
            }
            if (session.KeyType != keyType)
            {
                return false;
            }
            await sessionStore.RemoveAsync(session);
            publisher.Publish(new EaSessionDestroyedEvent(session));
            return true;
        }

        public async Task<bool> ValidateSessionAsync(string token, string key, string keyType)
        {
            var session = await sessionStore.FindByTokenAsync(token);
            if (session is null)
            {
                return false;
            }
            if (session.Key != key)
            {
                return false;
            }
            if (session.KeyType != keyType)
            {
                return false;
            }
            publisher.Publish(new EaSessionValidatedEvent(session));
            return true;
        }

        public async Task PublishOrderTextAsync(string key, string text)
        {
            var diff = await orderStore.SetPublishedOrderTextAsync(key, text);
            if (diff.HasDifference)
            {
                var result = await activeOrderStore.ApplyChangesAsync(diff);
                publisher.Publish(new OrderTextPublishedEvent(key, text));
                foreach (var item in result.Opened)
                {
                    publisher.Publish(new PublisherOrderOpenedEvent(item));
                }
                foreach (var item in result.Closed)
                {
                    publisher.Publish(new PublisherOrderClosedEvent(item));
                }
                foreach (var item in result.Ignored)
                {
                    publisher.Publish(new PublisherOrderIgnoredEvent(item));
                }
            }
        }

        public async Task<OrderText> GetOrderTextAsync(string key)
        {
            var orderText = await activeOrderStore.GetActiveOrderAsync();
            publisher.Publish(new ActiveOrderPublishedEvent(key, orderText));
            return orderText.OrderText;
        }
    }
}
