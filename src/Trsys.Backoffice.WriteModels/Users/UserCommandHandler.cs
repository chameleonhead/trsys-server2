using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Trsys.Backoffice.Abstractions;

namespace Trsys.Backoffice.WriteModels.Users
{
    public class UserCommandHandler : CommandHandler<UserAggregate, UserId, IExecutionResult, UserCreateCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
                UserAggregate aggregate,
                UserCreateCommand command,
                CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Create(command.Username, command.PasswordHash, command.Name, command.Role);

            return Task.FromResult(executionResult);
        }
    }
}
