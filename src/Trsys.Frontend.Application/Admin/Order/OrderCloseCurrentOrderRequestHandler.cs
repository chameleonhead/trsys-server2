using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderCloseCurrentOrderRequestHandler : IRequestHandler<OrderCloseCurrentOrderRequest, OrderCloseCurrentOrderResponse>
    {
        public Task<OrderCloseCurrentOrderResponse> Handle(OrderCloseCurrentOrderRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(OrderCloseCurrentOrderResponse));
        }
    }
}
