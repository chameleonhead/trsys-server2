using Akkatecture.Commands;

namespace Trsys.CopyTrading.Domain
{
    public class CreatePublishmentGroupCommand : Command<PublishmentGroupAggregate, PublishmentGroupId>
    {
        public CreatePublishmentGroupCommand(PublishmentGroupId aggregateId) : base(aggregateId)
        {
        }
    }
}
