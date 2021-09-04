using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryEaSessionStore : IEaSessionStore
    {
        private record DictionaryKey(string Key, string KeyType);

        private readonly ConcurrentDictionary<string, EaSession> _store = new();
        private readonly ConcurrentDictionary<DictionaryKey, EaSession> _byKeys = new();

        public Task<EaSession> FindByTokenAsync(string token)
        {
            if (_store.TryGetValue(token, out var session))
            {
                return Task.FromResult(session);
            }
            return Task.FromResult(default(EaSession));
        }

        public Task<EaSession> FindByKeyAsync(string key, string keyType)
        {
            if (_byKeys.TryGetValue(new DictionaryKey(key, keyType), out var session))
            {
                return Task.FromResult(session);
            }
            return Task.FromResult(default(EaSession));
        }

        public Task<EaSession> CreateSessionAsync(SecretKey secretKey)
        {
            var key = new DictionaryKey(secretKey.Key, secretKey.KeyType);
            if (_byKeys.TryGetValue(key, out var _))
            {
                throw new EaSessionAlreadyExistsException();
            }
            var session = new EaSession()
            {
                Key = secretKey.Key,
                KeyType = secretKey.KeyType,
                Token = Guid.NewGuid().ToString(),
            };
            _store.TryAdd(session.Token, session);
            _byKeys.TryAdd(key, session);
            return Task.FromResult(session);
        }

        public Task RemoveAsync(EaSession session)
        {
            _store.TryRemove(session.Token, out var _);
            _byKeys.TryRemove(new DictionaryKey(session.Key, session.KeyType), out var _);
            return Task.CompletedTask;
        }
    }
}
