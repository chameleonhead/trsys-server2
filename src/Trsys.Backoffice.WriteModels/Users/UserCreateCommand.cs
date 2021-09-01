using EventFlow.Commands;
using Trsys.Backoffice.Abstractions;

namespace Trsys.Backoffice.WriteModels.Users
{
    public class UserCreateCommand : Command<UserAggregate, UserId>
    {
        public UserCreateCommand(UserId aggregateId, string username, string passwordHash, string name, string role) : base(aggregateId)
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
