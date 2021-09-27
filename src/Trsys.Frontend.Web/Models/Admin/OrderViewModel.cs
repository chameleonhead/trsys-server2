using System.Collections.Generic;
using Trsys.Frontend.Application.Admin.Order;

namespace Trsys.Frontend.Web.Models.Admin
{
    public class OrderViewModel
    {
        public CurrentOrderViewModel CurrentOrder { get; set; }
        public List<CurrentOrderSubscriptionStatusViewModel> OrderStatus { get; set; }
        public List<string> SymbolSelection { get; set; }
        public OrderOpenCurrentOrderRequest Request { get; internal set; }
    }

    public class CurrentOrderViewModel
    {
    }

    public class CurrentOrderSubscriptionStatusViewModel
    {
    }
}
