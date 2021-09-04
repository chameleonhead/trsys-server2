using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryEaSessionStore : IEaSessionStore
    {
        private record DictionaryKey(string Key, string KeyType);

        private readonly Dictionary<string, EaSession> _store = new();
        private readonly Dictionary<DictionaryKey, EaSession> _byKeys = new();

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
            _store.Add(session.Token, session);
            _byKeys.Add(key, session);
            return Task.FromResult(session);
        }

        public Task RemoveAsync(EaSession session)
        {
            _store.Remove(session.Token);
            _byKeys.Remove(new DictionaryKey(session.Key, session.KeyType));
            return Task.CompletedTask;
        }
    }
}
