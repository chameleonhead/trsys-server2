using System;
using System.Collections.Generic;
using System.Linq;

namespace Trsys.CopyTrading.Abstractions
{
    public class ActiveOrder
    {
        public static readonly ActiveOrder Empty = new ActiveOrder(OrderText.Empty, Array.Empty<PublisherOrder>());

        public ActiveOrder(OrderText orderText, IEnumerable<PublisherOrder> publishedOrders)
        {
            OrderText = orderText;
            Orders = publishedOrders.ToList();
        }

        public OrderText OrderText { get; set; }
        public List<PublisherOrder> Orders {get;set;}
    }
}
