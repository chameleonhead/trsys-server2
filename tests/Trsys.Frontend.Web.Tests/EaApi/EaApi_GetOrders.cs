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
        public async Task ReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Publish empty text
            await client.RegisterSecretKeyAsync("ReturnSuccessAndCorrectContentType1", "Publisher");
            var publisherToken = await client.GenerateTokenAsync("ReturnSuccessAndCorrectContentType1", "Publisher");
            await client.PublishOrderAsync("ReturnSuccessAndCorrectContentType1", publisherToken, "");
            // Subscriber setup
            await client.RegisterSecretKeyAsync("ReturnSuccessAndCorrectContentType2", "Subscriber");
            var token = await client.GenerateTokenAsync("ReturnSuccessAndCorrectContentType2", "Subscriber");

            // Act
            var response = await client.GetAsync("/api/orders", "ReturnSuccessAndCorrectContentType2", "Subscriber", token: token);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("", await response.Content.ReadAsStringAsync());
        }
    }
}
