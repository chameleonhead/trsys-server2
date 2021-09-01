using MediatR;

namespace Trsys.Backoffice.ReadModels.Users
{
    public class FindUserByIdRequest : IRequest<User>
    {
        public string UserId { get; set; }
    }
}
