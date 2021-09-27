using MediatR;

namespace Trsys.Frontend.Application.Admin.ClientDetails
{
    public class ClientDetailsSubscriberTradeHistorySearchRequest : IRequest<ClientDetailsSubscriberTradeHistorySearchResponse>
    {
        public string SecretKeyId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
