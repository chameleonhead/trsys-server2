using System;
using System.Collections.Generic;
using System.Linq;

namespace Trsys.CopyTrading.Abstractions
{
    public class PublisherOrderDifference
    {
        public static readonly PublisherOrderDifference NoDifference = new(Array.Empty<PublisherOrder>(), Array.Empty<PublisherOrder>());

        public PublisherOrderDifference(IEnumerable<PublisherOrder> opened, IEnumerable<PublisherOrder> closed)
        {
            Opened = opened;
            Closed = closed;
            HasDifference = opened.Any() || closed.Any();
        }

        public IEnumerable<PublisherOrder> Opened { get; }
        public IEnumerable<PublisherOrder> Closed { get; }
        public bool HasDifference { get; }
    }
}
