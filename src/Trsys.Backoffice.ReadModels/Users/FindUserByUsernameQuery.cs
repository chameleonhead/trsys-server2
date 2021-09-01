using MediatR;

namespace Trsys.Backoffice.ReadModels.Users
{
    public class FindUserByUsernameQuery : IRequest<User>
    {
        public string Username { get; set; }
    }
}
