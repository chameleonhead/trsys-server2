using System.Threading;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public class CopyTradingEventHandler : IEventHandler
    {
        public Task Handle(IEvent e, CancellationToken cancellationToken)
        {
            switch (e)
            {
                case SecretKeyRegisteredEvent ev:
                    break;
                case SecretKeyUnregisteredEvent ev:
                    break;
                case EaSessionGeneratedEvent ev:
                    break;
                case EaSessionDiscardedEvent ev:
                    break;
                case EaSessionValidatedEvent ev:
                    break;
                case OrderTextPublishedEvent ev:
                    break;
                case PublisherOrderOpenPublishedEvent ev:
                    break;
                case PublisherOrderClosePublishedEvent ev:
                    break;
                case PublisherOrderIgnoredEvent ev:
                    break;
                case ActiveOrderPublishedEvent ev:
                    break;
                case SubscriberOrderOpenDeliveredEvent ev:
                    break;
                case SubscriberOrderCloseDeliveredEvent ev:
                    break;
                case EaLogReceivedEvent ev:
                    break;
                case EaLogReceivedV2Event ev:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
