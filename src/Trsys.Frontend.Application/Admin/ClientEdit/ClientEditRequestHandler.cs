using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.ClientEdit
{
    public class ClientEditRequestHandler : IRequestHandler<ClientEditRequest, ClientEditResponse>
    {
        public Task<ClientEditResponse> Handle(ClientEditRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(ClientEditResponse));
        }
    }
}
