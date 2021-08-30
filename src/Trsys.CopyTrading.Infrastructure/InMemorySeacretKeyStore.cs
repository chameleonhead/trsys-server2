using System.Collections.Generic;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemorySecretKeyStore : ISecretKeyStore
    {
        record DictionaryKey (string Key, string KeyType);

        private Dictionary<DictionaryKey, SecretKey> _store = new();

        public Task<SecretKey> FindAsync(string key, string keyType)
        {
            if (_store.TryGetValue(new DictionaryKey(key, keyType), out var secretKey))
            {
                return Task.FromResult(secretKey);
            }
            return Task.FromResult(default(SecretKey));
        }

        public Task AddAsync(string key, string keyType)
        {
            _store.Add(new DictionaryKey(key, keyType), new SecretKey()
            {
                Key = key,
                KeyType = keyType,
            });
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, string keyType)
        {
            _store.Remove(new DictionaryKey(key, keyType));
            return Task.CompletedTask;
        }
    }
}
