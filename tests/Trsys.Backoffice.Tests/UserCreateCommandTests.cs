using EventFlow.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Trsys.Backoffice.Tests
{
    [TestClass]
    public class UserCreateCommandTests
    {
        [TestMethod]
        public void Success()
        {
            using var services = new ServiceCollection().AddBackofficeInfrastructure().BuildServiceProvider();
            var resolver = services.GetService<IRootResolver>();
        }
    }
}
