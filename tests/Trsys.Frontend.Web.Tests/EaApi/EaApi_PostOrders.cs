using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Tests.EaApi
{
    [TestClass]
    public class EaApi_PostOrders
    {
        private WebApplicationFactory<Startup> _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [TestCleanup]
        public void Teardown()
        {
            _factory.Dispose();
        }

        [TestMethod]
        public async Task EmptyOrders_ReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.RegisterSecretKeyAsync("EmptyOrders_ReturnSuccess", "Publisher");
            var token = await client.GenerateTokenAsync("EmptyOrders_ReturnSuccess", "Publisher");

            // Act
            var response = await client.PostAsync("/api/orders", "EmptyOrders_ReturnSuccess", "Publisher", content: "", token: token);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
