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
            // Publisher setup
            await client.RegisterSecretKeyAsync("EmptyOrders_ReturnSuccessAndCorrectContent1", "Publisher");
            var publisherToken = await client.GenerateTokenAsync("EmptyOrders_ReturnSuccessAndCorrectContent1", "Publisher");
            // Subscriber setup
            await client.RegisterSecretKeyAsync("EmptyOrders_ReturnSuccessAndCorrectContent2", "Subscriber");
            var token = await client.GenerateTokenAsync("EmptyOrders_ReturnSuccessAndCorrectContent2", "Subscriber");
            // Set order text
            await client.PublishOrderAsync("EmptyOrders_ReturnSuccessAndCorrectContent1", publisherToken, "");

            // Act
            var response = await client.GetAsync("/api/orders", "EmptyOrders_ReturnSuccessAndCorrectContent2", "Subscriber", token: token);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("", await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task SingleOrder_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Publisher setup
            await client.RegisterSecretKeyAsync("SingleOrder_ReturnSuccessAndCorrectContent1", "Publisher");
            var publisherToken = await client.GenerateTokenAsync("SingleOrder_ReturnSuccessAndCorrectContent1", "Publisher");
            // Subscriber setup
            await client.RegisterSecretKeyAsync("SingleOrder_ReturnSuccessAndCorrectContent2", "Subscriber");
            var token = await client.GenerateTokenAsync("SingleOrder_ReturnSuccessAndCorrectContent2", "Subscriber");
            // Set order text
            await client.PublishOrderAsync("SingleOrder_ReturnSuccessAndCorrectContent1", publisherToken, "1:USDJPY:0:1:2:1617271883");

            // Act
            var response = await client.GetAsync("/api/orders", "SingleOrder_ReturnSuccessAndCorrectContent2", "Subscriber", token: token);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("1:USDJPY:0:1:2:1617271883", await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task MultipleOrder_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Publisher setup
            await client.RegisterSecretKeyAsync("MultipleOrder_ReturnSuccessAndCorrectContent1", "Publisher");
            var publisherToken = await client.GenerateTokenAsync("MultipleOrder_ReturnSuccessAndCorrectContent1", "Publisher");
            // Subscriber setup
            await client.RegisterSecretKeyAsync("MultipleOrder_ReturnSuccessAndCorrectContent2", "Subscriber");
            var token = await client.GenerateTokenAsync("MultipleOrder_ReturnSuccessAndCorrectContent2", "Subscriber");
            // Set order text
            await client.PublishOrderAsync("MultipleOrder_ReturnSuccessAndCorrectContent1", publisherToken, "1:USDJPY:0:1:2:1617271883@2:EURUSD:1:2:3:1617271884");

            // Act
            var response = await client.GetAsync("/api/orders", "MultipleOrder_ReturnSuccessAndCorrectContent2", "Subscriber", token: token);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("1:USDJPY:0:1:2:1617271883", await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task MultipleOrder2_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Publisher setup
            await client.RegisterSecretKeyAsync("MultipleOrder2_ReturnSuccessAndCorrectContent1", "Publisher");
            var publisherToken = await client.GenerateTokenAsync("MultipleOrder2_ReturnSuccessAndCorrectContent1", "Publisher");
            // Subscriber setup
            await client.RegisterSecretKeyAsync("MultipleOrder2_ReturnSuccessAndCorrectContent2", "Subscriber");
            var token = await client.GenerateTokenAsync("MultipleOrder2_ReturnSuccessAndCorrectContent2", "Subscriber");
            // Set order text
            await client.PublishOrderAsync("MultipleOrder2_ReturnSuccessAndCorrectContent1", publisherToken, "1:USDJPY:0:1:2:1617271883");
            await client.PublishOrderAsync("MultipleOrder2_ReturnSuccessAndCorrectContent1", publisherToken, "1:USDJPY:0:1:2:1617271883@2:EURUSD:1:2:3:1617271884");

            // Act
            var response = await client.GetAsync("/api/orders", "MultipleOrder2_ReturnSuccessAndCorrectContent2", "Subscriber", token: token);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("1:USDJPY:0:1:2:1617271883", await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task SingleOrderThenEmptyOrder_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Publisher setup
            await client.RegisterSecretKeyAsync("SingleOrderThenEmptyOrder_ReturnSuccessAndCorrectContent1", "Publisher");
            var publisherToken = await client.GenerateTokenAsync("SingleOrderThenEmptyOrder_ReturnSuccessAndCorrectContent1", "Publisher");
            // Subscriber setup
            await client.RegisterSecretKeyAsync("SingleOrderThenEmptyOrder_ReturnSuccessAndCorrectContent2", "Subscriber");
            var token = await client.GenerateTokenAsync("SingleOrderThenEmptyOrder_ReturnSuccessAndCorrectContent2", "Subscriber");
            // Set order text
            await client.PublishOrderAsync("SingleOrderThenEmptyOrder_ReturnSuccessAndCorrectContent1", publisherToken, "1:USDJPY:0:1:2:1617271883");
            await client.PublishOrderAsync("SingleOrderThenEmptyOrder_ReturnSuccessAndCorrectContent1", publisherToken, "");

            // Act
            var response = await client.GetAsync("/api/orders", "SingleOrderThenEmptyOrder_ReturnSuccessAndCorrectContent2", "Subscriber", token: token);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("", await response.Content.ReadAsStringAsync());
        }
    }
}
