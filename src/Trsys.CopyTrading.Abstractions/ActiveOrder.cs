using System;
using System.Collections.Generic;
using System.Linq;

namespace Trsys.CopyTrading.Abstractions
{
    public class ActiveOrder
    {
        public static readonly ActiveOrder Empty = new(OrderText.Empty, Array.Empty<PublisherOrder>());

        public ActiveOrder(OrderText orderText, IEnumerable<PublisherOrder> publisherOrders)
        {
            OrderText = orderText;
            Orders = publisherOrders.ToList();
        }

        public OrderText OrderText { get; set; }
        public List<PublisherOrder> Orders { get; set; }
    }
}
