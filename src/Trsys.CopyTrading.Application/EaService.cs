using System;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public class EaService : IEaService
    {
        private readonly ISecretKeyStore keyStore;
        private readonly IEaSessionStore sessionStore;
        private readonly IPublisherOrderStore publisherOrderStore;
        private readonly IActiveOrderStore activeOrderStore;
        private readonly ISubscriberOrderStore subscriberOrderStore;
        private readonly IEventPublisher publisher;

        public EaService(
            ISecretKeyStore keyStore,
            IEaSessionStore sessionStore,
            IPublisherOrderStore publisherOrderStore,
            IActiveOrderStore activeOrderStore,
            ISubscriberOrderStore subscriberOrderStore,
            IEventPublisher publisher)
        {
            this.keyStore = keyStore;
            this.sessionStore = sessionStore;
            this.publisherOrderStore = publisherOrderStore;
            this.activeOrderStore = activeOrderStore;
            this.subscriberOrderStore = subscriberOrderStore;
            this.publisher = publisher;
        }

        public async Task AddSecretKeyAsync(string key, string keyType)
        {
            var secretKey = await keyStore.AddAsync(key, keyType);
            publisher.Publish(new SecretKeyRegisteredEvent(secretKey));
        }

        public async Task<EaSession> GenerateSessionTokenAsync(string key, string keyType)
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

        public async Task<bool> DiscardSessionTokenAsync(string token, string key, string keyType)
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
            publisher.Publish(new EaSessionDiscardedEvent(session));
            return true;
        }

        public async Task<bool> ValidateSessionTokenAsync(string token, string key, string keyType)
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
            var diff = await publisherOrderStore.SetOrderTextAsync(key, text);
            if (diff.HasDifference)
            {
                var result = await activeOrderStore.ApplyChangesAsync(diff);
                publisher.Publish(new OrderTextPublishedEvent(key, text));
                if (result.Changed)
                {
                    publisher.Publish(new ActiveOrderPublishedEvent(key, result.ActiveOrder));
                }
                foreach (var item in result.Opened)
                {
                    publisher.Publish(new PublisherOrderOpenPublishedEvent(item));
                }
                foreach (var item in result.Closed)
                {
                    publisher.Publish(new PublisherOrderClosePublishedEvent(item));
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
            var diff = await subscriberOrderStore.SetOrderTextAsync(key, orderText);
            if (diff.HasDifference)
            {
                foreach (var item in diff.Opened)
                {
                    publisher.Publish(new SubscriberOrderOpenDeliveredEvent(item));
                }
                foreach (var item in diff.Closed)
                {
                    publisher.Publish(new SubscriberOrderCloseDeliveredEvent(item));
                }
            }
            return orderText.OrderText;
        }

        public Task ReceiveLogAsync(DateTimeOffset timestamp, string key, string keyType, string version, string token, string text)
        {
            publisher.Publish(new EaLogReceivedEvent(timestamp, key, keyType, version, text));
            return Task.CompletedTask;
        }

        public Task ReceiveLogAsync(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text)
        {
            publisher.Publish(new EaLogReceivedV2Event(serverTimestamp, eaTimestamp, key, keyType, version, text));
            return Task.CompletedTask;
        }

        public async Task RemvoeSecretKeyAsync(string key, string keyType)
        {
            var session = await sessionStore.FindByKeyAsync(key, keyType);
            if (session != null)
            {
                await sessionStore.RemoveAsync(session);
                publisher.Publish(new EaSessionDiscardedEvent(session));
            }
            var secretKey = await keyStore.RemoveAsync(key, keyType);
            publisher.Publish(new SecretKeyUnregisteredEvent(secretKey));
        }
    }
}
