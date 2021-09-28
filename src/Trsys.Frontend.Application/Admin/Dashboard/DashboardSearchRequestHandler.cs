using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.Dashboard
{
    public class DashboardSearchRequestHandler : IRequestHandler<DashboardSearchRequest, DashboardSearchResponse>
    {
        public Task<DashboardSearchResponse> Handle(DashboardSearchRequest request, CancellationToken cancellationToken)
        {
            var response = new DashboardSearchResponse
            {
                ConnectedKeys = new()
                {
                    new()
                    {
                        Id = "1",
                        Key = "MT4/OANDA Corporation/811631031/2",
                        KeyType = "Publisher",
                        Description = "山根さん",
                        IsActive = false,
                        IsConnected = false,
                    },
                    new()
                    {
                        Id = "2",
                        Key = "MT4/OANDA Corporation/811653730/2",
                        KeyType = "Subscriber",
                        Description = "大川さん",
                        IsActive = true,
                        IsConnected = true,
                    },
                },
                CurrentOrderText = new()
                {
                    Id = "1",
                    TicketNo = 1,
                    Symbol = "USDJPY",
                    OrderType = "BUY",
                },
                Trades = new()
                {
                }
            };
            return Task.FromResult(response);
        }
    }
}
