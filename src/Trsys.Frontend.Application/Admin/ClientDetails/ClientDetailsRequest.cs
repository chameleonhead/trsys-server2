using MediatR;

namespace Trsys.Frontend.Application.Admin.ClientDetails
{
    public class ClientDetailsRequest : IRequest<ClientDetailsResponse>
    {
        public string SecretKeyId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
