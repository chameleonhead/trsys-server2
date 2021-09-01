using EventFlow.Aggregates;

namespace Trsys.Backoffice.Abstractions
{
    public class UserNameChangedEvent : AggregateEvent<UserAggregate, UserId>
    {
        public UserNameChangedEvent(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}