using System;
using Trsys.CopyTrading.Events;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaBase : IDisposable
    {
        public EaBase(string key, string keyType, IEventQueue events)
        {
            Key = key;
            KeyType = keyType;
            Events = events;
            Events.Enqueue(new SecretKeyRegisteredEvent(key, keyType));
        }

        public string Key { get; }
        public string KeyType { get; }
        public EaSession Session { get; private set; }
        protected IEventQueue Events { get; }

        public virtual EaSession GenerateSession(string sessionToken)
        {
            if (Session != null)
            {
                throw new EaSessionAlreadyExistsException();
            }
            Session = new EaSession(Key, KeyType, sessionToken);
            Events.Enqueue(new EaSessionGeneratedEvent(Key, KeyType, Session.Token));
            return Session;
        }

        public virtual void DiscardSession(string token)
        {
            if (Session == null)
            {
                throw new EaSessionTokenNotFoundException();
            }
            if (Session.Token != token)
            {
                throw new EaSessionTokenInvalidException();
            }
            Events.Enqueue(new EaSessionDiscardedEvent(Key, KeyType, Session.Token));
            Session = null;
        }

        public virtual void ValidateSession(string token)
        {
            if (Session == null)
            {
                throw new EaSessionTokenNotFoundException();
            }
            if (Session.Token != token)
            {
                throw new EaSessionTokenInvalidException();
            }
            Events.Enqueue(new EaSessionGeneratedEvent(Key, KeyType, Session.Token));
        }

        public virtual void Dispose()
        {
            Events.Enqueue(new SecretKeyUnregisteredEvent(Key, KeyType));
            GC.SuppressFinalize(this);
        }
    }
}
