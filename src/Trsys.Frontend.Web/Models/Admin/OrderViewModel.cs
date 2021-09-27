using System.Collections.Generic;
using Trsys.Frontend.Application.Admin.Order;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Web.Models.Admin
{
    public class OrderViewModel
    {
        public CurrentOrderDto CurrentOrder { get; set; }
        public List<SubscriberOrderStateDto> SubscriberStates { get; set; }
        public List<string> SymbolSelection { get; set; }
        public OrderOpenCurrentOrderRequest OpenRequest { get; set; }
        public OrderCloseCurrentOrderRequest CloseRequest { get; set; }
    }
}
