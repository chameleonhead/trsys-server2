using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace Trsys.Backoffice.Abstractions
{
    public class UserAggregate : AggregateRoot<UserAggregate, UserId>
    {
        public UserAggregate(UserId id) : base(id)
        {
        }

        public IExecutionResult Create(string username, string passwordHash, string name, string role)
        {
            Emit(new UserCreatedEvent(username, passwordHash, name, role));
            return ExecutionResult.Success();
        }
    }
}