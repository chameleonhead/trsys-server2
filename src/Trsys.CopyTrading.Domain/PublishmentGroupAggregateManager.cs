using Akkatecture.Aggregates;
using Akkatecture.Commands;

namespace Trsys.CopyTrading.Domain
{
    public class PublishmentGroupAggregateManager : AggregateManager<PublishmentGroupAggregate, PublishmentGroupId, Command<PublishmentGroupAggregate, PublishmentGroupId>>
    {
    }
}
