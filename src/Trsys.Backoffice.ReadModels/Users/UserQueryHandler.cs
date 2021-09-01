using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Backoffice.ReadModels.Users
{
    public class UserQueryHandler : IRequestHandler<FindUserByIdQuery, User>
    {
        private readonly IUserStore store;

        public UserQueryHandler(IUserStore store)
        {
            this.store = store;
        }

        public async Task<User> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await store.FindByIdAsync(request.UserId);
        }
        public async Task<User> Handle(FindUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            return await store.FindByUsernameAsync(request.Username);
        }
    }
}
