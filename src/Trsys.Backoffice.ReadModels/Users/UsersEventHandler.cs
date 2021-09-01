using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Backoffice.ReadModels.Users
{
    public class UsersEventHandler : IRequestHandler<FindUserByIdRequest, User>
    {
        private readonly IUserStore store;

        public UsersEventHandler(IUserStore store)
        {
            this.store = store;
        }

        public async Task<User> Handle(FindUserByIdRequest request, CancellationToken cancellationToken)
        {
            return await store.FindByIdAsync(request.UserId);
        }
        public async Task<User> Handle(FindUserByUsernameRequest request, CancellationToken cancellationToken)
        {
            return await store.FindByUsernameAsync(request.Username);
        }
    }
}
