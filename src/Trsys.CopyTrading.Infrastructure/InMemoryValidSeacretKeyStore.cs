using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryValidSecretKeyStore : IValidSecretKeyStore
    {
        private Dictionary<string, SecretKey> _store = new();
        private Dictionary<string, HashSet<string>> _followersStore = new();
        private Dictionary<string, string> _followingStore = new();

        public Task<SecretKey> FindAsync(string key)
        {
            if (_store.TryGetValue(key, out var secretKey))
            {
                return Task.FromResult(secretKey);
            }
            return Task.FromResult(default(SecretKey));
        }

        public Task AddPublisherAsync(string key)
        {
            _store.Add(key, new SecretKey()
            {
                Key = key,
                KeyType = "Publisher",
            });
            _followersStore.Add(key, new());
            return Task.CompletedTask;
        }

        public async Task AddSubscriberAsync(string key, string following = null)
        {
            if (following == null)
            {
                following = _followersStore.Keys.First();
            }
            if (!_followersStore.TryGetValue(following, out var followers))
            {
                await AddPublisherAsync(key);
                followers = _followersStore[following];
            }
            followers.Add(key);
            _store.Add(key, new SecretKey()
            {
                Key = key,
                KeyType = "Subscriber",
            });
            _followingStore.Add(key, following);
        }

        public async Task RemoveAsync(string key)
        {
            var secretKey = await FindAsync(key);
            if (secretKey is null)
            {
                return;
            }
            _followersStore.Remove(key);
            _store.Remove(key);
        }

        public Task<IEnumerable<SecretKey>> SearchFollowersAsync(string key)
        {
            return Task.FromResult(_followersStore[key].Select(f => _store[f]));
        }
    }
}
