using Akkatecture.Aggregates;

namespace Trsys.CopyTrading.Domain
{
    public class SubscriberAccountAggregate : AggregateRoot<SubscriberAccountAggregate, SubscriberAccountId, SubscriberAccountState>
    {
        public SubscriberAccountAggregate(SubscriberAccountId id) : base(id)
        {
        }
    }
}