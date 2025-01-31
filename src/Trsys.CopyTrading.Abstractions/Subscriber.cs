﻿using System;
using Trsys.CopyTrading.Events;

namespace Trsys.CopyTrading.Abstractions
{
    public class Subscriber : EaBase
    {
        private OrderText publishedOrderText = null;

        public Subscriber(string key, IEventQueue events) : base(key, "Subscriber", events)
        {
        }

        public OrderText SubscribeOrderText(DateTimeOffset timestamp, OrderText currentOrderText)
        {
            if (publishedOrderText != currentOrderText)
            {
                publishedOrderText = currentOrderText;
                Events.Enqueue(new SubscriberOrderTextChangedEvent(timestamp, Key, currentOrderText.Text));
            }
            return currentOrderText;
        }

        record PublisherOrderKey(string PublisherKey, int TicketNo);
    }
}
