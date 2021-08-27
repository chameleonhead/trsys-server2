using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryEaSessionStore : IEaSessionStore
    {
        private Dictionary<string, EaSession> _store = new();
        private Dictionary<string, EaSession> _byKeys = new();

        public Task<EaSession> FindByTokenAsync(string token)
        {
            if (_store.TryGetValue(token, out var session))
            {
                return Task.FromResult(session);
            }
            return Task.FromResult(default(EaSession));
        }

        public Task<EaSession> FindByKeyAsync(string key)
        {
            if (_byKeys.TryGetValue(key, out var session))
            {
                return Task.FromResult(session);
            }
            return Task.FromResult(default(EaSession));
        }

        public Task<EaSession> CreateSessionAsync(SecretKey secretKey)
        {
            if (_byKeys.TryGetValue(secretKey.Key, out var _))
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
            _byKeys.Add(session.Key, session);
            return Task.FromResult(session);
        }

        public Task RemoveAsync(EaSession session)
        {
            _store.Remove(session.Token);
            _byKeys.Remove(session.Key);
            return Task.CompletedTask;
        }
    }
}
