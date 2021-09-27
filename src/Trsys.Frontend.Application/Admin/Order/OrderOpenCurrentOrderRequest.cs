using MediatR;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderOpenCurrentOrderRequest : IRequest<OrderOpenCurrentOrderResponse>
    {
        public string Symbol { get; set; }
        public string OrderType { get; set; }
    }
}
