using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.Clients
{
    public class ClientsSearchRequestHandler : IRequestHandler<ClientsSearchRequest, ClientsSearchResponse>
    {
        public Task<ClientsSearchResponse> Handle(ClientsSearchRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(ClientsSearchResponse));
        }
    }
}
