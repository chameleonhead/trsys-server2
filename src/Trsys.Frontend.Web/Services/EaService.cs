using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Services
{
    public class SecretKey
    {
        public string Key { get; set; }
        public string KeyType { get; set; }
    }

    public class ValidSeacretKeyStore
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
    }

    public class EaSession
    {
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Token { get; set; }
    }

    public class EaSessionStore
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

        public Task AddAsync(EaSession session)
        {
            _store.Add(session.Token, session);
            _byKeys.Add(session.Key, session);
            return Task.CompletedTask;
        }
    }

    public class EaService
    {
        public static EaService Instance = new EaService();
        private ValidSeacretKeyStore _keyStore = new();
        private EaSessionStore _sessionStore = new();

        public Task AddValidSecretKyeAsync(string key, string keyType)
        {
            return _keyStore.AddAsync(key, keyType);
        }

        public async Task<EaSession> GenerateTokenAsync(string key, string keyType)
        {
            var secretKey = await _keyStore.FindAsync(key);
            if (secretKey is null)
            {
                return null;
            }
            if (secretKey.KeyType != keyType)
            {
                return null;
            }
            var session = new EaSession()
            {
                Key = secretKey.Key,
                KeyType = secretKey.KeyType,
                Token = Guid.NewGuid().ToString(),
            };
            await _sessionStore.AddAsync(session);
            return session;
        }
    }
}
