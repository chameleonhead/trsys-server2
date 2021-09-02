using System;

namespace Trsys.Analytics.EaLogs
{
    public class OrderSetupMarginCalculatedLog : LogBase
    {
        public OrderSetupMarginCalculatedLog()
        {
        }

        public OrderSetupMarginCalculatedLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, decimal freeMargin, long leverage, decimal percentOfFreeMargin, decimal calculatedVolume) : base(timestamp, key, keyType, version, token)
        {
            FreeMargin = freeMargin;
            Leverage = leverage;
            PercentOfFreeMargin = percentOfFreeMargin;
            CalculatedVolume = calculatedVolume;
        }

        public decimal FreeMargin { get; set; }
        public long Leverage { get; set; }
        public decimal PercentOfFreeMargin { get; set; }
        public decimal CalculatedVolume { get; set; }
    }
}