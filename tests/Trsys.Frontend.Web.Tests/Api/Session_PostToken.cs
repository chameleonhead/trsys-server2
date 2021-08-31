using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Tests.Api
{
    [TestClass]
    public class Session_PostToken
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

            // Act
            var response = await client.PostAsync("/api/session", JsonContent.Create(new { username = "admin", password = "P@ssw0rd" }));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
