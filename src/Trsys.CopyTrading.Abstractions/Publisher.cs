using Trsys.CopyTrading.Events;

namespace Trsys.CopyTrading.Abstractions
{
    public class Publisher : EaBase
    {
        private OrderText CurrentOrder = OrderText.Empty;
        private IOrderNotificationBus orderBus;

        public Publisher(string key, IEventQueue events, IOrderNotificationBus orderBus) : base(key, "Publisher", events)
        {
            this.orderBus = orderBus;
        }

        public void UpdateOrderText(string text)
        {
            var orderText = OrderText.Parse(text);
            var diff = OrderDifference.CalculateDifference(CurrentOrder, orderText);
            if (diff.HasDifference)
            {
                CurrentOrder = orderText;
                Events.Enqueue(new PublisherOrderTextChangedEvent(Key, text));
                foreach (var item in diff.Opened)
                {
                    orderBus.PublishOpen(Key, item);
                    Events.Enqueue(new PublisherOrderOpenPublishedEvent(
                        Key,
                        item.Text,
                        item.TicketNo,
                        item.Symbol,
                        item.OrderType.ToString(),
                        item.Price,
                        item.Lots,
                        item.Time));
                }
                foreach (var item in diff.Closed)
                {
                    orderBus.PublishClose(Key, item);
                    Events.Enqueue(new PublisherOrderClosePublishedEvent(
                        Key,
                        item.Text,
                        item.TicketNo,
                        item.Symbol,
                        item.OrderType.ToString(),
                        item.Price,
                        item.Lots,
                        item.Time));
                }
            }
        }
    }
}
