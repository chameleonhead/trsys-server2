using System;

namespace Trsys.Analytics.EaLogs
{
    public class OrderSendExecutionSuccessLog : ILogInfo
    {
        public OrderSendExecutionSuccessLog()
        {
        }

        public OrderSendExecutionSuccessLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, long localTicketNo)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = timestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Token = token;
            ServerTicketNo = serverTicketNo;
            LocalTicketNo = localTicketNo;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "OrderSendExecutionSuccessLog";
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
        public long ServerTicketNo { get; set; }
        public long LocalTicketNo { get; set; }
    }
}