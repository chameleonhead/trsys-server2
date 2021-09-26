using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading
{
    public class CopyTradingOrderTextCache
    {
        private OrderText _orderCache = null;

        public OrderText GetOrderTextCache()
        {
            return _orderCache;
        }

        public void UpdateOrderTextCache(OrderText orderText)
        {
            _orderCache = orderText;
        }

        public void RemoveOrderTextCache()
        {
            _orderCache = null;
        }
    }
}
