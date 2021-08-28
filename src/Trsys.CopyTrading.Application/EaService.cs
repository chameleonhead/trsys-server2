using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public class EaService : IEaService
    {
        private readonly ISecretKeyStore keyStore;
        private readonly IEaSessionStore sessionStore;
        private readonly IOrderStore orderStore;

        public EaService(ISecretKeyStore keyStore, IEaSessionStore sessionStore, IOrderStore orderStore)
        {
            this.keyStore = keyStore;
            this.sessionStore = sessionStore;
            this.orderStore = orderStore;
        }

        public async Task AddSecretKeyAsync(string key, string keyType)
        {
            await keyStore.AddAsync(key, keyType);
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
            await orderStore.SetTextAsync(key, text);
        }

        public async Task<PublishedOrders> GetOrderTextAsync(string key)
        {
            return await orderStore.GetTextAsync(key);
        }
    }
}
