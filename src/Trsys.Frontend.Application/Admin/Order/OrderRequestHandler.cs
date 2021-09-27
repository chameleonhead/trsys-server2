using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderRequestHandler : IRequestHandler<OrderRequest, OrderResponse>
    {
        public Task<OrderResponse> Handle(OrderRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(OrderResponse));
        }
    }
}
