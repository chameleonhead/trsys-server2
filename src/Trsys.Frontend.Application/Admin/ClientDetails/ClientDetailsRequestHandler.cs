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
                TradeHistorySearchResult = new()
                {
                    CurrentYearMonth = new YearMonth(request.Year, request.Month),
                    TotalCount = 2,
                    TotalProfitLoss = -1000,
                    Items = new()
                    {
                        new()
                        {
                            CopyTradeId = "1",
                            Symbol = "AUDJPY",
                            OrderType = "BUY",
                            IsOpen = true,
                            ProfitLoss = 0,
                            OpenTimestamp = System.DateTimeOffset.Parse("2021-09-27T13:48:50.000Z"),
                        },
                        new()
                        {
                            CopyTradeId = "2",
                            Symbol = "USDJPY",
                            OrderType = "SELL",
                            IsOpen = false,
                            ProfitLoss = -1000,
                            OpenTimestamp = System.DateTimeOffset.Parse("2021-09-26T10:23:50.000Z"),
                            CloseTimestamp = System.DateTimeOffset.Parse("2021-09-26T10:28:30.000Z"),
                        }
                    }
                },
                YearMonthSelection = new()
                {
                    new YearMonth(request.Year, request.Month + 1),
                    new YearMonth(request.Year, request.Month),
                    new YearMonth(request.Year, request.Month - 1),
                },
            };
            return Task.FromResult(response);
        }
    }
}
