using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Trsys.Backoffice.Abstractions;

namespace Trsys.Backoffice.WriteModels.Users
{
    public class UserCommandHandler : IRequestHandler<UserCreateCommand, string>
    {
        private readonly IMediator mediator;

        public UserCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public Task<string> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid().ToString();
            mediator.Publish(new UserCreatedEvent(id, request.Username, request.PasswordHash));
            return Task.FromResult(id);
        }
    }
}
