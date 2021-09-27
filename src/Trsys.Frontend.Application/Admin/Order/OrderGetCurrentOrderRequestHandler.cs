using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderGetCurrentOrderRequestHandler : IRequestHandler<OrderGetCurrentOrderRequest, OrderGetCurrentOrderResponse>
    {
        public Task<OrderGetCurrentOrderResponse> Handle(OrderGetCurrentOrderRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(OrderGetCurrentOrderResponse));
        }
    }
}
