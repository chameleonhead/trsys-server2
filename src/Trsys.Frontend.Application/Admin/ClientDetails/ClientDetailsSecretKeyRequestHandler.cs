using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.ClientDetails
{
    public class ClientDetailsSecretKeyRequestHandler : IRequestHandler<ClientDetailsSecretKeyRequest, ClientDetailsSecretKeyResponse>
    {
        public Task<ClientDetailsSecretKeyResponse> Handle(ClientDetailsSecretKeyRequest request, CancellationToken cancellationToken)
        {
            var response = new ClientDetailsSecretKeyResponse()
            {
                SecretKey = new()
                {
                    Id = "2",
                    Key = "MT4/OANDA Corporation/811653730/2",
                    KeyType = "Subscriber",
                    Description = "大川さん",
                    IsActive = true,
                    IsConnected = true,
                },
            };
            return Task.FromResult(response);
        }
    }
}
