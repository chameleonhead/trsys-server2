using MediatR;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.ClientDetails
{
    public class ClientDetailsRequestHandler : IRequestHandler<ClientDetailsRequest, ClientDetailsResponse>
    {
        public Task<ClientDetailsResponse> Handle(ClientDetailsRequest request, CancellationToken cancellationToken)
        {
            var response = new ClientDetailsResponse()
            {
                SecretKey = new()
                {
                    Id = "2",
                    Key = "MT4/OANDA Corporation/811653730/2",
                    KeyType = "Subscriber",
                    Desctiption = "大川さん",
                    IsActive = true,
                    IsConnected = true,
                },
                TradeHistory = new()
                {
                    new()
                    {

                    }
                },
                YearMonthSelection = new()
                {
                    new YearMonth(2021, 9),
                },
            };
            return Task.FromResult(response);
        }
    }
}
