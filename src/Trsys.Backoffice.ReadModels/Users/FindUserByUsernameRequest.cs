using MediatR;

namespace Trsys.Backoffice.ReadModels.Users
{
    public class FindUserByUsernameRequest : IRequest<User>
    {
        public string Username { get; set; }
    }
}
