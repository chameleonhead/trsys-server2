using EventFlow;
using EventFlow.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using Trsys.Backoffice.Abstractions;
using Trsys.Backoffice.WriteModels.Users;

namespace Trsys.Backoffice.Tests
{
    [TestClass]
    public class UserCreateCommandTests
    {
        [TestMethod]
        public async Task Success()
        {
            using var services = new ServiceCollection().AddBackofficeInfrastructure().BuildServiceProvider();
            var resolver = services.GetService<IRootResolver>();
            var bus = resolver.Resolve<ICommandBus>();
            var result = await bus.PublishAsync(new UserCreateCommand(UserId.New, "admin", "PasswordHash", "Name", "Administrator"), CancellationToken.None);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
