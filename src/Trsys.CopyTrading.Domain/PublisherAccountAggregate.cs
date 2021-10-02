using Akkatecture.Aggregates;

namespace Trsys.CopyTrading.Domain
{
    public class PublisherAccountAggregate : AggregateRoot<PublisherAccountAggregate, PublisherAccountId, PublisherAccountState>
    {
        public PublisherAccountAggregate(PublisherAccountId id) : base(id)
        {
        }
    }
}
