using System;

namespace Trsys.Frontend.Application.Dtos
{
    public class SubscriberOrderStateDto
    {
        public enum SubscriberOrderStatus
        {
            Opening,
            Opened,
            Closing,
            Closed,
        }

        public string SecretKeyId { get; set; }
        public string CopyTradeId { get; set; }
        public string TicketNo { get; set; }
        public string Symbol { get; set; }
        public string OrderType { get; set; }
        public SubscriberOrderStatus Status { get; set; }
        public DateTimeOffset OpenTimestamp { get; set; }
    }
}
