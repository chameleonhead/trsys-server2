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
            client.RegisterSecretKeyAsync("ReturnSuccessAndCorrectContentType1", "Publisher");
            client.RegisterSecretKeyAsync("ReturnSuccessAndCorrectContentType2", "Subscriber");

            // Act
            var response = await client.GetAsync("/api/orders", "SECRETKEY", "Subscriber");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
