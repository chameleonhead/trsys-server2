using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.ClientAdd
{
    public class ClientAddRequestHandler : IRequestHandler<ClientAddRequest, ClientAddResponse>
    {
        public Task<ClientAddResponse> Handle(ClientAddRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(ClientAddResponse));
        }
    }
}
