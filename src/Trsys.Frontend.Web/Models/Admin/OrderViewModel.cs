using System.Collections.Generic;

namespace Trsys.Frontend.Web.Models.Admin
{
    public class OrderViewModel
    {
        public CurrentOrderViewModel CurrentOrder { get; set; }
        public List<CurrentOrderSubscriptionStatusViewModel> OrderStatus { get; set; }
    }

    public class CurrentOrderViewModel
    {
    }

    public class CurrentOrderSubscriptionStatusViewModel
    {
    }
}
