using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderRequest : IRequest<OrderResponse>
    {
    }
}
