using MediatR;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderCloseCurrentOrderRequest : IRequest<OrderCloseCurrentOrderResponse>
    {
        public string CopyTradeId { get; set; }
    }
}
