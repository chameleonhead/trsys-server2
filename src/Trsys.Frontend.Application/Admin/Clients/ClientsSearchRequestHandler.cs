using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Application.Admin.Clients
{
    public class ClientsSearchRequestHandler : IRequestHandler<ClientsSearchRequest, ClientsSearchResponse>
    {
        public Task<ClientsSearchResponse> Handle(ClientsSearchRequest request, CancellationToken cancellationToken)
        {
            var response = new ClientsSearchResponse()
            {
                Clients = new()
                {
                    new SecretKeyDto()
                    {
                        Id = "1",
                        Key = "MT4/OANDA Corporation/811631031/2",
                        KeyType = "Publisher",
                        Description = "山根さん",
                        IsActive = false,
                        IsConnected = false,
                    },
                    new SecretKeyDto()
                    {
                        Id = "2",
                        Key = "MT4/OANDA Corporation/811653730/2",
                        KeyType = "Subscriber",
                        Description = "大川さん",
                        IsActive = true,
                        IsConnected = true,
                    },
                }
            };
            return Task.FromResult(response);
        }
    }
}
