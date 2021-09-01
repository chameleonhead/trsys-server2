using System;

namespace Trsys.Backoffice.Abstractions
{
    public class UserCreatedEvent
    {
        public UserCreatedEvent()
        {
        }
        public UserCreatedEvent(string userId, string username, string passwordHash)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.UtcNow;
            UserId = userId;
            Username = username;
            PasswordHash = passwordHash;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "UserCreated";

        public string UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

    }
}
