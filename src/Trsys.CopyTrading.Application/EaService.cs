using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public class EaService : IEaService
    {
        private readonly IValidSecretKeyStore keyStore;
        private readonly IEaSessionStore sessionStore;

        public EaService(IValidSecretKeyStore keyStore, IEaSessionStore sessionStore)
        {
            this.keyStore = keyStore;
            this.sessionStore = sessionStore;
        }

        public Task AddValidSecretKeyAsync(string key, string keyType)
        {
            return keyStore.AddAsync(key, keyType);
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
    }
}
