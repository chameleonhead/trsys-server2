using System;
using System.Collections.Generic;
using System.Linq;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderDifference<T>
    {
        public static readonly OrderDifference<T> NoDifference = new(Array.Empty<T>(), Array.Empty<T>());

        public OrderDifference(IEnumerable<T> opened, IEnumerable<T> closed)
        {
            Opened = opened;
            Closed = closed;
            HasDifference = opened.Any() || closed.Any();
        }

        public IEnumerable<T> Opened { get; }
        public IEnumerable<T> Closed { get; }
        public bool HasDifference { get; }
    }
}
