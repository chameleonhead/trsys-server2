using System;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryEaSessionStore : IEaSessionStore
    {
        private InMemoryCopyTradingContext context;

        public InMemoryEaSessionStore(InMemoryCopyTradingContext context)
        {
            this.context = context;
        }

        public Task<EaSession> FindByTokenAsync(string token)
        {
            if (context.EaSessionStoreByToken.TryGetValue(token, out var session))
            {
                return Task.FromResult(session);
            }
            return Task.FromResult(default(EaSession));
        }

        public Task<EaSession> FindByKeyAsync(string key, string keyType)
        {
            if (context.EaSessionStoreByKey.TryGetValue(new InMemoryKeys.SecretKey(key, keyType), out var session))
            {
                return Task.FromResult(session);
            }
            return Task.FromResult(default(EaSession));
        }

        public Task<EaSession> CreateSessionAsync(SecretKey secretKey)
        {
            var key = new InMemoryKeys.SecretKey(secretKey.Key, secretKey.KeyType);
            if (context.EaSessionStoreByKey.TryGetValue(key, out var _))
            {
                throw new EaSessionAlreadyExistsException();
            }
            var session = new EaSession()
            {
                Key = secretKey.Key,
                KeyType = secretKey.KeyType,
                Token = Guid.NewGuid().ToString(),
            };
            context.EaSessionStoreByToken.TryAdd(session.Token, session);
            context.EaSessionStoreByKey.TryAdd(key, session);
            return Task.FromResult(session);
        }

        public Task RemoveAsync(EaSession session)
        {
            context.EaSessionStoreByToken.TryRemove(session.Token, out var _);
            context.EaSessionStoreByKey.TryRemove(new InMemoryKeys.SecretKey(session.Key, session.KeyType), out var _);
            return Task.CompletedTask;
        }
    }
}
