using Akkatecture.Aggregates;
using Akkatecture.Commands;

namespace Trsys.CopyTrading.Domain
{
    public class SubscriberAccountAggregateManager : AggregateManager<SubscriberAccountAggregate, SubscriberAccountId, Command<SubscriberAccountAggregate, SubscriberAccountId>>
    {
    }
}
