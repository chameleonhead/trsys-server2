using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Trsys.Events.Abstractions;

namespace Trsys.Events
{
    public class PostConfigureEventsOption : IPostConfigureOptions<EventsOptions>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IEventSubscriber subscriber;

        public PostConfigureEventsOption(IServiceProvider serviceProvider, IEventSubscriber subscriber)
        {
            this.serviceProvider = serviceProvider;
            this.subscriber = subscriber;
        }
        public void PostConfigure(string name, EventsOptions options)
        {
            subscriber.Subscribe(options.EventHandlerTypes.Select(type => serviceProvider.GetService(type) as IEventHandler).ToList());
        }
    }
}
