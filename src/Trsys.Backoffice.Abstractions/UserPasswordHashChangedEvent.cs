using EventFlow.Aggregates;

namespace Trsys.Backoffice.Abstractions
{
    public class UserPasswordHashChangedEvent : AggregateEvent<UserAggregate, UserId>
    {
        public UserPasswordHashChangedEvent(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
        public string PasswordHash { get; }
    }
}