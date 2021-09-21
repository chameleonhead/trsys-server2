using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderEventArgs : EventArgs
    {
        public OrderEventArgs(PublisherOrder order)
        {
            Order = order;
        }

        public PublisherOrder Order { get; }
    }
}