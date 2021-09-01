using MediatR;
using System;

namespace Trsys.Backoffice.WriteModels.Users
{
    public class UserCreateCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
