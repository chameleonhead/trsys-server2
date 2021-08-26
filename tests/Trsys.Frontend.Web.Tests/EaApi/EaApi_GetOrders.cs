using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Tests.EaApi
{
    [TestClass]
    public class EaApi_GetOrders
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
        public async Task EmptyOrders_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Publisher setup and set empty text
            await client.RegisterSecretKeyAsync("EmptyOrders_ReturnSuccessAndCorrectContent1", "Publisher");
            var publisherToken = await client.GenerateTokenAsync("EmptyOrders_ReturnSuccessAndCorrectContent1", "Publisher");
            await client.PublishOrderAsync("EmptyOrders_ReturnSuccessAndCorrectContent1", publisherToken, "");
            // Subscriber setup
            await client.RegisterSecretKeyAsync("EmptyOrders_ReturnSuccessAndCorrectContent2", "Subscriber");
            var token = await client.GenerateTokenAsync("EmptyOrders_ReturnSuccessAndCorrectContent2", "Subscriber");

            // Act
            var response = await client.GetAsync("/api/orders", "EmptyOrders_ReturnSuccessAndCorrectContent2", "Subscriber", token: token);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("", await response.Content.ReadAsStringAsync());
        }
    }
}
