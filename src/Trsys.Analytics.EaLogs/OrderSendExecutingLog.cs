using System;

namespace Trsys.Analytics.EaLogs
{
    public class OrderSendExecutingLog : ILogInfo
    {
        public OrderSendExecutingLog()
        {
        }

        public OrderSendExecutingLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, decimal freeMargin, long leverage, decimal percentOfFreeMargin, decimal calculatedVolume)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = timestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Token = token;
            FreeMargin = freeMargin;
            Leverage = leverage;
            PercentOfFreeMargin = percentOfFreeMargin;
            CalculatedVolume = calculatedVolume;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogOrderSendExecuting";
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
        public decimal FreeMargin { get; set; }
        public long Leverage { get; set; }
        public decimal PercentOfFreeMargin { get; set; }
        public decimal CalculatedVolume { get; set; }
    }
}