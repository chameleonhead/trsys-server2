using System.Collections.Generic;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Application.Admin.Order
{
    public class OrderGetCurrentOrderResponse
    {
        public CurrentOrderDto CurrentOrder { get; set; }
        public List<SubscriberOrderStateDto> SubscriberStates { get; set; }
    }
}