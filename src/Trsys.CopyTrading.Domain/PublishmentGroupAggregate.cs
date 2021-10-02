using Akkatecture.Aggregates;

namespace Trsys.CopyTrading.Domain
{
    public class PublishmentGroupAggregate : AggregateRoot<PublishmentGroupAggregate, PublishmentGroupId, PublishmentGroupState>
    {
        public PublishmentGroupAggregate(PublishmentGroupId id) : base(id)
        {
        }
    }
}
