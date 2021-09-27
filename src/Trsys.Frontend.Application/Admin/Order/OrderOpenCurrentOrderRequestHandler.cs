using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderOpenCurrentOrderRequestHandler : IRequestHandler<OrderOpenCurrentOrderRequest, OrderOpenCurrentOrderResponse>
    {
        public Task<OrderOpenCurrentOrderResponse> Handle(OrderOpenCurrentOrderRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(OrderOpenCurrentOrderResponse));
        }
    }
}
