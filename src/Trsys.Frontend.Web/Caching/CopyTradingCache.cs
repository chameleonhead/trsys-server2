using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.Frontend.Web.Caching
{
    public enum ValidateEaSessionTokenCacheResult
    {
        IN_CACHE,
        NOT_IN_CACHE,
    }
    public class CopyTradingCache
    {
        private record OrderTextCache(OrderText orderText, DateTimeOffset nextSyncTime);
        private ConcurrentDictionary<string, DateTimeOffset> _tokenCache = new();
        private OrderTextCache _orderCache = null;

        public ValidateEaSessionTokenCacheResult IsValidEaSessionToken(string token)
        {
            if (!_tokenCache.TryGetValue(token, out var nextSyncTime))
            {
                return ValidateEaSessionTokenCacheResult.NOT_IN_CACHE;
            }
            if (DateTimeOffset.UtcNow > nextSyncTime)
            {
                _tokenCache.TryRemove(new KeyValuePair<string, DateTimeOffset>(token, nextSyncTime));
                return ValidateEaSessionTokenCacheResult.NOT_IN_CACHE;
            }
            return ValidateEaSessionTokenCacheResult.IN_CACHE;

        }

        public void UpdateValidEaSessionTokenValidity(string token)
        {
            var nextSyncTime = DateTimeOffset.UtcNow.AddSeconds(1);
            if (_tokenCache.TryGetValue(token, out var currentNextSyncTime))
            {
                if (nextSyncTime > currentNextSyncTime)
                {
                    _tokenCache.TryUpdate(token, nextSyncTime, currentNextSyncTime);
                }
            }
            else
            {
                _tokenCache.TryAdd(token, nextSyncTime);
            }
        }

        public void RemoveValidEaSessionToken(string token)
        {
            _tokenCache.TryRemove(token, out var _);
        }

        public OrderText GetOrderTextHash(string key)
        {
            var orderCache = _orderCache;
            if (orderCache == null)
            {
                return null;
            }
            if (DateTimeOffset.UtcNow > orderCache.nextSyncTime)
            {
                _orderCache = null;
                return null;
            }
            return orderCache.orderText;
        }

        public void UpdateOrderTextCache(string key, OrderText orderText)
        {
            var nextSyncTime = DateTimeOffset.UtcNow.AddMilliseconds(100);
            var orderCache = _orderCache;
            if (orderCache != null)
            {
                if (nextSyncTime > orderCache.nextSyncTime)
                {
                    _orderCache = new OrderTextCache(orderText, nextSyncTime);
                }
            }
            else
            {
                _orderCache = new OrderTextCache(orderText, nextSyncTime);
            }
        }

        public void RemoveOrderTextCache(string key)
        {
            _orderCache = null;
        }
    }
}
