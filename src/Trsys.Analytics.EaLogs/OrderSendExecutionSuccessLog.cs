using System;

namespace Trsys.Analytics.EaLogs
{
    public class OrderSendExecutionSuccessLog : LogBase
    {
        public OrderSendExecutionSuccessLog()
        {
        }

        public OrderSendExecutionSuccessLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, long serverTicketNo, long localTicketNo) : base(timestamp, key, keyType, version, token)
        {
            ServerTicketNo = serverTicketNo;
            LocalTicketNo = localTicketNo;
        }

        public long ServerTicketNo { get; set; }
        public long LocalTicketNo { get; set; }
    }
}