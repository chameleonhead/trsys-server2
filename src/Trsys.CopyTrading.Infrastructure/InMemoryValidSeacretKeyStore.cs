using System.Collections.Generic;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryValidSecretKeyStore : IValidSecretKeyStore
    {
        private Dictionary<string, SecretKey> _store = new();

        public Task<SecretKey> FindAsync(string key)
        {
            if (_store.TryGetValue(key, out var secretKey))
            {
                return Task.FromResult(secretKey);
            }
            return Task.FromResult(default(SecretKey));
        }

        public Task AddAsync(string key, string keyType)
        {
            _store.Add(key, new SecretKey()
            {
                Key = key,
                KeyType = keyType
            });
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _store.Remove(key);
            return Task.CompletedTask;
        }
    }
}
