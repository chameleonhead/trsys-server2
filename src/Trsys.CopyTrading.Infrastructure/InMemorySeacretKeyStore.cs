using System.Collections.Concurrent;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemorySecretKeyStore : ISecretKeyStore
    {
        private record DictionaryKey(string Key, string KeyType);

        private ConcurrentDictionary<DictionaryKey, SecretKey> _store = new();

        public Task<SecretKey> FindAsync(string key, string keyType)
        {
            if (_store.TryGetValue(new DictionaryKey(key, keyType), out var secretKey))
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
            _store.TryAdd(new DictionaryKey(key, keyType), secretKey);
            return Task.FromResult(secretKey);
        }

        public Task<SecretKey> RemoveAsync(string key, string keyType)
        {
            var id = new DictionaryKey(key, keyType);
            _store.TryRemove(id, out var secretKey);
            return Task.FromResult(secretKey);
        }
    }
}
