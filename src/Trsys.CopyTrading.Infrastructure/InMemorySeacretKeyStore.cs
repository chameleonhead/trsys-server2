using System.Collections.Generic;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemorySecretKeyStore : ISecretKeyStore
    {
        private record DictionaryKey(string Key, string KeyType);

        private Dictionary<DictionaryKey, SecretKey> _store = new();

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
            _store.Add(new DictionaryKey(key, keyType), secretKey);
            return Task.FromResult(secretKey);
        }

        public Task<SecretKey> RemoveAsync(string key, string keyType)
        {
            var id = new DictionaryKey(key, keyType);
            if (_store.TryGetValue(id, out var secretKey))
            {
                _store.Remove(id);
            }
            return Task.FromResult(secretKey);
        }
    }
}
