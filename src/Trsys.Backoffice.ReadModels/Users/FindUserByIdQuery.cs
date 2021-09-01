using MediatR;

namespace Trsys.Backoffice.ReadModels.Users
{
    public class FindUserByIdQuery : IRequest<User>
    {
        public string UserId { get; set; }
    }
}
