using EventFlow.Aggregates;

namespace Trsys.Backoffice.Abstractions
{
    public class UserCreatedEvent : AggregateEvent<UserAggregate, UserId>
    {
        public UserCreatedEvent(string username, string passwordHash, string name, string role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Name = name;
            Role = role;
        }

        public string Username { get; }
        public string PasswordHash { get; }
        public string Name { get; }
        public string Role { get; }
    }
}
