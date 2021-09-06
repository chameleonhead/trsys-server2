using System;

namespace Trsys.CopyTrading.EaLogs
{
    public class OrderSetupCurrencyInfoFetchedLog : LogBase
    {
        public OrderSetupCurrencyInfoFetchedLog()
        {
        }

        public OrderSetupCurrencyInfoFetchedLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, string symbol, decimal marginForOneLot, decimal step) : base(timestamp, key, keyType, version, token)
        {
            Symbol = symbol;
            MarginForOneLot = marginForOneLot;
            Step = step;
        }

        public string Symbol { get; set; }
        public decimal MarginForOneLot { get; set; }
        public decimal Step { get; set; }
    }
}
