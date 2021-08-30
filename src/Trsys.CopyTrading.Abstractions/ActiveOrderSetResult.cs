using System;
using System.Collections.Generic;
using System.Linq;

namespace Trsys.CopyTrading.Abstractions
{
    public class ActiveOrderSetResult
    {
        public ActiveOrderSetResult()
        {
        }

        public ActiveOrderSetResult(IEnumerable<PublisherOrder> ignored, IEnumerable<PublisherOrder> opened, IEnumerable<PublisherOrder> closed, ActiveOrder activeOrder)
        {
            Changed = opened.Any() || closed.Any();
            Ignored = ignored;
            Opened = opened;
            Closed = closed;
            ActiveOrder = activeOrder;
        }

        public bool Changed { get; set; }
        public IEnumerable<PublisherOrder> Ignored { get; set; }
        public IEnumerable<PublisherOrder> Opened { get; set; }
        public IEnumerable<PublisherOrder> Closed { get; set; }
        public ActiveOrder ActiveOrder { get; set; }

        public static ActiveOrderSetResult Unchanged(ActiveOrder activeOrder)
        {
            return new(Array.Empty<PublisherOrder>(), Array.Empty<PublisherOrder>(), Array.Empty<PublisherOrder>(), activeOrder);
        }
    }
}
