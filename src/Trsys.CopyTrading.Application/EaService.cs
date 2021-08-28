using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public class EaService : IEaService
    {
        private readonly IValidSecretKeyStore keyStore;
        private readonly IEaSessionStore sessionStore;
        private readonly IOrderStore orderStore;

        public EaService(IValidSecretKeyStore keyStore, IEaSessionStore sessionStore, IOrderStore orderStore)
        {
            this.keyStore = keyStore;
            this.sessionStore = sessionStore;
            this.orderStore = orderStore;
        }

        public Task AddValidSecretKeyAsync(string key, string keyType)
        {
            if (keyType == "Publisher")
            {
                return keyStore.AddPublisherAsync(key);
            }
            else
            {
                return keyStore.AddSubscriberAsync(key);
            }
        }

        public async Task<EaSession> GenerateTokenAsync(string key, string keyType)
        {
            var secretKey = await keyStore.FindAsync(key);
            if (secretKey is null)
            {
                return null;
            }
            if (secretKey.KeyType != keyType)
            {
                return null;
            }
            return await sessionStore.CreateSessionAsync(secretKey);
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
            return true;
        }

        public async Task PublishOrderTextAsync(string key, string text)
        {
            var followers = await keyStore.SearchFollowersAsync(key);
            if (!followers.Any())
            {
                return;
            }
            foreach (var follower in followers)
            {
                await orderStore.SetTextAsync(follower.Key, text);
            }
        }

        public async Task<PublishedOrders> GetOrderTextAsync(string key)
        {
            return await orderStore.GetTextAsync(key);
        }
    }
}
