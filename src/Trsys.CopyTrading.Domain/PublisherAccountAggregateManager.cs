using Akkatecture.Aggregates;
using Akkatecture.Commands;

namespace Trsys.CopyTrading.Domain
{
    public class PublisherAccountAggregateManager : AggregateManager<PublisherAccountAggregate, PublisherAccountId, Command<PublisherAccountAggregate, PublisherAccountId>>
    {
    }
}
