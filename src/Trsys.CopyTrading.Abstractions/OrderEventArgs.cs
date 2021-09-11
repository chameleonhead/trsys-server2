using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class OrderEventArgs : EventArgs
    {
        public OrderEventArgs(string publisherKey, OrderTextItem orderText)
        {
            PublisherKey = publisherKey;
            OrderText = orderText;
        }

        public string PublisherKey { get; }
        public OrderTextItem OrderText { get; }
    }
}