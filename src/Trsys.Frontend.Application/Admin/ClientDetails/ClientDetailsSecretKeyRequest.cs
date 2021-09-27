using MediatR;

namespace Trsys.Frontend.Application.Admin.ClientDetails
{
    public class ClientDetailsSecretKeyRequest : IRequest<ClientDetailsSecretKeyResponse>
    {
        public string SecretKeyId { get; set; }
    }
}
