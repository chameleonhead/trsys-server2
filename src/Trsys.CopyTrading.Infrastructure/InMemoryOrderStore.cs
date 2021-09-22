using System;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class InMemoryOrderStore : IOrderStore, IDisposable
    {
        private readonly InMemoryCopyTradingContext context;
        private readonly IEventQueue events;
        private readonly IOrderNotificationBus orderBus;

        public InMemoryOrderStore(InMemoryCopyTradingContext context, IEventQueue events, IOrderNotificationBus orderBus)
        {
            this.context = context;
            this.events = events;
            this.orderBus = orderBus;
            orderBus.OrderOpenPublished += OnOrderOpenPublished;
            orderBus.OrderClosePublished += OnOrderClosePublished;
        }

        public OrderText GetOrderText()
        {
            return context.CurrentOrderText;
        }

        private void OnOrderOpenPublished(object sender, OrderEventArgs e)
        {
            if (context.CurrentOrderText != OrderText.Empty)
            {
                return;
            }
            context.CurrentOrders.Add(new InMemoryKeys.PublisherOrderKey(e.Order.PublisherKey, e.Order.TicketNo));
            context.CurrentOrderText = OrderText.Parse(e.Order.Text);
            orderBus.UpdateOrderText(context.CurrentOrderText);
        }

        private void OnOrderClosePublished(object sender, OrderEventArgs e)
        {
            if (context.CurrentOrderText == OrderText.Empty)
            {
                return;
            }
            var key = new InMemoryKeys.PublisherOrderKey(e.Order.PublisherKey, e.Order.TicketNo);
            if (context.CurrentOrders.Contains(key))
            {
                context.CurrentOrders.Remove(key);
                context.CurrentOrderText = OrderText.Empty;
                orderBus.UpdateOrderText(context.CurrentOrderText);
            }
        }

        public void Dispose()
        {
            orderBus.OrderOpenPublished -= OnOrderOpenPublished;
            orderBus.OrderClosePublished -= OnOrderClosePublished;
            GC.SuppressFinalize(this);
        }
    }
}
