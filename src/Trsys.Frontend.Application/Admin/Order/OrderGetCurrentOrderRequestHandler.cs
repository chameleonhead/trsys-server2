using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderGetCurrentOrderRequestHandler : IRequestHandler<OrderGetCurrentOrderRequest, OrderGetCurrentOrderResponse>
    {
        public Task<OrderGetCurrentOrderResponse> Handle(OrderGetCurrentOrderRequest request, CancellationToken cancellationToken)
        {
            var response = new OrderGetCurrentOrderResponse()
            {
                CurrentOrder = new()
                {
                    CopyTradeId = "1",
                    TicketNo = 1,
                    Symbol = "USDJPY",
                    OrderType = "SELL",
                    OpenTimestamp = DateTimeOffset.Parse("2021-09-27T11:23:32Z")
                },
                SubscriberStates = new()
                {
                },
            };
            return Task.FromResult(response);
        }
    }
}
