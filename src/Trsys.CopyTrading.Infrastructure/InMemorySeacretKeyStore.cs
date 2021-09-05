using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemorySecretKeyStore : ISecretKeyStore
    {
        private readonly InMemoryCopyTradingContext context;

        public InMemorySecretKeyStore(InMemoryCopyTradingContext context)
        {
            this.context = context;
        }

        public Task<SecretKey> FindAsync(string key, string keyType)
        {
            if (context.SecretKeyStore.TryGetValue(new InMemoryKeys.SecretKey(key, keyType), out var secretKey))
            {
                return Task.FromResult(secretKey);
            }
            return Task.FromResult(default(SecretKey));
        }

        public Task<SecretKey> AddAsync(string key, string keyType)
        {
            var secretKey = new SecretKey()
            {
                Key = key,
                KeyType = keyType,
            };
            context.SecretKeyStore.TryAdd(new InMemoryKeys.SecretKey(key, keyType), secretKey);
            return Task.FromResult(secretKey);
        }

        public Task<SecretKey> RemoveAsync(string key, string keyType)
        {
            var id = new InMemoryKeys.SecretKey(key, keyType);
            context.SecretKeyStore.TryRemove(id, out var secretKey);
            return Task.FromResult(secretKey);
        }
    }
}
