using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogOrderSetupCurrencyInfoFetchedEvent : IEvent
    {
        public EaLogOrderSetupCurrencyInfoFetchedEvent()
        {
        }

        public EaLogOrderSetupCurrencyInfoFetchedEvent(DateTimeOffset timestamp, string key, string keyType, string version, string token, string symbol, decimal marginForOneLot, decimal step)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = timestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Token = token;
            Symbol = symbol;
            MarginForOneLot = marginForOneLot;
            Step = step;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogOrderSetupCurrencyInfoFetched";
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
        public string Symbol { get; set; }
        public decimal MarginForOneLot { get; set; }
        public decimal Step { get; set; }
    }
}
